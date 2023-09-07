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

        /// <summary>
        /// Constructor that creates a simple UIRoot.
        /// </summary>
        protected UIStyledModel(GameObject parent, string name)
        {
            UIRoot = UIFactory.CreateUIObject(name, parent);
        }

        /// <summary>
        /// Constructor that uses a custom UIRoot that has already been constructed.
        /// </summary>
        protected UIStyledModel(GameObject uiRoot)
        {
            UIRoot = uiRoot;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        [System.Obsolete("Not implemented.", true)]
        public sealed override void ConstructUI(GameObject parent)
        {
            throw new System.NotImplementedException();
        }

        public abstract void ApplyStyle(TReadOnlyStyle style, IReadOnlyUISkin fallbackSkin = null);

        protected static void SetOffsets(GameObject obj, Vector4 padding, Vector2 offset = default)
        {
            RectTransform rectTransform = obj.transform.TryCast<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2( padding.x,  padding.w) + offset;
            rectTransform.offsetMax = new Vector2(-padding.y, -padding.z) + offset;
        }
    }
}
