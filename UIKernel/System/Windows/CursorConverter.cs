using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace System.Windows
{
    public class CursorConverter
    {
        public object ConvertFrom(object context, CultureInfo cultureInfo, object source)
        {
            Cursor cursor = new Cursor(CursorState.None);

            switch (source.ToString().ToLower())
            {
                case "hand":
                    cursor.Value = CursorState.Hand;
                    break;
                default:
                    cursor.Value = CursorState.Normal;
                    break;
            }

            return cursor;
        }
    }
}
