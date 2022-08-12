double __fabs(double x)
{
	union { double f; unsigned long i; } u = { x };
	u.i &= -1ULL / 2;
	return u.f;
}