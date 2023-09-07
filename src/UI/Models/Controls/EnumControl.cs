using System;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Models.Controls
{
    public class EnumControl<T> : UIControlModel<T, Dropdown>, IUIStyledModel<IReadOnlyDropdownStyle>
        where T : Enum
    {
        private static readonly ReadOnlyCollection<T> enumValues = new((T[])Enum.GetValues(typeof(T)));
        private static readonly ReadOnlyCollection<string> enumNames = new(Enum.GetNames(typeof(T)));

        public override GameObject UIRoot => styledDropdown.UIRoot;
        public override Dropdown Component => styledDropdown.Component;

        protected event Action<T> OnValueChanged;

        private readonly StyledDropdown styledDropdown;

        public EnumControl(GameObject parent, string name, Getter<T> getter, 
            Setter<T> setter = null, UnityEvent listenForUpdate = null)
            : base(getter, setter, listenForUpdate)
        {
            styledDropdown = new StyledDropdown(parent, name, 0, enumNames);
            styledDropdown.OnValueChanged += OnComponentValueChanged;
        }

        protected override void AddControlListener(Action<T> action)
        {
            OnValueChanged += action;
        }

        protected override void SetControlValue(T value, bool force = false)
        {
            styledDropdown.Value = enumValues.IndexOf(value);
        }

        protected virtual void OnComponentValueChanged(int index)
        {
            OnValueChanged?.Invoke(enumValues[index]);
        }

        public void ApplyStyle(IReadOnlyDropdownStyle style, IReadOnlyUISkin fallbackSkin = null)
        {
            styledDropdown.ApplyStyle(style, fallbackSkin);
        }
    }
}
