using System.ComponentModel;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UniverseLib.UGUI.Models
{
    public sealed class ButtonResult : UGUISelectableModel<Button>
    {
        private readonly ButtonRef buttonRef;

        public override Button Component => buttonRef.Component;
        public override Text TextComponent => buttonRef.ButtonText;
        public override Graphic BackgroundComponent => buttonRef.Component.image;
		public override RawImage ImageComponent { get; }

		internal ButtonResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position)
        {
            buttonRef = UIFactory.CreateButton(Container, "Button", content.text);

            TextComponent.transform.SetParent(Container.transform, worldPositionStays: false);
            TextComponent.raycastTarget = false;

            ImageComponent = CreateImage(Container, content.image);
            ImageComponent.raycastTarget = false;

            Style = style;
        }
    }
}
