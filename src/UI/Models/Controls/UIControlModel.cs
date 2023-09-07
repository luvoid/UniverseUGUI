using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UniverseLib.UI.Models.Controls
{
    public abstract class UIControlModel : UIBehaviourModel
    {
        private readonly bool isListening;

        private bool didStart = false;

        public Selectable Component => _selectableComponent;
        protected abstract Selectable _selectableComponent { get; }

        internal UIControlModel(UnityEvent listenForUpdate)
        {
            isListening = listenForUpdate != null;
            listenForUpdate?.AddListener(() => UpdateControlValue());
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [System.Obsolete("Not implemented.", true)]
        public sealed override void ConstructUI(GameObject parent)
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            if (!didStart)
            {
                Start();
                didStart = true;
            }

            if (!isListening)
            {
                UpdateControlValue();
            }
        }

        public abstract void Start();

        protected abstract void UpdateControlValue(bool force = false);
    }

    public abstract class UIControlModel<T, TSelectable> : UIControlModel
        where TSelectable : Selectable
    {
        protected sealed override Selectable _selectableComponent => Component;

        public GameObject GameObject => UIRoot;
        public new abstract TSelectable Component { get; }

        protected readonly Getter<T> Getter;
        protected readonly Setter<T> Setter;

        public UIControlModel(Getter<T> getter, Setter<T> setter = null, UnityEvent listenForUpdate = null)
            : base(listenForUpdate)
        {
            Getter = getter;
            Setter = setter;
        }

        public override void Start()
        {
            UpdateControlValue(force: true);
            Component.interactable = Setter != null;
            AddControlListener(OnControlValueChanged);
        }

        protected override void UpdateControlValue(bool force = false)
        {
            T value = Getter();
            SetControlValue(value, force);
        }

        protected void OnControlValueChanged(T value)
        {
            Setter?.Invoke(value);
        }

        protected abstract void AddControlListener(System.Action<T> action);

        protected abstract void SetControlValue(T value, bool force = false);
    }
}
