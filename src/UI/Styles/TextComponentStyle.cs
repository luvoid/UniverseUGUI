using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    [System.Serializable]
    public struct TextComponentStyle : IComponentStyle<Text>
    {
        /// <summary>
        /// If null, the Skin's font will be used.
        /// </summary>
        public Font Font = null;

        public FontStyle FontStyle = FontStyle.Normal;

        /// <summary>
        /// If zero, the font's default size will be used.
        /// </summary>
        public int FontSize = 0;

        public float LineSpacing = 1;

        public bool SupportRichText = true;

        public TextAnchor Alignment = TextAnchor.MiddleCenter;

        public HorizontalWrapMode HorizontalOverflow = HorizontalWrapMode.Overflow;
        public VerticalWrapMode VerticalOverflow = VerticalWrapMode.Overflow;

        public Color Color = Color.white;

        public TextComponentStyle() { }

        public void ApplyTo(Text component)
        {
            if (Font == null) throw new InvalidOperationException("Font is null. (Did you forget to use style.GetTextStyle().ApplyTo() instead of style.Text.ApplyTo()?)");

            component.font               = Font;
            component.fontStyle          = FontStyle;
            component.fontSize           = FontSize;
            component.lineSpacing        = LineSpacing;
            component.supportRichText    = SupportRichText;
            component.alignment          = Alignment;
            component.horizontalOverflow = HorizontalOverflow;
            component.verticalOverflow   = VerticalOverflow;
            component.color              = Color;
        }

        void IComponentStyle.ApplyTo(UIBehaviour component)
        {
            if (component is Text text)
            {

                ApplyTo(text);
            }
        }
    }
}
