using System;
using UnityEngineInternal;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UGUI.Models;

namespace UniverseLib.UGUI
{
    public class UGUI
    {
        private static IUniversalUGUIObject s_ActiveUGUI => UGUIUtility.s_ActiveUGUI;
        private static GameObject s_ActiveParent => UGUIUtility.s_ActiveParent;

        private static float s_ScrollStepSize = 10f;
        private static int s_ScrollControlId;
        private static int s_HotTextField = -1;
        private static readonly int s_BoxHash = "Box".GetHashCode();
        private static readonly int s_RepeatButtonHash = "repeatButton".GetHashCode();
        private static readonly int s_ToggleHash = "Toggle".GetHashCode();
        private static readonly int s_ButtonGridHash = "ButtonGrid".GetHashCode();
        private static readonly int s_SliderHash = "Slider".GetHashCode();
        private static readonly int s_BeginGroupHash = "BeginGroup".GetHashCode();
        private static readonly int s_ScrollviewHash = "scrollView".GetHashCode();
        private static GUISkin s_Skin = UGUIUtility.GetDefaultSkin();
        internal static Rect s_ToolTipRect;
        private static GenericStack s_ScrollViewStates = new GenericStack();
        internal static bool changed;

        static UGUI() => nextScrollStepTime = DateTime.Now;

        internal static int scrollTroughSide { get; set; }

        internal static DateTime nextScrollStepTime { get; set; }

        public static GUISkin skin
        {
            set
            {
                UGUIUtility.CheckOnUGUI();
                DoSetSkin(value);
            }
            get
            {
                UGUIUtility.CheckOnUGUI();
                return s_Skin;
            }
        }

        public static Color color = Color.white;
        public static Color backgroundColor = Color.white;
        public static Color contentColor = Color.white;

        internal static void DoSetSkin(GUISkin newSkin)
        {
            if (!(bool)newSkin)
                newSkin = UGUIUtility.GetDefaultSkin();
            s_Skin = newSkin;
            newSkin.MakeCurrent();
        }

        public static Matrix4x4 matrix
        {
            get => GUIClip.GetMatrix();
            set => GUIClip.SetMatrix(value);
        }

        public static string tooltip
        {
            get => throw new NotImplementedException(); //Internal_GetTooltip() ?? "";
            set => throw new NotImplementedException(); //Internal_SetTooltip(value);
        }

        protected static string mouseTooltip => throw new NotImplementedException(); //Internal_GetMouseTooltip();

        protected static Rect tooltipRect
        {
            get => s_ToolTipRect;
            set => s_ToolTipRect = value;
        }

        public static LabelResult Label(Rect position, UGUIContent content, GUIStyle style = null)
        {
            style ??= s_Skin.label;
            UGUIUtility.CheckOnUGUI();

            if (!UGUIUtility.TryGetControlModel(out LabelResult label, out int controlId))
            {
                label = new("Label", s_ActiveParent, position, content, style);
                UGUIUtility.SetControlModel(label, controlId);
            }
            label.SetState(position, content, style);

            return label;
        }



        /*
        public static void DrawTexture(Rect position, Texture image) => DrawTexture(position, image, ScaleMode.StretchToFill);

        public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode) => DrawTexture(position, image, scaleMode, true);

        public static void DrawTexture(
          Rect position,
          Texture image,
          ScaleMode scaleMode,
          bool alphaBlend)
        {
            DrawTexture(position, image, scaleMode, alphaBlend, 0.0f);
        }

        public static void DrawTexture(
          Rect position,
          Texture image,
          ScaleMode scaleMode,
          bool alphaBlend,
          float imageAspect)
        {
            UGUIUtility.CheckOnUGUI();
            if (UGUIEvent.current.type != EventType.Repaint)
                return;
            if ((Object)image == (Object)null)
            {
                Debug.LogWarning((object)"null texture passed to UI.DrawTexture");
            }
            else
            {
                if ((double)imageAspect == 0.0)
                    imageAspect = (float)image.width / (float)image.height;
                Material material = !alphaBlend ? blitMaterial : blendMaterial;
                Internal_DrawTextureArguments args = new Internal_DrawTextureArguments();
                args.leftBorder = 0;
                args.rightBorder = 0;
                args.topBorder = 0;
                args.bottomBorder = 0;
                args.color = (Color32)color;
                args.texture = image;
                args.mat = material;
                CalculateScaledTextureRects(position, scaleMode, imageAspect, ref args.screenRect, ref args.sourceRect);
                Graphics.Internal_DrawTexture(ref args);
            }
        }

        internal static bool CalculateScaledTextureRects(
          Rect position,
          ScaleMode scaleMode,
          float imageAspect,
          ref Rect outScreenRect,
          ref Rect outSourceRect)
        {
            float num1 = position.width / position.height;
            bool flag = false;
            switch (scaleMode)
            {
                case ScaleMode.StretchToFill:
                    outScreenRect = position;
                    outSourceRect = new Rect(0.0f, 0.0f, 1f, 1f);
                    flag = true;
                    break;
                case ScaleMode.ScaleAndCrop:
                    if ((double)num1 > (double)imageAspect)
                    {
                        float height = imageAspect / num1;
                        outScreenRect = position;
                        outSourceRect = new Rect(0.0f, (float)((1.0 - (double)height) * 0.5), 1f, height);
                        flag = true;
                        break;
                    }
                    float width = num1 / imageAspect;
                    outScreenRect = position;
                    outSourceRect = new Rect((float)(0.5 - (double)width * 0.5), 0.0f, width, 1f);
                    flag = true;
                    break;
                case ScaleMode.ScaleToFit:
                    if ((double)num1 > (double)imageAspect)
                    {
                        float num2 = imageAspect / num1;
                        outScreenRect = new Rect(position.xMin + (float)((double)position.width * (1.0 - (double)num2) * 0.5), position.yMin, num2 * position.width, position.height);
                        outSourceRect = new Rect(0.0f, 0.0f, 1f, 1f);
                        flag = true;
                        break;
                    }
                    float num3 = num1 / imageAspect;
                    outScreenRect = new Rect(position.xMin, position.yMin + (float)((double)position.height * (1.0 - (double)num3) * 0.5), position.width, num3 * position.height);
                    outSourceRect = new Rect(0.0f, 0.0f, 1f, 1f);
                    flag = true;
                    break;
            }
            return flag;
        }

        public static void DrawTextureWithTexCoords(Rect position, Texture image, Rect texCoords) => DrawTextureWithTexCoords(position, image, texCoords, true);

        public static void DrawTextureWithTexCoords(
          Rect position,
          Texture image,
          Rect texCoords,
          bool alphaBlend)
        {
            UGUIUtility.CheckOnUGUI();
            if (UGUIEvent.current.type != EventType.Repaint)
                return;
            Material material = !alphaBlend ? blitMaterial : blendMaterial;
            var args = new Internal_DrawTextureArguments()
            {
                texture = image,
                mat = material,
                leftBorder = 0,
                rightBorder = 0,
                topBorder = 0,
                bottomBorder = 0,
                color = (Color32)color,
                screenRect = position,
                sourceRect = texCoords
            };
            Graphics.Internal_DrawTexture(ref args);
        }
        */



