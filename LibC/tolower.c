#include "../Kernel/MOOS.h"

int tolower_(int ch)
{
    if ((unsigned int)(ch - 'A') < 26u)
        ch += 'a' - 'A';
    return ch;
}