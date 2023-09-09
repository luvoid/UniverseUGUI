using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UI.Models
{
    /// <summary>
    /// An abstract UI object which does not exist as an actual UI Component, but which may be a reference to one.
    /// </summary>
    public abstract class UIModel
    {
        public abstract GameObject UIRoot { get; }

        public bool Enabled
        {
            get => UIRoot && UIRoot.activeInHierarchy;
            set
            {
                if (!UIRoot || Enabled == value)
                    return;
                UIRoot.SetActive(value);
                OnToggleEnabled?.Invoke(value);
            }
        }

        public event Action<bool> OnToggleEnabled;

        /// <summary>
        /// Most inheritors don't implement this. Use a more specific method.
        /// </summary>
        [System.Obsolete("Most inheritors don't implement this. Use a more specific method.", true)]
        public virtual void ConstructUI(GameObject parent) => throw new System.NotImplementedException();

        /// <summary>
        /// Toggle the <see cref="UIModel"/> to be active / inacive.
        /// </summary>
        public virtual void Toggle() => SetActive(!Enabled);

        public virtual void SetActive(bool active)
        {
            UIRoot?.SetActive(active);
        }

        public virtual void Destroy()
        {
            if (UIRoot)
                GameObject.Destroy(UIRoot);
        }
    }
}
