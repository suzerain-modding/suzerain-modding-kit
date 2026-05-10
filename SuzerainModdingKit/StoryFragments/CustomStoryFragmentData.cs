using Il2Cpp;
using SuzerainModdingKit.StoryPack;

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
    /// The story pack that this story fragment should appear in.
    /// </summary>
    public StoryPackInfo StoryPack
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
    /// The unique identifier of this story fragment.
    /// </param>
    /// <param name="storyPack">
    /// The story pack that this story fragment should appear in.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this story fragment should appear on (eg. "Sordland_City_Holsord").
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    internal CustomStoryFragmentData(string name, StoryPackInfo storyPack, string assignedTokenName)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        StoryPack = storyPack ?? throw new ArgumentNullException(nameof(storyPack));
        AssignedTokenName = assignedTokenName ??
            throw new ArgumentNullException(nameof(assignedTokenName));
    }

    internal abstract StoryFragmentData RegisterInSuzerain();
}
