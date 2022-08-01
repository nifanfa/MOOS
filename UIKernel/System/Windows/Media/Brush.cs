using System;
using System.Collections.Generic;


namespace System.Windows.Media
{
    public class Brush
    {
        public uint Value { set; get; }
        public Brush()
        {
            Value = 0xFF222222;
        }

        public Brush(uint value)
        {
            Value = value;
        }
    }
}
