# Environment Setup (Windows)

A guide to setting up a Suzerain mod development environment on Windows.

This guide covers Windows-specific setup only. For the full mod development walkthrough including code examples, see [Developing Mods](developing-mods.md). For Linux setup, see [Environment Setup (Linux)](env-setup-linux.md).

## Setup

1. Follow the [Installing Mods](../user/installing-mods.md) guide to install MelonLoader and Suzerain Modding Kit.
2. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/) (Visual Studio, not Visual Studio Code).
    - In the Visual Studio Installer, ensure you also install the ".NET desktop development" workload.
3. Download the [MelonLoader VS Wizard](https://github.com/TrevTV/MelonLoader.VSWizard/releases) and run the `.vsix` file to install it as a Visual Studio extension.
4. Open Visual Studio and create a new project. Select "MelonLoader Mod" as the project template.
5. Fill in the project info and create. Visual Studio will open a file selector, navigate to and select `Suzerain.exe`.
    - The default install location is `C:\Program Files (x86)\Steam\steamapps\common\Suzerain\Suzerain.exe`.
6. Open the `.csproj` and add `<Reference Include="SuzerainModdingKit"><HintPath>$(GamePath)\Mods\SuzerainModdingKit.dll</HintPath></Reference>` where the other references are (usually in the first `ItemGroup`).
7. In Visual Studio, set the build platform to `x64`.
    1. Select Build > Configuration Manager.
    2. Open the active solution platform dropdown. If `x64` is already there, skip the next step. Otherwise, continue.
    3. Select new. Set the new platform to `x64`, copy settings from `Any CPU`, and enable "Create new project platforms." Select OK.
    4. Set the active solution platform to `x64`.
    5. Under project contexts, open the platform dropdown. Select `x64`.
    6. Close the configuration manager.
    7. Use your `Debug` configuration when building for development and `Release` when building to publish.
8. Recommended: Update `MelonInfo`.
    1. When creating the project, the auto generated `MelonInfo` will look something like this: `[assembly: MelonInfo(typeof(MyMod.Core), "MyMod", "1.0.0", "userprofilename", null)]`.
    2. Add spaces to the mod name (second argument) and change your user profile name (fourth argument) to your Nexus Mods account name.
    3. It should look like this now: `[assembly: MelonInfo(typeof(MyMod.Core), "My Mod", "1.0.0", "My User Name", null)]`.
9. Recommended: Set the `Core` class to `internal sealed`: `internal sealed class Core : MelonMod`.

