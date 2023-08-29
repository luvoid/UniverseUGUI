
namespace UniverseLib.UI.Styles
{
    [System.Serializable]
    public sealed class ScrollbarStyle : ControlStyle<ScrollbarStyle, ReadOnlyScrollbarStyle>
    {
        /// <inheritdoc/>
        public ScrollbarStyle() : base() { }

        private ScrollbarStyle(ScrollbarStyle toCopy)
            : base(toCopy)
        { }

        public override ScrollbarStyle DeepCopy()
        {
            return new ScrollbarStyle(this);
        }
    }

    public sealed class ReadOnlyScrollbarStyle : ReadOnlyControlStyle<ScrollbarStyle, ReadOnlyScrollbarStyle>
    {
        protected override ScrollbarStyle WrappedStyle { get; }

        public ReadOnlyScrollbarStyle(ScrollbarStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }
    }
}
