using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public struct LayoutElementStyle : IComponentStyle<LayoutElement>
    {
        public bool IgnoreLayout = false;
        public Vector2 MinSize       = new Vector2(-1f, -1f);
        public Vector2 PreferredSize = new Vector2(-1f, -1f);
        public Vector2 FlexibleSize  = new Vector2(-1f, -1f);

        public float MinWidth        { get => MinSize      .x; set => MinSize      .x = value; }
        public float MinHeight       { get => MinSize      .y; set => MinSize      .y = value; }
        public float PreferredWidth  { get => PreferredSize.x; set => PreferredSize.x = value; }
        public float PreferredHeight { get => PreferredSize.y; set => PreferredSize.y = value; }
        public float FlexibleWidth   { get => FlexibleSize .x; set => FlexibleSize .x = value; }
        public float FlexibleHeight  { get => FlexibleSize .y; set => FlexibleSize .y = value; }

        public LayoutElementStyle()
        { }

        public void ApplyTo(LayoutElement component)
        {
            component.ignoreLayout    = IgnoreLayout   ;
            component.minWidth        = MinWidth       ;
            component.minHeight       = MinHeight      ;
            component.preferredWidth  = PreferredWidth ;
            component.preferredHeight = PreferredHeight;
            component.flexibleWidth   = FlexibleWidth  ;
            component.flexibleHeight  = FlexibleHeight ;
        }

        public void ApplyTo(UIBehaviour component)
        {
            if (component is LayoutElement layoutElement)
            {
                ApplyTo(layoutElement);
            }
        }
    }
}
