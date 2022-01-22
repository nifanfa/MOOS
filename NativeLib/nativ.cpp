#include <intrin.h>

void Stosb(void* p, unsigned char value, unsigned long count)
{
	__stosb(p, value, count);
}

void Invlpg(unsigned long physicalAddress)
{
	__invlpg(physicalAddress);
}

void WriteCR3(unsigned long value)
{
	__writecr3(value);
}

void Movsb(void* dest, void* source, unsigned long count)
{
	__movsb(dest, source, count);
}

void Hlt()
{
	_hlt();
}

void Cli()
{
	_cli();
}

void Sti()
{
	_sti();
}