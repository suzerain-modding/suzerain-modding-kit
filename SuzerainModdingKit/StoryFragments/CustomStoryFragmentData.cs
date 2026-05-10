using Il2Cpp;

namespace SuzerainModdingKit.StoryFragments;

/// <summary>
/// Represents the data required to define a custom story fragment for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomStoryFragment"/>
public abstract class CustomStoryFragmentData
{
    /// <summary>
    /// The unique identifier of this story fragment.
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// The name of the token this story fragment should appear on (eg. "Sordland_City_Holsord").
    /// </summary>
    public string AssignedTokenName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of the bill.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this story fragment should appear on (eg. "Sordland_City_Holsord").
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    internal CustomStoryFragmentData(string name, string assignedTokenName)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AssignedTokenName = assignedTokenName ??
            throw new ArgumentNullException(nameof(assignedTokenName));
    }

    internal abstract StoryFragmentData RegisterInSuzerain();
}
