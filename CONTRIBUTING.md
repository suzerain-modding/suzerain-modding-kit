# Contributing

This is a guide to contributing code to Suzerain Modding Kit.

## Coding Guidelines and Info

Please read and follow the [coding guidelines](CODING_GUIDELINES.md) when contributing to Suzerain Modding Kit.

Reading the [Suzerain Implementation Info](suzerain_implementation_info.md) document will also be helpful in understanding how Suzerain works.

## Basic Setup

This contributor workflow is verified on Windows with the Steam version of Suzerain.
The game assemblies are required for a full build.

1. Install the .NET SDK version specified in `global.json` (currently **10.0.402**).
   Visual Studio is optional for command-line builds. If using it, install a version that
   supports this SDK and the .NET desktop development workload.
2. Follow the instructions on the [MelonLoader Wiki](https://melonwiki.xyz/#/README) to download and install MelonLoader.
3. Install a .NET decompiler like [dotPeek](https://www.jetbrains.com/decompiler/) or similar program of your choice. Note that the assemblies that MelonLoader generates only contain the type names (class names, method names, etc), not the actual code. In most cases, this is sufficient. If actual code is required, see the [Decompiling with Ghidra](#decompiling-with-ghidra) section.
4. Launch Suzerain so MelonLoader can create the game assembly. Launching the game normally from Steam should launch the MelonLoader console before launching the game. Ensure everything runs successfully. Close the game before continuing.
5. Open `C:\Program Files (x86)\Steam\steamapps\common\Suzerain\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` in the decompiler to view Suzerain's types. Suzerain types are under the `Il2Cpp` namespace (the other namespaces are Suzerain's dependencies).
6. Fork and clone this repository and open it in Visual Studio.
7. Configure, check, and build the project as described below. Builds no longer copy files
   into the game's `Mods` directory. Stage or deploy explicitly when ready.

## Local Build Configuration

Copy `Directory.Build.local.props.example` to `Directory.Build.local.props` if your game is
not at the default Steam location. The local file is ignored by Git. Use absolute paths.

- `GamePath`: Suzerain installation used to resolve game and MelonLoader assemblies.
- `ArtifactsPath`: compiler output, intermediate files, and package assets; defaults to
  `artifacts/` in this checkout.
- `StagingPath`: defaults to `artifacts/staging/<Configuration>/<Platform>/`.
- `DeploymentPath`: defaults to `$(GamePath)/Mods`.
- `ExpectedGameVersion`: optional preflight override; otherwise SMK's target version is
  read from `SmkInfo.cs`.

MSBuild command-line properties take precedence over the local file, for example
`-p:GamePath="D:/SteamLibrary/steamapps/common/Suzerain"`.

## Preflight, Build, and Deployment

Run these commands from the repository root:

```powershell
pwsh -NoProfile -File scripts/preflight.ps1
dotnet build -c Debug -p:Platform=x64
dotnet build -c Release -p:Platform=x64
```

The SDK supplies the C# 12 compiler. The mod still targets **.NET 6**; keep the .NET 6 Desktop
Runtime required by MelonLoader installed. The SDK and game runtime are separate dependencies.

The preflight checks the evaluated configuration, SDK, game references, .NET 6 runtimes,
loader versions, startup-log versions, and writable artifact/staging directories. Startup
logs describe the last launch and cannot prove current interop assemblies are up to date.
Warnings are reported separately; missing dependencies return a nonzero exit code.

Preflight does not build, launch, or deploy. It leaves the game installation untouched by
default. Add `-CheckDeploymentAccess` only when you want it to create and remove a temporary
file in the deployment directory. It also accepts `-Configuration`, `-GamePath`,
`-ArtifactsPath`, `-StagingPath`, and `-DeploymentPath`.

Build output is under `artifacts/bin/SuzerainModdingKit/debug/` or `release/` by default;
preflight prints the exact DLL path. To collect only the mod DLL and available PDB in a
`Mods` subdirectory for manual installation:

```powershell
dotnet build -c Debug -p:Platform=x64 -t:Stage
```

Close Suzerain, then deploy explicitly:

```powershell
dotnet build -c Debug -p:Platform=x64 -t:Deploy
```

`Deploy` builds and stages before copying the DLL and available PDB to `DeploymentPath`.
It does not copy Unity, game, or loader dependencies from the compiler output.
Deployment can require additional filesystem permission. Restricted shells may not see
the game process; verify that the game is closed yourself. Launch it through Steam afterward
and inspect the MelonLoader log.

### Restricted Shells

If .NET first-run setup or workload checks are denied in a sandbox, use process-local settings:

```powershell
$env:DOTNET_CLI_HOME = Join-Path (Get-Location) 'artifacts/dotnet-home'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = '0'
$env:DOTNET_GENERATE_ASPNET_CERTIFICATE = 'false'
$env:MSBuildEnableWorkloadResolver = 'false'
```

The preflight applies these settings in its own process. Apply them in the build shell as
well if needed. NuGet package downloads and deployment can still require separate access.

## Decompiling with Ghidra

The standard decompiling method explained above only allows you to see type names. This should be sufficient in most cases, but we can also decompile the actual code using Ghidra.

See [this gist](https://gist.github.com/BadMagic100/47096cbcf64ec0509cf75d48cfbdaea5) which explains how to decompile IL2CPP games (like Suzerain) with Ghidra. **You must make the following changes to the guide to use it in newer versions of Ghidra:**

- The guide tells you to use OpenJDK 17, but newer versions of Ghidra require a different version. See [the latest requirements](https://github.com/NationalSecurityAgency/ghidra/blob/master/GhidraDocs/GettingStarted.md#minimum-requirements).
- Add `# @runtime Jython` to the first line of `ghidra_with_struct.py` before running it in Ghidra.
