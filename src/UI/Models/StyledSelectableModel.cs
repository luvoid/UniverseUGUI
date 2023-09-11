using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public abstract class StyledSelectableModel<TSelectable, TReadOnlyStyle> 
        : StyledComponentModel<TSelectable, TReadOnlyStyle>,
          ISelectableModel<TSelectable>
        where TSelectable : Selectable
        where TReadOnlyStyle : IReadOnlyControlStyle
    {
        /// <inheritdoc/>
        protected StyledSelectableModel(GameObject parent, string name, Vector2 sizeDelta = default)
            : base(parent, name, sizeDelta)
        { }


        /// <inheritdoc/>
        protected StyledSelectableModel(GameObject uiRoot) : base(uiRoot)
        { }
    }
}
