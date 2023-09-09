using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyControlStyle : IReadOnlyUIObjectStyle<IReadOnlySelectableComponentStyle, Selectable>, IReadOnlyLabelStyle
    { }

    [System.Serializable]
    public abstract class ControlStyle<T, TReadOnly> 
        : UIObjectStyle<SelectableComponentStyle, IReadOnlySelectableComponentStyle, Selectable>,
          IReadOnlyControlStyle,
          IReadOnlyLabelStyle,
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

        /// <inheritdoc cref="UIObjectStyle{T0, T1, T2}(IReadOnlyUIObjectStyle{T1, T2})"/>
        internal ControlStyle(IReadOnlyControlStyle toCopy)
            : base(toCopy)
        { }

        public TReadOnly AsReadOnly()
        {
            return AsReadOnly<T, TReadOnly>();
        }
        public abstract T DeepCopy();


        /// <inheritdoc cref="IReadOnlyLabelStyle.Text"/>
        public TextComponentStyle Text = new();
        string IReadOnlyLabelStyle.Name => Name;
        TextComponentStyle IReadOnlyLabelStyle.Text => Text;
        Vector2 IReadOnlyLabelStyle.LabelOffset => Vector2.zero;
        public void GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
            => LabelStyleHelper.GetTextStyle(this, fallbackSkin, fallbackFont);
    }

    public abstract class ReadOnlyControlStyle<T, TReadOnly>
        : ReadOnlyUIObjectStyle<T, TReadOnly>,
          IReadOnlyUIObjectStyle<IReadOnlySelectableComponentStyle, Selectable>
        where T : ControlStyle<T, TReadOnly>
        where TReadOnly : ReadOnlyControlStyle<T, TReadOnly>
    {
        /// <inheritdoc/>
        internal ReadOnlyControlStyle(T toWrap) : base(toWrap) { }

        public new IReadOnlySelectableComponentStyle Background => WrappedStyle.Background;
    }
}
