// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System
{
    public unsafe struct Double
    {
        public override string ToString()
        {
            char* p = stackalloc char[22];
            string s = new(p, 0, 22);
            Dtoa(s, this);
            return s;

        }

        private static void Dtoa(string c, double d)
        {
            int i = 0, e = 0, n = 0, flag = 0;//flag=0E+;1E-

            if (d < 0)
            {
                c[i++] = '-';
                d = -d;
            }
            while (d >= 10)
            {
                d /= 10;//here is the problem
                e++;
            }
            while (d < 1)
            {
                d *= 10;
                e++;
                flag = 1;
            }
            int v = (int)d, dot;
            c[i++] = (char)('0' + v);//the integer part
            dot = i;
            n++;
            c[i++] = '.';
            d -= v;
            while (d != 0 && n < 10)
            {
                d *= 10;
                v = (int)d;
                c[i++] = (char)('0' + v);
                n++;
                d -= v;
            }
            if (d != 0)
            {

                if (d * 10 >= 5)//rounding
                {
                    int j = i - 1;
                    c[j]++;
                    while (c[j] > '9')
                    {
                        c[j] = '0';
                        if (j - 1 == dot)
                        {
                            j--;
                        }

                        c[--j]++;
                    }
                }
            }
            else
            {
                while (n < 10)
                {
                    c[i++] = '0';
                    n++;
                }
            }

            if (e != 0)
            {
                c[i++] = 'E';
                c[i++] = (flag == 0) ? '+' : '-';
                if (e >= 100)
                {
                    int tmp = e / 100;
                    c[i++] = (char)('0' + tmp);
                    e -= (tmp * 100);
                    c[i++] = (char)('0' + e / 10);
                    c[i++] = (char)('0' + e % 10);
                }
                else if (e <= 9)
                {
                    c[i++] = '0';
                    c[i++] = '0';
                    c[i++] = (char)('0' + e);
                }
                else
                {
                    c[i++] = '0';
                    c[i++] = (char)('0' + e / 10);
                    c[i++] = (char)('0' + e % 10);
                }
            }
            c[i] = '\0';
        }
    }
}
