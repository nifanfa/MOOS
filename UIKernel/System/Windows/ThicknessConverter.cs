using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace System.Windows
{
    internal class ThicknessConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Thickness thickness = new Thickness();

            if (string.IsNullOrEmpty(source.ToString()))
            {
                return thickness;
            }

            thickness = new Thickness(Convert.ToInt32(source.ToString()));

            return thickness;
        }
    }
}
