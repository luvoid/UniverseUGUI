using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Controls
{
    public class BoolControl : UIControlModel<bool, Toggle>, IUIStyledModel<IReadOnlyToggleStyle>
    {
        public override GameObject UIRoot => styledToggle.UIRoot;
        public override Toggle Component => styledToggle.Component;

        private readonly StyledToggle styledToggle;

        public BoolControl(GameObject parent, string name, Getter<bool> getter, Setter<bool> setter = null, UnityEvent listenForUpdate = null)
            : base(getter, setter, listenForUpdate)
        {
            styledToggle = new StyledToggle(parent, name, "");
            styledToggle.Label.gameObject.SetActive(false);
        }

        public BoolControl(GameObject parent, string name, string labelText, Getter<bool> getter, Setter<bool> setter = null, UnityEvent listenForUpdate = null)
            : base(getter, setter, listenForUpdate)
        {
            styledToggle = new StyledToggle(parent, name, labelText);
        }

        public void ApplyStyle(IReadOnlyToggleStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            styledToggle.ApplyStyle(style, fallbackSkin);
        }
        
        protected override void AddControlListener(Action<bool> action)
        {
            Component.onValueChanged.AddListener(action);
        }

        protected override void SetControlValue(bool value, bool force = false)
        {
            Component.isOn = value;
        }
    }
}
