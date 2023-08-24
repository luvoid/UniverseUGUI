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
        public override InputField Component { get => inputFieldRef.Component; protected set => throw new System.InvalidOperationException(); }
        public override Text TextComponent { get => inputFieldRef.Component.textComponent; protected set => throw new System.InvalidOperationException(); }
        public override Graphic BackgroundComponent { get => inputFieldRef.Component.image; protected set => throw new System.InvalidOperationException(); }

        internal TextFieldResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position)
        {
            inputFieldRef = UIFactory.CreateInputField(Container, "InputField", string.Empty);
            inputFieldRef.Component.placeholder.raycastTarget = false;

            TextComponent.transform.parent.parent = Container.transform;
            TextComponent.raycastTarget = false;
            TextComponent.text = content.text;
            Text = content.text;

            ImageComponent = CreateImage(Container, content.image);
            ImageComponent.raycastTarget = false;

            Style = style;
        }
    }
}
