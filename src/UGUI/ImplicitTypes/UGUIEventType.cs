using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI.ImplicitTypes
{
    internal enum UGUIEventType
    {

        MouseDown = 0,
        MouseUp = 1,
        MouseMove = 2,
        MouseDrag = 3,
        KeyDown = 4,
        KeyUp = 5,
        ScrollWheel = 6,
        Repaint = 7,
        Layout = 8,
        DragUpdated = 9,
        DragPerform = 10,
        DragExited = 0xF,
        Ignore = 11,
        Used = 12,
        ValidateCommand = 13,
        ExecuteCommand = 14,
        ContextClick = 0x10,
        MouseEnterWindow = 20,
        MouseLeaveWindow = 21,



        InitialLayout = int.MaxValue - 2,
        InitialRepaint = int.MaxValue - 1,
    }
}
