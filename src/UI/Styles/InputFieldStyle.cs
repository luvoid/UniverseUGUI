﻿using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyInputFieldStyle : IReadOnlyControlStyle
    {

    }

    [System.Serializable]
    public sealed class InputFieldStyle : ControlStyle<InputFieldStyle, ReadOnlyInputFieldStyle>, IReadOnlyInputFieldStyle
    {
        /// <inheritdoc/>
        public InputFieldStyle() : base() { }

        /// <inheritdoc cref="ControlStyle{T0, T1}(IReadOnlyControlStyle)"/>
        public InputFieldStyle(IReadOnlyInputFieldStyle toCopy)
            : base(toCopy)
        { }

        public override InputFieldStyle DeepCopy()
        {
            return new InputFieldStyle(this);
        }
    }

    public sealed class ReadOnlyInputFieldStyle : ReadOnlyControlStyle<InputFieldStyle, ReadOnlyInputFieldStyle>, IReadOnlyInputFieldStyle
    {
        public ReadOnlyInputFieldStyle(InputFieldStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        protected override InputFieldStyle WrappedStyle { get; }
    }
}
