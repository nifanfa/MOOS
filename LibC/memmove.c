/*
 *	mymemmove.c: mymemmove compat implementation.
 *
 *	Copyright (c) 2001-2006, NLnet Labs. All rights reserved.
 *
 * See LICENSE for the license.
*/

void* mymemmove(void* dest, const void* src, long n)
{
	char* from = (char*)src;
	char* to = (char*)dest;

	if (from == to || n == 0)
		return dest;
	if (to > from && to - from < (int)n) {
		/* to overlaps with from */
		/*  <from......>         */
		/*         <to........>  */
		/* copy in reverse, to avoid overwriting from */
		int i;
		for (i = n - 1; i >= 0; i--)
			to[i] = from[i];
		return dest;
	}
	if (from > to && from - to < (int)n) {
		/* to overlaps with from */
		/*        <from......>   */
		/*  <to........>         */
		/* copy forwards, to avoid overwriting from */
		long i;
		for (i = 0; i < n; i++)
			to[i] = from[i];
		return dest;
	}
	memcpy(dest, src, n);
	return dest;
}