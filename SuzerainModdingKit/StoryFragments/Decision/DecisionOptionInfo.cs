namespace SuzerainModdingKit.StoryFragments.Decision;

/// <summary>
/// Read-only representation of an option in a decision.
/// </summary>
/// <seealso cref="DecisionManager"/>
public class DecisionOptionInfo
{
    /// <summary>
    /// The text of the option.
    /// </summary>
    public string Text
    {
        get;
    }
    /// <summary>
    /// The name of the decision that this option belongs to.
    /// </summary>
    public string DecisionName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="text">
    /// The text of the option.
    /// </param>
    /// <param name="decisionName">
    /// The name of the decision that this option belongs to.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public DecisionOptionInfo(string text, string decisionName)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        DecisionName = decisionName ?? throw new ArgumentNullException(nameof(text));
    }
}
