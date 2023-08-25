using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UGUI;
using UniverseLib.UI;

namespace UniverseLib.UGUI.Models
{
    public abstract class UGUIContentModel : UGUIModel
    {
        public virtual Text TextComponent { get; }
        public virtual Graphic BackgroundComponent { get; }
        public virtual RawImage ImageComponent { get; }

        /// <summary>
        /// The string text assigned to the <see cref="TextComponent"/>.
        /// </summary>
        public virtual string Text
        {
            get => TextComponent.text;
            set => TextComponent.text = value;
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        internal UGUIContentModel(in string name, GameObject parent, Rect position)
        {
            Container = UIFactory.CreateUIObject(name, parent);
            Transform.anchorMin = new Vector2(0, 1);
            Transform.anchorMax = new Vector2(0, 1);
            Transform.pivot = new Vector2(0, 1);

            UGUIUtility.SetRect(Transform, position);
        }

        internal UGUIContentModel(in string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : this(name, parent, position)
        {
            var background = UIFactory.CreateUIObject("Background", Container).AddComponent<Image>();
            background.type = Image.Type.Sliced;
            BackgroundComponent = background;

            ImageComponent = CreateImage(Container, content.image);

            TextComponent = UIFactory.CreateLabel(Container, "Text", content.text);

            Style = style;
        }

        protected static RawImage CreateImage(GameObject parent, Texture texture)
        {
            var image = UIFactory.CreateUIObject("Image", parent).AddComponent<RawImage>();
            image.texture = texture;
            image.enabled = texture != null;
            return image;
        }

        protected override void ApplyStyle(GUIStyle style)
        {
            ApplyTextStyle(style);
            ApplyBackgroundStyle(style);
            ApplyImageStyle(style);
        }

        protected virtual void ApplyTextStyle(GUIStyle style)
        {
            style.ApplyToText(TextComponent);
            SetOffsets(TextComponent.gameObject, style.padding, style.contentOffset);
        }

        protected virtual void ApplyBackgroundStyle(GUIStyle style)
        {
            style.ApplyToBackground(BackgroundComponent);
            BackgroundComponent.enabled = BackgroundComponent is not Image image || image.sprite != null;
            SetOffsets(BackgroundComponent.gameObject, style.overflow.Negative());
        }

        protected virtual void ApplyImageStyle(GUIStyle style)
        {
            SetOffsets(ImageComponent.gameObject, style.padding, style.contentOffset);
        }

        internal virtual void SetState(in Rect position, UGUIContent content, GUIStyle style)
        {
            base.SetState(position, style);
            SetContentIfChanged(content);
        }

        private int _contentHash = -1;
        protected void SetContentIfChanged(UGUIContent content)
        {
            if (_contentHash != content.hash)
            {
                _contentHash = content.hash;
                SetContent(content);
            }
        }

        protected virtual void SetContent(UGUIContent content)
        {
            Text = content.text;
            ImageComponent.texture = content.image;
            ImageComponent.enabled = content.image != null;
        }
    }

    public abstract class UGUIContentModel<T> : UGUIContentModel
        where T : Component
    {
        public override GameObject GameObject => Component.gameObject;
        public abstract T Component { get; }

        /// <inheritdoc/>
        internal UGUIContentModel(string name, GameObject parent, Rect position)
            : base(name, parent, position)
        { }

        /// <inheritdoc/>
        internal UGUIContentModel(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position, content, style)
        { }
    }
}
