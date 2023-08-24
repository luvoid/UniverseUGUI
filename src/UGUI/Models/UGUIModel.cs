using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UniverseLib.UGUI.Models
{
    public abstract class UGUIModel : UIBehaviourModel
    {
        /// <summary>
        /// The root transform of the <see cref="UGUIModel"/>.
        /// </summary>
        public sealed override GameObject UIRoot => Container;

        public abstract GameObject GameObject { get; }

        public GameObject Container { get; protected set; }

        /// <summary>
        /// The <see cref="RectTransform"/> of the <see cref="Container"/>
        /// </summary>
        public RectTransform Transform => Container.GetComponent<RectTransform>();

        private GUIStyle _style;
        public GUIStyle Style
        {
            get => _style;
            set
            {
                if (_style == value) return;
                _style = value ?? throw new System.ArgumentNullException($"Cannot set {nameof(Style)} to null");
                ApplyStyle(_style);
            }
        }

        internal UGUIModel() { }

        /// <summary>
        /// Not implemented.
        /// </summary>
        [System.Obsolete("Not implemented.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public sealed override void ConstructUI(GameObject parent)
        {
            throw new System.InvalidOperationException("Not implemented.");
        }

        protected abstract void ApplyStyle(GUIStyle style);

        protected void SetOffsets(GameObject gameObject, RectOffset rectOffset, Vector2 positionOffset = default)
        {
            positionOffset.y *= -1;
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(rectOffset.left, rectOffset.bottom) + positionOffset;
            rectTransform.offsetMax = new Vector2(-rectOffset.right, -rectOffset.top) + positionOffset;
        }

        protected virtual void SetState(in Rect position, GUIStyle style)
        {
            SetPosition(position);
            SetStyle(style);
        }

        protected virtual void SetPosition(in Rect position)
        {
            UGUIUtility.SetRect(Transform, position);
        }

        protected virtual void SetStyle(GUIStyle style)
        {
            ApplyStyle(style);
        }
    }
}
