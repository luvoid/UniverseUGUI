using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniverseLib.UGUI.Models;

namespace UniverseLib.UGUI
{
    /// <summary>
    /// An implementation of <see cref="IUniversalUGUIObject"/> wrapping a <see cref="IUniversalUGUIBehaviour"/>
    /// </summary>
    internal class UGUIWrapperObject : IUniversalUGUIObject
    {
        private readonly UGUIBase owner;
        public readonly IUniversalUGUIBehaviour behaviour;
        private readonly GameObject contentRoot;
        private readonly Action postUGUI;

        public UGUIWrapperObject(UGUIBase owner, IUniversalUGUIBehaviour behaviour, GameObject contentRoot, Action postUGUI = null)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (contentRoot == null) throw new ArgumentNullException(nameof(contentRoot));

            this.owner = owner;
            this.behaviour = behaviour;
            this.contentRoot = contentRoot;
            this.postUGUI = postUGUI;
        }

        string IUniversalUGUIObject.Name => behaviour.name;
        bool IUniversalUGUIObject.ActiveInHierarchy => behaviour.isActiveAndEnabled;
        bool IUniversalUGUIObject.UseUGUILayout => behaviour.useGUILayout;
        UGUIBase IUniversalUGUIObject.Owner => owner;
        GameObject IUniversalUGUIObject.ContentRoot => contentRoot;
        Dictionary<int, UGUIModel> IUniversalUGUIObject.Models { get; } = new();

        int IUniversalUGUIObject.GetInstanceID()
            => behaviour.GetInstanceID();
        void IUniversalUGUIObject.OnUGUIStart()
        {
            behaviour.OnUGUIStart();
            postUGUI?.Invoke();
        }
        void IUniversalUGUIObject.OnUGUI()
        {
            behaviour.OnUGUI();
            postUGUI?.Invoke();
        }
    }
}
