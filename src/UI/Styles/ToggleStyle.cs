using UnityEngine;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyToggleStyle : IReadOnlyControlStyle
    {
        /// <summary>
        /// The checkmark style of the toggle.
        /// </summary>
        public IReadOnlyImageComponentStyle Checkmark { get; }

        /// <summary>
        /// If equal to <see cref="Vector2.zero"/>, will default normal background sizing behavior.
        /// </summary>
        public Vector2 CheckboxSize { get; }

        /// <summary>
        /// The padding inside the checkbox which affects the size of the checkmark.
        /// </summary>
        public Vector4 CheckboxPadding { get; }
    }

    [System.Serializable]
    public sealed class ToggleStyle : ControlStyle<ToggleStyle, ReadOnlyToggleStyle>, IReadOnlyToggleStyle
    {
        /// <inheritdoc cref="IReadOnlyToggleStyle.Checkmark"/>
        public ImageComponentStyle Checkmark;

        /// <inheritdoc cref="IReadOnlyToggleStyle.CheckboxSize"/>
        public Vector2 CheckboxSize;

        /// <inheritdoc cref="IReadOnlyToggleStyle.CheckboxPadding"/>
        public Vector4 CheckboxPadding;


        IReadOnlyImageComponentStyle IReadOnlyToggleStyle.Checkmark => Checkmark;
        Vector2 IReadOnlyToggleStyle.CheckboxSize => CheckboxSize;
        Vector4 IReadOnlyToggleStyle.CheckboxPadding => CheckboxPadding;


        /// <summary>
        /// Creates a new instance
        /// with a new <see cref="SelectableComponentStyle"/> for the Background
        /// and a new <see cref="ImageComponentStyle"/> for the Checkmark.
        /// </summary>
        public ToggleStyle() : base() 
        {
            Checkmark = new ImageComponentStyle();
        }

        /// <inheritdoc cref="ControlStyle{T0, T1}(IReadOnlyControlStyle)"/>
        public ToggleStyle(IReadOnlyToggleStyle toCopy)
            : base(toCopy)
        {
            Checkmark = toCopy.Checkmark.Copy();
            CheckboxSize = toCopy.CheckboxSize;
            CheckboxPadding = toCopy.CheckboxPadding;
        }

        public override ToggleStyle DeepCopy()
        {
            return new ToggleStyle(this);
        }
    }

    public sealed class ReadOnlyToggleStyle : ReadOnlyControlStyle<ToggleStyle, ReadOnlyToggleStyle>, IReadOnlyToggleStyle
    {
        protected override ToggleStyle WrappedStyle { get; }

        public ReadOnlyToggleStyle(ToggleStyle wrappedStyle) : base(wrappedStyle)
        {
            WrappedStyle = wrappedStyle;
        }

        public IReadOnlyImageComponentStyle Checkmark => ((IReadOnlyToggleStyle)WrappedStyle).Checkmark;

        public Vector2 CheckboxSize => ((IReadOnlyToggleStyle)WrappedStyle).CheckboxSize;

        public Vector4 CheckboxPadding => ((IReadOnlyToggleStyle)WrappedStyle).CheckboxPadding;
    }
}
