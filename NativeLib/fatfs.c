#include "ff.h"
#include "..\LibC\printf.h"
#include <stdbool.h>
#include <intrin.h>

//The maximum file length of exFAT
#define INFO_NAME_LENGTH 255

#define MAX_ITEM_FOR_EACH_DIR 255
#define MAX_WORK_SIZE 16384

struct Info
{
	WCHAR Name[INFO_NAME_LENGTH];
	BYTE Attribute;
};

FATFS fs;           /* Filesystem object */
FIL fil;            /* File object */
FRESULT res;  /* API result code */
UINT bw;            /* Bytes written */
BYTE* work;
DIR dir;
FILINFO info;
DIR dir;                    // Directory
FILINFO fno;                // File Info

struct Info *infos;

void fatfs_init()
{
	work = malloc(MAX_WORK_SIZE);
	infos = malloc(sizeof(struct Info) * MAX_ITEM_FOR_EACH_DIR);

	res = f_mount(&fs, L"0:", 0);
	if (res != FR_OK)
	{
		printf("Can't mount the partition");
	}
}

void format_exfat()
{
	f_unmount(&fs, L"0:", 0);
	f_mkfs(L"0:", FS_EXFAT, 0, work, MAX_WORK_SIZE);
	f_mount(&fs, L"0:", 0);
	if (res != FR_OK)
	{
		printf("Can't mount the partition");
	}
}

struct Info* get_files(unsigned short* directory)
{
	res = f_opendir(&dir, directory);   // Open Root
	if (res != FR_OK)
	{
		printf("Can't open directory speficed error code: %d", res);
		while (true);
	}
	int i = 0;
	for(;;i++)
	{
		res = f_readdir(&dir, &fno);
		if (res != FR_OK || fno.fname[0] == 0) break;  /* Break on error or end of dir */
		memcpy(infos[i].Name, fno.fname, INFO_NAME_LENGTH);
		infos[i].Attribute = fno.fattrib;
	}
	infos[i].Name[0] = 0;

	f_closedir(&dir);

	return infos;
}

void write_all_bytes(TCHAR* filename, void* data,long filesize)
{
retry:
	res = f_open(&fil, filename, FA_CREATE_NEW | FA_WRITE | FA_READ);

	if (res != FR_OK)
	{
		if(res == FR_EXIST)
		{
			f_unlink(filename);
			goto retry;
		}
		printf("Can't open file speficed error code: %d", res);
		while (true);
	}

	UINT i;

	f_write(&fil, data, filesize, &i);

	f_close(&fil);
}

UINT read_all_bytes(TCHAR* filename,void** data)
{
	res = f_open(&fil, filename, FA_READ);

	if(res != FR_OK)
	{
		printf("Can't open file speficed error code: %d", res);
		while (true);
	}

	long filesize = f_size(&fil);
	void* buffer = malloc(filesize);

	*data = buffer;
	f_lseek(&fil, 0);
	UINT i;

	f_read(&fil, buffer, filesize, &i);

	f_close(&fil);

	return i;
}