using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyGraphicComponentStyle : IComponentStyle<Image>, System.ICloneable
    {
        public Sprite Sprite { get; }
        public Color Color { get; }
    }

    [System.Serializable]
    public abstract class GraphicComponentStyle : IReadOnlyGraphicComponentStyle
    {
        public Sprite Sprite { get; set; } = null;
        public Color Color = Color.white;
        Color IReadOnlyGraphicComponentStyle.Color => Color;

        internal GraphicComponentStyle()
        { }

        internal GraphicComponentStyle(IReadOnlyGraphicComponentStyle toCopy)
        {
            Sprite = toCopy.Sprite;
            Color  = toCopy.Color ;
        }

        public abstract object Clone();

        public void ApplyTo(Image image)
        {
            image.sprite = Sprite;
            image.color = Color;
        }

        void IComponentStyle.ApplyTo(UIBehaviour component)
        {
            ApplyTo(component);
        }

        protected virtual void ApplyTo(UIBehaviour component)
        {
            if (component is Image image)
            {
                ApplyTo(image);
            }
        }
    }
}
