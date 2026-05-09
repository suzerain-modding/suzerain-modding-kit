using HarmonyLib;
using Il2Cpp;
using MelonLoader;

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

[HarmonyPatch(typeof(DecisionPanel), nameof(DecisionPanel.OnFinish))]
internal static class DecisionPanel_OnFinish_Patch
{
    public static void Postfix(DecisionProperties.DecisionOption decisionOption)
    {
        DecisionOptionInfo info = new(decisionOption.Text, DecisionManager.CurrentDecisionName);
        Melon<Core>.Logger.Msg(
            $"Event: OnDecisionFinished ({DecisionManager.CurrentDecisionName}).");
        Events.TriggerOnDecisionFinished(info);
    }
}
