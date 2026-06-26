using Il2Cpp;
using MelonLoader;
using SuzerainModdingKit.StoryPack;
using SuzerainModdingKit.Utils;
using SuzerainModdingKit.VanillaData;
using ConversationType = Il2Cpp.ConversationData.ConversationType;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// Represents the data required to define a custom conversation story fragment for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomStoryFragment"/>
public class CustomConversationFragment : CustomStoryFragmentData
{
    /// <summary>
    /// The short title of this bill shown under the assigned token.
    /// </summary>
    public string HubTitle
    {
        get;
    }
    /// <summary>
    /// The short description of this bill shown under the assigned token.
    /// </summary>
    public string HubDescription
    {
        get;
    }
    /// <summary>
    /// The conversation type.
    /// </summary>
    public ConversationType Type
    {
        get;
    }
    /// <summary>
    /// The name of the conversation that this story fragment opens.
    /// </summary>
    public string ConversationName
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
    /// <param name="hubTitle">
    /// The short title of this bill shown under the assigned token.
    /// </param>
    /// <param name="hubDescription">
    /// The short description of this bill shown under the assigned token.
    /// </param>
    /// <param name="type">
    /// The conversation type.
    /// </param>
    /// <param name="conversationName">
    /// The name of the conversation that this story fragment opens.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public CustomConversationFragment(
        string name,
        StoryPackInfo storyPack,
        string assignedTokenName,
        string hubTitle,
        string hubDescription,
        ConversationType type,
        string conversationName) : base(name, storyPack, assignedTokenName)
    {
        HubTitle = hubTitle ?? throw new ArgumentNullException(nameof(hubTitle));
        HubDescription = hubDescription ?? throw new ArgumentNullException(nameof(hubDescription));
        Type = type;
        ConversationName = conversationName
            ?? throw new ArgumentNullException(nameof(conversationName));
    }

    internal override ConversationData RegisterInSuzerain(AddStoryFragmentOptions options)
    {
        if (DialogueUtils.GetConversation(ConversationName) == null)
        {
            Melon<Core>.Logger.Error("Failed to register conversation story fragment: " +
                $"The referenced conversation '{ConversationName}' does not exist.");
            return null;
        }

        ConversationData data = ToSuzerainStoryFragmentData();
        Func<ConversationData, bool> match = d => string.Equals(
            d.NameInDatabase,
            data.NameInDatabase,
            StringComparison.Ordinal);
        bool existsInRegistry = EntityDataManager.AllConversationsData.Exists(match);
        if (!existsInRegistry)
        {
            EntityDataManager.AllConversationsData.Add(data);
        }
        return data;
    }

    private ConversationData ToSuzerainStoryFragmentData()
    {
        ConversationProperties properties = new()
        {
            Dialogue = ConversationName,
            Subtitle = HubDescription,
            Title = HubTitle,
        };
        ConversationData data = new()
        {
            AppBundleProperties = StoryPack.ToAppBundleProperties(),
            AssignedTokenProperties = new()
            {
                AssignedToken = AssignedTokenName,
            },
            ConversationProperties = properties,
            NameInDatabase = Name,
            StoryFragmentProperties = new()
            {
                IsDone = false,
                OnStoryFragmentBeginInstruction = string.Empty,
                OnStoryFragmentEndInstruction = string.Empty,
            },
            Type = Type,
        };
        return data;
    }
}
