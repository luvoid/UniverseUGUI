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
        public Font Font;

        public FontStyle FontStyle;

        /// <summary>
        /// If zero, the font's default size will be used.
        /// </summary>
        public int FontSize;

        public float LineSpacing;

        public bool SupportRichText;

        public TextAnchor Alignment;

        public HorizontalWrapMode HorizontalOverflow;
        public VerticalWrapMode VerticalOverflow;

        public Color Color;

        public TextComponentStyle()
        {
            Font               = null;
            FontStyle          = FontStyle.Normal;
            FontSize           = 0;
            LineSpacing        = 1;
            SupportRichText    = true;
            Alignment          = TextAnchor.MiddleCenter;
            HorizontalOverflow = HorizontalWrapMode.Overflow;
            VerticalOverflow   = VerticalWrapMode.Overflow;
            Color              = Color.white;
        }

        /// <summary>
        /// Create a copy of the TextComponentStyle.
        /// </summary>
        public TextComponentStyle(TextComponentStyle toCopy)
        {
            Font               = toCopy.Font              ;
            FontStyle          = toCopy.FontStyle         ;
            FontSize           = toCopy.FontSize          ;
            LineSpacing        = toCopy.LineSpacing       ;
            SupportRichText    = toCopy.SupportRichText   ;
            Alignment          = toCopy.Alignment         ;
            HorizontalOverflow = toCopy.HorizontalOverflow;
            VerticalOverflow   = toCopy.VerticalOverflow  ;
            Color              = toCopy.Color             ;
        }

        public void ApplyTo(Text component)
        {
            if (Font == null) throw new System.InvalidOperationException("Font is null. (Did you forget to use style.GetTextStyle().ApplyTo() instead of style.Text.ApplyTo()?)");

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
