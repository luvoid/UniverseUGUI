using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledLabel : StyledComponent<Text, IReadOnlyPanelStyle>
    {
        public sealed override Text Component { get; }
        public sealed override Image Background { get; }

        public string Text { get => Component.text; set => Component.text = value; }

        public StyledLabel(GameObject parent, string name, string text) : base(parent, name)
        {
            Component = UIFactory.CreateLabel(UIRoot, name, text);
            Background = UIFactory.CreateUIObject("Background", UIRoot).AddComponent<Image>();
            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyPanelStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            Background.enabled = style.UseBackground;
            style.Background?.ApplyTo(Background);
            SetOffsets(Background.gameObject, -style.Overflow);

            style.GetTextStyle(fallbackSkin).ApplyTo(Component);
            SetOffsets(GameObject, style.LayoutGroup.Padding, style.LabelOffset);
        }
    }
}
