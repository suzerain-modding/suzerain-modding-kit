namespace SuzerainModdingKit;

/// <summary>
/// Info about the current SMK build.
/// </summary>
public static class SmkInfo
{
    /// <summary>
    /// The 'era' component of the current SMK version.
    /// </summary>
    public const int VersionEra = 1;
    /// <summary>
    /// The 'release' component of the current SMK version.
    /// </summary>
    public const int VersionRelease = 3;
    /// <summary>
    /// The current SMK version as a string.
    /// </summary>
    public const string VersionStr = "1.3";
    /// <summary>
    /// The version of the mod.
    /// </summary>
    [Obsolete("Deprecated in v1.3. Use VersionEra, VersionRelease, and/or VersionStr instead.")]
    public static readonly Version ModVersion = new(1, 3);
    /// <summary>
    /// The expected Suzerain version.
    /// </summary>
    public const string TargetSuzerainVersion = "3.1.0.1.175";
}
