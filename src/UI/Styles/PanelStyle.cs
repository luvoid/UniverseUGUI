using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyPanelStyle : IReadOnlyUIObjectStyle<IReadOnlyImageComponentStyle, Image>
    {
        public bool UseBackground { get; }
    }

    [System.Serializable]
    public sealed class PanelStyle
        : UIObjectStyle<ImageComponentStyle, IReadOnlyImageComponentStyle, Image>,
          IReadOnlyPanelStyle,
          IDeepCopyable<PanelStyle>,
          IConvertibleToReadOnly<ReadOnlyPanelStyle>
    {
        private bool _useBackground = true;
        public bool UseBackground
        { 
            get => _useBackground && Background != null; 
            set => _useBackground = value;
        }

        /// <summary>
        /// Creates a new instance
        /// with a new <see cref="ImageComponentStyle"/> for the Background.
        /// </summary>
        public PanelStyle() : base() { }

        internal PanelStyle(PanelStyle toCopy)
            : base(toCopy)
        {
            _useBackground = toCopy._useBackground;
        }

        public PanelStyle DeepCopy()
        {
            return new PanelStyle(this);
        }

        public ReadOnlyPanelStyle AsReadOnly()
        {
            return AsReadOnly<PanelStyle, ReadOnlyPanelStyle>();
        }
    }

    public sealed class ReadOnlyPanelStyle : ReadOnlyUIObjectStyle<PanelStyle, ReadOnlyPanelStyle>, IReadOnlyPanelStyle
    {
        protected override PanelStyle WrappedStyle { get; }

        public ReadOnlyPanelStyle(PanelStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        public bool UseBackground => WrappedStyle.UseBackground;
        public IReadOnlyImageComponentStyle Background => ((IReadOnlyUIObjectStyle<IReadOnlyImageComponentStyle, Image>)WrappedStyle).Background;
    }
}
