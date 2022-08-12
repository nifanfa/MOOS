#include "../Kernel/MOOS.h"

#define NULL 0

char*
strdup_(str)
const char* str;
{
	long len;
	char* copy;

	len = strlen_(str) + 1;
	if (!(copy = kmalloc((unsigned int)len)))
		return (NULL);
	memcpy(copy, str, len);
	return (copy);
}