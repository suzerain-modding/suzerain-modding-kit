# Back Up Saves

If something goes wrong, your save files could get lost or corrupted.

## Windows

1. Press Win + R to open Run.
2. Type `%localappdata%low\Torpor Games\Suzerain` and press OK.
3. All saves are named `Autosave`, `FinalSave`, `ManualSave`, or `TurnSave` and then the date. Copy the ones you want to back up (or all of them) into another folder somewhere safe.
    1. Make sure you are only copying the save files.
    2. If you have played with mods before, also copy the `moddingkit` folder. This folder contains additional save data related to mods.
4. If you need to restore the saves, copy the back ups back into `%localappdata%low\Torpor Games\Suzerain`.

## Linux

### Find Your Save Directory

Your saves are stored at:

```
~/.local/share/Steam/steamapps/compatdata/1207650/pfx/drive_c/users/steamuser/AppData/LocalLow/Torpor Games/Suzerain/
```

> [!NOTE]
> If you installed Steam via Flatpak, the path begins with `~/.var/app/com.valvesoftware.Steam/.local/share/Steam/` instead of `~/.local/share/Steam/`.

### Back Up

1. Navigate to the save directory above.
2. All saves are named `Autosave`, `FinalSave`, `ManualSave`, or `TurnSave` followed by the date. Copy the ones you want to back up (or all of them) into another folder somewhere safe.
    1. Make sure you are only copying the save files.
    2. If you have played with mods before, also copy the `moddingkit` folder. This folder contains additional save data related to mods.
3. If you need to restore the saves, copy the back ups back into the save directory.

