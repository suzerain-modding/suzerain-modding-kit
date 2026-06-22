using System.Globalization;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal static class ConversationInjector
{
    private static readonly HashSet<string> _conversationsPatched = [];

    private static bool CompareLink(Link link, Link other)
    {
        return link.destinationDialogueID == other.destinationDialogueID
            && link.destinationConversationID == other.destinationConversationID;
    }
    private static bool CompareLink(Link link, DialogueEntry other)
    {
        return link.destinationDialogueID == other.id
            && link.destinationConversationID == other.conversationID;
    }

    private static bool CompareEntry(DialogueEntry entry, DialogueEntry other)
    {
        return entry.id == other.id && entry.conversationID == other.conversationID;
    }

    private static List<DialogueEntry> FindFinalNodes(
        DialogueEntry originEntry,
        IEnumerable<Link> parentLinks)
    {
        Stack<DialogueEntry> stack = new();
        stack.Push(originEntry);
        List<DialogueEntry> finalNodes = [];
        List<Link> visited = [.. parentLinks];

        while (stack.Count > 0)
        {
            DialogueEntry entry = stack.Pop();
            if (entry.outgoingLinks.Count == 0)
            {
                finalNodes.Add(entry);
                continue;
            }
            foreach (Link link in entry.outgoingLinks)
            {
                // Check if we've already visited a link with the same destination.
                bool linkExists(Link other)
                {
                    return CompareLink(link, other);
                }
                if (visited.Exists(linkExists))
                {
                    // Ignore circular references.
                    continue;
                }
                visited.Add(link);

                // Get the destination entry.
                DialogueConversation conversation =
                    DialogueUtils.GetConversation(link.destinationConversationID);
                DialogueEntry nextEntry = conversation?.GetDialogueEntry(link.destinationDialogueID);
                if (nextEntry == null)
                {
                    // Ignore null references.
                    continue;
                }
                stack.Push(nextEntry);
            }
        }

        return finalNodes;
    }

    private static void HookNode(ResolvedConversationNodeHook resolvedHook)
    {
        ConversationNodeHook hook = resolvedHook.Hook;
        InjectedConversationNode node = resolvedHook.Node;
        DialogueEntry parent = resolvedHook.ResolvedParent;

        Link link = new(
            parent.conversationID, parent.id,
            node.Conversation.id, node.Entry.id)
        {
            priority = hook.ConditionPriority,
        };

        // The 'End' function ends the conversation (as you might've guessed) and is included
        // in all final nodes in the vanilla dialogues. Remove 'End' calls from the parent
        // so the new nodes will play.
        parent.userScript = parent.userScript
            ?.Replace("End()", string.Empty, StringComparison.Ordinal);
        // 'End' seems to not actually be required to end a conversation since Dialogue System
        // does it automatically if the last played node has no outgoing links, so we don't
        // need to add 'End' to our custom nodes.

        // ConditionGated: Choose the first (sorted by priority) outgoing link with a successful
        // condition. For choices, all with successful conditions will show.
        // This is the default behavior of Dialogue System, so just add it to the
        // outgoingLinks and let Dialogue System handle it.
        if (hook.Mode == ConversationNodeHook.HookMode.ConditionGated)
        {
            parent.outgoingLinks.Add(link);
            return;
        }

        // Override: Delete all other outgoing links and add this one.
        if (hook.Mode == ConversationNodeHook.HookMode.Override)
        {
            parent.outgoingLinks.Clear();
            parent.outgoingLinks.Add(link);
            return;
        }

        // Split: Break the chain at this point and insert this node in-between.

        List<DialogueEntry> finalNodes = FindFinalNodes(
            node.Entry,
            Il2CppUtils.ListFromIl2CppList(parent.outgoingLinks));

        // Copy the parent's outgoing links to each final entry.
        foreach (DialogueEntry finalEntry in finalNodes)
        {
            foreach (Link nextLink in parent.outgoingLinks)
            {
                finalEntry.outgoingLinks.Add(new Link(
                    finalEntry.conversationID, finalEntry.id,
                    nextLink.destinationConversationID, nextLink.destinationDialogueID));
            }
        }

        // Clear the parent's links and add this one, which should eventually link back
        // to the parent's original links.
        parent.outgoingLinks.Clear();
        parent.outgoingLinks.Add(link);
    }

    private static void CreateNodeOutgoingLinks(InjectedConversationNode node)
    {
        for (int i = 0; i < node.Node.NextNodes.Count; i++)
        {
            ConversationNodeSelector selector = node.Node.NextNodes[i];
            DialogueEntry entry = selector.Resolve(node.Conversation);
            if (entry == null)
            {
                Melon<Core>.Logger.Warning("Failed to resolve next node " +
                    $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                    $"'{node.Node.Name}'.");
                continue;
            }

            Func<Link, bool> exists = (link) => CompareLink(link, entry);
            bool linkExists = node.Entry.outgoingLinks.Exists(exists);
            if (linkExists)
            {
                Melon<Core>.Logger.Warning("Found duplicate outgoing links " +
                    $"from conversation node '{node.Node.Name}'.");
                continue;
            }

            Link nextLink = new(
                node.Conversation.id, node.Entry.id,
                entry.conversationID, entry.id);
            node.Entry.outgoingLinks.Add(nextLink);
        }
    }

    private static List<ResolvedConversationNodeHook> ResolveHooks(
        IEnumerable<InjectedConversationNode> nodes)
    {
        List<ResolvedConversationNodeHook> resolvedHooks = [];
        foreach (InjectedConversationNode node in nodes)
        {
            List<DialogueEntry> resolvedParents = [];

            for (int i = 0; i < node.Node.Hooks.Count; i++)
            {
                ConversationNodeHook hook = node.Node.Hooks[i];
                ConversationNodeSelector selector = hook.Selector;
                DialogueEntry parentEntry = selector.Resolve(node.Conversation);
                if (parentEntry == null)
                {
                    Melon<Core>.Logger.Warning("Failed to resolve hook " +
                        $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                        $"'{node.Node.Name}'.");
                    continue;
                }

                // Check if this node hooks to the same parent multiple times.
                bool parentExists(DialogueEntry entry)
                {
                    return CompareEntry(entry, parentEntry);
                }
                if (resolvedParents.Exists(parentExists))
                {
                    Melon<Core>.Logger.Warning($"Conversation node '{node.Node.Name}' has " +
                        "duplicate hooks.");
                    continue;
                }
                resolvedParents.Add(parentEntry);

                // Check if the parent already has a link to this node.
                Func<Link, bool> linkExists = (link) => CompareLink(link, node.Entry);
                bool doesLinkExist = parentEntry.outgoingLinks.Exists(linkExists);
                if (doesLinkExist)
                {
                    Melon<Core>.Logger.Warning("Found duplicate incoming links " +
                        $"to conversation node '{node.Node.Name}'.");
                    continue;
                }

                ResolvedConversationNodeHook resolved = new(hook, node, parentEntry);
                resolvedHooks.Add(resolved);
            }
        }
        return resolvedHooks;
    }

    private static void LinkInjectedNodes(IEnumerable<InjectedConversationNode> nodes)
    {
        // Create the outgoing links first. All the outgoing links have to be created before
        // we can create hooks.
        foreach (InjectedConversationNode node in nodes)
        {
            CreateNodeOutgoingLinks(node);
        }

        // Resolve and create hooks.
        List<ResolvedConversationNodeHook> resolvedHooks = ResolveHooks(nodes);
        resolvedHooks = [.. resolvedHooks.OrderBy(h => h.Hook.Priority)];
        foreach (ResolvedConversationNodeHook hook in resolvedHooks)
        {
            HookNode(hook);
        }
    }

    /// <summary>
    /// Injects a node into a conversation.
    /// </summary>
    /// <param name="node">
    /// The node to inject.
    /// </param>
    /// <param name="conversation">
    /// The target conversation.
    /// </param>
    /// <returns>
    /// An InjectNodeResult.
    /// </returns>
    private static InjectNodeResult InjectNode(
        ConversationNode node,
        DialogueConversation conversation)
    {
        int? speakerID = null;
        if (node.SpeakerSelector == null)
        {
            if (!node.IsOverride)
            {
                // Default to the player as the speaker if speakerSelector is null
                // and the node is not an override.
                // conversation.ActorID is the player actor.
                speakerID = conversation.ActorID;
            }
        }
        else
        {
            speakerID = node.SpeakerSelector.Resolve();
            if (speakerID == null)
            {
                Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                    "Speaker character could not be resolved.");
                return new(success: false);
            }
        }

        DialogueEntry newEntry;
        if (node.IsOverride)
        {
            newEntry = node.OverrideTarget.Resolve(conversation);
            if (newEntry == null)
            {
                Melon<Core>.Logger.Error("Failed to inject conversation node override " +
                    $"'{node.Name}': Target could not be resolved.");
                return new(success: false);
            }
        }
        else
        {
            Template template = Template.FromDefault();
            int newID = template.GetNextDialogueEntryID(conversation);

            newEntry = template.CreateDialogueEntry(newID, conversation.id, node.Name);
            string articyID = ArticyIDGenerator.GenerateArticyID(node.Name);
            newEntry.SetTextField("Articy Id", articyID);
            newEntry.SetTextField("SuzerainModdingKit.NodeName", node.Name);
        }

        // Note that 'newEntry' references an existing entry if 'node' is an override.
        if (node.Text != null)
        {
            newEntry.currentLocalizedDialogueText = node.Text;
        }
        if (node.LuaScript != null)
        {
            newEntry.userScript = node.LuaScript;
        }
        if (node.LuaCondition != null)
        {
            newEntry.conditionsString = node.LuaCondition;
        }
        if (node.Sequence != null)
        {
            newEntry.currentLocalizedSequence = node.Sequence;
        }
        if (node.MenuText != null)
        {
            newEntry.currentLocalizedMenuText = node.MenuText;
        }

        // Actor = The person speaking the line.
        // Conversant = The person listening to the line.
        newEntry.ActorID = speakerID ?? newEntry.ActorID;
        newEntry.ConversantID = conversation.ActorID;

        // Early return before adding to dialogueEntries because the node already exists there.
        if (node.IsOverride)
        {
            return new(success: true);
        }

        conversation.dialogueEntries.Add(newEntry);
        InjectedConversationNode injectedNode = new(node, newEntry, conversation);
        return new(success: true, injectedNode);
    }

    public static void PatchConversation(DialogueConversation conversation)
    {
        if (_conversationsPatched.Contains(conversation.Title, StringComparer.Ordinal))
        {
            return;
        }
        _conversationsPatched.Add(conversation.Title);

        Melon<Core>.Logger.Msg($"Patching conversation '{conversation.Title}'.");

        List<InjectedConversationNode> injectedNodes = [];
        int overrideCount = 0;
        foreach (ConversationInjection injection in ConversationRegistry.Injections)
        {
            if (!injection.ConversationTitle.Equals(conversation.Title, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (ConversationNode node in injection.Nodes)
            {
                InjectNodeResult result = InjectNode(node, conversation);
                if (result.InjectedNode != null)
                {
                    injectedNodes.Add(result.InjectedNode);
                }
                else if (result.Success)
                {
                    // The operation was successful but InjectedNode is null, so it must have been
                    // an override.
                    overrideCount++;
                }
            }
        }
        Melon<Core>.Logger.Msg(string.Create(CultureInfo.InvariantCulture,
            $"Injected {injectedNodes.Count} nodes and {overrideCount} overrides successfully."));

        if (injectedNodes.Count > 0)
        {
            LinkInjectedNodes(injectedNodes);
        }

        Melon<Core>.Logger.Msg($"Patched conversation '{conversation.Title}'.");
    }

    /// <summary>
    /// The result of the 'InjectNode' method.
    /// </summary>
    private sealed class InjectNodeResult
    {
        /// <summary>
        /// Was it successful?
        /// </summary>
        public bool Success
        {
            get;
        }
        /// <summary>
        /// The injected node. Null if the node is an override.
        /// </summary>
        public InjectedConversationNode InjectedNode
        {
            get;
        }

        public InjectNodeResult(bool success, InjectedConversationNode injectedNode = null)
        {
            Success = success;
            InjectedNode = injectedNode;
        }
    }
}
