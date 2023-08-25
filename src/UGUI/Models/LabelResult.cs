using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UniverseLib.UGUI.Models
{
    public sealed class LabelResult : UGUIContentModel<Text>
    {
        /// <summary>
        /// The same as TextComponent.
        /// </summary>
        public override Text Component => TextComponent;

        internal LabelResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position, content, style)
        { }
    }
}
