using MOOS.FS;
using MOOS.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;

namespace System.Windows
{
    public enum CursorState
    {
        None,
        Normal,
        Grab,
        Moving,
        TextSelect,
        Hand,
        Cross,
        Wait,
    }

    public class CursorManager
    {
        static Image CursorNormal { set; get; }
        static Image CursorMoving { set; get; }
        static Image CursorTextSelect { set; get; }
        static Image CursorHand{ set; get; }
        public static Cursor State { set; get; }
        public static Widget FocusControl { set; get; }

        public static void Initialize()
        {
            //Sized width to 512
            //https://gitlab.com/Enthymeme/hackneyed-x11-cursors/-/blob/master/theme/right-handed-white.svg
            CursorNormal = new PNG(File.Instance.ReadAllBytes("sys/media/Cursor.png"));
            CursorMoving = new PNG(File.Instance.ReadAllBytes("sys/media/Grab.png"));
            CursorTextSelect = new PNG(File.Instance.ReadAllBytes("sys/media/CursorTextSelect.png"));
            CursorHand = new PNG(File.Instance.ReadAllBytes("sys/media/CursorHand.png"));
            State = new Cursor(CursorState.Normal); 
        }

        public static Image GetCursor
        {
            get
            {
                switch (State.Value)
                {
                    case CursorState.Normal:
                        return CursorNormal;
                    case CursorState.Grab:
                        return CursorMoving;
                    case CursorState.TextSelect:
                        return CursorTextSelect;
                    case CursorState.Hand:
                        return CursorHand;
                    default:
                        return CursorNormal;
                }
            }
        }

        public static void Update()
        {
            if (WindowManager.HasWindowMoving)
            {
                State.Value = CursorState.Grab;
                return;
            }
            
            if (FocusControl != null)
            {
                if (FocusControl.MouseEnter)
                {
                    if (FocusControl.Cursor.Value != CursorState.None)
                    {
                        State.Value = FocusControl.Cursor.Value;
                        return;
                    }
                }
            }

            State.Value = CursorState.Normal;
        }
    }
}
