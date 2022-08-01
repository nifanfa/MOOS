using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;

namespace System.Windows
{
    public  class FontWeightConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            FontWeight fontWeight = null;

            if (string.IsNullOrEmpty(source.ToString()))
            {
                return fontWeight;
            }

            fontWeight = new FontWeight();

            return fontWeight;
        }
    }
}
