using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public class ToggleModel : StyledSelectableModel<Toggle, IReadOnlyToggleStyle>
    {
        public override Toggle Component { get; }
        public override Image Background { get; }

        public readonly Text Label;

        public readonly GameObject Checkbox;

        public readonly Image Checkmark;

        private readonly GameObject spacer;

        public ToggleModel(GameObject parent, string name, string text)
            : base(UIFactory.CreateToggle(parent, name, out Toggle toggle, out Text label))
        {
            Component = toggle;

            Label = label;
            Label.text = text;
            Object.Destroy(Label.GetComponent<LayoutElement>());

            /*
            spacer = UIFactory.CreateUIObject(UIRoot, "Spacer");
            spacer.transform.SetAsFirstSibling();
            */

            Checkbox = UIFactory.CreateUIObject(UIRoot, "Checkbox");
            //UIFactory.SetLayoutElement(Checkbox, ignoreLayout: true);
            Checkbox.transform.SetAsFirstSibling();

            Background = UIRoot.GetComponentInChildren<Image>();
            Background.transform.SetParent(Checkbox.transform, false);
            UIFactory.SetLayoutElement(Background.gameObject, ignoreLayout: true);

            Checkmark = Background.transform.GetChild(0).GetComponent<Image>();
            Checkmark.transform.SetParent(Checkbox.transform, false);

        }

        public override void ApplyStyle(IReadOnlyToggleStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            ApplyStyle(Component, Checkbox, Background, Checkmark, Label, style, fallbackSkin);
        }

        internal static void ApplyStyle(Toggle component, GameObject checkbox, Image background, Image checkmark, Text label,
            IReadOnlyToggleStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(component.gameObject, style.LayoutGroup);

            style.GetTextStyle(fallbackSkin).ApplyTo(label);

            style.Background.ApplyTo(background);
            UIFactory.SetOffsets(background.gameObject, -style.Overflow);

            style.Background.ApplyTo(component);

            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(checkbox, style.CheckboxPadding, spacing: (int)style.LayoutGroup.Spacing.x);
            if (style.CheckboxSize != Vector2.zero)
            {
                /*
                var checkboxTransform = checkbox.transform.TryCast<RectTransform>();
                checkboxTransform.pivot     = new Vector2(0, 0.5f);
                checkboxTransform.anchorMin = new Vector2(0, 0.5f);
                checkboxTransform.anchorMax = new Vector2(0, 0.5f);
                checkboxTransform.anchoredPosition = Vector2.zero;
                checkboxTransform.sizeDelta = style.CheckboxSize;
                */

                //spacer.gameObject.SetActive(true);
                UIFactory.SetLayoutElement(
                    checkbox,
                    ignoreLayout: false,
                    preferredWidth: (int)style.CheckboxSize.x,
                    preferredHeight: (int)style.CheckboxSize.y,
                    flexibleWidth: 0,
                    flexibleHeight: 0
                );
            }
            else
            {
                //spacer.gameObject.SetActive(false);
                UIFactory.SetLayoutElement(checkbox, ignoreLayout: true);
                UIFactory.SetOffsets(checkbox, Vector4.zero);
            }

            style.Checkmark.ApplyTo(checkmark);
        }
    }
}
