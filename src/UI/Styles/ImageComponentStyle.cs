using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyImageComponentStyle : IReadOnlyGraphicComponentStyle
    {
        public ImageComponentStyle Copy();
    }

    [System.Serializable]
    public sealed class ImageComponentStyle : GraphicComponentStyle, IReadOnlyImageComponentStyle
    {
        public ImageComponentStyle()
        { }

        public ImageComponentStyle(GraphicComponentStyle toCopy)
        {
            Sprite = toCopy.Sprite;
            Color  = toCopy.Color ;
        }

        public override object Clone() => Copy();
        public ImageComponentStyle Copy()
        {
            return new ImageComponentStyle(this);
        }
    }
}
