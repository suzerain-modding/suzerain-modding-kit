using HarmonyLib;
using Il2Cpp;

namespace SuzerainModdingKit.Save;

[HarmonyPatch(typeof(JsonSaveLoad), nameof(JsonSaveLoad.SaveDataToFile))]
internal static class JsonSaveLoad_SaveDataToFile_Patch
{
    public static void Postfix(string path)
    {
        if (!GameState.IsGameActive)
        {
            // Return if the game is not active. This function may be called
            // before the game actually starts loading, so if we create a
            // save file, it would overwrite the existing save.
            return;
        }

        string fileName = Path.GetFileName(path);
        SaveManager.Save(fileName);
    }
}

[HarmonyPatch(typeof(PersistenceManager), nameof(PersistenceManager.LoadSaveFile))]
internal static class PersistenceManager_LoadSaveFile_Patch
{
    public static void Postfix(SaveFile saveFile)
    {
        string fileName = Path.GetFileName(saveFile.path);
        bool isActiveSave = saveFile.saveData.saveFileType == PersistenceManager.SaveFileType.Active;
        SaveManager.OnSuzerainLoadSaveFile(fileName, isActiveSave);
    }
}
