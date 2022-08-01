//
// Copyright(C) 2005-2014 Simon Howard
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// DESCRIPTION:
//     Search for and locate an IWAD file, and initialize according
//     to the IWAD type.
//

#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <string.h>

#include "config.h"
#include "deh_str.h"
#include "doomkeys.h"
#include "d_iwad.h"
#include "i_system.h"
#include "m_argv.h"
#include "m_config.h"
#include "m_misc.h"
#include "w_wad.h"
#include "z_zone.h"

static const iwad_t iwads[] =
{
    { "doom2.wad",    doom2,     commercial, "Doom II" },
    { "plutonia.wad", pack_plut, commercial, "Final Doom: Plutonia Experiment" },
    { "tnt.wad",      pack_tnt,  commercial, "Final Doom: TNT: Evilution" },
    { "doom.wad",     doom,      retail,     "Doom" },
    { "DOOM1.WAD",    doom,      shareware,  "Doom Shareware" },
    { "chex.wad",     pack_chex, shareware,  "Chex Quest" },
    { "hacx.wad",     pack_hacx, commercial, "Hacx" },
    { "freedm.wad",   doom2,     commercial, "FreeDM" },
    { "freedoom2.wad", doom2,    commercial, "Freedoom: Phase 2" },
    { "freedoom1.wad", doom,     retail,     "Freedoom: Phase 1" },
    { "heretic.wad",  heretic,   retail,     "Heretic" },
    { "heretic1.wad", heretic,   shareware,  "Heretic Shareware" },
    { "hexen.wad",    hexen,     commercial, "Hexen" },
    //{ "strife0.wad",  strife,    commercial, "Strife" }, // haleyjd: STRIFE-FIXME
    { "strife1.wad",  strife,    commercial, "Strife" },
};

// Array of locations to search for IWAD files
//
// "128 IWAD search directories should be enough for anybody".

#define MAX_IWAD_DIRS 128

static boolean iwad_dirs_built = false;
static char *iwad_dirs[MAX_IWAD_DIRS];
static int num_iwad_dirs = 0;

static void AddIWADDir(char *dir)
{
    if (num_iwad_dirs < MAX_IWAD_DIRS)
    {
        iwad_dirs[num_iwad_dirs] = dir;
        ++num_iwad_dirs;
    }
}

// This is Windows-specific code that automatically finds the location
// of installed IWAD files.  The registry is inspected to find special
// keys installed by the Windows installers for various CD versions
// of Doom.  From these keys we can deduce where to find an IWAD.

#if defined(_WIN32) && !defined(_WIN32_WCE)

#define WIN32_LEAN_AND_MEAN
#include <windows.h>

typedef struct 
{
    HKEY root;
    char *path;
    char *value;
} registry_value_t;

#define UNINSTALLER_STRING "\\uninstl.exe /S "

// Keys installed by the various CD editions.  These are actually the 
// commands to invoke the uninstaller and look like this:
//
// C:\Program Files\Path\uninstl.exe /S C:\Program Files\Path
//
// With some munging we can find where Doom was installed.

// [AlexMax] From the persepctive of a 64-bit executable, 32-bit registry
// keys are located in a different spot.
#if _WIN64
#define SOFTWARE_KEY "Software\\Wow6432Node"
#else
#define SOFTWARE_KEY "Software"
#endif

static registry_value_t uninstall_values[] =
{
    // Ultimate Doom, CD version (Depths of Doom trilogy)

    {
        HKEY_LOCAL_MACHINE,
        SOFTWARE_KEY "\\Microsoft\\Windows\\CurrentVersion\\"
            "Uninstall\\Ultimate Doom for Windows 95",
        "UninstallString",
    },

    // Doom II, CD version (Depths of Doom trilogy)

    {
        HKEY_LOCAL_MACHINE,
        SOFTWARE_KEY "\\Microsoft\\Windows\\CurrentVersion\\"
            "Uninstall\\Doom II for Windows 95",
        "UninstallString",
    },

    // Final Doom

    {
        HKEY_LOCAL_MACHINE,
        SOFTWARE_KEY "\\Microsoft\\Windows\\CurrentVersion\\"
            "Uninstall\\Final Doom for Windows 95",
        "UninstallString",
    },

    // Shareware version

    {
        HKEY_LOCAL_MACHINE,
        SOFTWARE_KEY "\\Microsoft\\Windows\\CurrentVersion\\"
            "Uninstall\\Doom Shareware for Windows 95",
        "UninstallString",
    },
};

// Value installed by the Collector's Edition when it is installed

static registry_value_t collectors_edition_value =
{
    HKEY_LOCAL_MACHINE,
    SOFTWARE_KEY "\\Activision\\DOOM Collector's Edition\\v1.0",
    "INSTALLPATH",
};

// Subdirectories of the above install path, where IWADs are installed.

static char *collectors_edition_subdirs[] = 
{
    "Doom2",
    "Final Doom",
    "Ultimate Doom",
};

