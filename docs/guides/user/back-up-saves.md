# Back Up Saves

If something goes wrong, your save files could get lost or corrupted.

Save files are named `Autosave`, `FinalSave`, `ManualSave`, or `TurnSave` followed by the date. To back them up, copy them to another folder somewhere else on your computer. If you have played with mods before, also copy the `moddingkit` folder — it contains additional mod-related save data. To restore, copy your backups back into the same folder.

The save folder location depends on your OS.

## Windows

1. Press Win + R to open Run.
2. Type `%localappdata%low\Torpor Games\Suzerain` and press OK.
3. Back up (or later restore) the files described above.

## Linux (Steam Proton)

Suzerain saves into its Proton prefix rather than the host filesystem.

1. Open a file manager and navigate to:
   `~/.steam/steam/steamapps/compatdata/1207650/pfx/drive_c/users/steamuser/AppData/LocalLow/Torpor Games/Suzerain`
   - `1207650` is Suzerain's Steam app ID.
   - On Debian/Ubuntu, replace `~/.steam/steam` with `~/.steam/debian-installation` if that's where your Steam library lives.
   - On Flatpak Steam, the path starts with `~/.var/app/com.valvesoftware.Steam/data/Steam/...` instead.
2. Back up (or later restore) the files described above.
