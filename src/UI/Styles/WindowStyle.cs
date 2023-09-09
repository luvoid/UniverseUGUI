using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyWindowStyle : IReadOnlyUIObjectStyle<IReadOnlyImageComponentStyle, Image>
    {
        public IReadOnlyFrameStyle Titlebar { get; }
        public int TitlebarHeight { get; }
    }

    [System.Serializable]
    public sealed class WindowStyle
        : UIObjectStyle<ImageComponentStyle, IReadOnlyImageComponentStyle, Image>,
          IReadOnlyWindowStyle,
          IDeepCopyable<WindowStyle>,
          IConvertibleToReadOnly<ReadOnlyWindowStyle>
    {
        public FrameStyle Titlebar = new();
        public int TitlebarHeight = 25;

        IReadOnlyFrameStyle IReadOnlyWindowStyle.Titlebar => Titlebar.AsReadOnly();
        int IReadOnlyWindowStyle.TitlebarHeight => TitlebarHeight;

        /// <summary>
        /// Creates a new instance
        /// with a new <see cref="ImageComponentStyle"/> for the Background.
        /// </summary>
        public WindowStyle() : base() { }

        /// <inheritdoc cref="UIObjectStyle{T0, T1, T2}(IReadOnlyUIObjectStyle{T1, T2})"/>
        public WindowStyle(WindowStyle toCopy)
            : base(toCopy)
        {
            Titlebar = toCopy.Titlebar.DeepCopy();
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

        public IReadOnlyFrameStyle Titlebar => ((IReadOnlyWindowStyle)WrappedStyle).Titlebar;

        public int TitlebarHeight => ((IReadOnlyWindowStyle)WrappedStyle).TitlebarHeight;

    }
}
