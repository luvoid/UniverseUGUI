using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledPanel : UIStyledModel<IReadOnlyPanelStyle>
    {
        public GameObject ContentRoot { get; }
        public override Image Background { get; }

        public GameObject GameObject => UIRoot;

        public StyledPanel(GameObject parent, string name) : base(parent, name)
        {
            Background = UIFactory.CreateUIObject("Background", UIRoot).AddComponent<Image>();

            ContentRoot = UIFactory.CreateUIObject("ContentHolder", UIRoot);
            ContentRoot.AddComponent<RectMask2D>();
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(ContentRoot, true, true, true, true, 0);

            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyPanelStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(Background);
            SetOffsets(Background.gameObject, -style.Overflow);

            SetOffsets(ContentRoot, Vector4.zero);
            style.LayoutGroup.ApplyTo(ContentRoot.GetComponent<LayoutGroup>());
        }
    }
}
