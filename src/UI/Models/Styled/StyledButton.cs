using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledButton : StyledSelectable<Button, IReadOnlyButtonStyle>, IButtonRef
    {
        public Action OnClick { get; set; }
        public override Button Component { get; }
        public override Image Background { get; }
        public Text Label { get; }
        public RectTransform Transform => GameObject.transform.TryCast<RectTransform>();
        Text IButtonRef.ButtonText => Label;

        public StyledButton(GameObject parent, string name, string text) : base(parent, name)
        {
            Component = UIRoot.AddComponent<Button>();

            Background = UIFactory.CreateUIObject("Background", UIRoot).AddComponent<Image>();
            Component.targetGraphic = Background;

            Label = UIFactory.CreateUIObject("ButtonText", UIRoot).AddComponent<Text>();
            Label.text = text;

            Component.onClick.AddListener(() => { OnClick?.Invoke(); });
            UIFactory.SetButtonDeselectListener(Component);

            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyButtonStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(Component);

            style.Background.ApplyTo(Background);
            SetOffsets(Background.gameObject, -style.Overflow);

            style.GetTextStyle(fallbackSkin).ApplyTo(Label);
            SetOffsets(Label.gameObject, style.LayoutGroup.Padding, style.LabelOffset);
        }
    }
}
