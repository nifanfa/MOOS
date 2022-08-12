#define NULL 0

char*
__strdup(str)
const char* str;
{
	long len;
	char* copy;

	len = __strlen(str) + 1;
	if (!(copy = kmalloc((unsigned int)len)))
		return (NULL);
	memcpy(copy, str, len);
	return (copy);
}