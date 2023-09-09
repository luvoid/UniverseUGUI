using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public class StyledFrame : UIStyledModel<IReadOnlyFrameStyle>
    {
        public override Image Background { get; }

        public readonly GameObject ContentRoot;


        public StyledFrame(GameObject parent, string name) : base(parent, name)
        {
            Background = UIFactory.CreateUIObject(UIRoot, "Background").AddComponent<Image>();
            ContentRoot = UIFactory.CreateUIObject(UIRoot, "ContentHolder");
            UIFactory.SetLayoutAutoSize(ContentRoot);
            UIFactory.SetLayoutAutoSize(UIRoot);
        }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            ApplyStyle(Background, ContentRoot, style, fallbackSkin);
        }

        internal static void ApplyStyle(Image background, GameObject contentRoot, IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            style.Background.ApplyTo(background);
            SetOffsets(background.gameObject, -style.Overflow);

            SetOffsets(contentRoot, Vector4.zero);
        }
    }
}
