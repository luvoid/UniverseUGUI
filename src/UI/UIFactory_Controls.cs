using UnityEngine.Events;
using UnityEngine;
using UniverseLib.Utility;
using UniverseLib.UI.Models.Controls;
using UniverseLib.UI.Styles;
using UnityEngine.UI;
using System.Xml.Linq;
using Mono.Cecil.Rocks;
using Microsoft.Cci;

namespace UniverseLib.UI
{
    public delegate ref T RefGetter<T>();
    public delegate T Getter<out T>();
    public delegate void Setter<in T>(T value);

    public sealed partial class UIFactory
    {
        #region BoolControl
        public BoolControl BoolControl(GameObject parent, string name, Getter<bool> get, 
            Setter<bool> set = null, UnityEvent listenForUpdate = null, IReadOnlyToggleStyle style = null)
        {
            style ??= (IReadOnlyToggleStyle)Skin?.Toggle ?? UISkin.Default.Toggle;

            BoolControl boolControl = new(parent, name, get, set, listenForUpdate);
            boolControl.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(boolControl.GameObject, style);

            return boolControl;
        }

        public BoolControl BoolControl(GameObject parent, string name, string labelText, Getter<bool> get,
            Setter<bool> set = null, UnityEvent listenForUpdate = null, IReadOnlyToggleStyle style = null)
        {
            style ??= (IReadOnlyToggleStyle)Skin?.Toggle ?? UISkin.Default.Toggle;

            BoolControl boolControl = new(parent, name, labelText, get, set, listenForUpdate);
            boolControl.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(boolControl.GameObject, style);

            return boolControl;
        }

        public Property<BoolControl> BoolProperty(GameObject parent, string propertyName, Getter<bool> get,
            Setter<bool> set = null, UnityEvent listenForUpdate = null,
            IReadOnlyToggleStyle toggleStyle = null, IReadOnlyPanelStyle labelStyle = null)
        {
            toggleStyle ??= (IReadOnlyToggleStyle)Skin?.Toggle ?? UISkin.Default.Toggle;

            BoolControl control = new(null, "Control", get, set, listenForUpdate);
            control.ApplyStyle(toggleStyle, Skin);

            return Property(parent, propertyName, control, labelStyle);
        }

#if !(UNITY_EDITOR && UNITY_5)
        public BoolControl BoolControl(GameObject parent, string name,RefGetter<bool> refGet, 
            System.Action onSet = null, UnityEvent listenForUpdate = null, IReadOnlyToggleStyle style = null)
        {
            return BoolControl(
                parent,
                name,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                style
            );
        }

        public BoolControl BoolControl(GameObject parent, string name, string labelText, RefGetter<bool> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null, IReadOnlyToggleStyle style = null)
        {
            return BoolControl(
                parent,
                name,
                labelText,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                style
            );
        }

        public Property<BoolControl> BoolProperty(GameObject parent, string propertyName, RefGetter<bool> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null, 
            IReadOnlyToggleStyle toggleStyle = null, IReadOnlyPanelStyle labelStyle = null)
        {
            return BoolProperty(
                parent,
                propertyName,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                toggleStyle,
                labelStyle
            );
        }
#endif
        #endregion // BoolControl



        #region StringControl
        public StringControl StringControl(GameObject parent, string name, Getter<string> get,
            Setter<string> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle style = null)
        {
            style ??= (IReadOnlyInputFieldStyle)Skin?.InputField ?? UISkin.Default.InputField;

            StringControl stringControl = new(parent, name, get, set, listenForUpdate, callbackMode);
            stringControl.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(stringControl.GameObject, style);

            return stringControl;
        }

