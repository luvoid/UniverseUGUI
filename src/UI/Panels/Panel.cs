﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Panels
{
    public abstract class Panel : UIBehaviourModel
    {
        public UIBase Owner { get; }

        public abstract string Name { get; }

        public virtual IReadOnlyUISkin Skin => UISkin.Default;
        public virtual IReadOnlyWindowStyle Style => Skin?.Window ?? UISkin.Default.Window;

        /// <summary>
        /// An instance of a <see cref="UIFactory"/> using the panel's Skin.
        /// </summary>
        protected UIFactory Create => _skinnedFactory ??= UIFactory.CreateSkinnedFactory(Skin);
        private UIFactory _skinnedFactory;

        public virtual Vector2 MinSize => new Vector2(LayoutUtility.GetMinSize(Rect, 0), LayoutUtility.GetMinSize(Rect, 1));
        public virtual Vector2 PreferredSize => new Vector2(LayoutUtility.GetPreferredSize(Rect, 0), LayoutUtility.GetPreferredSize(Rect, 1));
        public abstract Vector2 DefaultAnchorMin { get; }
        public abstract Vector2 DefaultAnchorMax { get; }
        public virtual Vector2 DefaultPosition { get; }

        public virtual bool CanDragAndResize => true;
        public PanelDragger Dragger { get; internal set; }

        public sealed override GameObject UIRoot => _uiRoot;
        /// <summary> This should only be used by <see cref="PanelBase"/> for backwards compatibility. </summary>
        internal virtual GameObject _uiRoot { get; set; }

        public RectTransform Rect { get; private set; }
        public GameObject ContentRoot { get; protected set; }

        public GameObject TitleBar { get; private set; }

        public Panel(UIBase owner)
        {
            Owner = owner;

            ConstructUI();

            // Add to owner
            Owner.Panels.AddPanel(this);
        }

        public override void Destroy()
        {
            Owner.Panels.RemovePanel(this);
            base.Destroy();
        }

        public virtual void OnFinishResize()
        {
        }

        public virtual void OnFinishDrag()
        {
        }

        public override void SetActive(bool active)
        {
            if (this.Enabled != active)
                base.SetActive(active);

            if (!active)
                this.Dragger.WasDragging = false;
            else
            {
                this.UIRoot.transform.SetAsLastSibling();
                this.Owner.Panels.InvokeOnPanelsReordered();
            }
        }

        protected virtual void OnClosePanelClicked()
        {
            this.SetActive(false);
        }

        // Setting size and position

        public virtual void SetDefaultSizeAndPosition()
        {
            Rect.anchorMin = DefaultAnchorMin;
            Rect.anchorMax = DefaultAnchorMax;

            LayoutRebuilder.ForceRebuildLayoutImmediate(this.Rect);

            Rect.pivot = new Vector2(0f, 1f);
            Rect.anchoredPosition = DefaultPosition;

            Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PreferredSize.x);
            Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical  , PreferredSize.y);

            EnsureValidPosition();
            EnsureValidSize();

            Dragger.OnEndResize();
        }

        public virtual void EnsureValidSize()
        {
            if (Rect.rect.width < MinSize.x)
                Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MinSize.x);

            if (Rect.rect.height < MinSize.y)
                Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MinSize.y);

            Dragger.OnEndResize();
        }

        public virtual void EnsureValidPosition()
        {
            // Prevent panel going oustide screen bounds

            Vector3 pos = this.Rect.localPosition;
            Vector2 dimensions = Owner.Panels.ScreenDimensions;

            float halfW = dimensions.x * 0.5f;
            float halfH = dimensions.y * 0.5f;

            pos.x = Mathf.Max(-halfW - this.Rect.rect.width + 50, Mathf.Min(pos.x, halfW - 50));
            pos.y = Mathf.Max(-halfH + 50, Mathf.Min(pos.y, halfH));

            this.Rect.localPosition = pos;
        }

        // UI Construction

        protected abstract void ConstructPanelContent();

        protected virtual GameObject CreateTitleBar(GameObject parent, out GameObject buttonHolder)
        {
            IReadOnlyUISkin skin = Skin ?? UISkin.Default;
            IReadOnlyWindowStyle windowStyle = Style ?? UISkin.Default.Window;
            IReadOnlyFrameStyle titlebarStyle = windowStyle.Titlebar ?? UISkin.Default.Window.Titlebar;

            // Title bar
            var titleBar = Create.HorizontalFrame(parent, "TitleBar", style: titlebarStyle);
            UIFactory.SetLayoutElement(titleBar.ContentRoot, 
                minHeight: windowStyle.TitlebarHeight, preferredHeight: windowStyle.TitlebarHeight, 
                flexibleWidth: 1, flexibleHeight: 0);

            buttonHolder = titleBar.ContentRoot;

            // Title text
            GameObject titleHolder = Create.HorizontalGroup(
                titleBar.GameObject, "Title", true, true, true, true,
                style: titlebarStyle.LayoutGroup
            );
            UIFactory.SetLayoutElement(titleHolder,
                minHeight: windowStyle.TitlebarHeight, preferredHeight: windowStyle.TitlebarHeight,
                flexibleWidth: 1, flexibleHeight: 0);
            UIFactory.SetOffsets(titleHolder.gameObject, Vector4.zero);
            Text titleTxt = Create.UIObject(titleHolder, "Label").AddComponent<Text>();
            titleTxt.text = Name;
            titlebarStyle.GetTextStyle(skin).ApplyTo(titleTxt);

            buttonHolder.transform.SetAsLastSibling();

            return titleBar.GameObject;
        }

        protected virtual IButtonModel CreateCloseButton(GameObject parent)
        {
            IReadOnlyUISkin skin = Skin ?? UISkin.Default;
            IReadOnlyWindowStyle windowStyle = Style ?? UISkin.Default.Window;

            ButtonModel closeBtn = Create.Button(parent, "CloseButton", "—", skin.Button);
            int btnSqueeze = (int)windowStyle.Titlebar.LayoutGroup.Padding.z + (int)windowStyle.Titlebar.LayoutGroup.Padding.w;
            int btnHeight = Mathf.Min(windowStyle.TitlebarHeight - btnSqueeze, 25);
            UIFactory.SetLayoutElement(closeBtn.Component.gameObject, btnHeight, btnHeight, 0, 0, btnHeight, btnHeight);

            return closeBtn;
        }

        protected virtual GameObject CreateBackground(GameObject parent)
        {
            IReadOnlyWindowStyle style = Skin?.Window ?? UISkin.Default.Window;

            Image background = UIFactory.CreateUIObject(parent, "Background").AddComponent<Image>();

            style.Background.ApplyTo(background);
            UIFactory.SetOffsets(background.gameObject, -style.Overflow);

            return background.gameObject;
        }

        protected virtual PanelDragger CreatePanelDragger() => new(this);

        public virtual void ConstructUI()
        {
            IReadOnlyWindowStyle windowStyle = Style ?? UISkin.Default.Window;

            // Create Core Canvas
            _uiRoot = Create.UIObject(Owner.Panels.PanelHolder, Name);
            _uiRoot.AddComponent<RectMask2D>();
            Rect = UIRoot.GetComponent<RectTransform>();
            UIFactory.SetLayoutGroup_<VerticalLayoutGroup>(UIRoot, false, false, true, true, 0, new Vector4(0, 0, 0, 0), TextAnchor.UpperLeft);

            // Background
            GameObject background = CreateBackground(UIRoot);
            UIFactory.SetLayoutElement(background.gameObject, ignoreLayout: true);
            foreach (var image in background.GetComponentsInChildren<Image>())
            {
                image.maskable = false;
            }

            // Title Bar (abstract)
            TitleBar = CreateTitleBar(UIRoot, out GameObject buttonHolder);

            // Close Button (abstract)
            IButtonModel closeBtn = CreateCloseButton(buttonHolder);
            closeBtn.OnClick += () =>
            {
                OnClosePanelClicked();
            };
            if (!CanDragAndResize)
                TitleBar.SetActive(false);

            // Panel dragger
            Dragger = CreatePanelDragger();
            Dragger.OnFinishResize += OnFinishResize;
            Dragger.OnFinishDrag += OnFinishDrag;

            // Content (abstract)
            ContentRoot = Create.UIObject(UIRoot, "ContentHolder");
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(ContentRoot, windowStyle.LayoutGroup);
            try
            {
                ConstructPanelContent();
            }
            catch (System.Exception ex)
            {
                Universe.Logger.LogException(ex, UIRoot);
            }
            SetDefaultSizeAndPosition();

            RuntimeHelper.StartCoroutine(LateSetupCoroutine());
        }

        [System.Obsolete("Should only be used by PanelBase.")]
        internal void LegacyConstructUI()
        {
            // create core canvas 
            _uiRoot = UIFactory.CreatePanel(Name, Owner.Panels.PanelHolder, out GameObject contentRoot);
            ContentRoot = contentRoot;
            Rect = this._uiRoot.GetComponent<RectTransform>();

            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(this.ContentRoot, false, false, true, true, 2, 2, 2, 2, 2, TextAnchor.UpperLeft);
            UIFactory.SetLayoutElement(ContentRoot, 0, 0, flexibleWidth: 9999, flexibleHeight: 9999);

            // Title bar
            TitleBar = UIFactory.CreateHorizontalGroup(ContentRoot, "TitleBar", false, true, true, true, 2,
                new Vector4(2, 2, 2, 2), new Color(0.06f, 0.06f, 0.06f));
            UIFactory.SetLayoutElement(TitleBar, minHeight: 25, flexibleHeight: 0);


            // Title text

            Text titleTxt = UIFactory.CreateLabel(TitleBar, "TitleBar", Name, TextAnchor.MiddleLeft);
            UIFactory.SetLayoutElement(titleTxt.gameObject, 50, 25, 9999, 0);

            // close button

            GameObject closeHolder = UIFactory.CreateUIObject("CloseHolder", TitleBar);
            UIFactory.SetLayoutElement(closeHolder, minHeight: 25, flexibleHeight: 0, minWidth: 30, flexibleWidth: 9999);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(closeHolder, false, false, true, true, 3, childAlignment: TextAnchor.MiddleRight);
            ButtonRef closeBtn = UIFactory.CreateButton(closeHolder, "CloseButton", "—");
            UIFactory.SetLayoutElement(closeBtn.Component.gameObject, minHeight: 25, minWidth: 25, flexibleWidth: 0);
            RuntimeHelper.SetColorBlock(closeBtn.Component, new Color(0.33f, 0.32f, 0.31f));

            closeBtn.OnClick += () =>
            {
                OnClosePanelClicked();
            };

            if (!CanDragAndResize)
                TitleBar.SetActive(false);

            // Panel dragger

            Dragger = CreatePanelDragger();
            Dragger.OnFinishResize += OnFinishResize;
            Dragger.OnFinishDrag += OnFinishDrag;

            // content (abstract)

            ConstructPanelContent();
            SetDefaultSizeAndPosition();

            RuntimeHelper.StartCoroutine(LateSetupCoroutine());
        }

        private IEnumerator LateSetupCoroutine()
        {
            yield return new WaitUntil(() => UIRoot == null || UIRoot.activeInHierarchy);

            LateConstructUI();
        }

        protected virtual void LateConstructUI()
        {
            SetDefaultSizeAndPosition();
        }

        [System.Obsolete("Not used. Use ConstructUI() instead.", true)]
        public override void ConstructUI(GameObject parent) => ConstructUI();
    }

    [System.Obsolete($"Use {nameof(Panel)} instead.", true)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [System.ComponentModel.Browsable(false)]
    public abstract class SkinnedPanelBase : Panel
    {
        protected SkinnedPanelBase(UIBase owner) : base(owner)
        { }
    }
}