        public static BoxResult Box(Rect position, UGUIContent content, GUIStyle style = null)
        {
            style ??= s_Skin.box;
            UGUIUtility.CheckOnUGUI();

            if (!UGUIUtility.TryGetControlModel(out BoxResult box, out int controlId))
            {
                box = new("Box", s_ActiveParent, position, content, style);
                UGUIUtility.SetControlModel(box, controlId);
            }
            box.SetState(position, content, style);
            return box;
        }



        public static ButtonResult Button(Rect position, UGUIContent content, GUIStyle style = null)
        {
            var button = DoButton(position, content, style);
            button.SetState(position, content, style);
            return button;
        }

        private static ButtonResult DoButton(Rect position, UGUIContent content, GUIStyle style = null)
        {
            style ??= s_Skin.button;
            UGUIUtility.CheckOnUGUI();

            //if (UGUIEvent.current.type != EventType.Repaint) return null;

            if (!UGUIUtility.TryGetControlModel(out ButtonResult button, out int controlId))
            {
                button = new("Button", s_ActiveParent, position, content, style);
                UGUIUtility.SetControlModel(button, controlId);
            }

            return button;
        }



        /*
        public static IObservable<bool> RepeatButton(Rect position, UGUIContent content) => DoRepeatButton(position, content, s_Skin.button, FocusType.Passive);

        public static IObservable<bool> RepeatButton(Rect position, UGUIContent content, GUIStyle style) => DoRepeatButton(position, content, style, FocusType.Passive);

        private static IObservable<bool> DoRepeatButton(
          Rect position,
          UGUIContent content,
          GUIStyle style,
          FocusType focusType)
        {
            UGUIUtility.CheckOnUGUI();
            int controlId = UGUIUtility.GetControlID(s_RepeatButtonHash, focusType, position);
            switch (Event.current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    if (position.Contains(Event.current.mousePosition))
                    {
                        UGUIUtility.hotControl = controlId;
                        Event.current.Use();
                    }
                    return false;
                case EventType.MouseUp:
                    if (UGUIUtility.hotControl != controlId)
                        return false;
                    UGUIUtility.hotControl = 0;
                    Event.current.Use();
                    return position.Contains(Event.current.mousePosition);
                case EventType.Repaint:
                    style.Draw(position, content, controlId);
                    return controlId == UGUIUtility.hotControl && position.Contains(Event.current.mousePosition);
                default:
                    return false;
            }
        }
        */



        public static TextFieldResult TextField(Rect position, string text, GUIStyle style)
            => TextField(position, text, style: style);

        public static TextFieldResult TextField(Rect position, string text, int maxLength = -1, GUIStyle style = null)
        {
            style ??= skin.textField;
            UGUIUtility.CheckOnUGUI();
            UGUIContent content = text;
            return DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, style);
        }



        /*
        public static IObservable<string> PasswordField(Rect position, string password, char maskChar) 
            => PasswordField(position, password, maskChar, -1, skin.textField);

        public static IObservable<string> PasswordField(
          Rect position,
          string password,
          char maskChar,
          int maxLength)
        {
            return PasswordField(position, password, maskChar, maxLength, skin.textField);
        }

        public static IObservable<string> PasswordField(
          Rect position,
          string password,
          char maskChar,
          GUIStyle style)
        {
            return PasswordField(position, password, maskChar, -1, style);
        }

        public static IObservable<string> PasswordField(
          Rect position,
          string password,
          char maskChar,
          int maxLength,
          GUIStyle style)
        {
            UGUIUtility.CheckOnUGUI();
            UGUIContent content = PasswordFieldGetStrToShow(password, maskChar);
            bool changed = changed;
            changed = false;
            if (TouchScreenKeyboard.isSupported)
                DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard), content, false, maxLength, style, password, maskChar);
            else
                DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, false, maxLength, style);
            string str = !changed ? password : content.text;
            changed |= changed;
            return str;
        }

        internal static IObservable<string> PasswordFieldGetStrToShow(string password, char maskChar) => Event.current.type == EventType.Repaint || Event.current.type == EventType.MouseDown ? "".PadRight(password.Length, maskChar) : password;

        public static IObservable<string> TextArea(Rect position, string text)
        {
            UGUIContent content = text;
            DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, true, -1, skin.textArea);
            return content.text;
        }

        public static IObservable<string> TextArea(Rect position, string text, int maxLength)
        {
            UGUIContent content = text;
            DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, true, maxLength, skin.textArea);
            return content.text;
        }

        public static IObservable<string> TextArea(Rect position, string text, GUIStyle style)
        {
            UGUIContent content = text;
            DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, true, -1, style);
            return content.text;
        }

        public static IObservable<string> TextArea(Rect position, string text, int maxLength, GUIStyle style)
        {
            UGUIContent content = text;
            DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content, false, maxLength, style);
            return content.text;
        }

        private static IObservable<string> TextArea(
          Rect position,
          UGUIContent content,
          int maxLength,
          GUIStyle style)
        {
            UGUIContent content1 = new(content);
            DoTextField(position, UGUIUtility.GetControlID(FocusType.Keyboard, position), content1, false, maxLength, style);
            return content1.text;
        }

        */


        internal static TextFieldResult DoTextField(
          Rect position,
          int id,
          UGUIContent content,
          bool multiline,
          int maxLength,
          GUIStyle style,
          string secureText = null,
          char maskChar = char.MinValue)
            => DoTextField(position, id, content, style, multiline, maxLength, secureText, maskChar);


