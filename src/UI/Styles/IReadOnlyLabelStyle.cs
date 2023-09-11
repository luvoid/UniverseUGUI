using UnityEngine;
using UniverseLib.Utility;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyLabelStyle
    {
        public string Name { get; }
        /// <summary>
        /// If null, will use the <see cref="UISkin"/>'s Text style.
        /// </summary>
        public TextComponentStyle Text { get; }
        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null);
    }

    /// <summary>
    /// Provides default implementations for <see cref="IReadOnlyLabelStyle"/>
    /// </summary>
    public static class LabelStyleHelper
    {
        public static TextComponentStyle GetTextStyle(IReadOnlyLabelStyle labelStyle, IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
        {
            TextComponentStyle textStyle = labelStyle.Text;

            textStyle.Font ??= fallbackFont ?? fallbackSkin?.Text.Font ?? UniversalUI.DefaultFont;
            if (textStyle.Font.IsNullOrDestroyed())
            {
                textStyle.Font = null;
            }
            textStyle.FontSize = textStyle.FontSize == 0 ? fallbackSkin?.Text.FontSize ?? textStyle.Font?.fontSize ?? 14 : textStyle.FontSize;

            return textStyle;
        }
    }
}
