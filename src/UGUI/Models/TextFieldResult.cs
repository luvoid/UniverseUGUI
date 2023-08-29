using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UGUI.ImplicitTypes;
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

        internal TextFieldResult(string name, GameObject parent, Rect position, UGUIContent content, UGUIStyle style)
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


        #region String Implementation
        /*
        public char this[int index] => Text[index];
        public int Length => Text.Length;
        public override bool Equals(object obj) => (obj is string str) ? Equals(str) : base.Equals(obj);
        public bool Equals(string str) => Text.Equals(str);
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) => Text.CopyTo(sourceIndex, destination, destinationIndex, count);
        public char[] ToCharArray() => Text.ToCharArray();
        public char[] ToCharArray(int startIndex, int length) => Text.ToCharArray(startIndex, length);
        public int CompareTo(string strB) => Text.CompareTo(strB);
        public bool Contains(string value) => Text.Contains(value);
        public void EndsWith(string value) => Text.EndsWith(value);
        */

        public static implicit operator string(TextFieldResult textFieldResult)
        {
            return textFieldResult.Text;
        }
        public static bool operator ==(TextFieldResult textFieldResult, string str)
        {
            return textFieldResult.Text == str;
        }
        public static bool operator !=(TextFieldResult textFieldResult, string str)
        {
            return textFieldResult.Text != str;
        }
        #endregion
    }
}
