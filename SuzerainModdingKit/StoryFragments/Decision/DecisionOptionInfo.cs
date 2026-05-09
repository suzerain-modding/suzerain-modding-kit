namespace SuzerainModdingKit.StoryFragments.Decision;

public class DecisionOptionInfo
{
    public string Text
    {
        get;
    }
    public string DecisionName
    {
        get;
    }

    public DecisionOptionInfo(string text, string decisionName)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        DecisionName = decisionName ?? throw new ArgumentNullException(nameof(text));
    }
}
