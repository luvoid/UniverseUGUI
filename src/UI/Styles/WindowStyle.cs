using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyWindowStyle : IReadOnlyUIObjectStyle<IReadOnlyImageComponentStyle, Image>
    {
        public int TitlebarHeight { get; }
    }

    [System.Serializable]
    public sealed class WindowStyle
        : UIObjectStyle<ImageComponentStyle, IReadOnlyImageComponentStyle, Image>,
          IReadOnlyWindowStyle,
          IDeepCopyable<WindowStyle>,
          IConvertibleToReadOnly<ReadOnlyWindowStyle>
    {
        public int TitlebarHeight = 25;

        int IReadOnlyWindowStyle.TitlebarHeight => TitlebarHeight;

        /// <summary>
        /// Creates a new instance
        /// with a new <see cref="ImageComponentStyle"/> for the Background.
        /// </summary>
        public WindowStyle() : base() { }

        private WindowStyle(WindowStyle toCopy)
            : base(toCopy)
        {
            TitlebarHeight = toCopy.TitlebarHeight;
        }

        public WindowStyle DeepCopy()
        {
            return new WindowStyle(this);
        }

        public ReadOnlyWindowStyle AsReadOnly()
        {
            return AsReadOnly<WindowStyle, ReadOnlyWindowStyle>();
        }
    }

    public sealed class ReadOnlyWindowStyle : ReadOnlyUIObjectStyle<WindowStyle, ReadOnlyWindowStyle>, IReadOnlyWindowStyle
    {
        protected override WindowStyle WrappedStyle { get; }

        public ReadOnlyWindowStyle(WindowStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        public new IReadOnlyImageComponentStyle Background => ((IReadOnlyUIObjectStyle<IReadOnlyImageComponentStyle, Image>)WrappedStyle).Background;

        public int TitlebarHeight => ((IReadOnlyWindowStyle)WrappedStyle).TitlebarHeight;
    }
}
