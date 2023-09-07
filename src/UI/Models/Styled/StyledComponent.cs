using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Styled
{
    public abstract class StyledComponent<TComponent, TReadOnlyStyle> : UIStyledModel<TReadOnlyStyle>
        where TComponent : UIBehaviour
        where TReadOnlyStyle : IReadOnlyUIObjectStyle
    {
        public abstract TComponent Component { get; }
        public GameObject GameObject => Component.gameObject;

        /// <inheritdoc/>
        protected StyledComponent(GameObject parent, string name) : base(parent, name)
        { }

        /// <inheritdoc/>
        protected StyledComponent(GameObject uiRoot) : base(uiRoot)
        { }
    }
}
