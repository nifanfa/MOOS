#include "ff.h"
#include "libcore.h"

FATFS fs;           /* Filesystem object */
FIL fil;            /* File object */
FRESULT res;  /* API result code */
UINT bw;            /* Bytes written */
BYTE* work;
DIR dir;
FILINFO info;

void fatfs_init()
{
	work = malloc(FF_MAX_SS);

	//res = f_mkfs(L"0:", FS_EXFAT, 0, work, FF_MAX_SS);

	res = f_mount(&fs, L"0:", 0);
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