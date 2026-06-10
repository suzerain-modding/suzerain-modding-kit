using Il2Cpp;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.Journal;

/// <summary>
/// Experimental: Use at your own risk. Only supports Sordland.
/// </summary>
public class CustomJournalEntryData
{
    public string Name
    {
        get;
    }
    public string Description
    {
        get;
    }
    public int TurnNum
    {
        get;
    }

    public CustomJournalEntryData(string name, string description, int turnNum)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        TurnNum = turnNum;
    }

    internal JournalEntryData ToSuzerainJournalEntryData()
    {
        JournalEntryProperties properties = new()
        {
            Description = $"[MOD] {Description}",
            TurnNo = TurnNum,
        };
        JournalEntryData data = new()
        {
            AppBundleProperties = new AppBundleProperties(string.Empty)
            {
                StoryPacks = Il2CppUtils.CreateIl2CppList(["StoryPack_Main"]),
            },
            JournalEntryProperties = properties,
            NameInDatabase = Name,
            Path = "Sordland/Journal Entries",
            TagsProperties = new TagsProperties(),
        };
        return data;
    }
}
