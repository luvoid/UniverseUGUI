using Mono.Cecil.Rocks;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;

namespace UniverseLib.UI.Controls
{
    public abstract class ParsedControl<T> : UIControlModel<T, InputField>
    {
        protected abstract Parser Parser { get; }

        protected readonly ControlCallbackMode CallbackMode;

        protected event System.Action<T> OnValueParsed;

        protected T CurrentValidValue { get; set; }

        internal ParsedControl(Getter<T> getter, Setter<T> setter = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit)
            : base(getter, setter, listenForUpdate)
        {
            CallbackMode = callbackMode;
        }

        protected override void AddControlListener(System.Action<T> action)
        {
            OnValueParsed += (x) => action(x);
        }

        protected override void SetControlValue(T value, bool force = false)
        {
            if (force || !value.Equals(CurrentValidValue))
            {
                Component.text = Parser.ToStringForInput<T>(value);
            }
            CurrentValidValue = value;
        }

        protected virtual void OnComponentValueChanged(string text)
        {
            if (Parser.TryParse(text, out T value, out System.Exception ex))
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

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
    public class ParsedControl<T, TParser> : ParsedControl<T>, IStyledModel<IReadOnlyInputFieldStyle>
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

        public override GameObject UIRoot => inputFieldModel.UIRoot;
        public override InputField Component => inputFieldModel.Component;
        protected override Parser Parser => parser;

        private readonly InputFieldModel inputFieldModel;

        public ParsedControl(GameObject parent, string name, Getter<T> getter,
            Setter<T> setter = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit)
            : base(getter, setter, listenForUpdate, callbackMode)
        {
            inputFieldModel = new InputFieldModel(parent, name, parser.GetExampleInput<T>());

            switch (callbackMode)
            {
                case ControlCallbackMode.OnValueChenged:
                    inputFieldModel.OnValueChanged += OnComponentValueChanged;
                    break;
                case ControlCallbackMode.OnEndEdit:
                    Component.GetOnEndEdit().AddListener(OnComponentValueChanged);
                    break;
            }
        }

        public void ApplyStyle(IReadOnlyInputFieldStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            inputFieldModel.ApplyStyle(style, fallbackSkin);
        }
    }
}
