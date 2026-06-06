using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.Utils;

internal static class DialogueUtils
{
    /// <summary>
    /// Gets a conversation from the master database.
    /// </summary>
    /// <param name="title">
    /// The title of the conversation.
    /// </param>
    /// <returns>
    /// The conversation if it exists, otherwise null.
    /// </returns>
    public static DialogueConversation GetConversation(string title)
    {
        return string.IsNullOrWhiteSpace(title)
            ? null
            : DialogueManager.MasterDatabase?.GetConversation(title);
    }
    /// <summary>
    /// Gets a conversation from the master database.
    /// </summary>
    /// <param name="id">
    /// The numerical ID of the conversation.
    /// </param>
    /// <returns>
    /// The conversation if it exists, otherwise null.
    /// </returns>
    public static DialogueConversation GetConversation(int id)
    {
        return DialogueManager.MasterDatabase?.GetConversation(id);
    }
}
