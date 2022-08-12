#include "../Kernel/MOOS.h"

long
strlen_(str)
const char* str;
{
	register const char* s;

	for (s = str; *s; ++s);
	return(s - str);
}