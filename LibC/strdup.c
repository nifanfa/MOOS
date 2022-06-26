#define NULL 0

char*
mystrdup(str)
const char* str;
{
	long len;
	char* copy;

	len = mystrlen(str) + 1;
	if (!(copy = kmalloc((unsigned int)len)))
		return (NULL);
	memcpy(copy, str, len);
	return (copy);
}