using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    [System.Serializable]
    public sealed class InputFieldStyle : ControlStyle<InputFieldStyle, ReadOnlyInputFieldStyle>
    {
        /// <inheritdoc/>
        public InputFieldStyle() : base() { }

        private InputFieldStyle(InputFieldStyle toCopy)
            : base(toCopy)
        { }

        public override InputFieldStyle DeepCopy()
        {
            return new InputFieldStyle(this);
        }
    }

    public sealed class ReadOnlyInputFieldStyle : ReadOnlyControlStyle<InputFieldStyle, ReadOnlyInputFieldStyle>
    {
        public ReadOnlyInputFieldStyle(InputFieldStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        protected override InputFieldStyle WrappedStyle { get; }
    }
}
