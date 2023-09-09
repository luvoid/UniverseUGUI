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
    [System.Obsolete($"Use {nameof(Panel)} instead."), Browsable(false)]
    public abstract class PanelBase : Panel
    {
        public sealed override IReadOnlyUISkin Skin => null;

        public PanelBase(UIBase owner) : base(owner)
        { }

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

        // UI Construction

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        [System.Obsolete("Did not exist in original PanelBase"), Browsable(false)]
        protected sealed override GameObject CreateTitleBar(GameObject contentRoot)
        {
            throw new System.NotImplementedException();
        }

        [System.Obsolete("Did not exist in original PanelBase"), Browsable(false)]
        protected sealed override IButtonRef CreateCloseButton(GameObject titleBar)
        {
            throw new System.NotImplementedException();
        }

        [System.Obsolete("Did not exist in original PanelBase"), Browsable(false)]
        protected sealed override GameObject CreateBackground(GameObject uiRoot)
        {
            throw new System.NotImplementedException();
        }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        public override void ConstructUI()
        {
            base.LegacyConstructUI();
        }
    }
}