        internal static TextFieldResult DoTextField(
          Rect position,
          int id,
          UGUIContent content,
          GUIStyle style,
          bool multiline = false,
          int maxLength = -1,
          string secureText = null,
          char maskChar = char.MinValue)
        {
            if (maxLength >= 0 && content.text.Length > maxLength)
                content.text = content.text.Substring(0, maxLength);

            //if (UGUIEvent.current.type != EventType.Repaint) return null;
            if (!UGUIUtility.TryGetControlModel(out TextFieldResult textField, out int controlId, id))
            {
                textField = new("TextField", s_ActiveParent, position, content, style);
                UGUIUtility.SetControlModel(textField, controlId);
            }
            textField.SetState(position, content, style);
            return textField;


            //TextEditor stateObject = (TextEditor)UGUIUtility.GetStateObject(typeof(TextEditor), id);
            //stateObject.text = content.text;
            //stateObject.SaveBackup();
            //stateObject.position = position;
            //stateObject.style = style;
            //stateObject.multiline = multiline;
            //stateObject.controlID = id;
            //stateObject.DetectFocusChange();
            //
            //if (TouchScreenKeyboard.isSupported)
            //	HandleTextFieldEventForTouchscreen(position, id, content, multiline, maxLength, style, secureText, maskChar, stateObject);
            //else
            //	HandleTextFieldEventForDesktop(position, id, content, multiline, maxLength, style, stateObject);
            //
            //stateObject.UpdateScrollOffsetIfNeeded(Event.current);
        }



        /*
        private static void HandleTextFieldEventForTouchscreen(
          Rect position,
          int id,
          UGUIContent content,
          bool multiline,
          int maxLength,
          GUIStyle style,
          string secureText,
          char maskChar,
          TextEditor editor)
        {
            Event current = Event.current;
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (!position.Contains(current.mousePosition))
                        break;
                    UGUIUtility.hotControl = id;
                    if (s_HotTextField != -1 && s_HotTextField != id)
                        ((TextEditor)UGUIUtility.GetStateObject(typeof(TextEditor), s_HotTextField)).keyboardOnScreen = (TouchScreenKeyboard)null;
                    s_HotTextField = id;
                    if (UGUIUtility.keyboardControl != id)
                        UGUIUtility.keyboardControl = id;
                    editor.keyboardOnScreen = TouchScreenKeyboard.Open(secureText == null ? content.text : secureText, TouchScreenKeyboardType.Default, true, multiline, secureText != null);
                    current.Use();
                    break;
                case EventType.Repaint:
                    if (editor.keyboardOnScreen != null)
                    {
                        content.text = editor.keyboardOnScreen.text;
                        if (maxLength >= 0 && content.text.Length > maxLength)
                            content.text = content.text.Substring(0, maxLength);
                        if (editor.keyboardOnScreen.done)
                        {
                            editor.keyboardOnScreen = (TouchScreenKeyboard)null;
                            changed = true;
                        }
                    }
                    string text = content.text;
                    if (secureText != null)
                        content.text = PasswordFieldGetStrToShow(text, maskChar);
                    style.Draw(position, content, id, false);
                    content.text = text;
                    break;
            }
        }

        private static void HandleTextFieldEventForDesktop(
          Rect position,
          int id,
          UGUIContent content,
          bool multiline,
          int maxLength,
          GUIStyle style,
          TextEditor editor)
        {
            Event current = Event.current;
            bool flag = false;
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (position.Contains(current.mousePosition))
                    {
                        UGUIUtility.hotControl = id;
                        UGUIUtility.keyboardControl = id;
                        editor.m_HasFocus = true;
                        editor.MoveCursorToPosition(Event.current.mousePosition);
                        if (Event.current.clickCount == 2 && skin.settings.doubleClickSelectsWord)
                        {
                            editor.SelectCurrentWord();
                            editor.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                            editor.MouseDragSelectsWholeWords(true);
                        }
                        if (Event.current.clickCount == 3 && skin.settings.tripleClickSelectsLine)
                        {
                            editor.SelectCurrentParagraph();
                            editor.MouseDragSelectsWholeWords(true);
                            editor.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                        }
                        current.Use();
                        break;
                    }
                    break;
                case EventType.MouseUp:
                    if (UGUIUtility.hotControl == id)
                    {
                        editor.MouseDragSelectsWholeWords(false);
                        UGUIUtility.hotControl = 0;
                        current.Use();
                        break;
                    }
                    break;
                case EventType.MouseDrag:
                    if (UGUIUtility.hotControl == id)
                    {
                        if (current.shift)
                            editor.MoveCursorToPosition(Event.current.mousePosition);
                        else
                            editor.SelectToPosition(Event.current.mousePosition);
                        current.Use();
                        break;
                    }
                    break;
                case EventType.KeyDown:
                    if (UGUIUtility.keyboardControl != id)
                        return;
                    if (editor.HandleKeyEvent(current))
                    {
                        current.Use();
                        flag = true;
                        content.text = editor.text;
                        break;
                    }
                    if (current.keyCode == KeyCode.Tab || current.character == '\t')
                        return;
                    char character = current.character;
                    if (character == '\n' && !multiline && !current.alt)
                        return;
                    Font font = style.font;
                    if (!(bool)(Object)font)
                        font = skin.font;
                    if (!font.HasCharacter(character))
                    {
                        switch (character)
                        {
                            case char.MinValue:
                                if (Input.compositionString.Length > 0)
                                {
                                    editor.ReplaceSelection("");
                                    flag = true;
                                }
                                current.Use();
                                goto label_34;
                            case '\n':
                                break;
                            default:
                                goto label_34;
                        }
                    }
                    editor.Insert(character);
                    flag = true;
                    break;
                case EventType.Repaint:
                    if (UGUIUtility.keyboardControl != id)
                    {
                        style.Draw(position, content, id, false);
                        break;
                    }
                    editor.DrawCursor(content.text);
                    break;
            }
        label_34:
            if (UGUIUtility.keyboardControl == id)
                UGUIUtility.textFieldInput = true;
            if (!flag)
                return;
            changed = true;
            content.text = editor.text;
            if (maxLength >= 0 && content.text.Length > maxLength)
                content.text = content.text.Substring(0, maxLength);
            current.Use();
        }
        */



        public static ToggleResult Toggle(Rect position, bool value, UGUIContent content, GUIStyle style = null)
            => Toggle(position, UGUIUtility.GetControlID(s_ToggleHash, FocusType.Passive, position), value, content, style);

        public static ToggleResult Toggle(
          Rect position,
          int controlId,
          bool value,
          UGUIContent content,
          GUIStyle style = null)
        {
            style ??= s_Skin.toggle;
            UGUIUtility.CheckOnUGUI();
            return DoToggle(position, controlId, value, content, style);
        }



