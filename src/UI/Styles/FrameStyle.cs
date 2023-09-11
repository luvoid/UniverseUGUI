using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyFrameStyle : IReadOnlyUIModelStyle<IReadOnlyImageComponentStyle, Image>, IReadOnlyLabelStyle
    {
        public bool UseBackground { get; }
    }

    [System.Serializable]
    public sealed class FrameStyle
        : UIModelStyle<ImageComponentStyle, IReadOnlyImageComponentStyle, Image>,
          IReadOnlyFrameStyle,
          IDeepCopyable<FrameStyle>,
          IConvertibleToReadOnly<ReadOnlyFrameStyle>
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
        public FrameStyle() : base() { }

        /// <inheritdoc cref="UIModelStyle{T0, T1, T2}(IReadOnlyUIModelStyle{T1, T2})"/>
        public FrameStyle(FrameStyle toCopy)
            : base(toCopy)
        {
            _useBackground = toCopy._useBackground;
            Text = toCopy.Text;
        }

        public FrameStyle DeepCopy()
        {
            return new FrameStyle(this);
        }

        public ReadOnlyFrameStyle AsReadOnly()
        {
            return AsReadOnly<FrameStyle, ReadOnlyFrameStyle>();
        }



        /// <inheritdoc cref="IReadOnlyLabelStyle.Text"/>
        public TextComponentStyle Text = new();
        string IReadOnlyLabelStyle.Name => Name;
        TextComponentStyle IReadOnlyLabelStyle.Text => Text;
        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
            => LabelStyleHelper.GetTextStyle(this, fallbackSkin, fallbackFont);
    }

    [System.Serializable]
    public sealed class ReadOnlyFrameStyle : ReadOnlyUIObjectStyle<FrameStyle, ReadOnlyFrameStyle>, IReadOnlyFrameStyle
    {
        protected override FrameStyle WrappedStyle { get; }

        public ReadOnlyFrameStyle(FrameStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        public bool UseBackground => WrappedStyle.UseBackground;
        public new IReadOnlyImageComponentStyle Background => ((IReadOnlyUIModelStyle<IReadOnlyImageComponentStyle, Image>)WrappedStyle).Background;
        public TextComponentStyle Text => ((IReadOnlyLabelStyle)WrappedStyle).Text;
        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
        {
            return WrappedStyle.GetTextStyle(fallbackSkin, fallbackFont);
        }
    }
}
