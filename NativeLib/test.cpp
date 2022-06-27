#include "..\LibC\printf.h"

void test()
{
	int i = 0x2000;
	printf("hello world %x\n", i);

	/*
	FILE* fp;
	fp = fopen("TEST1.TXT", "r");
	fseek(fp, 0, SEEK_SET);
	char buffer[20];
	fread(&buffer, 20, 1, fp);
	print(&buffer);
	print("\n");
	*/
}