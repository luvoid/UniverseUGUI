using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledInputField : StyledComponent<InputField, IReadOnlyInputFieldStyle>, IInputFieldRef
    {
        public override InputField Component => inputFieldRef.Component;
        public override Image Background { get; }
        public Text PlaceholderText => inputFieldRef.PlaceholderText;
        public bool ReachedMaxVerts => inputFieldRef.ReachedMaxVerts;
        public string Text { get => inputFieldRef.Text; set => inputFieldRef.Text = value; }
        public TextGenerator TextGenerator => inputFieldRef.TextGenerator;
        public RectTransform Transform => inputFieldRef.Transform;

        public event System.Action<string> OnValueChanged
        {
            add => inputFieldRef.OnValueChanged += value;
            remove => inputFieldRef.OnValueChanged -= value;
        }


        private readonly InputFieldRef inputFieldRef;
        private readonly GameObject textArea;


        private static GameObject CreateUIRoot(GameObject parent, string name, string placeholderText, out InputFieldRef inputFieldRef)
        {
            inputFieldRef = UIFactory.CreateInputField(parent, name, placeholderText);
            Object.Destroy(inputFieldRef.GameObject.GetComponent<Image>());
            return inputFieldRef.UIRoot;
        }

        public StyledInputField(GameObject parent, string name, string placeholderText)
            : base(CreateUIRoot(parent, name, placeholderText, out InputFieldRef inputFieldRef))
        {
            this.inputFieldRef = inputFieldRef;
            textArea = inputFieldRef.Transform.FindChild("TextArea").gameObject;

            Background = UIFactory.CreateUIObject("Background", UIRoot).AddComponent<Image>();
            Background.transform.SetAsFirstSibling();
            Component.targetGraphic = Background;

            UIFactory.SetLayoutAutoSize(UIRoot);
            UIFactory.SetLayoutAutoSize(textArea);
        }

        public override void ApplyStyle(IReadOnlyInputFieldStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(Component);

            style.Background.ApplyTo(Background);
            SetOffsets(Background.gameObject, -style.Overflow);


            var textStyle = style.GetTextStyle(fallbackSkin);

            textStyle.ApplyTo(Component.textComponent);
            Component.textComponent.supportRichText = false;

            textStyle.ApplyTo(PlaceholderText);
            PlaceholderText.color = PlaceholderText.color * new Color(1, 1, 1, 0.5f);
            PlaceholderText.supportRichText = false;

            SetOffsets(textArea, Vector4.zero, style.LabelOffset);
            SetOffsets(Component.textComponent.gameObject, style.LayoutGroup.Padding);
            SetOffsets(PlaceholderText.gameObject, style.LayoutGroup.Padding);
        }
    }
}
