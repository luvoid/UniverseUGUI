using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;
using static UniverseLib.UI.Models.Controls.StringControl;

namespace UniverseLib.UI.Models.Controls
{
    public enum ControlCallbackMode
    {
        OnValueChenged,
        OnEndEdit
    }

    public class StringControl : UIControlModel<string, InputField>, IUIStyledModel<IReadOnlyInputFieldStyle>
    {

        public override GameObject UIRoot => styledInputField.UIRoot;
        public override InputField Component => styledInputField.Component;

        private readonly StyledInputField styledInputField;

        protected readonly ControlCallbackMode callbackMode;

        public StringControl(GameObject parent, string name, Getter<string> getter,
            Setter<string> setter = null, UnityEvent listenForUpdate = null, 
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged)
            : base(getter, setter, listenForUpdate)
        {
            styledInputField = new StyledInputField(parent, name, null);
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