        /*
        public static IObservable<int> Toolbar(Rect position, int selected, string[] texts) => Toolbar(position, selected, UGUIContent.Temp(texts), s_Skin.button);

        public static IObservable<int> Toolbar(Rect position, int selected, Texture[] images) => Toolbar(position, selected, UGUIContent.Temp(images), s_Skin.button);

        public static IObservable<int> Toolbar(Rect position, int selected, UGUIContent[] content) => Toolbar(position, selected, content, s_Skin.button);

        public static IObservable<int> Toolbar(Rect position, int selected, string[] texts, GUIStyle style) => Toolbar(position, selected, UGUIContent.Temp(texts), style);

        public static IObservable<int> Toolbar(Rect position, int selected, Texture[] images, GUIStyle style) => Toolbar(position, selected, UGUIContent.Temp(images), style);

        public static IObservable<int> Toolbar(Rect position, int selected, UGUIContent[] contents, GUIStyle style)
        {
            UGUIUtility.CheckOnUGUI();
            GUIStyle firstStyle;
            GUIStyle midStyle;
            GUIStyle lastStyle;
            FindStyles(ref style, out firstStyle, out midStyle, out lastStyle, "left", "mid", "right");
            return DoButtonGrid(position, selected, contents, contents.Length, style, firstStyle, midStyle, lastStyle);
        }

        public static IObservable<int> SelectionGrid(Rect position, int selected, string[] texts, int xCount) => SelectionGrid(position, selected, UGUIContent.Temp(texts), xCount, (GUIStyle)null);

        public static IObservable<int> SelectionGrid(Rect position, int selected, Texture[] images, int xCount) => SelectionGrid(position, selected, UGUIContent.Temp(images), xCount, (GUIStyle)null);

        public static IObservable<int> SelectionGrid(Rect position, int selected, UGUIContent[] content, int xCount) => SelectionGrid(position, selected, content, xCount, (GUIStyle)null);

        public static IObservable<int> SelectionGrid(
          Rect position,
          int selected,
          string[] texts,
          int xCount,
          GUIStyle style)
        {
            return SelectionGrid(position, selected, UGUIContent.Temp(texts), xCount, style);
        }

        public static IObservable<int> SelectionGrid(
          Rect position,
          int selected,
          Texture[] images,
          int xCount,
          GUIStyle style)
        {
            return SelectionGrid(position, selected, UGUIContent.Temp(images), xCount, style);
        }

        public static IObservable<int> SelectionGrid(
          Rect position,
          int selected,
          UGUIContent[] contents,
          int xCount,
          GUIStyle style)
        {
            if (style == null)
                style = s_Skin.button;
            return DoButtonGrid(position, selected, contents, xCount, style, style, style, style);
        }

        internal static void FindStyles(
          ref GUIStyle style,
          out GUIStyle firstStyle,
          out GUIStyle midStyle,
          out GUIStyle lastStyle,
          string first,
          string mid,
          string last)
        {
            if (style == null)
                style = skin.button;
            string name = style.name;
            midStyle = skin.FindStyle(name + mid);
            if (midStyle == null)
                midStyle = style;
            firstStyle = skin.FindStyle(name + first);
            if (firstStyle == null)
                firstStyle = midStyle;
            lastStyle = skin.FindStyle(name + last);
            if (lastStyle != null)
                return;
            lastStyle = midStyle;
        }

        internal static int CalcTotalHorizSpacing(
          int xCount,
          GUIStyle style,
          GUIStyle firstStyle,
          GUIStyle midStyle,
          GUIStyle lastStyle)
        {
            if (xCount < 2)
                return 0;
            if (xCount == 2)
                return Mathf.Max(firstStyle.margin.right, lastStyle.margin.left);
            int num = Mathf.Max(midStyle.margin.left, midStyle.margin.right);
            return Mathf.Max(firstStyle.margin.right, midStyle.margin.left) + Mathf.Max(midStyle.margin.right, lastStyle.margin.left) + num * (xCount - 3);
        }

        private static IObservable<int> DoButtonGrid(
          Rect position,
          int selected,
          UGUIContent[] contents,
          int xCount,
          GUIStyle style,
          GUIStyle firstStyle,
          GUIStyle midStyle,
          GUIStyle lastStyle)
        {
            UGUIUtility.CheckOnUGUI();
            int length = contents.Length;
            if (length == 0)
                return selected;
            if (xCount <= 0)
            {
                Debug.LogWarning((object)"You are trying to create a SelectionGrid with zero or less elements to be displayed in the horizontal direction. Set xCount to a positive value.");
                return selected;
            }
            int controlId = UGUIUtility.GetControlID(s_ButtonGridHash, FocusType.Passive, position);
            int num1 = length / xCount;
            if (length % xCount != 0)
                ++num1;
            float num2 = (float)CalcTotalHorizSpacing(xCount, style, firstStyle, midStyle, lastStyle);
            float num3 = (float)(Mathf.Max(style.margin.top, style.margin.bottom) * (num1 - 1));
            float elemWidth = (position.width - num2) / (float)xCount;
            float elemHeight = (position.height - num3) / (float)num1;
            if ((double)style.fixedWidth != 0.0)
                elemWidth = style.fixedWidth;
            if ((double)style.fixedHeight != 0.0)
                elemHeight = style.fixedHeight;
            switch (Event.current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    if (position.Contains(Event.current.mousePosition) && GetButtonGridMouseSelection(CalcMouseRects(position, length, xCount, elemWidth, elemHeight, style, firstStyle, midStyle, lastStyle, false), Event.current.mousePosition, true) != -1)
                    {
                        UGUIUtility.hotControl = controlId;
                        Event.current.Use();
                        break;
                    }
                    break;
                case EventType.MouseUp:
                    if (UGUIUtility.hotControl == controlId)
                    {
                        UGUIUtility.hotControl = 0;
                        Event.current.Use();
                        int gridMouseSelection = GetButtonGridMouseSelection(CalcMouseRects(position, length, xCount, elemWidth, elemHeight, style, firstStyle, midStyle, lastStyle, false), Event.current.mousePosition, true);
                        changed = true;
                        return gridMouseSelection;
                    }
                    break;
                case EventType.MouseDrag:
                    if (UGUIUtility.hotControl == controlId)
                    {
                        Event.current.Use();
                        break;
                    }
                    break;
                case EventType.Repaint:
                    GUIStyle guiStyle1 = (GUIStyle)null;
                    GUIClip.Push(position, Vector2.zero, Vector2.zero, false);
                    position = new Rect(0.0f, 0.0f, position.width, position.height);
                    Rect[] buttonRects = CalcMouseRects(position, length, xCount, elemWidth, elemHeight, style, firstStyle, midStyle, lastStyle, false);
                    int gridMouseSelection1 = GetButtonGridMouseSelection(buttonRects, Event.current.mousePosition, controlId == UGUIUtility.hotControl);
                    UGUIUtility.mouseUsed |= position.Contains(Event.current.mousePosition);
                    for (int index = 0; index < length; ++index)
                    {
                        GUIStyle guiStyle2 = index == 0 ? firstStyle : midStyle;
                        if (index == length - 1)
                            guiStyle2 = lastStyle;
                        if (length == 1)
                            guiStyle2 = style;
                        if (index != selected)
                            guiStyle2.Draw(buttonRects[index], contents[index], index == gridMouseSelection1 && (enabled || controlId == UGUIUtility.hotControl) && (controlId == UGUIUtility.hotControl || UGUIUtility.hotControl == 0), controlId == UGUIUtility.hotControl && enabled, false, false);
                        else
                            guiStyle1 = guiStyle2;
                    }
                    if (selected < length && selected > -1)
                        guiStyle1.Draw(buttonRects[selected], contents[selected], selected == gridMouseSelection1 && (enabled || controlId == UGUIUtility.hotControl) && (controlId == UGUIUtility.hotControl || UGUIUtility.hotControl == 0), controlId == UGUIUtility.hotControl, true, false);
                    if (gridMouseSelection1 >= 0)
                        tooltip = contents[gridMouseSelection1].tooltip;
                    GUIClip.Pop();
                    break;
            }
            return selected;
        }

        private static Rect[] CalcMouseRects(
          Rect position,
          int count,
          int xCount,
          float elemWidth,
          float elemHeight,
          GUIStyle style,
          GUIStyle firstStyle,
          GUIStyle midStyle,
          GUIStyle lastStyle,
          bool addBorders)
        {
            int num1 = 0;
            int num2 = 0;
            float xMin = position.xMin;
            float yMin = position.yMin;
            GUIStyle guiStyle1 = style;
            Rect[] rectArray = new Rect[count];
            if (count > 1)
                guiStyle1 = firstStyle;
            for (int index = 0; index < count; ++index)
            {
                rectArray[index] = addBorders ? guiStyle1.margin.Add(new Rect(xMin, yMin, elemWidth, elemHeight)) : new Rect(xMin, yMin, elemWidth, elemHeight);
                rectArray[index].width = Mathf.Round(rectArray[index].xMax) - Mathf.Round(rectArray[index].x);
                rectArray[index].x = Mathf.Round(rectArray[index].x);
                GUIStyle guiStyle2 = midStyle;
                if (index == count - 2)
                    guiStyle2 = lastStyle;
                xMin += elemWidth + (float)Mathf.Max(guiStyle1.margin.right, guiStyle2.margin.left);
                ++num2;
                if (num2 >= xCount)
                {
                    ++num1;
                    num2 = 0;
                    yMin += elemHeight + (float)Mathf.Max(style.margin.top, style.margin.bottom);
                    xMin = position.xMin;
                }
            }
            return rectArray;
        }

        private static int GetButtonGridMouseSelection(
          Rect[] buttonRects,
          Vector2 mousePos,
          bool findNearest)
        {
            for (int index = 0; index < buttonRects.Length; ++index)
            {
                if (buttonRects[index].Contains(mousePos))
                    return index;
            }
            if (!findNearest)
                return -1;
            float num1 = 1E+07f;
            int num2 = -1;
            for (int index = 0; index < buttonRects.Length; ++index)
            {
                Rect buttonRect = buttonRects[index];
                Vector2 vector2 = new Vector2(Mathf.Clamp(mousePos.x, buttonRect.xMin, buttonRect.xMax), Mathf.Clamp(mousePos.y, buttonRect.yMin, buttonRect.yMax));
                float sqrMagnitude = (mousePos - vector2).sqrMagnitude;
                if ((double)sqrMagnitude < (double)num1)
                {
                    num2 = index;
                    num1 = sqrMagnitude;
                }
            }
            return num2;
        }

        public static IObservable<float> HorizontalSlider(
          Rect position,
          float value,
          float leftValue,
          float rightValue)
        {
            return Slider(position, value, 0.0f, leftValue, rightValue, skin.horizontalSlider, skin.horizontalSliderThumb, true, 0);
        }

        public static IObservable<float> HorizontalSlider(
          Rect position,
          float value,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb)
        {
            return Slider(position, value, 0.0f, leftValue, rightValue, slider, thumb, true, 0);
        }

        public static IObservable<float> VerticalSlider(
          Rect position,
          float value,
          float topValue,
          float bottomValue)
        {
            return Slider(position, value, 0.0f, topValue, bottomValue, skin.verticalSlider, skin.verticalSliderThumb, false, 0);
        }

        public static IObservable<float> VerticalSlider(
          Rect position,
          float value,
          float topValue,
          float bottomValue,
          GUIStyle slider,
          GUIStyle thumb)
        {
            return Slider(position, value, 0.0f, topValue, bottomValue, slider, thumb, false, 0);
        }

        public static IObservable<float> Slider(
          Rect position,
          float value,
          float size,
          float start,
          float end,
          GUIStyle slider,
          GUIStyle thumb,
          bool horiz,
          int id)
        {
            UGUIUtility.CheckOnUGUI();
            if (id == 0)
                id = UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive, position);
            return new SliderHandler(position, value, size, start, end, slider, thumb, horiz, id).Handle();
        }

        public static IObservable<float> HorizontalScrollbar(
          Rect position,
          float value,
          float size,
          float leftValue,
          float rightValue)
        {
            return Scroller(position, value, size, leftValue, rightValue, skin.horizontalScrollbar, skin.horizontalScrollbarThumb, skin.horizontalScrollbarLeftButton, skin.horizontalScrollbarRightButton, true);
        }

        public static IObservable<float> HorizontalScrollbar(
          Rect position,
          float value,
          float size,
          float leftValue,
          float rightValue,
          GUIStyle style)
        {
            return Scroller(position, value, size, leftValue, rightValue, style, skin.GetStyle(style.name + "thumb"), skin.GetStyle(style.name + "leftbutton"), skin.GetStyle(style.name + "rightbutton"), true);
        }

        internal static IObservable<bool> ScrollerRepeatButton(int scrollerID, Rect rect, GUIStyle style)
        {
            bool flag1 = false;
            if (DoRepeatButton(rect, UGUIContent.none, style, FocusType.Passive))
            {
                bool flag2 = s_ScrollControlId != scrollerID;
                s_ScrollControlId = scrollerID;
                if (flag2)
                {
                    flag1 = true;
                    nextScrollStepTime = DateTime.Now.AddMilliseconds(250.0);
                }
                else if (DateTime.Now >= nextScrollStepTime)
                {
                    flag1 = true;
                    nextScrollStepTime = DateTime.Now.AddMilliseconds(30.0);
                }
                if (Event.current.type == EventType.Repaint)
                    InternalRepaintEditorWindow();
            }
            return flag1;
        }

        public static IObservable<float> VerticalScrollbar(
          Rect position,
          float value,
          float size,
          float topValue,
          float bottomValue)
        {
            return Scroller(position, value, size, topValue, bottomValue, skin.verticalScrollbar, skin.verticalScrollbarThumb, skin.verticalScrollbarUpButton, skin.verticalScrollbarDownButton, false);
        }

        public static IObservable<float> VerticalScrollbar(
          Rect position,
          float value,
          float size,
          float topValue,
          float bottomValue,
          GUIStyle style)
        {
            return Scroller(position, value, size, topValue, bottomValue, style, skin.GetStyle(style.name + "thumb"), skin.GetStyle(style.name + "upbutton"), skin.GetStyle(style.name + "downbutton"), false);
        }

        internal static IObservable<float> Scroller(
          Rect position,
          float value,
          float size,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb,
          GUIStyle leftButton,
          GUIStyle rightButton,
          bool horiz)
        {
            UGUIUtility.CheckOnUGUI();
            int controlId = UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive, position);
            Rect position1;
            Rect rect1;
            Rect rect2;
            if (horiz)
            {
                position1 = new Rect(position.x + leftButton.fixedWidth, position.y, position.width - leftButton.fixedWidth - rightButton.fixedWidth, position.height);
                rect1 = new Rect(position.x, position.y, leftButton.fixedWidth, position.height);
                rect2 = new Rect(position.xMax - rightButton.fixedWidth, position.y, rightButton.fixedWidth, position.height);
            }
            else
            {
                position1 = new Rect(position.x, position.y + leftButton.fixedHeight, position.width, position.height - leftButton.fixedHeight - rightButton.fixedHeight);
                rect1 = new Rect(position.x, position.y, position.width, leftButton.fixedHeight);
                rect2 = new Rect(position.x, position.yMax - rightButton.fixedHeight, position.width, rightButton.fixedHeight);
            }
            value = Slider(position1, value, size, leftValue, rightValue, slider, thumb, horiz, controlId);
            bool flag = false;
            if (Event.current.type == EventType.MouseUp)
                flag = true;
            if (ScrollerRepeatButton(controlId, rect1, leftButton))
                value -= s_ScrollStepSize * ((double)leftValue >= (double)rightValue ? -1f : 1f);
            if (ScrollerRepeatButton(controlId, rect2, rightButton))
                value += s_ScrollStepSize * ((double)leftValue >= (double)rightValue ? -1f : 1f);
            if (flag && Event.current.type == EventType.Used)
                s_ScrollControlId = 0;
            value = (double)leftValue >= (double)rightValue ? Mathf.Clamp(value, rightValue, leftValue - size) : Mathf.Clamp(value, leftValue, rightValue - size);
            return value;
        }

        public static void BeginClip(
          Rect position,
          Vector2 scrollOffset,
          Vector2 renderOffset,
          bool resetOffset)
        {
            UGUIUtility.CheckOnUGUI();
            GUIClip.Push(position, scrollOffset, renderOffset, resetOffset);
        }

        public static void BeginGroup(Rect position) => BeginGroup(position, UGUIContent.none, GUIStyle.none);

        public static void BeginGroup(Rect position, UGUIContent content) => BeginGroup(position, content, GUIStyle.none);

        public static void BeginGroup(Rect position, GUIStyle style) => BeginGroup(position, UGUIContent.none, style);
        
        public static void BeginGroup(Rect position, UGUIContent content, GUIStyle style)
        {
            UGUIUtility.CheckOnUGUI();
            int controlId = UGUIUtility.GetControlID(s_BeginGroupHash, FocusType.Passive);
            if (content != UGUIContent.none || style != GUIStyle.none)
            {
                if (Event.current.type == EventType.Repaint)
                    style.Draw(position, content, controlId);
                else if (position.Contains(Event.current.mousePosition))
                    UGUIUtility.mouseUsed = true;
            }
            GUIClip.Push(position, Vector2.zero, Vector2.zero, false);
        }

        public static void EndGroup()
        {
            UGUIUtility.CheckOnUGUI();
            GUIClip.Internal_Pop();
        }

        public static void BeginClip(Rect position)
        {
            UGUIUtility.CheckOnUGUI();
            GUIClip.Push(position, Vector2.zero, Vector2.zero, false);
        }

        public static void EndClip()
        {
            UGUIUtility.CheckOnUGUI();
            GUIClip.Pop();
        }

        public static IObservable<Vector2> BeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect)
        {
            return BeginScrollView(position, scrollPosition, viewRect, false, false, skin.horizontalScrollbar, skin.verticalScrollbar, skin.scrollView);
        }

        public static IObservable<Vector2> BeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical)
        {
            return BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, skin.horizontalScrollbar, skin.verticalScrollbar, skin.scrollView);
        }

        public static IObservable<Vector2> BeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar)
        {
            return BeginScrollView(position, scrollPosition, viewRect, false, false, horizontalScrollbar, verticalScrollbar, skin.scrollView);
        }

        public static IObservable<Vector2> BeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar)
        {
            return BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, skin.scrollView);
        }

        protected static IObservable<Vector2> DoBeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar,
          GUIStyle background)
        {
            return BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
        }

        internal static IObservable<Vector2> BeginScrollView(
          Rect position,
          Vector2 scrollPosition,
          Rect viewRect,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar,
          GUIStyle background)
        {
            UGUIUtility.CheckOnUGUI();
            ScrollViewState stateObject = (ScrollViewState)UGUIUtility.GetStateObject(typeof(ScrollViewState), UGUIUtility.GetControlID(s_ScrollviewHash, FocusType.Passive));
            if (stateObject.apply)
            {
                scrollPosition = stateObject.scrollPosition;
                stateObject.apply = false;
            }
            stateObject.position = position;
            stateObject.scrollPosition = scrollPosition;
            stateObject.visibleRect = stateObject.viewRect = viewRect;
            stateObject.visibleRect.width = position.width;
            stateObject.visibleRect.height = position.height;
            s_ScrollViewStates.Push((object)stateObject);
            Rect screenRect = new Rect(position);
            switch (Event.current.type)
            {
                case EventType.Layout:
                    UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                    UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                    UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                    goto case EventType.Used;
                case EventType.Used:
                    GUIClip.Push(screenRect, new Vector2(Mathf.Round(-scrollPosition.x - viewRect.x), Mathf.Round(-scrollPosition.y - viewRect.y)), Vector2.zero, false);
                    return scrollPosition;
                default:
                    bool flag1 = alwaysShowVertical;
                    bool flag2 = alwaysShowHorizontal;
                    if (flag2 || (double)viewRect.width > (double)screenRect.width)
                    {
                        stateObject.visibleRect.height = position.height - horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
                        screenRect.height -= horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
                        flag2 = true;
                    }
                    if (flag1 || (double)viewRect.height > (double)screenRect.height)
                    {
                        stateObject.visibleRect.width = position.width - verticalScrollbar.fixedWidth + (float)verticalScrollbar.margin.left;
                        screenRect.width -= verticalScrollbar.fixedWidth + (float)verticalScrollbar.margin.left;
                        flag1 = true;
                        if (!flag2 && (double)viewRect.width > (double)screenRect.width)
                        {
                            stateObject.visibleRect.height = position.height - horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
                            screenRect.height -= horizontalScrollbar.fixedHeight + (float)horizontalScrollbar.margin.top;
                            flag2 = true;
                        }
                    }
                    if (Event.current.type == EventType.Repaint && background != GUIStyle.none)
                        background.Draw(position, position.Contains(Event.current.mousePosition), false, flag2 && flag1, false);
                    if (flag2 && horizontalScrollbar != GUIStyle.none)
                    {
                        scrollPosition.x = HorizontalScrollbar(new Rect(position.x, position.yMax - horizontalScrollbar.fixedHeight, screenRect.width, horizontalScrollbar.fixedHeight), scrollPosition.x, Mathf.Min(screenRect.width, viewRect.width), 0.0f, viewRect.width, horizontalScrollbar);
                    }
                    else
                    {
                        UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                        UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                        UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                        scrollPosition.x = horizontalScrollbar == GUIStyle.none ? Mathf.Clamp(scrollPosition.x, 0.0f, Mathf.Max(viewRect.width - position.width, 0.0f)) : 0.0f;
                    }
                    if (flag1 && verticalScrollbar != GUIStyle.none)
                    {
                        scrollPosition.y = VerticalScrollbar(new Rect(screenRect.xMax + (float)verticalScrollbar.margin.left, screenRect.y, verticalScrollbar.fixedWidth, screenRect.height), scrollPosition.y, Mathf.Min(screenRect.height, viewRect.height), 0.0f, viewRect.height, verticalScrollbar);
                        goto case EventType.Used;
                    }
                    else
                    {
                        UGUIUtility.GetControlID(s_SliderHash, FocusType.Passive);
                        UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                        UGUIUtility.GetControlID(s_RepeatButtonHash, FocusType.Passive);
                        scrollPosition.y = verticalScrollbar == GUIStyle.none ? Mathf.Clamp(scrollPosition.y, 0.0f, Mathf.Max(viewRect.height - position.height, 0.0f)) : 0.0f;
                        goto case EventType.Used;
                    }
            }
        }

        public static void EndScrollView() => EndScrollView(Observable.Create(true));

        public static void EndScrollView(IObservable<bool> handleScrollWheel)
        {
            UGUIUtility.CheckOnUGUI();
            ScrollViewState scrollViewState = (ScrollViewState)s_ScrollViewStates.Peek();
            GUIClip.Pop();
            s_ScrollViewStates.Pop();
            if (!handleScrollWheel || Event.current.type != EventType.ScrollWheel || !scrollViewState.position.Contains(Event.current.mousePosition))
                return;
            scrollViewState.scrollPosition.x = Mathf.Clamp(scrollViewState.scrollPosition.x + Event.current.delta.x * 20f, 0.0f, scrollViewState.viewRect.width - scrollViewState.visibleRect.width);
            scrollViewState.scrollPosition.y = Mathf.Clamp(scrollViewState.scrollPosition.y + Event.current.delta.y * 20f, 0.0f, scrollViewState.viewRect.height - scrollViewState.visibleRect.height);
            scrollViewState.apply = true;
            Event.current.Use();
        }

        internal static ScrollViewState GetTopScrollView() => s_ScrollViewStates.Count != 0 ? (ScrollViewState)s_ScrollViewStates.Peek() : (ScrollViewState)null;

        public static void ScrollTo(Rect position) => GetTopScrollView()?.ScrollTo(position);

        public static bool ScrollTowards(Rect position, float maxDelta)
        {
            ScrollViewState topScrollView = GetTopScrollView();
            return topScrollView != null && topScrollView.ScrollTowards(position, maxDelta);
        }
        */