// Location where Steam is installed

static registry_value_t steam_install_location =
{
    HKEY_LOCAL_MACHINE,
    SOFTWARE_KEY "\\Valve\\Steam",
    "InstallPath",
};

// Subdirs of the steam install directory where IWADs are found

static char *steam_install_subdirs[] =
{
    "steamapps\\common\\doom 2\\base",
    "steamapps\\common\\final doom\\base",
    "steamapps\\common\\ultimate doom\\base",
    "steamapps\\common\\heretic shadow of the serpent riders\\base",
    "steamapps\\common\\hexen\\base",
    "steamapps\\common\\hexen deathkings of the dark citadel\\base",

    // From Doom 3: BFG Edition:

    "steamapps\\common\\DOOM 3 BFG Edition\\base\\wads",
};

#define STEAM_BFG_GUS_PATCHES \
    "steamapps\\common\\DOOM 3 BFG Edition\\base\\classicmusic\\instruments"

static char *GetRegistryString(registry_value_t *reg_val)
{
    HKEY key;
    DWORD len;
    DWORD valtype;
    char *result;

    // Open the key (directory where the value is stored)

    if (RegOpenKeyEx(reg_val->root, reg_val->path,
                     0, KEY_READ, &key) != ERROR_SUCCESS)
    {
        return NULL;
    }

    result = NULL;

    // Find the type and length of the string, and only accept strings.

    if (RegQueryValueEx(key, reg_val->value,
                        NULL, &valtype, NULL, &len) == ERROR_SUCCESS
     && valtype == REG_SZ)
    {
        // Allocate a buffer for the value and read the value

        result = kmalloc(len);

        if (RegQueryValueEx(key, reg_val->value, NULL, &valtype,
                            (unsigned char *) result, &len) != ERROR_SUCCESS)
        {
            kfree(result);
            result = NULL;
        }
    }

    // Close the key

    RegCloseKey(key);

    return result;
}

// Check for the uninstall strings from the CD versions

static void CheckUninstallStrings(void)
{
    unsigned int i;

    for (i=0; i<arrlen(uninstall_values); ++i)
    {
        char *val;
        char *path;
        char *unstr;

        val = GetRegistryString(&uninstall_values[i]);

        if (val == NULL)
        {
            continue;
        }

        unstr = mystrstr(val, UNINSTALLER_STRING);

        if (unstr == NULL)
        {
            kfree(val);
        }
        else
        {
            path = unstr + mystrlen(UNINSTALLER_STRING);

            AddIWADDir(path);
        }
    }
}

// Check for Doom: Collector's Edition

static void CheckCollectorsEdition(void)
{
    char *install_path;
    char *subpath;
    unsigned int i;

    install_path = GetRegistryString(&collectors_edition_value);

    if (install_path == NULL)
    {
        return;
    }

    for (i=0; i<arrlen(collectors_edition_subdirs); ++i)
    {
        subpath = M_StringJoin(install_path, DIR_SEPARATOR_S,
                               collectors_edition_subdirs[i], NULL);

        AddIWADDir(subpath);
    }

    kfree(install_path);
}


// Check for Doom downloaded via Steam

static void CheckSteamEdition(void)
{
    char *install_path;
    char *subpath;
    size_t i;

    install_path = GetRegistryString(&steam_install_location);

    if (install_path == NULL)
    {
        return;
    }

    for (i=0; i<arrlen(steam_install_subdirs); ++i)
    {
        subpath = M_StringJoin(install_path, DIR_SEPARATOR_S,
                               steam_install_subdirs[i], NULL);

        AddIWADDir(subpath);
    }

    kfree(install_path);
}

// The BFG edition ships with a full set of GUS patches. If we find them,
// we can autoconfigure to use them.

static void CheckSteamGUSPatches(void)
{
    const char *current_path;
    char *install_path;
    char *patch_path;
    int len;

    // Already configured? Don't stomp on the user's choices.
    current_path = M_GetStrVariable("gus_patch_path");
    if (current_path != NULL && mystrlen(current_path) > 0)
    {
        return;
    }

    install_path = GetRegistryString(&steam_install_location);

    if (install_path == NULL)
    {
        return;
    }

    len = mystrlen(install_path) + mystrlen(STEAM_BFG_GUS_PATCHES) + 20;
    patch_path = kmalloc(len);

    kfree(patch_path);
    kfree(install_path);
}

// Default install directories for DOS Doom

static void CheckDOSDefaults(void)
{
    // These are the default install directories used by the deice
    // installer program:

    AddIWADDir("\\doom2");              // Doom II
    AddIWADDir("\\plutonia");           // Final Doom
    AddIWADDir("\\tnt");
    AddIWADDir("\\doom_se");            // Ultimate Doom
    AddIWADDir("\\doom");               // Shareware / Registered Doom
    AddIWADDir("\\dooms");              // Shareware versions
    AddIWADDir("\\doomsw");

    AddIWADDir("\\heretic");            // Heretic
    AddIWADDir("\\hrtic_se");           // Heretic Shareware from Quake disc

    AddIWADDir("\\hexen");              // Hexen
    AddIWADDir("\\hexendk");            // Hexen Deathkings of the Dark Citadel

    AddIWADDir("\\strife");             // Strife
}

