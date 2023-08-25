using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;
using BaseUniverseLib = UniverseLib;

namespace UniverseLib.UGUI.Models
{
    public class WindowResult : UGUIContentModel, IUniversalUGUIObject
    {
        public override GameObject GameObject => Container;

        private bool useUGUILayout;
        private readonly UGUIBase owner;
        private readonly int windowID;
        private readonly GameObject contentRoot;

        private readonly UGUI.WindowFunction onUGUIStart = null;
        private readonly UGUI.WindowFunction onUGUI = null;
        private readonly bool forceRect;
        private readonly GUISkin skin;

        internal WindowResult(string name, UGUIBase owner, Rect position, int id, UGUI.WindowFunction func, bool forceRect, UGUIContent titleContent, GUIStyle style)
            : base(name, owner.Panels.PanelHolder, position, titleContent, style)
        {
            this.owner = owner;
            this.windowID = id;
            this.forceRect = forceRect;
            contentRoot = UIFactory.CreateUIObject("ContentRoot", Container);
            useUGUILayout = UGUIUtility.s_ActiveUGUI.UseUGUILayout;

            if (UGUIEvent.current.uRawType == UGUIEventType.InitialLayout
                || UGUIEvent.current.uRawType == UGUIEventType.InitialRepaint)
            {
                onUGUIStart = func;
            }
            else
            {
                onUGUI = func;
            }

            owner.AddObject(this);

            _rect = contentRoot.GetComponent<RectTransform>().rect;
            skin = UGUI.skin;

            ApplyStyle(style);
        }

        protected override void ApplyStyle(GUIStyle style)
        {
            base.ApplyStyle(style);

            if (contentRoot != null)
            {
                SetOffsets(contentRoot, style.padding);
            }

            style.AddStyleComponentTo(GameObject);
        }

        internal override void SetState(in Rect position, UGUIContent content, GUIStyle style)
        {
            base.SetState(position, content, style);

            // Also grab the caller's UseUGUILayout state and use it.
            useUGUILayout = UGUIUtility.s_ActiveUGUI.UseUGUILayout;

            // Also reset the _rect
            _rect = contentRoot.GetComponent<RectTransform>().rect;
        }




        bool IUniversalUGUIObject.ActiveInHierarchy => GameObject.activeInHierarchy;
        bool IUniversalUGUIObject.UseUGUILayout => useUGUILayout;
        UGUIBase IUniversalUGUIObject.Owner => owner;
        GameObject IUniversalUGUIObject.ContentRoot => contentRoot;
        Dictionary<int, UGUIModel> IUniversalUGUIObject.Models { get; } = new();
        int IUniversalUGUIObject.GetInstanceID()
            => GameObject.GetInstanceID();
        void IUniversalUGUIObject.OnUGUIStart()
        {
            if (onUGUIStart == null) return;
            UGUI.CallWindowDelegate(
                onUGUIStart,
                windowID,
                GameObject.GetInstanceID(),
                skin,
                forceRect,
                contentRoot.GetComponent<RectTransform>().rect,
                Style
            );
        }
        void IUniversalUGUIObject.OnUGUI()
		{
			if (onUGUI == null) return;
			UGUI.CallWindowDelegate(
				onUGUI,
				windowID,
				GameObject.GetInstanceID(),
				skin,
				forceRect,
				contentRoot.GetComponent<RectTransform>().rect,
				Style
			);
		}




		#region Rect Implementation
#pragma warning disable IDE1006 // Naming Styles

		private Rect _rect;
        public Vector2 min => _rect.min;
        public Vector2 max => _rect.max;
        public Vector2 position { get => _rect.position; set => _rect.position = value; }
        public Vector2 size { get => _rect.size; set => _rect.size = value; }
        public Vector2 center { get => _rect.center; set => _rect.center = value; }
        public float width { get => _rect.width; set => _rect.width = value; }
        public float height { get => _rect.height; set => _rect.height = value; }
        public float x { get => _rect.x; set => _rect.x = value; }
        public float y { get => _rect.y; set => _rect.y = value; }
        public float xMin { get => _rect.xMin; set => _rect.xMin = value; }
        public float xMax { get => _rect.xMax; set => _rect.xMax = value; }
        public float yMin { get => _rect.yMin; set => _rect.yMin = value; }
        public float yMax { get => _rect.yMax; set => _rect.yMax = value; }
        [Obsolete] public float left => _rect.left;
        [Obsolete] public float right => _rect.right;
        [Obsolete] public float top => _rect.top;
        [Obsolete] public float bottom => _rect.bottom;

        public static implicit operator Rect(WindowResult windowResult) => windowResult._rect;

#pragma warning restore IDE1006 // Naming Styles
        #endregion
    }
}
