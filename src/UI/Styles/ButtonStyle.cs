using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    [System.Serializable]
    public sealed class ButtonStyle : ControlStyle<ButtonStyle, ReadOnlyButtonStyle>
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

    public class ReadOnlyButtonStyle : ReadOnlyControlStyle<ButtonStyle, ReadOnlyButtonStyle>
    {
        protected override ButtonStyle WrappedStyle { get; }

        public ReadOnlyButtonStyle(ButtonStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }
    }
}
