using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniverseLib.UGUI;

namespace UniverseLib.UI.Styles
{
    public struct RectSizeBool
    {
        public bool Width = false;
        public bool Height = false;

        public RectSizeBool() { }

        public RectSizeBool(bool width, bool height)
        {
            Width = width;
            Height = height;
        }
    }

    public struct LayoutGroupStyle : IComponentStyle<LayoutGroup>, IComponentStyle<HorizontalOrVerticalLayoutGroup>, IComponentStyle<HorizontalLayoutGroup>, IComponentStyle<VerticalLayoutGroup>, IComponentStyle<GridLayoutGroup>
    {
        public Vector4 Padding = Vector2.zero;
        public Vector2 Spacing = Vector2.zero;
        public TextAnchor ChildAlignment = TextAnchor.UpperLeft;
        public RectSizeBool ChildControl = new RectSizeBool(true, true);
        public RectSizeBool ChildForceExpand = new RectSizeBool(false, false);

        public LayoutGroupStyle() { }



        public void ApplyTo(UIBehaviour component)
        {
            if (component is LayoutGroup layoutGroup)
            {
                ApplyTo(layoutGroup);
            }
        }

        public void ApplyTo(LayoutGroup component)
        {
            if (component is GridLayoutGroup gridLayoutGroup)
            {
                ApplyTo(gridLayoutGroup);
            }
            else if (component is HorizontalOrVerticalLayoutGroup hvLayoutGroup)
            {
                ApplyTo(hvLayoutGroup);
            }
            else
            {
                ApplyToLayoutGroup(component);
            }
        }

        public void ApplyTo(HorizontalOrVerticalLayoutGroup component)
        {
            if (component is HorizontalLayoutGroup horizontalLayoutGroup)
            {
                ApplyTo(horizontalLayoutGroup);
            }
            else if (component is VerticalLayoutGroup verticalLayoutGroup)
            {
                ApplyTo(verticalLayoutGroup);
            }
        }



        public void ApplyTo(HorizontalLayoutGroup component)
        {
            ApplyToHorizontalOrVerticalLayoutGroup(component);
            component.spacing = Spacing.x;
        }

        public void ApplyTo(VerticalLayoutGroup component)
        {
            ApplyToHorizontalOrVerticalLayoutGroup(component);
            component.spacing = Spacing.y;
        }

        public void ApplyTo(GridLayoutGroup component)
        {
            ApplyToLayoutGroup(component);
            component.spacing = Spacing;
        }



        private void ApplyToLayoutGroup(LayoutGroup layoutGroup)
        {
            layoutGroup.padding.Set(Padding);
            layoutGroup.childAlignment = ChildAlignment;
        }

        private void ApplyToHorizontalOrVerticalLayoutGroup(HorizontalOrVerticalLayoutGroup hvlayoutGroup)
        {
            ApplyToLayoutGroup(hvlayoutGroup);

            hvlayoutGroup.SetChildControlWidth (ChildControl.Width);
            hvlayoutGroup.SetChildControlHeight(ChildControl.Height);
            hvlayoutGroup.childForceExpandWidth  = ChildForceExpand.Width;
            hvlayoutGroup.childForceExpandHeight = ChildForceExpand.Height;
        }
    }
}
