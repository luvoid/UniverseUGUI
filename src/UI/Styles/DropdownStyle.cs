using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UI.Styles
{
    public interface IReadOnlyDropdownStyle : IReadOnlyControlStyle
    {
        public IReadOnlyImageComponentStyle Arrow { get; }
        public ReadOnlyToggleStyle Item { get; }
        public ReadOnlyFrameStyle Viewport { get; }
        public ReadOnlyScrollbarStyle Scrollbar { get; }
    }

    public sealed class DropdownStyle : ControlStyle<DropdownStyle, ReadOnlyDropdownStyle>, IReadOnlyDropdownStyle
    {
        /// <summary>
        /// If this is used as <see cref="Arrow"/>'s sprite,
        /// the dropdown arrow will be <see cref="UnityEngine.UI.Text"/>
        /// showing "▼".
        /// </summary>
        public static readonly Sprite DefaultArrow = new Sprite();

        public ImageComponentStyle Arrow;
        public ToggleStyle Item;
        public FrameStyle Viewport;
        public ScrollbarStyle Scrollbar;

        IReadOnlyImageComponentStyle IReadOnlyDropdownStyle.Arrow => Arrow;
        ReadOnlyToggleStyle IReadOnlyDropdownStyle.Item => Item.AsReadOnly();
        ReadOnlyFrameStyle IReadOnlyDropdownStyle.Viewport => Viewport.AsReadOnly();
        ReadOnlyScrollbarStyle IReadOnlyDropdownStyle.Scrollbar => Scrollbar.AsReadOnly();

        /// <summary>
        /// Creates a new instance with 
        /// <br/> a new <see cref="ImageComponentStyle"/> using <see cref="DefaultArrow"/> for the Arrow,
        /// <br/> a new <see cref="ToggleStyle"/> for the Item,
        /// <br/> a new <see cref="FrameStyle"/> for the Viewport,
        /// <br/> and a new <see cref="ScrollbarStyle"/> for the Scrollbar.
        /// </summary>
        public DropdownStyle()
        {
            Arrow = new() { Sprite = DefaultArrow };
            Item = new();
            Viewport = new();
            Scrollbar = new();
        }

        /// <inheritdoc cref="ControlStyle{T0, T1}(IReadOnlyControlStyle)"/>
        public DropdownStyle(IReadOnlyDropdownStyle toCopy) : base(toCopy)
        {
            Arrow     = toCopy.Arrow.Copy();
            Item      = toCopy.Item.DeepCopy();
            Viewport  = toCopy.Viewport.DeepCopy();
            Scrollbar = toCopy.Scrollbar.DeepCopy();
        }

        public override DropdownStyle DeepCopy()
        {
            return new DropdownStyle(this);
        }
    }

    public sealed class ReadOnlyDropdownStyle : ReadOnlyControlStyle<DropdownStyle, ReadOnlyDropdownStyle>, IReadOnlyDropdownStyle
    {
        protected override DropdownStyle WrappedStyle { get; }

        public ReadOnlyDropdownStyle(DropdownStyle toWrap) : base(toWrap)
        {
            WrappedStyle = toWrap;
        }

        public IReadOnlyImageComponentStyle Arrow => ((IReadOnlyDropdownStyle)WrappedStyle).Arrow;

        public ReadOnlyToggleStyle Item => ((IReadOnlyDropdownStyle)WrappedStyle).Item;
        public ReadOnlyFrameStyle Viewport => ((IReadOnlyDropdownStyle)WrappedStyle).Viewport;
        public ReadOnlyScrollbarStyle Scrollbar => ((IReadOnlyDropdownStyle)WrappedStyle).Scrollbar;

    }
}
