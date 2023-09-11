using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models
{
    public interface IStyledModel<TReadOnlyStyle>
        where TReadOnlyStyle : IReadOnlyUIModelStyle
    {
        public void ApplyStyle(TReadOnlyStyle style, IReadOnlyUISkin fallbackSkin = null);
    }

    public abstract class StyledModel<TReadOnlyStyle> : UIModel, IStyledModel<TReadOnlyStyle>
        where TReadOnlyStyle : IReadOnlyUIModelStyle
    {
        public sealed override GameObject UIRoot { get; }
        public abstract Image Background { get; }
        public GameObject GameObject => UIRoot;

        /// <summary>
        /// Constructor that creates a simple <see cref="UIRoot"/> object.
        /// </summary>
        protected StyledModel(GameObject parent, string name, Vector2 sizeDelta = default)
        {
            UIRoot = UIFactory.CreateUIObject(parent, name, sizeDelta);
        }

        /// <summary>
        /// Constructor that uses a custom UIRoot that has already been constructed.
        /// </summary>
        protected StyledModel(GameObject uiRoot)
        {
            UIRoot = uiRoot;
        }

        public abstract void ApplyStyle(TReadOnlyStyle style, IReadOnlyUISkin fallbackSkin = null);
    }
}
