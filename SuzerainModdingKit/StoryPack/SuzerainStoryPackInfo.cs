namespace SuzerainModdingKit.StoryPack;

/// <summary>
/// Static class containing <c cref="StoryPackInfo">StoryPackInfo</c> objects representing the
/// story packs in vanilla Suzerain.
/// </summary>
public static class SuzerainStoryPackInfo
{
    public const string SordlandStoryPackName = "StoryPack_Main";
    public static readonly StoryPackInfo Sordland = new("AppBundle_Main", SordlandStoryPackName);
    public const string RiziaStoryPackName = "StoryPack_Rizia";
    public static readonly StoryPackInfo Rizia = new("AppBundle_Rizia", RiziaStoryPackName);
}
