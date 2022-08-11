namespace System.Text
{
    public unsafe class UTF8Encoding : Encoding
    {
        public override string GetString(byte* ptr)
        {
            int length = string.strlen(ptr);
            char* output = stackalloc char[length];
            int index = 0;
            for (int i = 0; i < length;)
            {
                if ((ptr[i] >> 7) == 0)
                {
                    output[index++] = (char)ptr[i++];
                }
                else if ((ptr[i] >> 5) == 0b110)
                {
                    int c = ((ptr[i++] & 0b11111) << 6) | ((ptr[i++] & 0b111111));
                    output[index++] = (char)c;
                }
                else if ((ptr[i] >> 4) == 0b1110)
                {
                    int c = ((ptr[i++] & 0b1111) << 12) | ((ptr[i++] & 0b111111) << 6) | ((ptr[i++] & 0b111111));
                    output[index++] = (char)c;
                }
            }
            return new string(output, 0, index);
        }
    }
}
