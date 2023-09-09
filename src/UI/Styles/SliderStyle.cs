
namespace UniverseLib.UI.Styles
{
    public interface IReadOnlySliderStyle : IReadOnlyControlStyle
    { }

    [System.Serializable]
    public sealed class SliderStyle : ControlStyle<SliderStyle, ReadOnlySliderStyle>, IReadOnlySliderStyle, IDeepCopyable<SliderStyle>
    {
        /// <inheritdoc/>
        public SliderStyle() : base()
        { }

        /// <inheritdoc cref="ControlStyle{T0, T1}(IReadOnlyControlStyle)"/>
        public SliderStyle(IReadOnlySliderStyle toCopy)
            : base(toCopy)
        { }

        public override SliderStyle DeepCopy()
        {
            return new SliderStyle(this);
        }
    }

    public sealed class ReadOnlySliderStyle : ReadOnlyControlStyle<SliderStyle, ReadOnlySliderStyle>, IReadOnlySliderStyle
    {
        public ReadOnlySliderStyle(SliderStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        protected override SliderStyle WrappedStyle { get; }
    }
}
