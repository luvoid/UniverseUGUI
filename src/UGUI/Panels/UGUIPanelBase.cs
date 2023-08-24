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
        public virtual GUISkin Skin { get; set; } = UGUIUtility.GetDefaultSkin();
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
                Skin.window.ApplyToBackground(UIRoot.GetComponent<Graphic>());

                if (ContentRoot != null)
                {
                    var contentRootGraphic = ContentRoot.GetComponent<Graphic>();
                    if (contentRootGraphic != null)
                        contentRootGraphic.enabled = false;
                }

                if (TitleBar != null)
                {
                    Skin.box.ApplyToBackground(TitleBar.GetComponent<Graphic>());

                    var titleText = TitleBar.GetComponentInChildren<Text>();
                    if (titleText != null)
                        Skin.label.ApplyToText(titleText, Skin.font);

                    var closeButton = TitleBar.GetComponentInChildren<Button>();
                    if (closeButton != null)
                    {
                        Skin.button.ApplyToBackground(closeButton.targetGraphic);
                        Skin.button.ApplyToSelectable(closeButton);

                        var closeText = closeButton.GetComponentInChildren<Text>();
                        if (closeText != null)
                            Skin.button.ApplyToText(closeText, Skin.font);
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
            OnUGUIStart();
            OnUGUIContentRootLayout();
        }
        void IUniversalUGUIObject.OnUGUI()
        {
            OnUGUI();
            OnUGUIContentRootLayout();
        }
        protected abstract void OnUGUIStart();
        protected abstract void OnUGUI();

        protected virtual void OnUGUIContentRootLayout()
        {
            if (!UseUGUILayout) return;
            if (UseUGUILayout)
            {
                UIFactory.SetLayoutElement(UGUIContentRoot,
                    minWidth: (int)UGUILayoutUtility.topLevel.minWidth,
                    minHeight: (int)UGUILayoutUtility.topLevel.minHeight,
                    preferredWidth: (int)UGUILayoutUtility.topLevel.maxWidth,
                    preferredHeight: (int)UGUILayoutUtility.topLevel.maxHeight
                );
            }
        }
    }
}
