/* Portable version of mystrrchr().
   This function is in the public domain. */

   /*
   NAME
	   mystrrchr -- return pointer to last occurance of a character

   SYNOPSIS
	   char *mystrrchr (const char *s, int c)

   DESCRIPTION
	   Returns a pointer to the last occurance of character C in
	   string S, or a NULL pointer if no occurance is found.

   BUGS
	   Behavior when character is the null character is implementation
	   dependent.
   */

char*
mystrrchr(s, c)
register const char* s;
int c;
{
	char* rtnval = 0;

	do {
		if (*s == c)
			rtnval = (char*)s;
	} while (*s++);
	return (rtnval);
}