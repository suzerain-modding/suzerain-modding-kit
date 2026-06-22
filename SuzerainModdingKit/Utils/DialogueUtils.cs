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

    /// <summary>
    /// Creates a conversation with only a START node.
    /// </summary>
    /// <param name="name">
    /// The name of the conversation.
    /// </param>
    /// <returns>
    /// The new conversation, the existing conversation if it already exists,
    /// or null if it couldn't be added.
    /// </returns>
    public static DialogueConversation CreateConversation(string name)
    {
        DialogueDatabase db = DialogueManager.MasterDatabase;
        if (db == null)
        {
            return null;
        }
        DialogueConversation existing = GetConversation(name);
        if (existing != null)
        {
            return existing;
        }

        DialogueConversation templateConv = db.conversations[0];
        Template template = Template.FromDefault();
        int id = template.GetNextConversationID(db);

        DialogueConversation conversation = template.CreateConversation(id, name);
        conversation.ActorID = templateConv.ActorID;
        conversation.ConversantID = templateConv.ConversantID;

        DialogueEntry startNode = template.CreateDialogueEntry(0, id, "START");
        conversation.dialogueEntries.Add(startNode);

        db.conversations.Add(conversation);

        return conversation;
    }
}
