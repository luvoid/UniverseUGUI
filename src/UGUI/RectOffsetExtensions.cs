using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI
{
    internal static class RectOffsetExtensions
    {
        public static RectOffset Negative(this RectOffset rectOffset)
        {
            return new RectOffset(
                -rectOffset.left,
                -rectOffset.right,
                -rectOffset.top,
                -rectOffset.bottom
            );
        }
        
        public static RectOffset Set(this RectOffset rectOffset, Vector4 offsets)
        {
            rectOffset.left   = (int)offsets.x;
            rectOffset.right  = (int)offsets.y;
            rectOffset.top    = (int)offsets.z;
            rectOffset.bottom = (int)offsets.w;

            return rectOffset;
        }
    }
}
