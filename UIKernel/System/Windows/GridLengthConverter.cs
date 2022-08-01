using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows
{
    public class GridLengthConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            GridLength gridLength = null;

            if (string.IsNullOrEmpty(source.ToString()))
            {
                return gridLength;
            }

            switch (source.ToString().ToLower())
            {
                case "auto":
                    gridLength = new GridLength(42, GridUnitType.Auto);
                    break;
                case "*":
                    gridLength = new GridLength(1, GridUnitType.Star);
                    break;
                default:
                    gridLength = new GridLength(Convert.ToInt32(source.ToString()), GridUnitType.Pixel);
                    break;
            }

            return gridLength;
        }
    }
}
