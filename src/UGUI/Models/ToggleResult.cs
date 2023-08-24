using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UGUI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using BaseUniverseLib = UniverseLib;

namespace UniverseLib.UGUI.Models
{
    public sealed class ToggleResult : UGUISelectableModel<Toggle>
    {
        public Image BackgroundImage => BackgroundComponent as Image;
        public Image CheckmarkImage => Component.graphic as Image;

        internal ToggleResult(string name, GameObject parent, Rect position, bool value, UGUIContent content, GUIStyle style)
            : base(name, parent, position)
        {
            UIFactory.CreateToggle(Container, "ToggleControl", out Toggle toggle, out Text text);
            Object.Destroy(toggle.GetComponent<HorizontalLayoutGroup>());

            Component = toggle;
            Component.isOn = value;

            TextComponent = text;
            Text = content.text;

            ImageComponent = CreateImage(Container, content.image);

            BackgroundComponent = toggle.targetGraphic;
            BackgroundImage.type = Image.Type.Sliced;
            CheckmarkImage.type = Image.Type.Sliced;

            Style = style;
        }

        protected override void ApplyStyle(GUIStyle style)
        {
            ApplyTextStyle(style);

            SetOffsets(Component.gameObject, new RectOffset());

            // TODO : Add support for using the sprites.
            Sprite bgSprite = null; //style.GetBackgroundSprite(BackgroundImage.sprite);
            if (bgSprite != null)
            {
                BackgroundImage.color = Color.white;
                CheckmarkImage.color = Color.white;

                ApplyBackgroundStyle(style);
                ApplySelectableStyle(style);
            }
            else
            {
                BackgroundImage.sprite = null;
                BackgroundImage.overrideSprite = null;
                BackgroundImage.color = new Color32(0x0A, 0x0A, 0x0A, 0xBF);

                CheckmarkImage.sprite = null;
                CheckmarkImage.overrideSprite = null;
                CheckmarkImage.color = style.normal.textColor;

                var bgTransform = BackgroundComponent.GetComponent<RectTransform>();
                bgTransform.anchorMin = new Vector2(0, 1);
                bgTransform.anchorMax = new Vector2(0, 1);
                bgTransform.pivot = new Vector2(0, 1);
                bgTransform.anchoredPosition = new Vector2(-style.overflow.left, style.overflow.top);
                bgTransform.sizeDelta = new Vector2(style.border.left, style.border.top);

                style.ApplyToSelectable(Component, useSprites: false);
            }

            style.AddStyleComponentTo(Container);
        }


        internal void SetState(in Rect position, bool value, UGUIContent content, GUIStyle style)
        {
            SetState(position, content, style);
            Component.isOn = value;
        }


        public static implicit operator bool(ToggleResult toggleResult)
        {
            return toggleResult.Component.isOn;
        }
    }
}
