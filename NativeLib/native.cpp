#include <intrin.h>

void Nop()
{
	__nop();
}

void Fxsave64(void* ptr)
{
	_fxsave64(ptr);
}

void Fxrstor64(void* ptr)
{
	_fxrstor64(ptr);
}

void Invlpg(unsigned long physicalAddress)
{
	__invlpg(physicalAddress);
}

void WriteCR3(unsigned long value)
{
	__writecr3(value);
}

unsigned long ReadCR2()
{
	return __readcr2();
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