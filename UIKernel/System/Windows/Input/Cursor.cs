using System;
using System.Collections.Generic;


namespace System.Windows.Input
{
    public class Cursor
    {
        public CursorState Value { set; get; }

        public Cursor(CursorState state)
        {
            Value = state;
        }

        public override string ToString()
        {
            switch (Value)
            {
                case CursorState.None:
                    return "None";
                case CursorState.Normal:
                    return "Normal";
                case CursorState.Hand:
                    return "Hand";
                case CursorState.TextSelect:
                    return "TextSelect";
                default:
                    return "Desconocido";
            }
        }
    }
}
