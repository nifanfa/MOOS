#include <intrin.h>

void Nop()
{
	__nop();
}

void Stosb(void* p, unsigned char value, unsigned long count)
{
	__stosb(p, value, count);
}

void Insd(unsigned short Port,unsigned int* Data,unsigned long Count)
{
	__indwordstring(Port, Data, Count);
}

void Outsd(unsigned short Port,unsigned int* Data,unsigned long Count)
{
	__outdwordstring(Port, Data, Count);
}

void Insw(unsigned short Port, unsigned short* Data, unsigned long Count)
{
	__inwordstring(Port, Data, Count);
}

void Outsw(unsigned short Port, unsigned short* Data, unsigned long Count)
{
	__outwordstring(Port, Data, Count);
}

void Insb(unsigned short Port, unsigned char* Data, unsigned long Count)
{
	__inbytestring(Port, Data, Count);
}

void Outsb(unsigned short Port, unsigned char* Data, unsigned long Count)
{
	__outbytestring(Port, Data, Count);
}

void Stosd(void* p, unsigned int value, unsigned long count)
{
	__stosd(p, value, count);
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

void Movsb(void* dest, void* source, unsigned long count)
{
	__movsb(dest, source, count);
}

void Movsd(void* dest, void* source, unsigned long count)
{
	__movsd(dest, source, count);
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