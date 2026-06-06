using System.Globalization;
using System.Text;

namespace SuzerainModdingKit.Utils;

/*
 * Sequence Information:
 * Contributors: Add any new information discovered here.
 * 
 * Syntax Details:
 * Semicolons end statements.
 * Quotations not required for strings, even if they include spaces.
 * 
 * Functions:
 * AddConversant(string name) - Adds a conversant to the screen. Note that the speaker is
 *   automatically added so this is only necessary for extra characters.
 * RemoveConversant(string name) - Removes a conversant from the screen.
 * WaitForMessage(Continue) - Presumably waits for thse player to press continue, but this is the
 *   default behavior. Explicitly adding it does nothing.
 * Continue() - Automatically continues to the next line.
 * PlaySoundEffect(string name) - Plays a sound effect.
 * PlaySceneMusic(string name) - Stops currently playing music and plays a new song.
 * SetRichPresenceData(string description) - Untested but presumably sets Discord rich presence.
*/

public class ConversationNodeSequenceBuilder
{
    private readonly StringBuilder _sequence = new();

    public static bool IsValidInput(string input)
    {
        return !string.IsNullOrWhiteSpace(input) &&
            !input.Contains('(', StringComparison.InvariantCulture) &&
            !input.Contains(')', StringComparison.InvariantCulture) &&
            !input.Contains(';', StringComparison.InvariantCulture);
    }

    public ConversationNodeSequenceBuilder AddConversant(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"AddConversant({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder RemoveConversant(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"RemoveConversant({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder PlaySceneMusic(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"PlaySceneMusic({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder PlaySoundEffect(string name)
    {
        if (IsValidInput(name))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"PlaySoundEffect({name});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder SetDiscordRichPresence(string description)
    {
        if (IsValidInput(description))
        {
            _sequence.Append(CultureInfo.InvariantCulture, $"SetRichPresenceData({description});");
        }
        return this;
    }

    public ConversationNodeSequenceBuilder Continue()
    {
        _sequence.Append("Continue();");
        return this;
    }

    public override string ToString()
    {
        return _sequence.ToString();
    }
}
