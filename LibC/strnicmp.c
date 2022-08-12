#include "../Kernel/MOOS.h"

int strnicmp_(const char* s1, const char* s2, long n) {
    if (n == 0)
        return 0;

    do {
        if (tolower_((unsigned char)*s1) != tolower_((unsigned char)*s2++))
            return (int)tolower_((unsigned char)*s1) -
            (int)tolower_((unsigned char)*--s2);
        if (*s1++ == 0)
            break;
    } while (--n != 0);

    return 0;
}