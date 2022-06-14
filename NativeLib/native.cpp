#include <intrin.h>

void Nop()
{
	__nop();
}

unsigned int cpuinfo[4];

unsigned int* CPUID(int index) 
{
	__cpuid(&cpuinfo, index);
	return &cpuinfo;
}

void Fxsave64(void* ptr)
{
	_fxsave64(ptr);
}

unsigned long long Rdmsr(unsigned long index)
{
	return __readmsr(index);
}

void Wrmsr(unsigned long index,unsigned long long value)
{
	__writemsr(index, value);
}

unsigned long Rdtsc()
{
	return __rdtsc();
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