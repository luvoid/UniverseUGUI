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

        public override Button Component { get => buttonRef.Component; protected set => throw new System.InvalidOperationException(); }
        public override Text TextComponent { get => buttonRef.ButtonText; protected set => throw new System.InvalidOperationException(); }
        public override Graphic BackgroundComponent { get => buttonRef.Component.image; protected set => throw new System.InvalidOperationException(); }

        internal ButtonResult(string name, GameObject parent, Rect position, UGUIContent content, GUIStyle style)
            : base(name, parent, position)
        {
            buttonRef = UIFactory.CreateButton(Container, "Button", content.text);

            TextComponent.transform.parent = Container.transform;
            TextComponent.raycastTarget = false;

            ImageComponent = CreateImage(Container, content.image);
            ImageComponent.raycastTarget = false;

            Style = style;
        }
    }
}
