using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UniverseLib.UGUI.Models
{
    public sealed class LabelResult : UGUIContentModel<Text>
    {
        /// <summary>
        /// The same as <see cref="TextComponent"/>
        /// </summary>
        public override Text Component { get => TextComponent; protected set => TextComponent = value; }

        internal LabelResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position, content, style)
        { }
    }
}
