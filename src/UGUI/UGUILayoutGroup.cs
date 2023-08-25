using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI
{
    public class UGUILayoutGroup : GUILayoutGroup
    {
        public new List<UGUILayoutGroup> entries = new List<UGUILayoutGroup>();

		public Rect contentRect;

        public UGUILayoutGroup() : base() { }
        public UGUILayoutGroup(GUIStyle style, GUILayoutOption[] options) : base(style, options) { }
    }
}
