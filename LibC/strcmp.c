#include "../Kernel/MOOS.h"

int
strcmp_(const char* s1, const char* s2)
{
	while (*s1 == *s2++)
		if (*s1++ == '\0')
			return (0);
	return (*(const unsigned char*)s1 - *(const unsigned char*)(s2 - 1));
}