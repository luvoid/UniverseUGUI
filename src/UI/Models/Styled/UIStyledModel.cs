using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public abstract class UIStyledModel<TReadOnlyStyle> : UIModel
        where TReadOnlyStyle : IReadOnlyUIObjectStyle
    {
        public sealed override GameObject UIRoot { get; }
        public abstract Image Background { get; }
        public GameObject GameObject => UIRoot;

        /// <summary>
        /// Constructor that creates a simple UIRoot.
        /// </summary>
        protected UIStyledModel(GameObject parent, string name)
        {
            UIRoot = UIFactory.CreateUIObject(parent, name);
        }

        /// <summary>
        /// Constructor that uses a custom UIRoot that has already been constructed.
        /// </summary>
        protected UIStyledModel(GameObject uiRoot)
        {
            UIRoot = uiRoot;
        }

        public abstract void ApplyStyle(TReadOnlyStyle style, IReadOnlyUISkin fallbackSkin = null);

        [System.Obsolete($"Use {nameof(UIFactory)}.{nameof(SetOffsets)}(...) instead.")]
        protected static void SetOffsets(GameObject obj, Vector4 padding, Vector2 offset = default)
        {
            UIFactory.SetOffsets(obj, padding, offset);
        }
    }
}
