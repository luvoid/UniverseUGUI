using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UGUI.ImplicitTypes;
using UniverseLib.UI;

namespace UniverseLib.UGUI.Models
{
    public abstract class UGUISelectableModel<T> : UGUIContentModel<T>
        where T : Selectable
    {
        internal UGUISelectableModel(string name, GameObject parent, Rect position)
            : base(name, parent, position)
        { }

        protected override void ApplyStyle(UGUIStyle style)
        {
            base.ApplyStyle(style);
            ApplySelectableStyle(style);
        }

        protected virtual void ApplySelectableStyle(UGUIStyle style)
        {
            style.ApplyToSelectable(Component);
        }
    }
}
