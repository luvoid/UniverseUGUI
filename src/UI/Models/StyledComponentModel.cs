using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public abstract class StyledComponentModel<TComponent, TReadOnlyStyle>
        : StyledModel<TReadOnlyStyle>,
          IComponentModel<TComponent>
        where TComponent : UIBehaviour
        where TReadOnlyStyle : IReadOnlyUIModelStyle
    {
        public abstract TComponent Component { get; }

        /// <inheritdoc/>
        protected StyledComponentModel(GameObject parent, string name, Vector2 sizeDelta = default)
            : base(parent, name, sizeDelta)
        { }

        /// <inheritdoc/>
        protected StyledComponentModel(GameObject uiRoot) : base(uiRoot)
        { }
    }
}
