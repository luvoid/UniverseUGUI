using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public interface IButtonModel : IComponentModel<Button>
    {
        /// <summary>
        /// Invoked when the Button is clicked.
        /// </summary>
        public System.Action OnClick { get; set; }

        /// <summary>
        /// The Text component on the button.
        /// </summary>
        public Text Label { get; }
    }

    public class ButtonModel : StyledSelectableModel<Button, IReadOnlyButtonStyle>, IButtonModel
    {
        public override Image Background { get; }
        public override Button Component { get; }
        public System.Action OnClick { get; set; }
        public Text Label { get; }
        public RectTransform Transform => GameObject.transform.TryCast<RectTransform>();

        public ButtonModel(GameObject parent, string name, string text)
            : base(parent, name)
        {
            Component = UIRoot.AddComponent<Button>();

            Background = UIFactory.CreateUIObject(UIRoot, "Background").AddComponent<Image>();
            Component.targetGraphic = Background;

            Label = UIFactory.CreateUIObject(UIRoot, "Label").AddComponent<Text>();
            Label.text = text;

            Component.onClick.AddListener(() => { OnClick?.Invoke(); });
            UIFactory.SetButtonDeselectListener(Component);

            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyButtonStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(Component);

            style.Background.ApplyTo(Background);
            UIFactory.SetOffsets(Background.gameObject, -style.Overflow);

            style.GetTextStyle(fallbackSkin).ApplyTo(Label);
            UIFactory.SetOffsets(Label.gameObject, style.LayoutGroup.Padding);
        }
    }
}
