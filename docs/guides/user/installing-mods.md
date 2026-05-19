# Installing Mods

A beginner's guide to installing Suzerain mods **on Windows**.

You must own Suzerain **on Steam** and you must have it installed.

## Understand the Risks

Mods are third-party code that run directly on your computer with the same permissions as the game itself. This means a malicious mod can cause serious harm.

**Only ever install mods from sources you trust. Do not download or install anything you are unsure about.**

We only distribute Suzerain Modding Kit on the [official repository](https://github.com/suzerain-modding/suzerain-modding-kit) and the official Nexus Mods page (TODO: link nexus mods post). **Do not download Suzerain Modding Kit from any other source.**

### Beta Disclaimer

Suzerain Modding Kit is currently in beta and should not be considered stable. Expect bugs and crashes.

## Back Up Saves

If something goes wrong, your save files could get lost or corrupted. See [Back Up Saves](back-up-saves.md) for back up instructions.

## Install .NET and Visual C++ Redistributable

To run mods for Suzerain, you must have the following installed on your computer. The exact versions linked here are required.

- [Microsoft Visual C++ 2015-2019 Redistributable 64 Bit](https://aka.ms/vs/16/release/vc_redist.x64.exe).
- [.NET Desktop Runtime 6.0.36](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.36-windows-x64-installer).

## Install MelonLoader

MelonLoader is the program that will inject the mods into Suzerain.

Install [MelonLoader.Installer.exe](https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.exe). Launch the installer, then select Suzerain.

**I want to install MelonLoader manually (I know what I am doing!):** See the [MelonLoader Wiki](https://melonwiki.xyz/#/README?id=manual-installation) for manual installation instructions.

## Launch Suzerain

You must launch Suzerain after installing MelonLoader atleast once so MelonLoader can create the necessary files.

Launch Suzerain from Steam as normal. This will launch the MelonLoader terminal before launching the game. The first launch may take a while. Once you reach the Suzerain main menu screen, just exit the game.

## Install Suzerain Modding Kit

Suzerain Modding Kit is the modding API for Suzerain.

To install Suzerain Modding Kit:

1. Download the latest version (TODO: add download link).
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

**Where do I find mods?** The recommended site to find mods on is [Nexus Mods](https://www.nexusmods.com/games/suzerain). Alternatively, see the `#mod-promotions` channel on our Discord server (TODO: add discord links).

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

