using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.UGUI
{
    public enum StyleState
    {
        None    = 0,

        Normal  = 1, //0b0001,
        Hover   = 2, //0b0010,
        Active  = 3, //0b0011,
        Focused = 4, //0b0100,

        On = 0b1000,

        OnNormal  = On | Normal ,
        OnHover   = On | Hover  ,
        OnActive  = On | Active ,
        OnFocused = On | Focused,
    };
}
