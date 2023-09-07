
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Contexts;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI
{
    public sealed partial class UIFactory
    {
        private readonly LayoutContext.Stack layoutContextStack = new LayoutContext.Stack();

        private LayoutElementStyle? LayoutContextStyle => layoutContextStack.SafePeek()?.Style;

        /// <summary>
        /// Creates a new context which adds a <see cref="LayoutElement"/>
        /// on styled objects created by this factory.
        /// <br/> Usage:
        /// <br/> <c> using (Create.LayoutContext(style)) { ... } </c>
        /// </summary>
        /// <param name="style">
        /// The style to use when adding the <see cref="LayoutElement"/>.
        /// Use null to prevent a <see cref="LayoutElement"/> from being added.
        /// </param>
        public LayoutContext LayoutContext(LayoutElementStyle? style = null)
        {
            return new LayoutContext(layoutContextStack, style);
        }

        public LayoutContext LayoutContext(int? minWidth = null, int? minHeight = null,
            int? flexibleWidth = null, int? flexibleHeight = null, int? preferredWidth = null, int? preferredHeight = null,
            bool? ignoreLayout = null)
        {
            LayoutElementStyle style = new();

            if (minWidth != null)
                style.MinWidth = (int)minWidth;

            if (minHeight != null)
                style.MinHeight = (int)minHeight;

            if (flexibleWidth != null)
                style.FlexibleWidth = (int)flexibleWidth;

            if (flexibleHeight != null)
                style.FlexibleHeight = (int)flexibleHeight;

            if (preferredWidth != null)
                style.PreferredWidth = (int)preferredWidth;

            if (preferredHeight != null)
                style.PreferredHeight = (int)preferredHeight;

            if (ignoreLayout != null)
                style.IgnoreLayout = (bool)ignoreLayout;

            return LayoutContext(style);
        }

            private void SetDefaultLayoutElement(GameObject gameObject, IReadOnlyUIObjectStyle uiObjectStyle)
        {
            LayoutElementStyle? layoutElementStyle = LayoutContextStyle;
            if (layoutElementStyle.HasValue)
            {
                SetLayoutElement(gameObject, layoutElementStyle.Value);
            }
        }
    }
}
