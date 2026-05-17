using Il2Cpp;
using SuzerainModdingKit.StoryPack;

namespace SuzerainModdingKit.Report;

/// <summary>
/// Represents the data required to define a custom report for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomReport"/>
public class CustomReportData
{
    /// <summary>
    /// The unique identifier of this report.
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// The story pack that this report should appear in.
    /// </summary>
    public StoryPackInfo StoryPack
    {
        get;
    }
    /// <summary>
    /// The name of the token this report should appear on (eg. "Sordland_City_Holsord").
    /// </summary>
    public string AssignedTokenName
    {
        get;
    }
    /// <summary>
    /// The title of this report.
    /// </summary>
    public string Title
    {
        get;
    }
    /// <summary>
    /// The description of this report.
    /// </summary>
    public string Description
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of this report.
    /// </param>
    /// <param name="storyPack">
    /// The story pack that this report should appear in.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this report should appear on (eg. "Sordland_City_Holsord").
    /// </param>
    /// <param name="title">
    /// The title of this report.
    /// </param>
    /// <param name="description">
    /// The description of this report.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public CustomReportData(
        string name,
        StoryPackInfo storyPack,
        string assignedTokenName,
        string title,
        string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        StoryPack = storyPack ?? throw new ArgumentNullException(nameof(storyPack));
        AssignedTokenName = assignedTokenName ??
            throw new ArgumentNullException(nameof(assignedTokenName));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    internal ReportData RegisterInSuzerain(int turnNum)
    {
        ReportData data = ToSuzerainReportData(turnNum);
        Func<ReportData, bool> match = d => string.Equals(
            d.NameInDatabase,
            data.NameInDatabase,
            StringComparison.Ordinal);
        bool existsInRegistry = EntityDataManager.ReportsData.Exists(match);
        if (!existsInRegistry)
        {
            EntityDataManager.ReportsData.Add(data);
        }
        return data;
    }

    private ReportData ToSuzerainReportData(int turnNum)
    {
        ReportProperties properties = new()
        {
            Title = Title,
            Description = Description,
            TurnNo = turnNum,
            IsEnabledVariable = string.Empty,
        };
        ReportData data = new()
        {
            AppBundleProperties = StoryPack.ToAppBundleProperties(),
            AssignedTokenProperties = new()
            {
                AssignedToken = AssignedTokenName,
            },
            NameInDatabase = Name,
            ReportProperties = properties,
        };
        return data;
    }
}
