using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Styles;
using System.ComponentModel;

namespace UniverseLib.UI.Models
{
    public abstract class LayoutFrameModel : FrameModel, IComponentModel<LayoutGroup>
    {
        public LayoutGroup Component => LayoutGroup;
        protected abstract LayoutGroup LayoutGroup { get; }

        internal LayoutFrameModel(GameObject parent, string name) : base(parent, name)
        { }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            base.ApplyStyle(style, fallbackSkin);
            style.LayoutGroup.ApplyTo(Component);
        }
    }

    public class LayoutFrameModel<T> : LayoutFrameModel, IComponentModel<T>
        where T : LayoutGroup, new()
    {
        public new T Component { get; }
        protected sealed override LayoutGroup LayoutGroup => Component;

        public LayoutFrameModel(GameObject parent, string name) : base(parent, name)
        {
            Component = ContentRoot.AddComponent<T>();
        }
    }
}
