using System.Globalization;
using Il2Cpp;

namespace SuzerainModdingKit.Journal;

public static class JournalManager
{
    public static void AddCustomJournalEntry(CustomJournalEntryData entry)
    {
        GameState.ThrowIfInactive();

        JournalDecisionsPage decisionsPage = Panels.Instance.JournalPanel.journalDecisionsPage;
        int index = decisionsPage.instantiatedJournalTurns.Count - entry.TurnNum;
        if (index < 0)
        {
            throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture,
                $"An instantiated journal turn does not exist for turn {entry.TurnNum}."));
        }

        TemplateJournalTurn journalTurn = decisionsPage.instantiatedJournalTurns[index];
        journalTurn.CreateJournalEntry(entry.ToSuzerainJournalEntryData());
    }
}
