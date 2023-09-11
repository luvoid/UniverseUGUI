using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniverseLib.Utility;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyUIModelStyle
    {
        public string Name { get; }
        public IComponentStyle Background { get; }
        public Vector4 Overflow { get; }
        //public LayoutElementStyle LayoutElement { get; }
        public LayoutGroupStyle LayoutGroup { get; }
    }

    public abstract class UIModelStyle : IReadOnlyUIModelStyle
    {
        public string Name = "";

        public Vector4 Overflow = default;

        //public LayoutElementStyle LayoutElement = new();

        public LayoutGroupStyle LayoutGroup = new();

        protected abstract IComponentStyle m_BackgroundComponentStyle { get; }

        string IReadOnlyUIModelStyle.Name => Name;
        IComponentStyle IReadOnlyUIModelStyle.Background => m_BackgroundComponentStyle;
        Vector4 IReadOnlyUIModelStyle.Overflow => Overflow;
        //LayoutElementStyle IReadOnlyUIObjectStyle.LayoutElement => LayoutElement;
        LayoutGroupStyle IReadOnlyUIModelStyle.LayoutGroup => LayoutGroup;

        private object _readonlyWrapper;
        protected TReadOnly AsReadOnly<T, TReadOnly>()
            where T : UIModelStyle, IDeepCopyable<T>, IConvertibleToReadOnly<TReadOnly>
            where TReadOnly : ReadOnlyUIObjectStyle<T, TReadOnly>
        {
            _readonlyWrapper ??= System.Activator.CreateInstance(typeof(TReadOnly), this);
            return _readonlyWrapper as TReadOnly;
        }
    }

    public abstract class ReadOnlyUIModelStyle : IReadOnlyUIModelStyle
    {
        protected abstract UIModelStyle WrappedUIObjectStyle { get; }

        /// <summary>
        /// This does nothing but serve as a reminder that 
        /// the WrappedStyle needs to be set in the implementer.
        /// </summary>
        internal ReadOnlyUIModelStyle(UIModelStyle toWrap) { }

        public string Name => ((IReadOnlyUIModelStyle)WrappedUIObjectStyle).Name;
        public IComponentStyle Background => ((IReadOnlyUIModelStyle)WrappedUIObjectStyle).Background;
        public Vector4 Overflow => ((IReadOnlyUIModelStyle)WrappedUIObjectStyle).Overflow;
        //public LayoutElementStyle LayoutElement => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).LayoutElement;
        public LayoutGroupStyle LayoutGroup => ((IReadOnlyUIModelStyle)WrappedUIObjectStyle).LayoutGroup;
    }

    public abstract class ReadOnlyUIObjectStyle<T, TReadOnly> : ReadOnlyUIModelStyle, IReadOnlyUIModelStyle, IDeepCopyable<T>
        where T : UIModelStyle, IDeepCopyable<T>, IConvertibleToReadOnly<TReadOnly>
        where TReadOnly : ReadOnlyUIObjectStyle<T, TReadOnly>
    {
        protected sealed override UIModelStyle WrappedUIObjectStyle => WrappedStyle;
        protected abstract T WrappedStyle { get; }

        /// <summary>
        /// This does nothing but serve as a reminder that 
        /// the WrappedStyle needs to be set in the implementer.
        /// </summary>
        internal ReadOnlyUIObjectStyle(T toWrap) : base(toWrap) { }

        public T DeepCopy()
        {
            return WrappedStyle.DeepCopy();
        }
    }

    public interface IReadOnlyUIModelStyle<TReadOnlyBackgroundStyle, TBackgroundComponent> : IReadOnlyUIModelStyle
        where TReadOnlyBackgroundStyle : IComponentStyle<TBackgroundComponent>
        where TBackgroundComponent : UIBehaviour
    {
        public new TReadOnlyBackgroundStyle Background { get; }
    }

    public abstract class UIModelStyle<TBackgroundStyle, TReadOnlyBackgroundStyle, TBackgroundComponent>
        : UIModelStyle, IReadOnlyUIModelStyle<TReadOnlyBackgroundStyle, TBackgroundComponent>
        where TBackgroundStyle : GraphicComponentStyle, IComponentStyle<TBackgroundComponent>, TReadOnlyBackgroundStyle, new()
        where TReadOnlyBackgroundStyle : IComponentStyle<TBackgroundComponent>, IReadOnlyGraphicComponentStyle
        where TBackgroundComponent : UIBehaviour
    {
        protected override IComponentStyle m_BackgroundComponentStyle => Background;

        TReadOnlyBackgroundStyle IReadOnlyUIModelStyle<TReadOnlyBackgroundStyle, TBackgroundComponent>.Background => Background;

        public TBackgroundStyle Background = null;

        /// <summary>
        /// Creates a new instance with a new object for the <see cref="Background"/>.
        /// </summary>
        internal UIModelStyle()
        {
            Background = new TBackgroundStyle();
        }

        /// <summary>
        /// Creates a deep copy of the style <paramref name="toCopy"/>.
        /// </summary>
        internal UIModelStyle(IReadOnlyUIModelStyle<TReadOnlyBackgroundStyle, TBackgroundComponent> toCopy)
        {
            Name = toCopy.Name + "Copy";
            Background = toCopy.Background?.Clone() as TBackgroundStyle;
            Overflow = toCopy.Overflow;
            //LayoutElement = toCopy.LayoutElement;
            LayoutGroup = toCopy.LayoutGroup;
        }
    }
}
