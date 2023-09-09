using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public abstract class StyledLayoutFrame : StyledComponent<LayoutGroup, IReadOnlyFrameStyle>
    {
        public override Image Background { get; }
        public override LayoutGroup Component => _layoutGroup;

        public GameObject ContentRoot { get; }

        protected abstract LayoutGroup _layoutGroup { get; }

        public StyledLayoutFrame(GameObject parent, string name) : base(parent, name)
        {
            Background = UIFactory.CreateUIObject(UIRoot, "Background").AddComponent<Image>();
            ContentRoot = UIFactory.CreateUIObject(UIRoot, "ContentHolder");
            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            StyledLayoutFrame.ApplyStyle(Background, ContentRoot, Component, style, fallbackSkin);
        }

        internal static void ApplyStyle(Image background, GameObject contentRoot, LayoutGroup component, IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            StyledFrame.ApplyStyle(background, contentRoot, style, fallbackSkin);
            style.LayoutGroup.ApplyTo(component);
        }
    }

    public class StyledLayoutFrame<T> : StyledLayoutFrame
        where T : LayoutGroup, new()
    {
        public new readonly T Component;

        protected override LayoutGroup _layoutGroup => Component;

        public StyledLayoutFrame(GameObject parent, string name) : base(parent, name)
        {
            Component = ContentRoot.AddComponent<T>();
        }
    }
}
