using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Models
{
    public interface IButtonRef
    {
        /// <summary>
        /// Invoked when the Button is clicked.
        /// </summary>
        public System.Action OnClick { get; set; }

        /// <summary>
        /// The actual Button component this object is a reference to.
        /// </summary>
        public Button Component { get; }

        /// <summary>
        /// The Text component on the button.
        /// </summary>
        public Text ButtonText { get; }

        /// <summary>
        /// The GameObject this Button is attached to.
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// The RectTransform for this Button.
        /// </summary>
        public RectTransform Transform { get; }

        /// <summary>
        /// Helper for <see cref="Button"/>.enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