        //public static WindowResult Window(
        //  int id,
        //  Getter<Rect> clientRect,
        //  WindowFunction func,
        //  UGUIContent content,
        //  GUIStyle style = null)
        //	=> Window(id, (ReadOnlyRef<Rect>)clientRect, func, content, style);

        public static WindowResult Window(
          int id,
          Rect clientRect,
          WindowFunction func,
          UGUIContent content,
          GUIStyle style = null)
        {
            //return DoWindow(id, clientRect, func, content, style, skin, true);

            UGUIUtility.CheckOnUGUI();
            style ??= s_Skin.window;


            int controlID = UGUIUtility.GetControlID<WindowResult>(id);
            if (!UGUIUtility.TryGetControlModel(out WindowResult window, controlID))
            {
                window = new("Window", s_ActiveUGUI.Owner, clientRect, id, func, content, style);
                UGUIUtility.SetControlModel(window, controlID);
            }

            window.SetState(clientRect, content, style);

            return window;
        }


        /*
        public static IObservable<Rect> ModalWindow(
          int id,
          Rect clientRect,
          WindowFunction func,
          UGUIContent content)
        {
            UGUIUtility.CheckOnUGUI();
            return DoModalWindow(id, clientRect, func, content, skin.window, skin);
        }

        public static IObservable<Rect> ModalWindow(
          int id,
          Rect clientRect,
          WindowFunction func,
          UGUIContent content,
          GUIStyle style)
        {
            UGUIUtility.CheckOnUGUI();
            return DoModalWindow(id, clientRect, func, content, style, skin);
        }

        private static IObservable<Rect> DoModalWindow(
          int id,
          Rect clientRect,
          WindowFunction func,
          UGUIContent content,
          GUIStyle style,
          GUISkin skin)
        {
            return Internal_DoModalWindow(id, UGUIUtility.s_OriginalID, clientRect, func, content, style, skin);
        }

        [RequiredByNativeCode]
        internal static void CallWindowDelegate(
          WindowFunction func,
          int id,
          int instanceID,
          GUISkin _skin,
          int forceRect,
          float width,
          float height,
          GUIStyle style)
        {
            UILayoutUtility.SelectIDList(id, true);
            GUISkin skin = UI.skin;
            if (Event.current.type == EventType.Layout)
            {
                if (forceRect != 0)
                {
                    GUILayoutOption[] options = new GUILayoutOption[2]
                    {
            GUILayout.Width(width),
            GUILayout.Height(height)
                    };
                    UILayoutUtility.BeginWindow(id, style, options);
                }
                else
                    UILayoutUtility.BeginWindow(id, style, (GUILayoutOption[])null);
            }
            else
                UILayoutUtility.BeginWindow(id, GUIStyle.none, (GUILayoutOption[])null);
            skin = _skin;
            func(id);
            if (Event.current.type == EventType.Layout)
                UILayoutUtility.Layout();
            UI.skin = skin;
        }

        public static void DragWindow() => DragWindow(new Rect(0.0f, 0.0f, 10000f, 10000f));

        internal static void BeginWindows(int skinMode, int editorWindowInstanceID)
        {
            GUILayoutGroup topLevel = UILayoutUtility.current.topLevel;
            GenericStack layoutGroups = UILayoutUtility.current.layoutGroups;
            GUILayoutGroup windows = UILayoutUtility.current.windows;
            Matrix4x4 matrix = UI.matrix;
            Internal_BeginWindows();
            UI.matrix = matrix;
            UILayoutUtility.current.topLevel = topLevel;
            UILayoutUtility.current.layoutGroups = layoutGroups;
            UILayoutUtility.current.windows = windows;
        }

        internal static void EndWindows()
        {
            GUILayoutGroup topLevel = UILayoutUtility.current.topLevel;
            GenericStack layoutGroups = UILayoutUtility.current.layoutGroups;
            GUILayoutGroup windows = UILayoutUtility.current.windows;
            Internal_EndWindows();
            UILayoutUtility.current.topLevel = topLevel;
            UILayoutUtility.current.layoutGroups = layoutGroups;
            UILayoutUtility.current.windows = windows;
        }

        public static Color color
        {
            get
            {
                Color color;
                INTERNAL_get_color(out color);
                return color;
            }
            set => INTERNAL_set_color(ref value);
        }

        public static Color backgroundColor
        {
            get
            {
                Color color;
                INTERNAL_get_backgroundColor(out color);
                return color;
            }
            set => INTERNAL_set_backgroundColor(ref value);
        }

        public static Color contentColor
        {
            get
            {
                Color color;
                INTERNAL_get_contentColor(out color);
                return color;
            }
            set => INTERNAL_set_contentColor(ref value);
        }
        */