#endif

// Returns true if the specified path is a path to a file
// of the specified name.

static boolean DirIsFile(char *path, char *filename)
{
    size_t path_len;
    size_t filename_len;

    path_len = mystrlen(path);
    filename_len = mystrlen(filename);

    return path_len >= filename_len + 1
        && path[path_len - filename_len - 1] == DIR_SEPARATOR
        && !strcasecmp(&path[path_len - filename_len], filename);
}


// When given an IWAD with the '-iwad' parameter,
// attempt to identify it by its name.

static GameMission_t IdentifyIWADByName(char *name, int mask)
{
    size_t i;
    GameMission_t mission;
    char *p;

    p = mystrrchr(name, DIR_SEPARATOR);

    if (p != NULL)
    {
        name = p + 1;
    }

    mission = none;

    for (i=0; i<arrlen(iwads); ++i)
    {
        // Check if the filename is this IWAD name.

        // Only use supported missions:

        if (((1 << iwads[i].mission) & mask) == 0)
            continue;

        // Check if it ends in this IWAD name.

        if (!strcasecmp(name, iwads[i].name))
        {
            mission = iwads[i].mission;
            break;
        }
    }

    return mission;
}

#if ORIGCODE
//
// Add directories from the list in the DOOMWADPATH environment variable.
//

static void AddDoomWadPath(void)
{
    char *doomwadpath;
    char *p;

    // Check the DOOMWADPATH environment variable.

    doomwadpath = getenv("DOOMWADPATH");

    if (doomwadpath == NULL)
    {
        return;
    }

    doomwadpath = mystrdup(doomwadpath);

    // Add the initial directory

    AddIWADDir(doomwadpath);

    // Split into individual dirs within the list.

    p = doomwadpath;

    for (;;)
    {
        p = strchr(p, PATH_SEPARATOR);

        if (p != NULL)
        {
            // Break at the separator and store the right hand side
            // as another iwad dir
  
            *p = '\0';
            p += 1;

            AddIWADDir(p);
        }
        else
        {
            break;
        }
    }
}

#endif

//
// Build a list of IWAD files
//

static void BuildIWADDirList(void)
{
#if ORIGCODE
    char *doomwaddir;

    if (iwad_dirs_built)
    {
        return;
    }

    // Look in the current directory.  Doom always does this.

    AddIWADDir(".");

    // Add DOOMWADDIR if it is in the environment

    doomwaddir = getenv("DOOMWADDIR");

    if (doomwaddir != NULL)
    {
        AddIWADDir(doomwaddir);
    }        

    // Add dirs from DOOMWADPATH

    AddDoomWadPath();

#ifdef _WIN32

    // Search the registry and find where IWADs have been installed.

    CheckUninstallStrings();
    CheckCollectorsEdition();
    CheckSteamEdition();
    CheckDOSDefaults();

    // Check for GUS patches installed with the BFG edition!

    CheckSteamGUSPatches();

#else

    // Standard places where IWAD files are installed under Unix.

    AddIWADDir("/usr/share/games/doom");
    AddIWADDir("/usr/local/share/games/doom");

#endif
#else
    AddIWADDir (FILES_DIR);

    // Don't run this function again.

    iwad_dirs_built = true;
#endif
}


//
// Get the IWAD name used for savegames.
//

char *D_SaveGameIWADName(GameMission_t gamemission)
{
    size_t i;

    // Determine the IWAD name to use for savegames.
    // This determines the directory the savegame files get put into.
    //
    // Note that we match on gamemission rather than on IWAD name.
    // This ensures that doom1.wad and doom.wad saves are stored
    // in the same place.

    for (i=0; i<arrlen(iwads); ++i)
    {
        if (gamemission == iwads[i].mission)
        {
            return iwads[i].name;
        }
    }

    // Default fallback:

    return "unknown.wad";
}

char *D_SuggestIWADName(GameMission_t mission, GameMode_t mode)
{
    int i;

    for (i = 0; i < arrlen(iwads); ++i)
    {
        if (iwads[i].mission == mission && iwads[i].mode == mode)
        {
            return iwads[i].name;
        }
    }

    return "unknown.wad";
}

char *D_SuggestGameName(GameMission_t mission, GameMode_t mode)
{
    int i;

    for (i = 0; i < arrlen(iwads); ++i)
    {
        if (iwads[i].mission == mission
         && (mode == indetermined || iwads[i].mode == mode))
        {
            return iwads[i].description;
        }
    }

    return "Unknown game?";
}

