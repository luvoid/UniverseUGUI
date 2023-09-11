using UnityEngine;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public class AutoFrameModel : FrameModel
    {
        public AutoFrameModel(GameObject parent, string name) : base(parent, name)
        {
            UIFactory.SetLayoutAutoSize(ContentRoot);
        }

        public override void ApplyStyle(IReadOnlyFrameStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            base.ApplyStyle(style, fallbackSkin);
            UIFactory.SetOffsets(ContentRoot, style.LayoutGroup.Padding);
        }
    }
}
