using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using DecisionOption = Il2Cpp.DecisionProperties.DecisionOption;

namespace SuzerainModdingKit.StoryFragments.Decision;

[HarmonyPatch(typeof(DecisionPanel), nameof(DecisionPanel.Show))]
internal static class DecisionPanel_Show_Patch
{
    public static void Postfix()
    {
        Melon<Core>.Logger.Msg(
            $"Event: OnDecisionShow ({DecisionManager.CurrentDecisionName}).");
        Events.TriggerOnDecisionShow();
    }
}

[HarmonyPatch(typeof(TemplateDecisionOptionButton), nameof(TemplateDecisionOptionButton.OnClick))]
internal static class TemplateDecisionOptionButton_OnClick_Patch
{
    public static void Postfix(TemplateDecisionOptionButton __instance)
    {
        DecisionOption option = __instance.currentDecisionOption;
        DecisionOptionInfo info = new(option.Text, DecisionManager.CurrentDecisionName);
        Melon<Core>.Logger.Msg(
            $"Event: OnDecisionFinished ({DecisionManager.CurrentDecisionName}).");
        Events.TriggerOnDecisionFinished(info);
    }
}
