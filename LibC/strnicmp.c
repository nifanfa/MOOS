int __strnicmp(const char* s1, const char* s2, long n) {
    if (n == 0)
        return 0;

    do {
        if (__tolower((unsigned char)*s1) != __tolower((unsigned char)*s2++))
            return (int)__tolower((unsigned char)*s1) -
            (int)__tolower((unsigned char)*--s2);
        if (*s1++ == 0)
            break;
    } while (--n != 0);

    return 0;
}

int __tolower(int ch)
{
    if ((unsigned int)(ch - 'A') < 26u)
        ch += 'a' - 'A';
    return ch;
}