using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UniverseLib.UGUI.Models
{
    public sealed class TextFieldResult : UGUISelectableModel<InputField>
    {
        private InputFieldRef inputFieldRef;

        public override string Text { get => inputFieldRef.Text; set => inputFieldRef.Text = value; }
        public override InputField Component => inputFieldRef.Component;
        public override Text TextComponent => inputFieldRef.Component.textComponent;
        public override Graphic BackgroundComponent => inputFieldRef.Component.image;
		public override RawImage ImageComponent { get; }

        internal TextFieldResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position)
        {
            inputFieldRef = UIFactory.CreateInputField(Container, "InputField", string.Empty);
            inputFieldRef.Component.placeholder.raycastTarget = false;

            TextComponent.transform.parent.SetParent(Container.transform, worldPositionStays: false);
            TextComponent.raycastTarget = false;

            ImageComponent = CreateImage(Container, content.image);
            ImageComponent.raycastTarget = false;

            Style = style;

			SetContent(content);
            TextComponent.text = content.text; // It won't show until it's interacted with otherwise.
		}
	}
}
