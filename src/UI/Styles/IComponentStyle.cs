using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniverseLib.UI.Styles
{
    public interface IComponentStyle
    {
        public void ApplyTo(UIBehaviour component);
    }

    public interface IComponentStyle<T> : IComponentStyle
        where T : UIBehaviour
    {
        public void ApplyTo(T component);
    }
}