        internal static ToggleResult DoToggle(
          Rect position,
          int controlId,
          bool value,
          UGUIContent content,
          GUIStyle style)
        {
            //if (UGUIEvent.current.type != EventType.Repaint) return null;

            if (!UGUIUtility.TryGetControlModel(out ToggleResult toggle, controlId))
            {
                toggle = new("Toggle", s_ActiveParent, position, value, content, style);
                UGUIUtility.SetControlModel(toggle, controlId);
            }
            toggle.SetState(position, content, style);
            return toggle;
        }



        /*
        private static IObservable<Rect> Internal_DoModalWindow(
          int id,
          int instanceID,
          Rect clientRect,
          WindowFunction func,
          UGUIContent content,
          GUIStyle style,
          GUISkin skin)
        {
            Rect rect;
            INTERNAL_CALL_Internal_DoModalWindow(id, instanceID, ref clientRect, func, content, style, skin, out rect);
            return rect;
        }

        public static void DragWindow(Rect position) => INTERNAL_CALL_DragWindow(ref position);
        */


        public delegate void WindowFunction(int id);


        /*
        public abstract class Scope : IDisposable
        {
            private bool m_Disposed;

            protected abstract void CloseScope();

            ~Scope()
            {
                if (this.m_Disposed)
                    return;
                Debug.LogError((object)"Scope was not disposed! You should use the 'using' keyword or manually call Dispose.");
            }

            public void Dispose()
            {
                if (this.m_Disposed)
                    return;
                this.m_Disposed = true;
                if (UGUIUtility.guiIsExiting)
                    return;
                this.CloseScope();
            }
        }

        public class GroupScope : Scope
        {
            public GroupScope(Rect position) => BeginGroup(position);

            public GroupScope(Rect position, string text) => BeginGroup(position, text);

            public GroupScope(Rect position, Texture image) => BeginGroup(position, image);

            public GroupScope(Rect position, UGUIContent content) => BeginGroup(position, content);

            public GroupScope(Rect position, GUIStyle style) => BeginGroup(position, style);

            public GroupScope(Rect position, string text, GUIStyle style) => BeginGroup(position, text, style);

            public GroupScope(Rect position, Texture image, GUIStyle style) => BeginGroup(position, image, style);

            protected override void CloseScope() => EndGroup();
        }

        public class ScrollViewScope : Scope
        {
            public ScrollViewScope(Rect position, Vector2 scrollPosition, Rect viewRect)
            {
                this.handleScrollWheel = true;
                this.scrollPosition = BeginScrollView(position, scrollPosition, viewRect);
            }

            public ScrollViewScope(
              Rect position,
              Vector2 scrollPosition,
              Rect viewRect,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical)
            {
                this.handleScrollWheel = true;
                this.scrollPosition = BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical);
            }

            public ScrollViewScope(
              Rect position,
              Vector2 scrollPosition,
              Rect viewRect,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar)
            {
                this.handleScrollWheel = true;
                this.scrollPosition = BeginScrollView(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
            }

            public ScrollViewScope(
              Rect position,
              Vector2 scrollPosition,
              Rect viewRect,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar)
            {
                this.handleScrollWheel = true;
                this.scrollPosition = BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar);
            }

            internal ScrollViewScope(
              Rect position,
              Vector2 scrollPosition,
              Rect viewRect,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar,
              GUIStyle background)
            {
                this.handleScrollWheel = true;
                this.scrollPosition = BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
            }

            public Vector2 scrollPosition { get; private set; }

            public bool handleScrollWheel { get; set; }

            protected override void CloseScope() => EndScrollView(this.handleScrollWheel);
        }

        public class ClipScope : Scope
        {
            public ClipScope(Rect position) => BeginClip(position);

            protected override void CloseScope() => EndClip();
        }
        */
    }
}
