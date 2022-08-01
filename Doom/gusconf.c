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
//     GUS emulation code.
//
//     Actually emulating a GUS is far too much work; fortunately
//     GUS "emulation" already exists in the form of Timidity, which
//     supports GUS patch files. This code therefore converts Doom's
//     DMXGUS lump into an equivalent Timidity configuration file.
//


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#include "w_wad.h"
#include "z_zone.h"

#define MAX_INSTRUMENTS 256

typedef struct
{
    char *patch_names[MAX_INSTRUMENTS];
    int mapping[MAX_INSTRUMENTS];
} gus_config_t;

char *gus_patch_path = "";
unsigned int gus_ram_kb = 1024;

static unsigned int MappingIndex(void)
{
    unsigned int result = gus_ram_kb / 256;

    if (result < 1)
    {
        return 1;
    }
    else if (result > 4)
    {
        return 4;
    }
    else
    {
        return result;
    }
}

static int SplitLine(char *line, char **fields, unsigned int max_fields)
{
    unsigned int num_fields;
    char *p;

    fields[0] = line;
    num_fields = 1;

    for (p = line; *p != '\0'; ++p)
    {
        if (*p == ',')
        {
            *p = '\0';

            // Skip spaces following the comma.
            do
            {
                ++p;
            } while (*p != '\0' && myisspace(*p));

            fields[num_fields] = p;
            ++num_fields;
            --p;

            if (num_fields >= max_fields)
            {
                break;
            }
        }
        else if (*p == '#')
        {
            *p = '\0';
            break;
        }
    }

    // Strip off trailing whitespace from the end of the line.
    p = fields[num_fields - 1] + mystrlen(fields[num_fields - 1]);
    while (p > fields[num_fields - 1] && myisspace(*(p - 1)))
    {
        --p;
        *p = '\0';
    }

    return num_fields;
}

static void ParseLine(gus_config_t *config, char *line)
{
    char *fields[6];
    unsigned int num_fields;
    unsigned int instr_id, mapped_id;

    num_fields = SplitLine(line, fields, 6);

    if (num_fields < 6)
    {
        return;
    }

    instr_id = myatoi(fields[0]);
    mapped_id = myatoi(fields[MappingIndex()]);

    kfree(config->patch_names[instr_id]);
    config->patch_names[instr_id] = mystrdup(fields[5]);
    config->mapping[instr_id] = mapped_id;
}

static void ParseDMXConfig(char *dmxconf, gus_config_t *config)
{
    char *p, *newline;
    unsigned int i;

    memset(config, 0, sizeof(gus_config_t));

    for (i = 0; i < MAX_INSTRUMENTS; ++i)
    {
        config->mapping[i] = -1;
    }

    p = dmxconf;

    for (;;)
    {
        newline = strchr(p, '\n');

        if (newline != NULL)
        {
            *newline = '\0';
        }

        ParseLine(config, p);

        if (newline == NULL)
        {
            break;
        }
        else
        {
            p = newline + 1;
        }
    }
}

static void FreeDMXConfig(gus_config_t *config)
{
    unsigned int i;

    for (i = 0; i < MAX_INSTRUMENTS; ++i)
    {
        kfree(config->patch_names[i]);
    }
}

static char *ReadDMXConfig(void)
{
    int lumpnum;
    unsigned int len;
    char *data;

    // TODO: This should be chosen based on gamemode == commercial:

    lumpnum = W_CheckNumForName("DMXGUS");

    if (lumpnum < 0)
    {
        lumpnum = W_GetNumForName("DMXGUSC");
    }

    len = W_LumpLength(lumpnum);
    data = Z_Malloc(len + 1, PU_STATIC, NULL);
    W_ReadLump(lumpnum, data);

    return data;
}
