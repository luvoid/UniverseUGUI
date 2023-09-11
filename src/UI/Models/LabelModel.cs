using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public class LabelModel : StyledComponentModel<Text, IReadOnlyFrameStyle>
    {
        public sealed override Text Component { get; }
        public sealed override Image Background { get; }

        public string Text { get => Component.text; set => Component.text = value; }

        public LabelModel(GameObject parent, string name, string text) : base(parent, name)
        {
            Component = UIFactory.CreateLabel(UIRoot, "Text", text);
            Background = UIFactory.CreateUIObject(UIRoot, "Background").AddComponent<Image>();
            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            Background.enabled = style.UseBackground;
            style.Background?.ApplyTo(Background);
            UIFactory.SetOffsets(Background.gameObject, -style.Overflow);

            style.GetTextStyle(fallbackSkin).ApplyTo(Component);
            UIFactory.SetOffsets(Component.gameObject, style.LayoutGroup.Padding);
        }
    }
}
