using Il2Cpp;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.StoryPack;

/// <summary>
/// Read-only representation of a story pack.
/// </summary>
/// <seealso cref="SuzerainStoryPackInfo"/>
public class StoryPackInfo
{
    /// <summary>
    /// The name of the app bundle (eg. "AppBundle_Main", "AppBundle_Rizia").
    /// </summary>
    public string AppBundleName
    {
        get;
    }
    /// <summary>
    /// The name of the story pack (eg. "StoryPack_Main", "StoryPack_Rizia").
    /// </summary>
    public string StoryPackName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="appBundleName">
    /// The name of the app bundle (eg. "AppBundle_Main", "AppBundle_Rizia").
    /// </param>
    /// <param name="storyPackName">
    /// The name of the story pack (eg. "StoryPack_Main", "StoryPack_Rizia").
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public StoryPackInfo(string appBundleName, string storyPackName)
    {
        AppBundleName = appBundleName ?? throw new ArgumentNullException(nameof(appBundleName));
        StoryPackName = storyPackName ?? throw new ArgumentNullException(nameof(storyPackName));
    }

    internal AppBundleProperties ToAppBundleProperties()
    {
        return new AppBundleProperties()
        {
            AppBundle = AppBundleName,
            StoryPacks = Il2CppUtils.CreateIl2CppList([StoryPackName]),
        };
    }
}
