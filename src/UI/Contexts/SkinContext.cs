using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Contexts
{
    public class SkinContext : Context<SkinContext>
    {
        public readonly IReadOnlyUISkin Skin;

        public SkinContext(Stack contextStack, IReadOnlyUISkin skin) : base(contextStack)
        {
            Skin = skin;
        }
    }
}
