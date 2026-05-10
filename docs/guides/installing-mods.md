# Installing Mods

A beginner's guide to installing mods for Suzerain **on Windows**.

You must own Suzerain **on Steam** and you must have it installed.

## Understand the Risks

Mods are third-party code that run directly on your computer with the same permissions as the game itself. This means a malicious mod can cause serious harm.

**Only ever install mods from sources you trust. Do not download or install anything you are unsure about.**

We only distribute Suzerain Modding Kit on the [official repository](https://github.com/suzerain-modding/suzerain-modding-kit). **Never download Suzerain Modding Kit from anywhere but the official repository.**

## Back Up Saves

If something goes wrong, your save files could get lost or corrupted. If you don't care about losing your saves, skip to the next section.

To back up your saves:

1. Press Win + R to open Run.
2. Type `%localappdata%low\Torpor Games\Suzerain` and press OK.
3. All saves are named `Autosave`, `FinalSave`, `ManualSave`, or `TurnSave` and then the date. Copy the ones you want to back up (or all of them) into another folder somewhere else on your computer. Don't forget where you put the back ups.
4. If you need to restore the saves, copy the back ups back into `%localappdata%low\Torpor Games\Suzerain`.

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
    2. Once you're in the main menu screen, press Ctrl + D. If Suzerain Modding Kit is working, you'll see a debug menu appear. Press Ctrl + D again or press the "Hide" button in the top right to close the debug menu.
    3. Quit the game.

## Install Mods

Remember to **only ever install mods from sources you trust!**

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

