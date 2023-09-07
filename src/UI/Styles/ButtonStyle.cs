using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyButtonStyle : IReadOnlyControlStyle
    { }

    [System.Serializable]
    public sealed class ButtonStyle : ControlStyle<ButtonStyle, ReadOnlyButtonStyle>, IReadOnlyButtonStyle
    {
        /// <inheritdoc/>
        public ButtonStyle() : base() { }

        internal ButtonStyle(ButtonStyle toCopy)
            : base(toCopy)
        { }

        public override ButtonStyle DeepCopy()
        {
            return new ButtonStyle(this);
        }
    }

    public class ReadOnlyButtonStyle : ReadOnlyControlStyle<ButtonStyle, ReadOnlyButtonStyle>, IReadOnlyButtonStyle
    {
        protected override ButtonStyle WrappedStyle { get; }

        public ReadOnlyButtonStyle(ButtonStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }
    }
}
