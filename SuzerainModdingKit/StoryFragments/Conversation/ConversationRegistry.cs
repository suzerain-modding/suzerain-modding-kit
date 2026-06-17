using System.Collections.ObjectModel;
using MelonLoader;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// Static interface for registering <c cref="ConversationInjection">ConversationInjections</c>
/// and new conversations.
/// </summary>
public static class ConversationRegistry
{
    private static readonly List<ConversationInjection> _injections = [];
    /// <summary>
    /// Read-only list of registered <c cref="ConversationInjection">ConversationInjections</c>.
    /// </summary>
    public static ReadOnlyCollection<ConversationInjection> Injections
    {
        get;
    } = new(_injections);

    internal static readonly HashSet<string> CustomConversations = [];

    /// <summary>
    /// If `true`, no new registrations will be accepted.
    /// </summary>
    public static bool IsRegistrationClosed
    {
        get; private set;
    }

    private static void ThrowIfClosed()
    {
        if (IsRegistrationClosed)
        {
            throw new InvalidOperationException("Registration is closed.");
        }
    }

    internal static void CloseRegistration()
    {
        IsRegistrationClosed = true;
    }

    /// <summary>
    /// Register a new <c cref="ConversationInjection">ConversationInjection</c>.
    /// </summary>
    /// <param name="injection">
    /// The <c cref="ConversationInjection">ConversationInjection</c> to register.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if registration is closed.
    /// </exception>
    /// <seealso cref="ConversationInjection.Register"/>
    public static void RegisterInjection(ConversationInjection injection)
    {
        ArgumentNullException.ThrowIfNull(injection);
        ThrowIfClosed();

        injection.Seal();
        _injections.Add(injection);
    }

    /// <summary>
    /// Register a new conversation.
    /// </summary>
    /// <remarks>
    /// This method creates a new conversation with only a START node. Use
    /// <c cref="ConversationInjection">ConversationInjection</c>s to add dialogue.
    /// Hook new dialogue to the START node using
    /// <c cref="ConversationStartNodeSelector">ConversationStartNodeSelector</c>.
    /// </remarks>
    /// <param name="name">
    /// The name of the conversation.
    /// </param>
    /// <returns>
    /// Returns a boolean indicating whether the conversation was registered successfully or not.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if registration is closed.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if any arguments are invalid.
    /// </exception>
    public static bool RegisterConversation(string name)
    {
        ThrowIfClosed();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(name));
        }

        if (CustomConversations.Contains(name, StringComparer.Ordinal))
        {
            Melon<Core>.Logger.Warning($"A custom conversation with the name '{name}' " +
                "has already been registered.");
            return false;
        }

        CustomConversations.Add(name);
        return true;
    }
}
