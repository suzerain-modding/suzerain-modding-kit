using Il2Cpp;
using SuzerainModdingKit.StoryPack;
using SuzerainModdingKit.VanillaData;

namespace SuzerainModdingKit.StoryFragments.Bill;

/// <summary>
/// Represents the data required to define a custom bill story fragment for injection
/// into the game.
/// </summary>
/// <seealso cref="GameState.AddCustomStoryFragment"/>
public class CustomBillFragment : CustomStoryFragmentData
{
    /// <summary>
    /// The full title of this bill shown in the bill panel.
    /// </summary>
    public string Title
    {
        get;
    }
    /// <summary>
    /// The full description of this bill shown in the bill panel.
    /// </summary>
    public string Description
    {
        get;
    }
    /// <summary>
    /// The short title of this bill shown under the assigned token.
    /// </summary>
    public string HubTitle
    {
        get;
    }
    /// <summary>
    /// The short description of this bill shown under the assigned token.
    /// </summary>
    public string HubDescription
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of this story fragment.
    /// </param>
    /// <param name="storyPack">
    /// The story pack that this story fragment should appear in.
    /// See <c cref="SuzerainStoryPackInfo">SuzerainStoryPackInfo</c>.
    /// </param>
    /// <param name="assignedTokenName">
    /// The name of the token this story fragment should appear on.
    /// See <c cref="SuzerainTokenName">SuzerainTokenName</c>.
    /// </param>
    /// <param name="title">
    /// The full title of this bill shown in the bill panel.
    /// </param>
    /// <param name="description">
    /// The full description of this bill shown in the bill panel.
    /// </param>
    /// <param name="hubTitle">
    /// The short title of this bill shown under the assigned token.
    /// </param>
    /// <param name="hubDescription">
    /// The short description of this bill shown under the assigned token.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public CustomBillFragment(
        string name,
        StoryPackInfo storyPack,
        string assignedTokenName,
        string title,
        string description,
        string hubTitle,
        string hubDescription) : base(name, storyPack, assignedTokenName)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        HubTitle = hubTitle ?? throw new ArgumentNullException(nameof(hubTitle));
        HubDescription = hubDescription ?? throw new ArgumentNullException(nameof(hubDescription));
    }

    internal override BillData RegisterInSuzerain(AddStoryFragmentOptions options)
    {
        bool? shouldDisableVeto = null;
        if (options is AddBillOptions o)
        {
            shouldDisableVeto = o.ShouldDisableVeto;
        }

        BillData data = ToSuzerainStoryFragmentData(shouldDisableVeto);
        Func<BillData, bool> match = d => string.Equals(
            d.NameInDatabase,
            data.NameInDatabase,
            StringComparison.Ordinal);
        bool existsInRegistry = EntityDataManager.AllBillsData.Exists(match);
        if (!existsInRegistry)
        {
            EntityDataManager.AllBillsData.Add(data);
        }
        return data;
    }

    private string GetVetoDisabledCondition(bool? shouldDisable)
    {
        return shouldDisable != null
            ? (bool)shouldDisable ? "true" : "false"
            : StoryPack.StoryPackName switch
            {
                SuzerainStoryPackInfo.SordlandStoryPackName =>
                    "BaseGame.Policy_Law_Veto_Removed == true",
                SuzerainStoryPackInfo.RiziaStoryPackName =>
                    "RiziaDLC.Reform_Monarch_Veto == false",
                _ => "false",
            };
    }

    private BillData ToSuzerainStoryFragmentData(bool? shouldDisableVeto)
    {
        BillProperties properties = new()
        {
            Title = Title,
            Description = Description,
            HubTitle = HubTitle,
            HubDescription = HubDescription,
            IsVetoDisabledCondition = GetVetoDisabledCondition(shouldDisableVeto),
            // The game crashes if these properties are not defined.
            SignVariables = string.Empty,
            VetoVariables = string.Empty,
        };
        BillData data = new()
        {
            AppBundleProperties = StoryPack.ToAppBundleProperties(),
            AssignedTokenProperties = new()
            {
                AssignedToken = AssignedTokenName,
            },
            BillProperties = properties,
            NameInDatabase = Name,
            StoryFragmentProperties = new()
            {
                IsDone = false,
                OnStoryFragmentBeginInstruction = string.Empty,
                OnStoryFragmentEndInstruction = string.Empty,
            },
        };
        return data;
    }
}
