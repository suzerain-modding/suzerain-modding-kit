using Il2CppPixelCrushers.DialogueSystem;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

public class ConversationStartNodeSelector : ConversationNodeSelector
{
    /// <summary>
    /// Optional: The name of the conversation to search for the node in. If null, searches
    /// the current conversation.
    /// </summary>
    public string ConversationName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="conversationName">
    /// Optional: The name of the conversation to search for the node in. If null, searches
    /// the current conversation.
    /// </param>
    public ConversationStartNodeSelector(string conversationName = null)
    {
        ConversationName = conversationName;
    }

    public override DialogueEntry Resolve(DialogueConversation currentConversation)
    {
        DialogueConversation conversation =
            DialogueUtils.GetConversation(ConversationName) ?? currentConversation;

        return conversation.GetDialogueEntry(0);
    }
}
