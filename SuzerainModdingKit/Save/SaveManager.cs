using System.Text.Json;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;

namespace SuzerainModdingKit.Save;

internal static class SaveManager
{
    internal sealed class ModSaveData
    {
        public Dictionary<string, object> Variables
        {
            get; set;
        }
    }

    public static readonly string UserDir = Environment.GetEnvironmentVariable("userprofile");
    public static readonly string SuzerainSavePath = Path.Combine(UserDir,
        @"AppData\LocalLow\Torpor Games\Suzerain");
    public static readonly string ModSavePath = Path.Combine(SuzerainSavePath, "moddingkit");

    // Suzerain's active save (Active_[date].json) is the currently loaded save. Its file name
    // is date-stamped and can change, and Suzerain also updates it by copying other saves onto
    // it, which doesn't trigger the SaveDataToFile patch. Rather than mirror that file name, we
    // maintain our own active save under this fixed name so it always stays in sync with the
    // data we save and load.
    internal const string ActiveSaveFileName = "active.json";

    public static void CleanupOrphanedModSaves()
    {
        if (!Directory.Exists(ModSavePath))
        {
            return;
        }

        HashSet<string> suzerainSaveNames = Directory
            .GetFiles(SuzerainSavePath, "*.json", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        string[] modSavePaths = Directory.GetFiles(
            ModSavePath,
            "*.json",
            SearchOption.TopDirectoryOnly);

        foreach (string filePath in modSavePaths)
        {
            string fileName = Path.GetFileName(filePath);
            if (string.Equals(fileName, ActiveSaveFileName, StringComparison.OrdinalIgnoreCase))
            {
                // Our active save has no matching Suzerain save file, so exclude it from cleanup.
                continue;
            }
            if (!suzerainSaveNames.Contains(fileName))
            {
                File.Delete(filePath);
            }
        }
    }

    public static void OnSuzerainLoadSaveFile(string fileName, bool isActiveSave)
    {
        if (isActiveSave)
        {
            // Suzerain is loading its active save, so load our own active save.
            LoadSave(ActiveSaveFileName);
            return;
        }

        LoadSave(fileName);

        // Suzerain copies the loaded save onto its active save. That copy doesn't trigger the
        // SaveDataToFile patch, so mirror it here to keep our active save in sync.
        CopyToActiveSave(fileName);
    }

    public static void Save(string fileName, bool isActiveSave)
    {
        if (isActiveSave)
        {
            // Save our active save under a fixed file name instead of matching Suzerain's
            // date-stamped active save file name.
            fileName = ActiveSaveFileName;
        }

        string savePath = Path.Combine(ModSavePath, fileName);

        Dictionary<string, object> variables = [];
        try
        {
            foreach (string variableName in Variables.RegisteredVariables)
            {
                if (!DialogueLua.DoesVariableExist(variableName))
                {
                    // Silently skip if it doesn't exist.
                    continue;
                }

                Lua.Result valueLua = DialogueLua.GetVariable(variableName);
                if (valueLua.IsBool)
                {
                    variables[variableName] = valueLua.AsBool;
                }
                else if (valueLua.IsString)
                {
                    variables[variableName] = valueLua.AsString;
                }
                else if (valueLua.IsNumber)
                {
                    variables[variableName] = valueLua.AsFloat;
                }
                else
                {
                    Melon<Core>.Logger.Warning($"Game variable '{variableName}' not saved. " +
                        "Invalid type.");
                }
            }
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Error($"Failed to save mod data: {e}");
            return;
        }

        if (variables.Count == 0)
        {
            // There is nothing to save. Don't create an empty save file.
            return;
        }

        ModSaveData modSaveData = new()
        {
            Variables = variables,
        };
        string json = JsonSerializer.Serialize(modSaveData);

        Directory.CreateDirectory(ModSavePath);
        File.WriteAllText(savePath, json);

        Melon<Core>.Logger.Msg($"Saved mod data to '{savePath}'.");
    }

    private static void CopyToActiveSave(string fileName)
    {
        if (string.Equals(fileName, ActiveSaveFileName, StringComparison.OrdinalIgnoreCase))
        {
            // The active save is already the source; nothing to copy.
            return;
        }

        string sourcePath = Path.Combine(ModSavePath, fileName);
        string activePath = Path.Combine(ModSavePath, ActiveSaveFileName);

        if (!File.Exists(sourcePath))
        {
            // The loaded save has no mod data, so our active save shouldn't either.
            if (File.Exists(activePath))
            {
                File.Delete(activePath);
            }
            return;
        }

        File.Copy(sourcePath, activePath, overwrite: true);
    }

    private static void LoadSave(string fileName)
    {
        string savePath = Path.Combine(ModSavePath, fileName);

        if (!File.Exists(savePath))
        {
            return;
        }

        string json = File.ReadAllText(savePath);
        ModSaveData modSaveData;
        try
        {
            modSaveData = JsonSerializer.Deserialize<ModSaveData>(json);
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Error($"Failed to deserialize mod save at '{savePath}': {e}");
            return;
        }

        if (modSaveData == null || modSaveData.Variables == null)
        {
            Melon<Core>.Logger.Error($"Invalid mod save at '{savePath}'.");
            return;
        }

        try
        {
            foreach (KeyValuePair<string, object> entry in modSaveData.Variables)
            {
                SetVariableJsonElement(entry.Key, (JsonElement)entry.Value);
            }
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Error("Failed to load game variables from mod save at " +
                $"'{savePath}': {e}");
            return;
        }

        Melon<Core>.Logger.Msg($"Loaded mod data from '{savePath}'.");
    }

    private static void SetVariableJsonElement(string variableName, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.True:
            case JsonValueKind.False:
                _ = Variables.Set(variableName, value.GetBoolean());
                break;
            case JsonValueKind.String:
                _ = Variables.Set(variableName, value.GetString());
                break;
            case JsonValueKind.Number:
                _ = Variables.Set(variableName, value.GetSingle());
                break;
            case JsonValueKind.Undefined:
            case JsonValueKind.Object:
            case JsonValueKind.Array:
            case JsonValueKind.Null:
            default:
                Melon<Core>.Logger.Warning($"Game variable '{variableName}' not loaded. " +
                    "Invalid type.");
                break;
        }
    }
}
