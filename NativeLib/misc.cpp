#include <stddef.h>

int
memcmp(const void* s1, const void* s2, size_t len)
{
#if defined(HAVE_MEMCMP)
    return memcmp(s1, s2, len);
#else
    char* s1p = (char*)s1;
    char* s2p = (char*)s2;
    while (len--) {
        if (*s1p != *s2p) {
            return (*s1p - *s2p);
        }
        ++s1p;
        ++s2p;
    }
    return 0;
#endif /* HAVE_MEMCMP */
}

char*
strchr(const char* string, int c)
{
#ifdef HAVE_STRCHR
    return SDL_const_cast(char*, strchr(string, c));
#elif defined(HAVE_INDEX)
    return SDL_const_cast(char*, index(string, c));
#else
    while (*string) {
        if (*string == c) {
            return (char*)string;
        }
        ++string;
    }
    return NULL;
#endif /* HAVE_STRCHR */
}