using Il2Cpp;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.StoryFragments.Bill;

/// <summary>
/// Represents the data required to define a custom bill story fragment for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomBill"/>
public class CustomBillData
{
    /// <summary>
    /// The unique identifier of the bill.
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// The name of the token this story fragment should appear on (eg. "Sordland_City_Holsord").
    /// </summary>
    public string AssignedTokenName
    {
        get;
    }
    /// <summary>
    /// The full title of the bill shown in the bill panel.
    /// </summary>
    public string Title
    {
        get;
    }
    /// <summary>
    /// The full description of the bill shown in the bill panel.
    /// </summary>
    public string Description
    {
        get;
    }
    /// <summary>
    /// The short title of the bill shown under the assigned token.
    /// </summary>
    public string HubTitle
    {
        get;
    }
    /// <summary>
    /// The short description of the bill shown under the assigned token.
    /// </summary>
    public string HubDescription
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of the bill.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this story fragment should appear on (eg. "Sordland_City_Holsord").
    /// </param>
    /// <param name="title">
    /// The full title of the bill shown in the bill panel.
    /// </param>
    /// <param name="description">
    /// The full description of the bill shown in the bill panel.
    /// </param>
    /// <param name="hubTitle">
    /// The short title of the bill shown under the assigned token.
    /// </param>
    /// <param name="hubDescription">
    /// The short description of the bill shown under the assigned token.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public CustomBillData(
        string name,
        string assignedTokenName,
        string title,
        string description,
        string hubTitle,
        string hubDescription)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AssignedTokenName = assignedTokenName ??
            throw new ArgumentNullException(nameof(assignedTokenName));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        HubTitle = hubTitle ?? throw new ArgumentNullException(nameof(hubTitle));
        HubDescription = hubDescription ?? throw new ArgumentNullException(nameof(hubDescription));
    }

    internal BillData ToSuzerainBillData()
    {
        BillProperties properties = new()
        {
            Title = Title,
            Description = Description,
            HubTitle = HubTitle,
            HubDescription = HubDescription,
            // The game crashes if these properties are not defined.
            SignVariables = string.Empty,
            VetoVariables = string.Empty,
        };
        BillData data = new()
        {
            AppBundleProperties = new AppBundleProperties()
            {
                AppBundle = "AppBundle_Main",
                StoryPacks = Il2CppUtils.CreateIl2CppList(["StoryPack_Main"]),
            },
            AssignedTokenProperties = new AssignedTokenProperties()
            {
                AssignedToken = AssignedTokenName,
            },
            BillProperties = properties,
            NameInDatabase = Name,
            Path = "Sordland/Bills",
            StoryFragmentProperties = new StoryFragmentProperties(),
            TagsProperties = new TagsProperties(),
        };
        return data;
    }
}
