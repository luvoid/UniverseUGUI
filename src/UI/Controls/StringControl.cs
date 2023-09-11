using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;
using static UniverseLib.UI.Controls.StringControl;

namespace UniverseLib.UI.Controls
{
    public enum ControlCallbackMode
    {
        OnValueChenged,
        OnEndEdit
    }

    public class StringControl : UIControlModel<string, InputField>, IStyledModel<IReadOnlyInputFieldStyle>
    {

        public override GameObject UIRoot => styledInputField.UIRoot;
        public override InputField Component => styledInputField.Component;

        private readonly InputFieldModel styledInputField;

        protected readonly ControlCallbackMode callbackMode;

        public StringControl(GameObject parent, string name, Getter<string> getter,
            Setter<string> setter = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged)
            : base(getter, setter, listenForUpdate)
        {
            styledInputField = new InputFieldModel(parent, name, null);
            this.callbackMode = callbackMode;
        }

        public void ApplyStyle(IReadOnlyInputFieldStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            styledInputField.ApplyStyle(style, fallbackSkin);
        }

        protected override void AddControlListener(Action<string> action)
        {
            switch (callbackMode)
            {
                case ControlCallbackMode.OnValueChenged:
                    styledInputField.OnValueChanged += action;
                    break;
                case ControlCallbackMode.OnEndEdit:
                    Component.GetOnEndEdit().AddListener(action);
                    break;
            }
        }

        protected override void SetControlValue(string value, bool force = false)
        {
            Component.text = value;
        }
    }
}
