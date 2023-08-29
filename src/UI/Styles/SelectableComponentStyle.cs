using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlySelectableComponentStyle : IReadOnlyGraphicComponentStyle, IComponentStyle<Selectable>
    {
        public SelectableComponentStyle Copy();
    }

    [System.Serializable]
    public class SelectableComponentStyle : GraphicComponentStyle, IReadOnlySelectableComponentStyle
    {
        public Selectable.Transition Transition = Selectable.Transition.ColorTint;

        public SpriteState SpriteState = default;

        public ColorBlock Colors = new ColorBlock()
        {
            colorMultiplier  = 1f,
            fadeDuration     = 0.1f,
            normalColor      = Color.white,
            highlightedColor = new Color(0.95f, 0.95f, 0.95f),
            pressedColor     = new Color(0.78f, 0.78f, 0.78f),
            disabledColor    = new Color(0.785f, 0.785f, 0.785f, 0.5f)
        };

        public SelectableComponentStyle() { }

        public SelectableComponentStyle(SelectableComponentStyle toCopy)
            : base(toCopy)
        {
            Transition  = toCopy.Transition;
            SpriteState = toCopy.SpriteState;
            Colors      = toCopy.Colors;
        }

        public override object Clone() => Copy();
        public SelectableComponentStyle Copy()
        {
            return new SelectableComponentStyle(this);
        }

        public void ApplyTo(Selectable selectable)
        {
            selectable.transition = Transition;
            RuntimeHelper.SetColorBlock(selectable, Colors);
            selectable.spriteState = SpriteState;
            if (selectable.targetGraphic is Image image)
            {
                base.ApplyTo(image);
            }
        }

        protected override void ApplyTo(UIBehaviour component)
        {
            if (component is Selectable selectable)
            {
                ApplyTo(selectable);
            }
            else
            {
                base.ApplyTo(component);
            }
        }
    }
}
