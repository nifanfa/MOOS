int mystricmp(a, b)
char* a, * b;
{
    int     ca, cb;
    while (1) {
        ca = *a;
        cb = *b;
        if (!ca) return(-cb);
        if (cb >= 'A' && cb <= 'Z') cb += 'a' - 'A';
        if (ca >= 'A' && ca <= 'Z') ca = ca + 'a' - 'A' - cb;
        else ca = ca - cb;
        if (ca) return(ca);
        a++;
        b++;
    }
    return(0);
}