﻿using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UGUI.ImplicitTypes;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using BaseUniverseLib = UniverseLib;

namespace UniverseLib.UGUI.Models
{
    public sealed class BoxResult : UGUIContentModel
    {
        /// <summary>
        /// The same as Container.
        /// </summary>
        public override GameObject GameObject => Container;

        internal BoxResult(string name, GameObject parent, Rect position, UGUIContent content, UGUIStyle style)
            : base(name, parent, position, content, style)
        { }
    }
}
