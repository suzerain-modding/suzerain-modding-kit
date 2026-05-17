using Il2Cpp;
using MelonLoader;

namespace SuzerainModdingKit.StoryFragments.Decision;

/// <summary>
/// Static interface for querying and modifying the current active decision.
/// </summary>
public static class DecisionManager
{
    private static DecisionPanel _panel => Panels.Instance?.DecisionPanel;

    /// <summary>
    /// The name of the current decision. May be null.
    /// </summary>
    public static string CurrentDecisionName => _panel?.currentDecisionData?.NameInDatabase;

    /// <summary>
    /// Get the list of options in the current decision.
    /// </summary>
    /// <returns>
    /// Returns a list of <c cref="DecisionOptionInfo">DecisionOptionInfo</c> objects representing
    /// the options in the decision.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the game is not active.
    /// </exception>
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

    /// <summary>
    /// Add an option to the current decision.
    /// </summary>
    /// <param name="text">
    /// The text of the new option.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the game is not active.
    /// </exception>
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
