using Il2Cpp;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.StoryPack;

public class StoryPackInfo
{
    public string AppBundleName
    {
        get;
    }
    public string StoryPackName
    {
        get;
    }

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
