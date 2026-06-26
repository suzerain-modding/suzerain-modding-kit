using Il2Cpp;
using SuzerainModdingKit.Utils;
using SuzerainModdingKit.VanillaData;

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
    [Obsolete("Deprecated in v1.2. Suzerain no longer uses this data as of v3.1.0.1.175.")]
    public string AppBundleName
    {
        get;
    } = string.Empty;
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
    [Obsolete("Deprecated in v1.2. Use another overload.")]
    public StoryPackInfo(string appBundleName, string storyPackName)
    {
        AppBundleName = appBundleName ?? throw new ArgumentNullException(nameof(appBundleName));
        StoryPackName = storyPackName ?? throw new ArgumentNullException(nameof(storyPackName));
    }
    /// <summary>
    /// Creates a new instance of this object.
    /// </summary>
    /// <param name="storyPackName">
    /// The name of the story pack (eg. "StoryPack_Main", "StoryPack_Rizia").
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public StoryPackInfo(string storyPackName)
    {
        StoryPackName = storyPackName ?? throw new ArgumentNullException(nameof(storyPackName));
    }

    internal AppBundleProperties ToAppBundleProperties()
    {
        return new AppBundleProperties(string.Empty)
        {
            StoryPacks = Il2CppUtils.CreateIl2CppList([StoryPackName]),
        };
    }
}
