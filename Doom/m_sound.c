#include "config.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <fcntl.h>

#include "deh_str.h"
#include "i_sound.h"
#include "i_system.h"
#include "i_swap.h"
#include "m_argv.h"
#include "m_misc.h"
#include "w_wad.h"
#include "z_zone.h"

#include "doomtype.h"

// Needed for calling the actual sound output.
#define SAMPLECOUN 512
#define NUM_CHANNELS 8

static snddevice_t _sound_devices[] =
{
	SNDDEVICE_SB,
	SNDDEVICE_PAS,
	SNDDEVICE_GUS,
	SNDDEVICE_WAVEBLASTER,
	SNDDEVICE_SOUNDCANVAS,
	SNDDEVICE_AWE32,
};

struct snd_buffer
{
	int16_t* buf;
	int16_t* ptr;
	size_t len;
	boolean canPlay;
	int32_t sep;
	int32_t vol;
};

struct snd_buffer buf[NUM_CHANNELS] = { 0 };

static void GetSfxLumpName(sfxinfo_t* sfx, char* buf, size_t buf_len)
{
	if (sfx->link != NULL)
	{
		sfx = sfx->link;
	}
	DEH_snprintf(buf, buf_len, "ds%s", DEH_String(sfx->name));
}

static int I_GetSfxLumpNum(sfxinfo_t* sfx)
{
	char namebuf[9];
	GetSfxLumpName(sfx, namebuf, sizeof(namebuf));
	return W_GetNumForName(namebuf);
}

static void I_PrecacheSounds(sfxinfo_t* sounds, int num_sounds)
{
}

static void I_UpdateSoundParams(int handle, int vol, int sep)
{
}

static int I_StartSound(sfxinfo_t* sfxinfo, int channel, int vol, int sep)
{
	unsigned int lumpnum = sfxinfo->lumpnum;
	byte* data = W_CacheLumpNum(lumpnum, PU_STATIC);
	int samplerate = *(uint16_t*)(data + 2);
	int lumplen = W_LumpLength(lumpnum);
	unsigned int length = *(uint32_t*)(data + 4);

	if (lumplen < 8 || *(uint16_t*)data != 3)
	{
		return false;
	}
	if (length > lumplen - 8 || length <= 48)
	{
		return false;
	}

	data += 16;
	length -= 32;

	uint64_t expanded_length = ((uint64_t)length * 48000) / samplerate;
	int ratio = (length * 256) / expanded_length;
	int16_t* buffer = malloc(expanded_length * sizeof(int16_t));

	for (int i = 0; i < expanded_length; ++i)
	{
		int src = (i * ratio) / 256;
		int16_t sample = *(int16_t*)(data + src);
		sample -= 32768;
		buffer[i] = sample;
	}

	for (int i = 0; i < NUM_CHANNELS; ++i)
	{
		if (!buf[i].canPlay)
		{
			buf[i].canPlay = true;
			buf[i].len = expanded_length;
			buf[i].buf = buffer;
			buf[i].ptr = buffer;
			buf[i].sep = sep;
			buf[i].vol = vol;
			return i + 1;
		}
	}

	free(buffer);
	return false;
}

static void I_StopSound(int handle)
{
	if (handle >= 1 && handle <= NUM_CHANNELS)
	{
		buf[handle - 1].len = 0;
	}
}

static boolean I_QrySongPlaying(int handle)
{
	if (handle >= 1 && handle <= NUM_CHANNELS)
	{
		return (buf[handle - 1].canPlay);
	}
	return false;
}

static int16_t cache[SAMPLECOUN * 2];

static void I_UpdateSound(void)
{
	for (int i = 0; i < NUM_CHANNELS; ++i)
	{
		if (!buf[i].canPlay) continue;

		while (buf[i].len != 0)
		{
			int sample_count = buf[i].len > SAMPLECOUN ? SAMPLECOUN : buf[i].len;

			for (int j = 0; j < sample_count; ++j)
			{
				cache[j * 2] = buf[i].ptr[j] * (((254 - buf[i].sep) * buf[i].vol) / 127) / 256;
				cache[j * 2 + 1] = buf[i].ptr[j] * (((buf[i].sep) * buf[i].vol) / 127) / 256;
			}

			size_t bytes = snd_write(cache, sizeof(int16_t) * sample_count * 2);
			if (bytes <= 0) break;
			buf[i].ptr += bytes / 4;
			buf[i].len -= bytes / 4;
		}

		if (buf[i].len == 0)
		{
			buf[i].canPlay = false;
			free(buf[i].buf);
		}
	}
}

static void I_ShutdownSound(void)
{
}

static boolean _I_InitSound(boolean _use_sfx_prefix)
{
	return true;
}

sound_module_t _sound_module =
{
	_sound_devices,
	arrlen(_sound_devices),
	_I_InitSound,
	I_ShutdownSound,
	I_GetSfxLumpNum,
	I_UpdateSound,
	I_UpdateSoundParams,
	I_StartSound,
	I_StopSound,
	I_QrySongPlaying,
	I_PrecacheSounds,
};