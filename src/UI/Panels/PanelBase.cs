using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Input;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Panels
{
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    [System.Obsolete($"Use {nameof(Panel)} instead."), Browsable(false)]
    public abstract class PanelBase : Panel
    {
        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        public sealed override IReadOnlyUISkin Skin => null;

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        protected new UIFactory Create => base.Create;

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        public override Vector2 MinSize => new Vector2(MinWidth, MinHeight);

        public abstract int MinWidth { get; }
        public abstract int MinHeight { get; }

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        public override Vector2 PreferredSize => MinSize;

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        internal override GameObject _uiRoot
        {
            get => uiRoot;
            set
            {
                base._uiRoot = value;
                uiRoot = value;
            }
        }
        protected GameObject uiRoot;


        public PanelBase(UIBase owner) : base(owner)
        { }


        // UI Construction

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        protected sealed override GameObject CreateTitleBar(GameObject contentRoot, out GameObject buttonHolder)
        {
            throw new System.NotImplementedException();
        }

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        protected sealed override IButtonModel CreateCloseButton(GameObject titleBar)
        {
            throw new System.NotImplementedException();
        }

        [System.Obsolete("Did not exist in original PanelBase", true), Browsable(false)]
        protected sealed override GameObject CreateBackground(GameObject uiRoot)
        {
            throw new System.NotImplementedException();
        }

        public override void ConstructUI()
        {
            base.LegacyConstructUI();
        }

        public override void SetDefaultSizeAndPosition()
        {
            Rect.anchorMin = DefaultAnchorMin;
            Rect.anchorMax = DefaultAnchorMax;

            Rect.pivot = new Vector2(0f, 1f);
            Rect.anchoredPosition = DefaultPosition;

            LayoutRebuilder.ForceRebuildLayoutImmediate(this.Rect);

            EnsureValidPosition();
            EnsureValidSize();

            Dragger.OnEndResize();
        }
    }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
}
