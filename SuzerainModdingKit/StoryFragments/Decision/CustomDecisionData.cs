using Il2Cpp;
using SuzerainModdingKit.StoryPack;

namespace SuzerainModdingKit.StoryFragments.Decision;

/// <summary>
/// Represents the data required to define a custom decision story fragment for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomStoryFragment"/>
public class CustomDecisionData : CustomStoryFragmentData
{
    /// <summary>
    /// The full title of this decision shown in the decision panel.
    /// </summary>
    public string Title
    {
        get;
    }
    /// <summary>
    /// The full description of this decision shown in the decision panel.
    /// </summary>
    public string Description
    {
        get;
    }
    /// <summary>
    /// The short title of this decision shown under the assigned token.
    /// </summary>
    public string HubTitle
    {
        get;
    }
    /// <summary>
    /// The short description of this decision shown under the assigned token.
    /// </summary>
    public string HubDescription
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
    /// See <c cref="SuzerainStoryPackInfo">SuzerainStoryPackInfo</c>.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this story fragment should appear on.
    /// See <c cref="SuzerainTokenName">SuzerainTokenName</c>.
    /// </param>
    /// <param name="title">
    /// The full title of this decision shown in the decision panel.
    /// </param>
    /// <param name="description">
    /// The full description of this decision shown in the decision panel.
    /// </param>
    /// <param name="hubTitle">
    /// The short title of this decision shown under the assigned token.
    /// </param>
    /// <param name="hubDescription">
    /// The short description of this decision shown under the assigned token.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public CustomDecisionData(
        string name,
        StoryPackInfo storyPack,
        string assignedTokenName,
        string title,
        string description,
        string hubTitle,
        string hubDescription) : base(name, storyPack, assignedTokenName)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        HubTitle = hubTitle ?? throw new ArgumentNullException(nameof(hubTitle));
        HubDescription = hubDescription ?? throw new ArgumentNullException(nameof(hubDescription));
    }

    internal override DecisionData RegisterInSuzerain()
    {
        DecisionData data = ToSuzerainStoryFragmentData();
        Func<DecisionData, bool> match = d => string.Equals(
            d.NameInDatabase,
            data.NameInDatabase,
            StringComparison.Ordinal);
        bool existsInRegistry = EntityDataManager.AllDecisionsData.Exists(match);
        if (!existsInRegistry)
        {
            EntityDataManager.AllDecisionsData.Add(data);
        }
        return data;
    }

    private DecisionData ToSuzerainStoryFragmentData()
    {
        DecisionProperties properties = new()
        {
            Title = Title,
            Description = Description,
            HubTitle = HubTitle,
            HubDescription = HubDescription,
        };
        DecisionData data = new()
        {
            AppBundleProperties = StoryPack.ToAppBundleProperties(),
            AssignedTokenProperties = new AssignedTokenProperties()
            {
                AssignedToken = AssignedTokenName,
            },
            DecisionProperties = properties,
            NameInDatabase = Name,
            Path = "Sordland/Decisions",
            StoryFragmentProperties = new StoryFragmentProperties()
            {
                IsDone = false,
                OnStoryFragmentBeginInstruction = string.Empty,
                OnStoryFragmentEndInstruction = string.Empty,
            },
            TagsProperties = new TagsProperties(),
        };
        return data;
    }
}
