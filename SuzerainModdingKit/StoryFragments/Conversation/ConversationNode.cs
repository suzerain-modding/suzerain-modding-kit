using System.Collections.ObjectModel;
using SuzerainModdingKit.Character;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// A dialogue line or choice in a conversation.
/// </summary>
public class ConversationNode
{
    /// <summary>
    /// The unique identifier of the node.
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// The text of the node. This may be null.
    /// </summary>
    public string Text
    {
        get;
    }
    /// <summary>
    /// The text shown in the options list. This may be null.
    /// </summary>
    /// <remarks>
    /// The text shown in the options list. Possibly null.
    /// If null, the text shown will be the same as <c cref="Text">Text</c>.
    /// If the node isn't a choice (spoken by the player), this property is ignored.
    /// </remarks>
    public string MenuText
    {
        get;
    }
    /// <summary>
    /// A read-only list of hooks. This is always an empty collection if the node is an override.
    /// </summary>
    /// <remarks>
    /// Hooks are nodes that this node should attach (or hook) to.
    /// </remarks>
    public ReadOnlyCollection<ConversationNodeHook> Hooks
    {
        get;
    }
    /// <summary>
    /// A read-only list of next node selectors. This is always an empty collection if the
    /// node is an override.
    /// </summary>
    /// <remarks>
    /// These are the nodes that will show after this one.
    /// Only the first node with a successful condition will show. If the nodes are choices,
    /// all choice nodes with successful conditions will show.
    /// </remarks>
    public ReadOnlyCollection<ConversationNodeSelector> NextNodes
    {
        get;
    }
    /// <summary>
    /// The speaker of the line. This property is optional. If null, defaults to the player.
    /// If the speaker is the player, it will show up as a choice.
    /// </summary>
    public CharacterSelector SpeakerSelector
    {
        get;
    }
    /// <summary>
    /// Optional: A Lua script to run when the dialogue is spoken.
    /// </summary>
    public string LuaScript
    {
        get;
    }
    /// <summary>
    /// Optional: A Lua condition to determine whether the dialogue should be spoken or not.
    /// </summary>
    public string LuaCondition
    {
        get;
    }
    /// <summary>
    /// Optional conversation-related actions to perform when this dialogue is spoken.
    /// </summary>
    /// <seealso cref="ConversationNodeSequenceBuilder"/>
    public string Sequence
    {
        get;
    }
    /// <summary>
    /// Optional: A selector targeting a node to override.
    /// </summary>
    public ConversationNodeSelector OverrideTarget
    {
        get;
    }
    /// <summary>
    /// Returns a boolean indicating whether the node is meant to override an existing node.
    /// (is <c cref="OverrideTarget">OverrideTarget</c> not null?)
    /// </summary>
    public bool IsOverride => OverrideTarget != null;

    /// <summary>
    /// Creates a new custom conversation node.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of the node.
    /// </param>
    /// <param name="text">
    /// Optional: The text of the node.
    /// </param>
    /// <param name="hooks">
    /// Optional: A list of hooks. Hooks are nodes that this node should attach (or hook) to.
    /// Hooks should only be used when trying to link to a node that you don't control
    /// (such as a node from vanilla Suzerain or another mod). If you control the node you are
    /// trying to hook to, you should add this node to the other node's
    /// <c cref="NextNodes">NextNodes</c> instead.
    /// </param>
    /// <param name="nextNodes">
    /// Optional: A list of next node selectors. These are the nodes that will show after this one.
    /// Only the first node with a successful condition will show. If the nodes are choices,
    /// all choice nodes with successful conditions will show.
    /// </param>
    /// <param name="speakerSelector">
    /// Optional: The speaker of the line. If null, defaults to the player.
    /// If the speaker is the player, it will show up as a choice.
    /// </param>
    /// <param name="luaScript">
    /// Optional: A Lua script to run when the dialogue is spoken.
    /// </param>
    /// <param name="luaCondition">
    /// Optional: A Lua condition to determine whether the dialogue should be spoken or not.
    /// </param>
    /// <param name="sequence">
    /// Optional: Conversation-related actions to perform when this dialogue is spoken.
    /// See <see cref="ConversationNodeSequenceBuilder"/>.
    /// </param>
    /// <param name="menuText">
    /// The text shown in the options list.
    /// Ignored if the node isn't a choice (spoken by the player).
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if any arguments are invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNode(
        string name,
        string text = null,
        IReadOnlyList<ConversationNodeHook> hooks = null,
        IReadOnlyList<ConversationNodeSelector> nextNodes = null,
        CharacterSelector speakerSelector = null,
        string luaScript = null,
        string luaCondition = null,
        string sequence = null,
        string menuText = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));
        }

        Name = name;
        Text = text;
        Hooks = new(hooks != null ? [.. hooks] : []);
        NextNodes = new(nextNodes != null ? [.. nextNodes] : []);
        SpeakerSelector = speakerSelector;
        LuaScript = luaScript;
        LuaCondition = luaCondition;
        Sequence = sequence;
        MenuText = menuText;
    }

    /// <summary>
    /// Creates a new conversation node to override an existing node.
    /// </summary>
    /// <param name="overrideTarget">
    /// A selector targeting the node to override.
    /// </param>
    /// <param name="name">
    /// The unique identifier of the node.
    /// </param>
    /// <param name="text">
    /// Optional: Override the text of the node.
    /// </param>
    /// <param name="speakerSelector">
    /// Optional: Override the speaker of the node.
    /// </param>
    /// <param name="luaScript">
    /// Optional: Override the script of the node.
    /// </param>
    /// <param name="luaCondition">
    /// Optional: Override the condition of the node.
    /// </param>
    /// <param name="sequence">
    /// Optional: Override the sequence of the node.
    /// </param>
    /// <param name="menuText">
    /// Optional: Override the text shown in the options list.
    /// Ignored if the node isn't a choice (spoken by the player).
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if any arguments are invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNode(
        ConversationNodeSelector overrideTarget,
        string name,
        string text = null,
        CharacterSelector speakerSelector = null,
        string luaScript = null,
        string luaCondition = null,
        string sequence = null,
        string menuText = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));
        }

        OverrideTarget = overrideTarget ?? throw new ArgumentNullException(nameof(overrideTarget));
        Name = name;
        Text = text;
        Hooks = new([]);
        NextNodes = new([]);
        SpeakerSelector = speakerSelector;
        LuaScript = luaScript;
        LuaCondition = luaCondition;
        Sequence = sequence;
        MenuText = menuText;
    }
}
