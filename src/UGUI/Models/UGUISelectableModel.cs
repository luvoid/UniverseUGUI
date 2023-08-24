using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace UniverseLib.UGUI.Models
{
    public abstract class UGUISelectableModel<T> : UGUIContentModel<T>
        where T : Selectable
    {
        internal UGUISelectableModel(string name, GameObject parent, Rect position)
            : base(name, parent, position)
        { }

        protected override void ApplyStyle(GUIStyle style)
        {
            base.ApplyStyle(style);
            ApplySelectableStyle(style);
        }

        protected virtual void ApplySelectableStyle(GUIStyle style)
        {
            style.ApplyToSelectable(Component);
        }
    }
}
