using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine;
using UniverseLib.Utility;

namespace UniverseLib.UI
{
    public delegate T Getter<out T>();
    public delegate ref T RefGetter<T>();
    public delegate void Setter<in T>(T value);

    public static partial class UIFactory
    {
        public static GameObject CreateControl<T>(GameObject parent, string labelText, RefGetter<T> refGet, UnityAction onSet = null, UnityEvent listen = null)
        {
            return CreateControl(
                parent,
                labelText,
                () => refGet(),
                (T value) => { refGet() = value; onSet?.Invoke(); },
                listen
            );
        }

        public static GameObject CreateControl<T>(GameObject parent, string labelText, Getter<T> get, Setter<T> set = null, UnityEvent listen = null)
        {
            GameObject control = null;
            if (typeof(T) == typeof(bool))
            {
                if (get is not Getter<bool> getBool) throw new ArgumentException();
                if (set is not Setter<bool> setBool) setBool = null;
                control = CreateBoolControl(parent, labelText, getBool, setBool, listen);
            }
            else if (ParseUtility.CanParse(typeof(T)))
            {
                control = CreateParsedControl(parent, labelText, get, set, listen);
            }
            else
            {
                var name = labelText.Replace(" ", "");
                var errorText = CreateLabel(parent, $"ControlError_{name}", " " + labelText);
                errorText.text = $"{labelText} : Could not create control for type {typeof(T).Name}";
                control = errorText.gameObject;
                SetLayoutElement(control, minWidth: 200, minHeight: 25);
            }
            return control;
        }

        private static GameObject CreateParsedControl<T>(GameObject parent, string labelText, Getter<T> get, Setter<T> set = null, UnityEvent listen = null)
        {
            var name = labelText.Replace(" ", "");
            var row = CreateHorizontalGroup(parent, $"Control{typeof(T).Name}_{name}", false, false, true, true, childAlignment: TextAnchor.MiddleLeft);
            SetLayoutElement(row, minWidth: 200, minHeight: 25);
            {
                var inputFieldRef = CreateInputField(row, "Input", labelText);
                SetLayoutElement(inputFieldRef.GameObject, minWidth: 200, minHeight: 25);
                inputFieldRef.Text = ParseUtility.ToStringForInput(get(), typeof(T));
                listen?.AddListener(() => inputFieldRef.Text = ParseUtility.ToStringForInput(get(), typeof(T)));
                inputFieldRef.Component.interactable = (set != null);
                if (set != null)
                {
                    inputFieldRef.OnValueChanged += (str) =>
                    {
                        if (ParseUtility.TryParse(str, typeof(T), out var newValue, out _))
                        {
                            set((T)newValue);
                        }
                        inputFieldRef.Text = ParseUtility.ToStringForInput(get(), typeof(T));
                    };
                }

                CreateLabel(row, "Label", " " + labelText);
                SetLayoutElement(row, minWidth: 200, minHeight: 25);
            }
            return row;
        }

        private static GameObject CreateBoolControl(GameObject parent, string labelText, Getter<bool> get, Setter<bool> set = null, UnityEvent listen = null)
        {
            var name = "ControlBool_" + labelText.Replace(" ", "");
            var toggleRoot = CreateToggle(parent, name, out var toggle, out var label);
            label.text = labelText;
            toggle.isOn = get();
            listen?.AddListener(() => toggle.isOn = get());
            toggle.interactable = (set != null);
            if (set != null)
            {
                toggle.onValueChanged.AddListener((value) =>
                {
                    set(value);
                });
            }
            SetLayoutElement(toggleRoot, minWidth: 200, minHeight: 25);
            return toggleRoot;
        }
    }
}
