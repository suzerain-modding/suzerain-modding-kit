namespace SuzerainModdingKit.StoryFragments.Bill;

/// <summary>
/// Options for adding a bill to the game using
/// <c cref="GameState.AddCustomStoryFragment"/></c>.
/// </summary>
public class AddBillOptions : AddStoryFragmentOptions
{
    /// <summary>
    /// Optional: Should the veto button be disabled for this bill?
    /// If null, the veto button will be conditional depending on the story pack.
    /// </summary>
    /// <remarks>
    /// In Sordland, the veto button is disabled if the reformed constitution removes the
    /// veto power from the president (BaseGame.Policy_Law_Veto_Removed == true).
    /// 
    /// In Rizia, the veto button is disabled if the player has reformed the country into a
    /// constitutional monarchy without veto power for the monarch
    /// (RiziaDLC.Reform_Monarch_Veto == false).
    /// </remarks>
    public bool? ShouldDisableVeto
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="shouldDisableVeto">
    /// Optional: Should the veto button be disabled for this bill?
    /// If null, the veto button will be conditional depending on the story pack.
    /// </param>
    public AddBillOptions(bool? shouldDisableVeto = null)
    {
        ShouldDisableVeto = shouldDisableVeto;
    }
}
