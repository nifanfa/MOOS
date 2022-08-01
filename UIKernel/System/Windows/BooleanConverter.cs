using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Windows
{
    public class BooleanConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            if (string.IsNullOrEmpty(source.ToString()))
            {
                return false;
            }

            switch (source.ToString().ToLower())
            {
                case "true":
                    return true;
                    case "false":
                    return false;
                default:
                    return false;
            }
        }
    }
}
