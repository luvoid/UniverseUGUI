using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Panels
{
    public abstract class SkinnedPanelBase : PanelBase
    {
        public abstract IReadOnlyUISkin Skin { get; }

        /// <summary>
        /// An instance of a <see cref="UIFactory"/> using the panel's Skin.
        /// </summary>
        protected UIFactory Create => _skinnedFactory ??= UIFactory.CreateSkinnedFactory(Skin);
        private UIFactory _skinnedFactory;

        protected SkinnedPanelBase(UIBase owner) : base(owner)
        { }

        protected override GameObject CreateTitleBar(GameObject contentRoot)
        {
            IReadOnlyUISkin skin = Skin ?? UISkin.Default;
            IReadOnlyWindowStyle windowStyle = skin?.Window ?? UISkin.Default.Window;

            // Title bar
            var titleBar = UIFactory.CreateHorizontalGroup(
                contentRoot, "TitleBar", false, true, true, true, 2,
                new Vector4(2, 2, 2, 2), new Color(0.06f, 0.06f, 0.06f)
            );
            titleBar.GetComponent<Image>().enabled = false;
            UIFactory.SetLayoutElement(titleBar, minHeight: windowStyle.TitlebarHeight, flexibleHeight: 0);

            // Title text
            Text titleTxt = UIFactory.CreateLabel(titleBar, "Title", Name);
            windowStyle.GetTextStyle(skin).ApplyTo(titleTxt);
            titleTxt.alignment = TextAnchor.MiddleLeft;
            UIFactory.SetLayoutElement(titleTxt.gameObject, 50, windowStyle.TitlebarHeight, 9999, 0);

            return titleBar;
        }

        protected override IButtonRef CreateCloseButton(GameObject titleBar)
        {
            IReadOnlyUISkin skin = Skin ?? UISkin.Default;
            IReadOnlyWindowStyle windowStyle = skin?.Window ?? UISkin.Default.Window;

            GameObject closeHolder = UIFactory.CreateUIObject("CloseHolder", titleBar);
            UIFactory.SetLayoutElement(closeHolder, minHeight: windowStyle.TitlebarHeight, flexibleHeight: 0, minWidth: 30, flexibleWidth: 9999);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(closeHolder, false, false, true, true, 3, childAlignment: TextAnchor.MiddleRight);

            StyledButton closeBtn = Create.Button(closeHolder, "CloseButton", "—", skin.Button);
            int btnHeight = Mathf.Min(windowStyle.TitlebarHeight, 25);
            UIFactory.SetLayoutElement(closeBtn.Component.gameObject, btnHeight, btnHeight, flexibleWidth: 0);

            return closeBtn;
        }

        protected virtual GameObject CreateBackground(GameObject uiRoot)
        {
            IReadOnlyWindowStyle style = Skin?.Window ?? UISkin.Default.Window;

            Image background = UIFactory.CreateUIObject("Background", uiRoot).AddComponent<Image>();
            UIFactory.SetLayoutElement(background.gameObject, ignoreLayout: true);

            style.Background.ApplyTo(background);

            RectTransform rect = background.transform.TryCast<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(-style.Overflow.x, -style.Overflow.w);
            rect.offsetMax = new Vector2( style.Overflow.y,  style.Overflow.z);

            return background.gameObject;
        }

        public override void ConstructUI()
        {
            base.ConstructUI();

            Object.Destroy(UIRoot.GetComponent<Image>());
            Object.Destroy(UIRoot.GetComponent<Image>());
            Object.Destroy(ContentRoot.GetComponent<Image>());
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(UIRoot, true, true, true, true, 0, 0, 0, 0, 0);

            IReadOnlyWindowStyle style = Skin?.Window ?? UISkin.Default.Window;
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(ContentRoot, style.LayoutGroup);

            GameObject background = CreateBackground(UIRoot);
            background.transform.SetAsFirstSibling();
            foreach (var image in background.GetComponentsInChildren<Image>())
            {
                image.maskable = false;
            }
            
        }


    }
}
