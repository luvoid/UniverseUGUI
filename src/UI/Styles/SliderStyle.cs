
namespace UniverseLib.UI.Styles
{
    internal interface IReadOnlySliderStyle : IReadOnlyControlStyle
    { }

    [System.Serializable]
    public sealed class SliderStyle : ControlStyle<SliderStyle, ReadOnlySliderStyle>, IReadOnlySliderStyle, IDeepCopyable<SliderStyle>
    {
        /// <inheritdoc/>
        public SliderStyle() : base()
        { }

        private SliderStyle(SliderStyle toCopy)
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
