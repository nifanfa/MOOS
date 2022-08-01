using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Windows
{
    public enum GridUnitType
    {
        Auto = 0,
        Pixel = 1,
        Star = 2
    }

    public class GridLength
    {
        public GridUnitType GridUnitType { get; private set; }
        public bool IsAbsolute { get; private set; }
        public bool IsAuto { get; private set; }
        public bool IsStar { get; private set; }
        public int Value { get; set; }

        public GridLength(int value, GridUnitType unit)
        {
            this.GridUnitType = unit;
            this.Value = value;
            onSetProperties();
        }

        void onSetProperties()
        {
            switch (GridUnitType)
            {
                case GridUnitType.Auto:
                    IsAuto = true;
                    IsAbsolute = false;
                    IsStar = false;
                    break;
                case GridUnitType.Pixel:
                    IsAuto = false;
                    IsAbsolute = true;
                    IsStar = false;
                    break;
                case GridUnitType.Star:
                    IsAuto = false;
                    IsAbsolute = false;
                    IsStar = true;
                    break;
            }
        }
    }
}
