using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Contexts
{
    public class LayoutContext : Context<LayoutContext>
    {
        public readonly LayoutElementStyle? Style;

        public LayoutContext(Stack contextStack, LayoutElementStyle? style) : base(contextStack)
        {
            Style = style;
        }
    }
}
