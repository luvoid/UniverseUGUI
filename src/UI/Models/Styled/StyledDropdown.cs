using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledDropdown : StyledSelectable<Dropdown, IReadOnlyDropdownStyle>
    {
        public System.Action<int> OnValueChanged;

        public override Dropdown Component { get; }
        public override Image Background { get; }

        public int Value { get => Component.value; set => Component.value = value; }

        private readonly Text label;

        private readonly Text arrowText;
        private readonly Image arrow;

        private readonly Transform template;
        private readonly ScrollRect scrollRect;
        private readonly Image viewportBackground;
        private readonly Image viewportMask;
        private readonly Scrollbar scrollbar;

        private readonly Toggle itemToggle;
        private readonly GameObject itemCheckbox;
        private readonly Image itemBackground;
        private readonly Image itemCheckmark;
        private readonly Text itemLabel;


        private static GameObject CreateUIRoot(GameObject parent, string name, out Dropdown dropdown)
        {
            var uiRoot = UIFactory.CreateDropdown(parent, name, out dropdown, "", 14, null);
            Object.Destroy(uiRoot.GetComponent<Image>());
            return uiRoot;
        }

        public StyledDropdown(GameObject parent, string name, int defaultValue = 0, IEnumerable<string> options = null) 
            : base(CreateUIRoot(parent, name, out Dropdown dropdown))
        {
            Component = dropdown;
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(GameObject, forceWidth: false, forceHeight: false, childAlignment: TextAnchor.MiddleLeft);

            Background = UIFactory.CreateUIObject("Background", UIRoot).AddComponent<Image>();
            Background.transform.SetAsFirstSibling();
            UIFactory.SetLayoutElement(Background.gameObject, ignoreLayout: true);
            Component.targetGraphic = Background;

            label = UIRoot.transform.FindChild("Label").GetComponent<Text>();
            UIFactory.SetLayoutElement(label.gameObject, flexibleWidth: 2);

            arrowText = UIRoot.transform.FindChild("Arrow").GetComponent<Text>();
            UIFactory.SetLayoutElement(arrowText.gameObject, preferredWidth: 20, preferredHeight: 20);
            arrow = UIFactory.CreateUIObject("ArrowImage", arrowText.gameObject).AddComponent<Image>();
            SetOffsets(arrow.gameObject, Vector4.zero);

            RectTransform arrowRect = arrowText.transform.TryCast<RectTransform>();
            arrowRect.anchorMin = new Vector2(1, 0.5f);
            arrowRect.anchorMax = new Vector2(1, 0.5f);
            arrowRect.pivot = new Vector2(1, 0.5f);
            arrowRect.anchoredPosition = Vector3.zero;
            arrowRect.sizeDelta = new Vector2(20, 20);

            template = UIRoot.transform.FindChild("Template");
            UIFactory.SetLayoutElement(template.gameObject, ignoreLayout: true);
            Object.Destroy(template.GetComponent<Image>());
            scrollRect = template.GetComponent<ScrollRect>();
            var viewport = template.Find("Viewport");
            viewportMask = viewport.GetComponent<Image>();
            viewportBackground = UIFactory.CreateUIObject("Viewport Background", template.gameObject).AddComponent<Image>();
            viewportBackground.transform.SetAsFirstSibling();

            scrollbar = template.GetComponentInChildren<Scrollbar>();

            itemToggle = viewport.FindChild("Content").FindChild("Item").GetComponent<Toggle>();
            itemCheckbox = UIFactory.CreateUIObject("Item Checkbox", itemToggle.gameObject);
            itemCheckbox.transform.SetAsFirstSibling();
            itemBackground = itemToggle.transform.FindChild("Item Background").GetComponent<Image>();
            itemBackground.transform.SetParent(itemCheckbox.transform, false);
            UIFactory.SetLayoutElement(itemBackground.gameObject, ignoreLayout: true);
            itemCheckmark = itemToggle.transform.FindChild("Item Checkmark").gameObject.AddComponent<Image>();
            itemCheckmark.transform.SetParent(itemCheckbox.transform, false);
            itemToggle.graphic = itemCheckmark;
            itemLabel = itemToggle.transform.FindChild("Item Label").GetComponent<Text>();

            if (options != null)
            {
                foreach (string option in options)
                {
                    Component.options.Add(new Dropdown.OptionData(option));
                }
            }

            Value = defaultValue;
            Component.RefreshShownValue();
            Component.onValueChanged.AddListener((value) => OnValueChanged?.Invoke(value));
        }


        public override void ApplyStyle(IReadOnlyDropdownStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(Component);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(GameObject, style.LayoutGroup.Padding);

            style.Background.ApplyTo(Background);
            SetOffsets(Background.gameObject, -style.Overflow);

            var textStyle = style.GetTextStyle(fallbackSkin);
            textStyle.ApplyTo(label);
            SetOffsets(label.gameObject, style.LayoutGroup.Padding, style.LabelOffset);

            style.Viewport.Background.ApplyTo(viewportBackground);
            style.Viewport.Background.ApplyTo(viewportMask);
            SetOffsets(viewportBackground.gameObject, -style.Viewport.Overflow);
            SetOffsets(viewportMask.gameObject, -style.Viewport.Overflow);

            StyledToggle.ApplyStyle(
                itemToggle, itemCheckbox, itemBackground, itemCheckmark, itemLabel, 
                style.Item, fallbackSkin
            );

            if (style.Arrow.Sprite == DropdownStyle.DefaultArrow)
            {
                arrow.enabled = false;
                arrowText.enabled = true;

                arrowText.fontSize = textStyle.FontSize;
                arrowText.color = textStyle.Color;
                arrowText.horizontalOverflow = HorizontalWrapMode.Overflow;
                arrowText.verticalOverflow = VerticalWrapMode.Overflow;
                arrowText.alignment = TextAnchor.MiddleCenter;
            }
            else if (style.Arrow.Sprite == null)
            {
                arrow.enabled = false;
                arrowText.enabled = false;
            }
            else
            {
                arrow.enabled = true;
                arrowText.enabled = false;

                style.Arrow.ApplyTo(arrow);
            }
        }
    }
}
