#include "../Kernel/MOOS.h"

double fabs_(double x)
{
	union { double f; unsigned long i; } u = { x };
	u.i &= -1ULL / 2;
	return u.f;
}