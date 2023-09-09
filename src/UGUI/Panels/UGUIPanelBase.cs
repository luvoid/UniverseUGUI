using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UGUI.Models;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using static UnityEngine.UI.GridLayoutGroup;

namespace UniverseLib.UGUI.Panels
{
    public abstract class UGUIPanelBase : PanelBase, IUniversalUGUIObject
    {
        private UGUISkin _skin = null;
        public new UGUISkin Skin { get => _skin ?? Owner.Skin ?? UGUIUtility.GetDefaultSkin(); set => _skin = value; }
        public new UGUIBase Owner => base.Owner as UGUIBase;
        public virtual bool UseUGUILayout { get; set; } = true;
        public GameObject UGUIContentRoot { get; protected set; }
        GameObject IUniversalUGUIObject.ContentRoot => UGUIContentRoot;
        bool IUniversalUGUIObject.ActiveInHierarchy => UIRoot.activeInHierarchy;
        Dictionary<int, UGUIModel> IUniversalUGUIObject.Models { get; } = new();

        int IUniversalUGUIObject.GetInstanceID()
            => UIRoot.GetInstanceID();

        public UGUIPanelBase(UGUIBase owner)
            : base(owner)
        { }

        protected override void ConstructPanelContent()
        {
            if (Skin != null)
            {
                Skin.Window?.ApplyToBackground(UIRoot.GetComponent<Graphic>());

                if (ContentRoot != null)
                {
                    var contentRootGraphic = ContentRoot.GetComponent<Graphic>();
                    if (contentRootGraphic != null)
                        contentRootGraphic.enabled = false;
                }

                if (TitleBar != null)
                {
                    Skin.Box?.ApplyToBackground(TitleBar.GetComponent<Graphic>());

                    var titleText = TitleBar.GetComponentInChildren<Text>();
                    if (titleText != null)
                        Skin.Label?.ApplyToText(titleText, Skin);

                    var closeButton = TitleBar.GetComponentInChildren<Button>();
                    if (closeButton != null)
                    {
                        Skin.Button?.ApplyToBackground(closeButton.targetGraphic);
                        Skin.Button?.ApplyToSelectable(closeButton);

                        var closeText = closeButton.GetComponentInChildren<Text>();
                        if (closeText != null)
                            Skin.Button?.ApplyToText(closeText, Skin);
                    }
                }
            }
        }

        protected override void LateConstructUI()
        {
            Owner.AddObject(this);
            UGUIContentRoot = UIFactory.CreateUIObject("UGUIContentRoot", ContentRoot);
            base.LateConstructUI();
        }

        void IUniversalUGUIObject.OnUGUIStart()
        {
            UGUI.skin = Skin;
            OnUGUIStart();
            OnUGUIContentRootLayout();
        }
        void IUniversalUGUIObject.OnUGUI()
        {
            UGUI.skin = Skin;
            OnUGUI();
            OnUGUIContentRootLayout();
        }
        protected abstract void OnUGUIStart();
        protected abstract void OnUGUI();

        protected virtual void OnUGUIContentRootLayout()
        {
            if (!UseUGUILayout) return;
            UIFactory.SetLayoutElement(UGUIContentRoot,
                minWidth: (int)UGUILayoutUtility.topLevel.minWidth,
                minHeight: (int)UGUILayoutUtility.topLevel.minHeight,
                preferredWidth: (int)UGUILayoutUtility.topLevel.maxWidth,
                preferredHeight: (int)UGUILayoutUtility.topLevel.maxHeight
            );
        }
    }
}
