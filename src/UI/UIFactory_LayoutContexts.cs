
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
        /// <code> <see langword="using"/> (Create.LayoutContext(<see href="param"/>: value)) { ... } </code>
        /// </summary>
        public LayoutContext LayoutContext(float? minWidth = null, float? minHeight = null,
            float? flexibleWidth = null, float? flexibleHeight = null, float? preferredWidth = null, float? preferredHeight = null,
            bool? ignoreLayout = null)
        {
            LayoutElementStyle style = new();

            if (minWidth.HasValue)
                style.MinWidth = minWidth.Value;

            if (minHeight.HasValue)
                style.MinHeight = minHeight.Value;

            if (flexibleWidth.HasValue)
                style.FlexibleWidth = flexibleWidth.Value;

            if (flexibleHeight.HasValue)
                style.FlexibleHeight = flexibleHeight.Value;

            if (preferredWidth.HasValue)
                style.PreferredWidth = preferredWidth.Value;

            if (preferredHeight.HasValue)
                style.PreferredHeight = preferredHeight.Value;

            if (ignoreLayout.HasValue)
                style.IgnoreLayout = ignoreLayout.Value;

            return LayoutContext(style);
        }

            private void SetDefaultLayoutElement(GameObject gameObject, IReadOnlyUIModelStyle uiObjectStyle)
        {
            LayoutElementStyle? layoutElementStyle = LayoutContextStyle;
            if (layoutElementStyle.HasValue)
            {
                SetLayoutElement(gameObject, layoutElementStyle.Value);
            }
        }

        /// <summary>
        /// Creates a new context which adds a <see cref="LayoutElement"/>
        /// on styled objects created by this factory.
        /// <br/> Usage:
        /// <code> <see langword="using"/> (Create.LayoutContext(<paramref name="style"/>)) { ... } </code>
        /// </summary>
        /// <param name="style">
        /// The style to use when adding the <see cref="LayoutElement"/>.
        /// Use null to prevent a <see cref="LayoutElement"/> from being added.
        /// </param>
        public LayoutContext LayoutContext(LayoutElementStyle? style = null)
        {
            return new LayoutContext(layoutContextStack, style);
        }
    }
}
