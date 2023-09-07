using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public abstract class StyledSelectable<TSelectable, TReadOnlyStyle> : StyledComponent<TSelectable, TReadOnlyStyle>
        where TSelectable : Selectable
        where TReadOnlyStyle : IReadOnlyControlStyle
    {
        /// <inheritdoc/>
        protected StyledSelectable(GameObject parent, string name) : base(parent, name)
        { }


        /// <inheritdoc/>
        protected StyledSelectable(GameObject uiRoot) : base(uiRoot)
        { }
    }
}
