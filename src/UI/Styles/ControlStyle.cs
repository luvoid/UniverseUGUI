using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyControlStyle : IReadOnlyUIModelStyle<IReadOnlySelectableComponentStyle, Selectable>, IReadOnlyLabelStyle
    { }

    [System.Serializable]
    public abstract class ControlStyle<T, TReadOnly> 
        : UIModelStyle<SelectableComponentStyle, IReadOnlySelectableComponentStyle, Selectable>,
          IReadOnlyControlStyle,
          IDeepCopyable<T>,
          IConvertibleToReadOnly<TReadOnly>
        where T : ControlStyle<T, TReadOnly>
        where TReadOnly : ReadOnlyControlStyle<T, TReadOnly>
    {

        /// <summary>
        /// Creates a new instance
        /// with a new <see cref="SelectableComponentStyle"/> for the Background.
        /// </summary>
        internal ControlStyle() : base()
        { }

        /// <inheritdoc cref="UIModelStyle{T0, T1, T2}(IReadOnlyUIModelStyle{T1, T2})"/>
        internal ControlStyle(IReadOnlyControlStyle toCopy)
            : base(toCopy)
        {
            Text = toCopy.Text;
        }

        public TReadOnly AsReadOnly()
        {
            return AsReadOnly<T, TReadOnly>();
        }
        public abstract T DeepCopy();


        /// <inheritdoc cref="IReadOnlyLabelStyle.Text"/>
        public TextComponentStyle Text = new();
        string IReadOnlyLabelStyle.Name => Name;
        TextComponentStyle IReadOnlyLabelStyle.Text => Text;
        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
            => LabelStyleHelper.GetTextStyle(this, fallbackSkin, fallbackFont);
    }

    public abstract class ReadOnlyControlStyle<T, TReadOnly>
        : ReadOnlyUIObjectStyle<T, TReadOnly>,
          IReadOnlyUIModelStyle<IReadOnlySelectableComponentStyle, Selectable>,
          IReadOnlyLabelStyle
        where T : ControlStyle<T, TReadOnly>
        where TReadOnly : ReadOnlyControlStyle<T, TReadOnly>
    {
        /// <inheritdoc/>
        internal ReadOnlyControlStyle(T toWrap) : base(toWrap) { }

        public new IReadOnlySelectableComponentStyle Background => WrappedStyle.Background;
        public TextComponentStyle Text => WrappedStyle.Text;
        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin, Font fallbackFont)
            => WrappedStyle.GetTextStyle(fallbackSkin, fallbackFont);
    }
}
