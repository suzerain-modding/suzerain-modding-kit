using MelonLoader;
using SuzerainModdingKit;
using SuzerainModdingKit.Save;
using SuzerainModdingKit.StoryFragments.Conversation;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(Core), "Suzerain Modding Kit", SmkInfo.VersionStr, "Fluffyalien1422", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace SuzerainModdingKit;

internal sealed class Core : MelonMod
{
    private bool _isVersionMismatched;
    private bool _isVersionMismatchWarningIgnored;
    private bool _shouldShowVersionMismatchGui =>
        _isVersionMismatched && !_isVersionMismatchWarningIgnored;
    private bool _versionMismatchGuiShowAdvanced;

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg(
            $"Suzerain Modding Kit version: {SmkInfo.VersionStr}, " +
            $"Suzerain version: {Application.version}, " +
            $"Target Suzerain version: {SmkInfo.TargetSuzerainVersion}.");

        if (!string.Equals(
            Application.version,
            SmkInfo.TargetSuzerainVersion,
            StringComparison.Ordinal))
        {
            LoggerInstance.Warning(
                $"Expected Suzerain version {SmkInfo.TargetSuzerainVersion}, " +
                $"but got {Application.version}. Suzerain Modding Kit may not work properly.");
            _isVersionMismatched = true;
        }

        LoggerInstance.Msg("Cleaning up mod saves.");
        SaveManager.CleanupOrphanedModSaves();

        LoggerInstance.Msg("Pre-initialization complete.");
    }

    public override void OnLateInitializeMelon()
    {
        ConversationRegistry.CloseRegistration();
        LoggerInstance.Msg("Conversation registration closed.");

        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        if (kb.ctrlKey.isPressed && kb.dKey.wasPressedThisFrame)
        {
            //_ = GameState.AddCustomStoryFragment(new CustomConversationFragment(
            //     name: "ExampleMod.ExampleConversationFragment",
            //     storyPack: StoryPack.SuzerainStoryPackInfo.Sordland,
            //     assignedTokenName: StoryPack.SuzerainTokenName.SordlandCityHolsord,
            //     hubTitle: "hubTitle",
            //     hubDescription: "hubDescription",
            //     conversationName: "Sordland/Turn04/PSnW_SchoolVisit"));
            DebugOverlay.SetVisibility(value: !DebugOverlay.IsShowing());
        }
    }

    public override void OnGUI()
    {
        UpdateVersionMismatchGui();
        DebugOverlay.Update();
    }

    public void UpdateVersionMismatchGui()
    {
        if (!_shouldShowVersionMismatchGui)
        {
            return;
        }

        int size = 250;
        int sizeHalf = size / 2;
        int x = (Screen.width / 2) - sizeHalf;
        int y = (Screen.height / 2) - sizeHalf;
        GUILayout.BeginArea(new Rect(x, y, size, size));
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(size));

        GUILayout.Label("Suzerain Modding Kit Warning");
        GUILayout.Label($"VERSION MISMATCH: Expected Suzerain v{SmkInfo.TargetSuzerainVersion}, " +
            $"but got v{Application.version}.");
        GUILayout.Label("Suzerain Modding Kit may not work properly.");

        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
        if (_versionMismatchGuiShowAdvanced)
        {
            GUILayout.Label("Advanced:");
            if (GUILayout.Button("Continue Anyway"))
            {
                LoggerInstance.Warning("User ignored version mismatch warning.");
                _isVersionMismatchWarningIgnored = true;
            }
        }
        else if (GUILayout.Button("Advanced Options"))
        {
            _versionMismatchGuiShowAdvanced = true;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
