#include <intrin.h>

void Movsb(void* dest, void* source, unsigned long count)
{
	__movsb(dest, source, count);
}

void Movsd(unsigned int* dest, unsigned int* source, unsigned long count)
{
	__movsd(dest, source, count);
}

void Stosb(void* p, unsigned char value, unsigned long count)
{
	__stosb(p, value, count);
}

void Stosd(void* p, unsigned int value, unsigned long count)
{
	__stosd(p, value, count);
}