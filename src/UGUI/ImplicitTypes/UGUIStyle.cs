using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UGUI.Collections.Generic;
using UniverseLib.UI.Styles;

namespace UniverseLib.UGUI.ImplicitTypes
{
    public abstract class UGUIStyle
    {
        private static readonly Hashtable<IReadOnlyUIObjectStyle, ConvertedUIStyle> s_ConvertedUIStyleCache = new();
        private static readonly Hashtable<GUIStyle, ConvertedGUIStyle> s_ConvertedGUIStyleCache = new();

        public static implicit operator UGUIStyle(UIObjectStyle uiStyle)
        {
            if (!s_ConvertedUIStyleCache.TryGetValue(uiStyle, out ConvertedUIStyle uguiStyle))
            {
                uguiStyle = new ConvertedUIStyle(uiStyle);
            }
            return uguiStyle;
        }

        public static implicit operator UGUIStyle(ReadOnlyUIObjectStyle uiStyle)
        {
            if (!s_ConvertedUIStyleCache.TryGetValue(uiStyle, out ConvertedUIStyle uguiStyle))
            {
                uguiStyle = new ConvertedUIStyle(uiStyle);
            }
            return uguiStyle;
        }

        public static implicit operator UGUIStyle(GUIStyle guiStyle)
        {
            if (!s_ConvertedGUIStyleCache.TryGetValue(guiStyle, out ConvertedGUIStyle uguiStyle))
            {
                uguiStyle = new ConvertedGUIStyle(guiStyle);
            }
            return uguiStyle;
        }

        internal abstract object InternalStyle { get; }
        internal abstract GUIStyle InternalGUIStyle { get; }
        internal abstract void ApplyToText(Text text, UGUISkin fallbackSkin = null);
        internal abstract void ApplyToBackground(Graphic background);
        internal abstract void ApplyToSelectable(Selectable selectable, bool useSprites = true);
        internal abstract Vector2 CalcSizeWithConstraints(GUIContent content, Vector2 constraints);
        internal abstract RectOffset padding       { get; }
        internal abstract RectOffset overflow      { get; }
        internal abstract Vector2    contentOffset { get; }
        internal abstract Color      textColor     { get; }
        internal abstract bool isHeightDependantOnWidth { get; }





        private class ConvertedUIStyle : UGUIStyle
        {
            private readonly IReadOnlyUIObjectStyle style;
            private readonly GUIStyle guiStyle = new();
            internal override object InternalStyle => style;
            internal override GUIStyle InternalGUIStyle => guiStyle;

            public ConvertedUIStyle(IReadOnlyUIObjectStyle style)
            {
                this.style = style;
                guiStyle = UGUIUtility.GetDefaultSkin().Box.InternalGUIStyle;
                UpdateGUIStyle();
            }

            private void UpdateGUIStyle()
            {
                var textStyle = style.GetTextStyle();
                guiStyle.font = textStyle.Font;
                guiStyle.fontSize = textStyle.FontSize;
                guiStyle.fontStyle = textStyle.FontStyle;
                guiStyle.alignment = textStyle.Alignment;
                guiStyle.wordWrap = textStyle.HorizontalOverflow == HorizontalWrapMode.Wrap;
                guiStyle.padding = _padding.Set(style.Padding);
                guiStyle.overflow = _overflow.Set(style.Overflow);
                guiStyle.contentOffset = style.ContentOffset;
            }

            internal override void ApplyToText(Text text, UGUISkin fallbackSkin = null)
            {
                fallbackSkin ??= UGUI.skin;
                style.GetTextStyle(fallbackFont: fallbackSkin?.Font).ApplyTo(text);
            }

            internal override void ApplyToBackground(Graphic background)
            {
                style.Background?.ApplyTo(background);
            }

            internal override void ApplyToSelectable(Selectable selectable, bool useSprites = true)
            {
                style.Background?.ApplyTo(selectable);
            }

            internal override Vector2 CalcSizeWithConstraints(GUIContent content, Vector2 constraints)
            {
                UpdateGUIStyle();
                return guiStyle.CalcSizeWithConstraints(content, constraints);
            }

            private readonly RectOffset _padding = new();
            private readonly RectOffset _overflow = new();

            internal override RectOffset padding       => _padding.Set(style.Padding);
            internal override RectOffset overflow      => _overflow.Set(style.Overflow);
            internal override Vector2    contentOffset => style.ContentOffset;
            internal override Color      textColor     => style.GetTextStyle().Color;
            internal override bool isHeightDependantOnWidth => false;
        }

        private class ConvertedGUIStyle : UGUIStyle
        {
            private readonly GUIStyle guiStyle;
            internal override object InternalStyle => guiStyle;
            internal override GUIStyle InternalGUIStyle => guiStyle;

            public ConvertedGUIStyle(GUIStyle style)
            {
                this.guiStyle = style;
            }

            internal override void ApplyToText(Text text, UGUISkin fallbackSkin = null)
            {
                fallbackSkin ??= UGUI.skin;
                guiStyle.ApplyToText(text, fallbackSkin?.Font);
            }

            internal override void ApplyToBackground(Graphic background)
            {
                guiStyle.ApplyToBackground(background);
            }

            internal override void ApplyToSelectable(Selectable selectable, bool useSprites = true)
            {
                guiStyle.ApplyToSelectable(selectable);
            }

            internal override Vector2 CalcSizeWithConstraints(GUIContent content, Vector2 constraints)
            {
                return guiStyle.CalcSizeWithConstraints(content, constraints);
            }

            internal override RectOffset padding       => guiStyle.padding;
            internal override RectOffset overflow      => guiStyle.overflow;
            internal override Vector2    contentOffset => guiStyle.contentOffset;
            internal override Color      textColor     => guiStyle.normal.textColor;
            internal override bool isHeightDependantOnWidth => guiStyle.isHeightDependantOnWidth;
        }
    }
}
