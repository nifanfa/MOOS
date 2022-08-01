using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media;

namespace System.Windows.Media
{
    public class BrushConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Brush brush = null;

            if (source == null)
            {
                return brush;
            }

            brush = new Brush(ColorConverter.ConvertFromString(source.ToString()));

            return brush;
        }
    }

}
