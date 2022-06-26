int mystrnicmp(const char* s1, const char* s2, long n) {
    if (n == 0)
        return 0;

    do {
        if (mytolower((unsigned char)*s1) != mytolower((unsigned char)*s2++))
            return (int)mytolower((unsigned char)*s1) -
            (int)mytolower((unsigned char)*--s2);
        if (*s1++ == 0)
            break;
    } while (--n != 0);

    return 0;
}

int mytolower(int ch)
{
    if ((unsigned int)(ch - 'A') < 26u)
        ch += 'a' - 'A';
    return ch;
}