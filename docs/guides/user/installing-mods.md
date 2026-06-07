# Installing Mods

A beginner's guide to installing Suzerain mods on Windows or Linux.

You must own Suzerain on Steam. If you're on Linux, you must be running the game through Proton.

## Understand the Risks

Mods are third-party code that run directly on your computer with the same permissions as the game itself. This means a malicious mod can cause serious harm.

**Only ever install mods from sources you trust. Do not download or install anything you are unsure about.**

We only distribute Suzerain Modding Kit on the [official repository](https://github.com/suzerain-modding/suzerain-modding-kit) and the [official Nexus Mods page](https://www.nexusmods.com/suzerain/mods/7). **Do not download Suzerain Modding Kit from any other source.**

### Beta Disclaimer

Suzerain Modding Kit is currently in beta and should not be considered stable. Expect bugs and crashes.

## Back Up Saves

If something goes wrong, your save files could get lost or corrupted. See [Back Up Saves](back-up-saves.md) for back up instructions.

## Install MelonLoader (Windows)

First, install the following dependencies:

- [Microsoft Visual C++ 2015-2019 Redistributable 64 Bit](https://aka.ms/vs/16/release/vc_redist.x64.exe).
- [.NET Desktop Runtime 6.0.36](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.36-windows-x64-installer).

Next, install MelonLoader using the installer (recommended) or install it manually.

**Using the installer (recommended):**

1. Download [MelonLoader.Installer.exe](https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.exe).
2. Run the installer. It will list your installed Steam games. Select Suzerain and install.

**Install manually (I know what I am doing!):** See the [MelonLoader Wiki](https://melonwiki.xyz/#/README?id=manual-installation) for manual installation instructions.

## Install MelonLoader (Linux)

First, install MelonLoader using the installer or install it manually.

**Using the installer:**

1. Download [MelonLoader.Installer.Linux](https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.Linux).
2. Run the installer: `./MelonLoader.Installer.Linux`.
    - Grant executable permissions if neccessary: `chmod +x MelonLoader.Installer.Linux`.
3. The installer will list your installed Steam games. Select Suzerain and install.

**Installing manually:** See the [MelonLoader Wiki](https://melonwiki.xyz/#/README?id=manual-installation) for manual installation instructions.

Next, export the `WINEDLLOVERRIDES="version=n,b"` environment variable.

1. Select Suzerain in your Steam library.
2. Select the gear icon > Properties.
3. Under launch options, add `WINEDLLOVERRIDES="version=n,b"` before any other launch options.

Finally, install the Windows version of .NET 6 using protontricks.

1. Refer to the [protontricks installation guide](https://github.com/Matoking/protontricks#installation) to install for your distribution.
2. Run `protontricks 1207650 dotnetdesktop6`.

## Launch Suzerain

You must launch Suzerain after installing MelonLoader atleast once so MelonLoader can create the necessary files.

Launch Suzerain from Steam as normal. This will launch the MelonLoader console before launching the game. The first launch may take a while. Once you reach the Suzerain main menu screen, just exit the game.

If the game freezes when quitting, apply this fix:

1. Select Suzerain in your Steam library.
2. Select the gear icon > Properties.
3. Under launch options, add `--quitfix`.

## Install Suzerain Modding Kit

Suzerain Modding Kit is the modding API for Suzerain.

To install Suzerain Modding Kit:

1. Download the latest version from [Nexus Mods](https://www.nexusmods.com/suzerain/mods/7) or [GitHub Releases](https://github.com/suzerain-modding/suzerain-modding-kit/releases).
2. Launch Steam if it's not already open.
3. Select Suzerain in your library.
4. Select the gear icon > Manage > Browse local files.
5. Move the Suzerain Modding Kit DLL into the `Mods` folder.
6. Check if Suzerain Modding Kit is working:
    1. Launch Suzerain.
    2. Once you're in the main menu screen, press Ctrl + D. If Suzerain Modding Kit is working, you'll see a debug overlay appear. Press Ctrl + D again or press the "Hide" button in the top right to close the debug menu.
        - Don't worry if it says "GameFlowManager not loaded!" This just means that there is no loaded campaign, which is because you are in the main menu.
    3. Quit the game.

## Install Mods

Remember to **only ever install mods from sources you trust!**

**Where do I find mods?** The recommended site to find mods on is [Nexus Mods](https://www.nexusmods.com/games/suzerain). Alternatively, see the `#mod-promotions` channel on our [Discord server](https://discord.gg/za8eDBJ8TH).

If a mod provides its own installation instructions, follow them. Otherwise, follow the instructions below.

To install most mods:

1. Download the DLL and its required dependencies if it has any.
2. Launch Steam if it's not already open.
3. Select Suzerain in your library.
4. Select the gear icon > Manage > Browse local files.
5. Move the DLL(s) into the `Mods` folder.

## Disabling or Uninstalling Mods

See [Disabling Mods](disabling-mods.md) to learn how to disable mods without uninstalling them.

See [Uninstalling Mods](uninstalling-mods.md) to learn how to uninstall mods or uninstall MelonLoader entirely.
