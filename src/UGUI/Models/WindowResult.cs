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
        public override GameObject GameObject { get => Container; }

        private bool useUGUILayout;
        private readonly UGUIBase owner;
        private readonly int windowID;
        private readonly GameObject contentRoot;

        private readonly UGUI.WindowFunction onUGUIStart = null;
        private readonly UGUI.WindowFunction onUGUI = null;

        internal WindowResult(string name, UGUIBase owner, Rect position, int id, UGUI.WindowFunction func, UGUIContent titleContent, GUIStyle style)
            : base(name, owner.Panels.PanelHolder, position, titleContent, style)
        {
            this.owner = owner;
            windowID = id;
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

            ApplyStyle(style);
        }

        protected override void ApplyStyle(GUIStyle style)
        {
            base.ApplyStyle(style);

            if (contentRoot != null)
            {
                SetOffsets(contentRoot, style.padding);
            }
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
            => onUGUIStart?.Invoke(windowID);
        void IUniversalUGUIObject.OnUGUI()
            => onUGUI?.Invoke(windowID);




        #region Rect Implementation

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

        #endregion
    }
}
