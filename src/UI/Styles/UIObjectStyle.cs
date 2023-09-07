using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniverseLib.Utility;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyUIObjectStyle
    {
        public string Name { get; }
        public TextComponentStyle Text { get; }
        public IComponentStyle Background { get; }
        public Vector4 Overflow { get; }
        public Vector2 LabelOffset { get; }
        //public LayoutElementStyle LayoutElement { get; }
        public LayoutGroupStyle LayoutGroup { get; }

        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null);
    }

    public abstract class UIObjectStyle : IReadOnlyUIObjectStyle
    {
        public string Name = "";

        /// <summary>
        /// If null, will use the <see cref="UISkin"/>'s Text style.
        /// </summary>
        public TextComponentStyle Text = new();

        public Vector4 Overflow = default;

        public Vector2 LabelOffset = default;

        //public LayoutElementStyle LayoutElement = new();

        public LayoutGroupStyle LayoutGroup = new();

        protected abstract IComponentStyle m_BackgroundComponentStyle { get; }

        string IReadOnlyUIObjectStyle.Name => Name;
        TextComponentStyle IReadOnlyUIObjectStyle.Text => Text;
        IComponentStyle IReadOnlyUIObjectStyle.Background => m_BackgroundComponentStyle;
        Vector4 IReadOnlyUIObjectStyle.Overflow => Overflow;
        Vector2 IReadOnlyUIObjectStyle.LabelOffset => LabelOffset;
        //LayoutElementStyle IReadOnlyUIObjectStyle.LayoutElement => LayoutElement;
        LayoutGroupStyle IReadOnlyUIObjectStyle.LayoutGroup => LayoutGroup;

        public virtual TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
        {
            TextComponentStyle textStyle = Text;

            textStyle.Font ??= fallbackFont ?? fallbackSkin?.Text.Font ?? UniversalUI.DefaultFont;
            if (textStyle.Font.IsNullOrDestroyed())
            {
                textStyle.Font = null;
            }
            textStyle.FontSize = textStyle.FontSize == 0 ? fallbackSkin?.Text.FontSize ?? textStyle.Font?.fontSize ?? 14 : textStyle.FontSize;

            return textStyle;
        }

        private object _readonlyWrapper;
        protected TReadOnly AsReadOnly<T, TReadOnly>()
            where T : UIObjectStyle, IDeepCopyable<T>, IConvertibleToReadOnly<TReadOnly>
            where TReadOnly : ReadOnlyUIObjectStyle<T, TReadOnly>
        {
            _readonlyWrapper ??= System.Activator.CreateInstance(typeof(TReadOnly), this);
            return _readonlyWrapper as TReadOnly;
        }
    }

    public abstract class ReadOnlyUIObjectStyle : IReadOnlyUIObjectStyle
    {
        protected abstract UIObjectStyle WrappedUIObjectStyle { get; }

        /// <summary>
        /// This does nothing but serve as a reminder that 
        /// the WrappedStyle needs to be set in the implementer.
        /// </summary>
        internal ReadOnlyUIObjectStyle(UIObjectStyle toWrap) { }

        public string Name => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).Name;

        public TextComponentStyle Text => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).Text;

        public IComponentStyle Background => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).Background;

        public Vector4 Overflow => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).Overflow;

        public Vector2 LabelOffset => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).LabelOffset;

        //public LayoutElementStyle LayoutElement => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).LayoutElement;

        public LayoutGroupStyle LayoutGroup => ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).LayoutGroup;

        public TextComponentStyle GetTextStyle(IReadOnlyUISkin fallbackSkin = null, Font fallbackFont = null)
        {
            return ((IReadOnlyUIObjectStyle)WrappedUIObjectStyle).GetTextStyle(fallbackSkin, fallbackFont);
        }
    }

    public abstract class ReadOnlyUIObjectStyle<T, TReadOnly> : ReadOnlyUIObjectStyle, IReadOnlyUIObjectStyle, IDeepCopyable<T>
        where T : UIObjectStyle, IDeepCopyable<T>, IConvertibleToReadOnly<TReadOnly>
        where TReadOnly : ReadOnlyUIObjectStyle<T, TReadOnly>
    {
        protected sealed override UIObjectStyle WrappedUIObjectStyle => WrappedStyle;
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

    public interface IReadOnlyUIObjectStyle<TReadOnlyBackgroundStyle, TBackgroundComponent> : IReadOnlyUIObjectStyle
        where TReadOnlyBackgroundStyle : IComponentStyle<TBackgroundComponent>
        where TBackgroundComponent : UIBehaviour
    {
        public new TReadOnlyBackgroundStyle Background { get; }
    }

    public abstract class UIObjectStyle<TBackgroundStyle, TReadOnlyBackgroundStyle, TBackgroundComponent>
        : UIObjectStyle, IReadOnlyUIObjectStyle<TReadOnlyBackgroundStyle, TBackgroundComponent>
        where TBackgroundStyle : GraphicComponentStyle, IComponentStyle<TBackgroundComponent>, TReadOnlyBackgroundStyle, new()
        where TReadOnlyBackgroundStyle : IComponentStyle<TBackgroundComponent>, IReadOnlyGraphicComponentStyle
        where TBackgroundComponent : UIBehaviour
    {
        protected override IComponentStyle m_BackgroundComponentStyle => Background;

        TReadOnlyBackgroundStyle IReadOnlyUIObjectStyle<TReadOnlyBackgroundStyle, TBackgroundComponent>.Background => Background;

        public TBackgroundStyle Background = null;

        /// <summary>
        /// Creates a new instance with a new object for the <see cref="Background"/>.
        /// </summary>
        internal UIObjectStyle()
        {
            Background = new TBackgroundStyle();
        }

        internal UIObjectStyle(UIObjectStyle<TBackgroundStyle, TReadOnlyBackgroundStyle, TBackgroundComponent> toCopy)
        {
            Name = toCopy.Name + "Copy";
            Text = toCopy.Text;
            Background = toCopy.Background?.Clone() as TBackgroundStyle;
            Overflow = toCopy.Overflow; 
            LabelOffset = toCopy.LabelOffset;
            //LayoutElement = toCopy.LayoutElement;
            LayoutGroup = toCopy.LayoutGroup;
        }
    }
}
