using System.Collections.Generic;
using UniverseLib.UI.Contexts;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI
{
    public sealed partial class UIFactory
    {
        private readonly IReadOnlyUISkin _skin = null;
        private readonly SkinContext.Stack skinContextStack = new SkinContext.Stack();

        /// <summary>
        /// A default instance of a UIFactory for accessing styled creation methods, instance methods, and extension methods.
        /// </summary>
        public static readonly UIFactory Create = new();

        private UIFactory() { }
        private UIFactory(IReadOnlyUISkin skin)
        {
            _skin = skin;
        }

        /// <summary>
        /// The skin used when creating styled controls.
        /// Defaults to <see cref="UISkin.Default"/>.
        /// </summary>
        /// <remarks>
        /// Can be overriden temporarily with:
        /// <code>
        /// using (Create.SkinContext(skin)) { ... }
        /// </code>
        /// </remarks>
        public IReadOnlyUISkin Skin => skinContextStack.SafePeek()?.Skin ?? _skin ?? UISkin.Default;

        public static UIFactory CreateSkinnedFactory(IReadOnlyUISkin skin)
        {
            return new UIFactory(skin);
        }


        /// <summary>
        /// Creates a new context which overrides this factory's skin.
        /// 
        /// <br/> Usage:
        /// <br/> <c> using (Create.SkinContext(skin)) { ... } </c>
        /// </summary>
        public SkinContext SkinContext(IReadOnlyUISkin skin)
        {
            return new SkinContext(skinContextStack, skin);
        }
    }
}
