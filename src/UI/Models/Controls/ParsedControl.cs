using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;

namespace UniverseLib.UI.Models.Controls
{
    public class ParsedControl<T, TParser> : UIControlModel<T, InputField>, IUIStyledModel<IReadOnlyInputFieldStyle>
        where TParser : Parser, new()
    {
        private readonly static TParser parser = new();

        static ParsedControl()
        {
            if (!parser.CanParse<T>())
            {
                throw new System.ArgumentException(
                    $"The class {nameof(ParsedControl<T, TParser>)}<{typeof(T).Name}, {typeof(TParser).Name}> " +
                    $"is not valid because the type {typeof(T).Name} cannot be parsed by {typeof(TParser).Name}",
                    nameof(T)
                );
            }
        }


        public override GameObject UIRoot => styledInputField.UIRoot;
        public override InputField Component => styledInputField.Component;

        private readonly StyledInputField styledInputField;

        protected readonly ControlCallbackMode callbackMode;

        protected event System.Action<T> OnValueParsed;

        protected T CurrentValidValue { get; private set; }


        public ParsedControl(GameObject parent, string name, Getter<T> getter,
            Setter<T> setter = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit)
            : base(getter, setter, listenForUpdate)
        {
            styledInputField = new StyledInputField(parent, name, parser.GetExampleInput<T>());
            this.callbackMode = callbackMode;

            switch (callbackMode)
            {
                case ControlCallbackMode.OnValueChenged:
                    styledInputField.OnValueChanged += OnComponentValueChanged;
                    break;
                case ControlCallbackMode.OnEndEdit:
                    Component.GetOnEndEdit().AddListener(OnComponentValueChanged);
                    break;
            }
        }

        public void ApplyStyle(IReadOnlyInputFieldStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            styledInputField.ApplyStyle(style, fallbackSkin);
        }

        protected override void AddControlListener(System.Action<T> action)
        {
            OnValueParsed += (x) => action(x);
        }

        protected override void SetControlValue(T value, bool force = false)
        {
            if (force || !value.Equals(CurrentValidValue))
            {
                Component.text = parser.ToStringForInput<T>(value);
            }
            CurrentValidValue = value;
        }

        protected virtual void OnComponentValueChanged(string text)
        {
            if (parser.TryParse(text, out T value, out System.Exception ex))
            {
                T oldValue = CurrentValidValue;
                SetControlValue(value);
                T newValue = CurrentValidValue;

                if (!oldValue.Equals(newValue))
                {
                    OnValueParsed?.Invoke(value);
                }
            }
            else
            {
                SetControlValue(CurrentValidValue, true);
            }
        }
    }
}
