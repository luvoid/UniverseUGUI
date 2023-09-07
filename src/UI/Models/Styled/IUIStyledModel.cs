using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public interface IUIStyledModel<TReadOnlyStyle>
        where TReadOnlyStyle : IReadOnlyUIObjectStyle
    {
        public void ApplyStyle(TReadOnlyStyle style, IReadOnlyUISkin fallbackSkin = null);
    }
}
