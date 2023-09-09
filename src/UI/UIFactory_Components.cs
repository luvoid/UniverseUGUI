using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;
using UniverseLib.UI.Models.Styled;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;

namespace UniverseLib.UI
{
    public sealed partial class UIFactory
    {


        /// <summary>
        /// Create a styled label.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your label</param>
        /// <param name="text">The default text of the label</param>
        /// <param name="style">The style to use when creating the label. Defaults to <see cref="UISkin.Default"/>'s Label</param>
        /// <returns>A <see cref="StyledLabel"/> wrapper for your <see cref="Text"/> component.</returns>
        public StyledLabel Label(GameObject parent, string name, string text, IReadOnlyFrameStyle style = null)
        {
            style ??= (IReadOnlyFrameStyle)Skin?.Label ?? UISkin.Default.Label;

            StyledLabel label = new(parent, name, text);
            label.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(label.GameObject, style);

            return label;
        }


        /// <summary>
        /// Create a styled button.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your button</param>
        /// <param name="text">The default button text</param>
        /// <param name="style">The style to use when creating the button. Defaults to <see cref="Skin"/>'s Label.</param>
        /// <returns>A <see cref="StyledButton"/> wrapper for your <see cref="UnityEngine.UI.Button"/> component.</returns>
        public StyledButton Button(GameObject parent, string name, string text, IReadOnlyButtonStyle style = null)
        {
            style ??= (IReadOnlyButtonStyle)Skin?.Button ?? UISkin.Default.Button;

            StyledButton button = new(parent, name, text);
            button.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(button.GameObject, style);

            return button;
        }


        /// <summary>
        /// Create a styled Toggle control.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your toggle</param>
        /// <param name="style">The style to use when creating the toggle. Defaults to <see cref="Skin"/>'s Toggle.</param>
        /// <returns>A <see cref="StyledToggle"/> wrapper for your <see cref="UnityEngine.UI.Toggle"/> component.</returns>
        public StyledToggle Toggle(GameObject parent, string name, string text, IReadOnlyToggleStyle style = null)
        {
            style ??= (IReadOnlyToggleStyle)Skin?.Toggle ?? UISkin.Default.Toggle;

            StyledToggle toggle = new(parent, name, text);
            toggle.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(toggle.GameObject, style);

            return toggle;
        }

        /// <summary>
        /// Create a styled InputField control.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your InputField</param>
        /// <param name="placeHolderText">The placeholder text for your InputField component</param>
        /// <param name="style">The style to use when creating the input field. Defaults to <see cref="Skin"/>'s InputField.</param>
        /// <returns>An InputFieldRef wrapper for your InputField</returns>
        public StyledInputField InputField(GameObject parent, string name, string placeHolderText, IReadOnlyInputFieldStyle style = null)
        {
            style ??= (IReadOnlyInputFieldStyle)Skin?.InputField ?? UISkin.Default.InputField;

            StyledInputField inputField = new(parent, name, placeHolderText);
            inputField.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(inputField.GameObject, style);

            return inputField;
        }

        /// <summary>
        /// Create a styled Dropdown control.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your Dropdown</param>
        /// <param name="onValueChanged">Invoked when your Dropdown value is changed</param>
        /// <param name="style">The style to use when creating the Dropdown. Defaults to <see cref="Skin"/>'s Dropdown.</param>
        /// <param name="defaultOptions">Optional default options for the dropdown</param>
        /// <returns>A <see cref="StyledButton"/> wrapper for your <see cref="Button"/> component.</returns>
        public StyledDropdown Dropdown(GameObject parent, string name, Action<int> onValueChanged, IReadOnlyDropdownStyle style = null,
            int defaultValue = 0, params string[] defaultOptions)
        {
            style ??= (IReadOnlyDropdownStyle)Skin?.Dropdown ?? UISkin.Default.Dropdown;

            StyledDropdown dropdown = new(parent, name, defaultValue, defaultOptions);
            dropdown.OnValueChanged = onValueChanged;
            dropdown.ApplyStyle(style, Skin);

            SetDefaultLayoutElement(dropdown.GameObject, style);

            return dropdown;
        }
    }
}
