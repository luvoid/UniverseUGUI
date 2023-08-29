using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UGUI.ImplicitTypes;
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

        internal LabelResult(string name, GameObject parent, Rect position, UGUIContent content, UGUIStyle style)
            : base(name, parent, position, content, style)
        { }
    }
}
