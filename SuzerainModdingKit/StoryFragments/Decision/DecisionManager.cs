using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit.StoryFragments.Decision;

public static class DecisionManager
{
    private static DecisionPanel _panel => Panels.Instance?.DecisionPanel;
    public static string CurrentDecisionName => _panel?.currentDecisionData?.NameInDatabase;

    public static IList<DecisionOptionInfo> GetOptions()
    {
        GameState.ThrowIfInactive();

        List<DecisionOptionInfo> options = [];
        foreach (TemplateDecisionOptionButton button in _panel.instantiatedDecisionOptionButtons)
        {
            // Reading these values can get very error-prone in certain cases.
            // Use a try..catch.
            try
            {
                string text = button.currentDecisionOption.Text;
                DecisionOptionInfo info = new(text, CurrentDecisionName);
                options.Add(info);
            }
            catch (Exception ex)
            {
                Melon<Core>.Logger.Warning("An exception occured while trying to get an " +
                    $"instantiated decision option: {ex}");
            }
        }
        return options;
    }

    public static void AddOption(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        GameState.ThrowIfInactive();

        int nextIndex = _panel.instantiatedDecisionOptionButtons.Count;
        DecisionProperties.DecisionOption option = new()
        {
            Text = text,
            // The game crashes when selecting the option if these properties are not defined.
            Condition = string.Empty,
            Instruction = string.Empty,
        };

        _panel.CreateDecisionOptionButton(option, nextIndex);
    }
}