        public Property<StringControl> StringProperty(GameObject parent, string propertyName, Getter<string> get,
            Setter<string> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
        {
            inputFieldStyle ??= (IReadOnlyInputFieldStyle)Skin?.InputField ?? UISkin.Default.InputField;

            StringControl control = new(parent, "Control", get, set, listenForUpdate, callbackMode);
            control.ApplyStyle(inputFieldStyle, Skin);

            return Property(parent, propertyName, control, labelStyle);
        }

#if !(UNITY_EDITOR && UNITY_5)
        public StringControl StringControl(GameObject parent, string name, RefGetter<string> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null, 
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle style = null)
        {
            return StringControl(
                parent,
                name,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                callbackMode,
                style
            );
        }
        
        public Property<StringControl> StringProperty(GameObject parent, string propertyName, RefGetter<string> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null, 
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
        {
            return StringProperty(
                parent,
                propertyName,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                callbackMode,
                inputFieldStyle,
                labelStyle
            );
        }
#endif
        #endregion // StringControl



        #region ParsedControl
        public ParsedControl<T, ParserDefault> ParsedControl<T>(GameObject parent, string name, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit,
            IReadOnlyInputFieldStyle style = null)
            => ParsedControl<T, ParserDefault>(parent, name, get, set, listenForUpdate, callbackMode, style);

        public ParsedControl<T, TParser> ParsedControl<T, TParser>(GameObject parent, string name, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit,
            IReadOnlyInputFieldStyle style = null)
            where TParser : Parser, new()
        {
            style ??= (IReadOnlyInputFieldStyle)Skin?.InputField ?? UISkin.Default.InputField;

            ParsedControl<T, TParser> parsedControl = new ParsedControl<T, TParser>(parent, name, get, set, listenForUpdate, callbackMode);
            parsedControl.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(parsedControl.GameObject, style);

            return parsedControl;
        }

        public Property<ParsedControl<T, ParserDefault>> ParsedProperty<T>(GameObject parent, string propertyName, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
            => ParsedProperty<T, ParserDefault>(parent, propertyName, get, set, listenForUpdate, callbackMode, inputFieldStyle, labelStyle);

        public Property<ParsedControl<T, TParser>> ParsedProperty<T, TParser>(GameObject parent, string propertyName, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnEndEdit,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
            where TParser : Parser, new()
        {
            inputFieldStyle ??= (IReadOnlyInputFieldStyle)Skin?.InputField ?? UISkin.Default.InputField;

            ParsedControl<T, TParser> control = new(parent, "Control", get, set, listenForUpdate, callbackMode);
            control.ApplyStyle(inputFieldStyle, Skin);

            return Property(parent, propertyName, control, labelStyle);
        }

#if !(UNITY_EDITOR && UNITY_5)
        public ParsedControl<T, ParserDefault> ParsedControl<T>(GameObject parent, string name, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle style = null)
            => ParsedControl<T, ParserDefault>(parent, name, refGet, onSet, listenForUpdate, callbackMode, style);


        public ParsedControl<T, TParser> ParsedControl<T, TParser>(GameObject parent, string name, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle style = null)
            where TParser : Parser, new()
        {
            return ParsedControl<T, TParser>(
                parent,
                name,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                callbackMode,
                style
            );
        }

        public Property<ParsedControl<T, ParserDefault>> ParsedProperty<T>(GameObject parent, string name, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
            => ParsedProperty<T, ParserDefault>(parent, name, refGet, onSet, listenForUpdate, callbackMode, inputFieldStyle, labelStyle);

        public Property<ParsedControl<T, TParser>> ParsedProperty<T, TParser>(GameObject parent, string name, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            ControlCallbackMode callbackMode = ControlCallbackMode.OnValueChenged,
            IReadOnlyInputFieldStyle inputFieldStyle = null, IReadOnlyPanelStyle labelStyle = null)
            where TParser : Parser, new()
        {
            return ParsedProperty<T, TParser>(
                parent,
                name,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                callbackMode,
                inputFieldStyle,
                labelStyle
            );
        }
#endif
        #endregion // ParsedControl



        #region EnumControl
        public EnumControl<T> EnumControl<T>(GameObject parent, string name, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            IReadOnlyDropdownStyle style = null)
            where T : System.Enum
        {
            style ??= (IReadOnlyDropdownStyle)Skin?.Dropdown ?? UISkin.Default.Dropdown;

            EnumControl<T> enumControl = new(parent, name, get, set, listenForUpdate);
            enumControl.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(enumControl.GameObject, style);

            return enumControl;
        }

        public Property<EnumControl<T>> EnumProperty<T>(GameObject parent, string propertyName, Getter<T> get,
            Setter<T> set = null, UnityEvent listenForUpdate = null,
            IReadOnlyDropdownStyle dropdownStyle = null, IReadOnlyPanelStyle labelStyle = null)
            where T : System.Enum
        {
            dropdownStyle ??= (IReadOnlyDropdownStyle)Skin?.Dropdown ?? UISkin.Default.Dropdown;

            EnumControl<T> control = new(parent, "Control", get, set, listenForUpdate);
            control.ApplyStyle(dropdownStyle, Skin);

            return Property(parent, propertyName, control, labelStyle);
        }

#if !(UNITY_EDITOR && UNITY_5)
        public EnumControl<T> EnumControl<T>(GameObject parent, string name, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            IReadOnlyDropdownStyle style = null)
            where T : System.Enum
        {
            return EnumControl(
                parent,
                name,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                style
            );
        }

        public Property<EnumControl<T>> EnumProperty<T>(GameObject parent, string propertyName, RefGetter<T> refGet,
            System.Action onSet = null, UnityEvent listenForUpdate = null,
            IReadOnlyDropdownStyle dropdownStyle = null, IReadOnlyPanelStyle labelStyle = null)
            where T : System.Enum
        {
            return EnumProperty(
                parent,
                propertyName,
                () => refGet(),
                (value) => { refGet() = value; onSet?.Invoke(); },
                listenForUpdate,
                dropdownStyle,
                labelStyle
            );
        }
#endif
        #endregion // EnumControl



        public Property<T> Property<T>(GameObject parent, string propertyName, T control,
            IReadOnlyPanelStyle style = null)
            where T : UIControlModel
        {
            style ??= (IReadOnlyPanelStyle)Skin?.Label ?? UISkin.Default.Label;

            Property<T> property = new Property<T>(parent, propertyName, control);
            property.Label.ApplyStyle(style);

            SetDefaultLayoutElement(property.GameObject, style);

            return property;
        }


        #region GenericControl
        /*
        public static GameObject CreateControl<T>(GameObject parent, string labelText, Getter<T> get, 
            Setter<T> set = null, UnityEvent listen = null, T _ = default)
            where T : IEquatable<T>
        {
            GameObject control = null;
            if (ParseUtility.CanParse(typeof(T)))
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

        public static GameObject CreateControl<T>(GameObject parent, string labelText, RefGetter<T> refGet,
            Action onSet = null, UnityEvent listen = null, IReadOnlyToggleStyle style = null, T _ = default)
            where T : IEquatable<T>
        {
            return CreateControl(
                parent,
                labelText,
                () => refGet(),
                (T value) => { refGet() = value; onSet?.Invoke(); },
                listen
            );
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
        */
        #endregion // GenericControl
    }
}
