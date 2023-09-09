
namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyScrollbarStyle : IReadOnlyControlStyle
    {
    }

    [System.Serializable]
    public sealed class ScrollbarStyle : ControlStyle<ScrollbarStyle, ReadOnlyScrollbarStyle>, IReadOnlyScrollbarStyle
    {
        /// <inheritdoc/>
        public ScrollbarStyle() : base() { }

        /// <inheritdoc cref="ControlStyle{T0, T1}(IReadOnlyControlStyle)"/>
        public ScrollbarStyle(IReadOnlyScrollbarStyle toCopy)
            : base(toCopy)
        { }

        public override ScrollbarStyle DeepCopy()
        {
            return new ScrollbarStyle(this);
        }
    }

    public sealed class ReadOnlyScrollbarStyle : ReadOnlyControlStyle<ScrollbarStyle, ReadOnlyScrollbarStyle>, IReadOnlyScrollbarStyle
    {
        protected override ScrollbarStyle WrappedStyle { get; }

        public ReadOnlyScrollbarStyle(ScrollbarStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }
    }
}
