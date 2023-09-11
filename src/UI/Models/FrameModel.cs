using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public class FrameModel : StyledModel<IReadOnlyFrameStyle>
    {
        public override Image Background { get; }
        public readonly GameObject ContentRoot;

        /// <summary>
        /// Constructor that creates a simple frame object.
        /// It creates the UIRoot with a <see cref="LayoutAutoSize"/>, the <see cref="Background"/>,
        /// and the <see cref="ContentRoot"/>.
        /// </summary>
        public FrameModel(GameObject parent, string name, Vector2 sizeDelta = default)
            : base(parent, name, sizeDelta)
        {
            Background = UIFactory.CreateUIObject(UIRoot, "Background").AddComponent<Image>();
            ContentRoot = UIFactory.CreateUIObject(UIRoot, "ContentHolder");
            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            if (style.UseBackground)
            {
                Background.enabled = true;
                style.Background.ApplyTo(Background);
            }
            else
            {
                Background.enabled = false;
            }
            UIFactory.SetOffsets(Background.gameObject, -style.Overflow);

            UIFactory.SetOffsets(ContentRoot, Vector4.zero);
        }
    }
}
