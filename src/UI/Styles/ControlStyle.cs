using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyControlStyle : IReadOnlyUIObjectStyle<IReadOnlySelectableComponentStyle, Selectable>
    { }

    [System.Serializable]
    public abstract class ControlStyle<T, TReadOnly> 
        : UIObjectStyle<SelectableComponentStyle, IReadOnlySelectableComponentStyle, Selectable>,
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

        internal ControlStyle(ControlStyle<T, TReadOnly> toCopy)
            : base(toCopy)
        { }

        public TReadOnly AsReadOnly()
        {
            return AsReadOnly<T, TReadOnly>();
        }
        public abstract T DeepCopy();
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
