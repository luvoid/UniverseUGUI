using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UniverseLib.UGUI.ImplicitTypes;
using UniverseLib.UGUI.Models;

namespace UniverseLib.UGUI
{
    public static class UGUILayout
    {
        // /// <summary>
        // /// Returns an instance of <see cref="UGUILayout"/>
        // /// for using extension methods.
        // /// </summary>
        // public static UGUILayout Ext => new UGUILayout();
        // private UGUILayout()
        // { }

        public static void Label(UGUIContent content, UGUIStyle style = null, params GUILayoutOption[] options)
        {
            style ??= UGUI.skin.Label;
            UGUI.Label(UGUILayoutUtility.GetRect(content, style, options), content, style);
        }

        public static void Box(UGUIContent content, UGUIStyle style = null, params GUILayoutOption[] options)
        {
            style ??= UGUI.skin.Box;
            UGUI.Box(UGUILayoutUtility.GetRect(content, style, options), content, style);
        }

        public static ButtonResult Button(UGUIContent content, UGUIStyle style = null, params GUILayoutOption[] options)
        {
            style ??= UGUI.skin.Button;
            return UGUI.Button(UGUILayoutUtility.GetRect(content, style, options), content, style);
        }



        /*
        public static IObservable<bool> RepeatButton(UGUIContent content, params GUILayoutOption[] options) => DoRepeatButton(content, UGUI.skin.button, options);

        public static IObservable<bool> RepeatButton(string text, GUIStyle style, params GUILayoutOption[] options) => DoRepeatButton(UGUIContent.Temp(text), style, options);

        public static IObservable<bool> RepeatButton(UGUIContent content, GUIStyle style, params GUILayoutOption[] options) => DoRepeatButton(content, style, options);

        private static IObservable<bool> DoRepeatButton( UGUIContent content, GUIStyle style, GUILayoutOption[] options)
        {
            return UGUI.RepeatButton(UGUILayoutUtility.GetRect(content, style, options), content, style);
        }
        */



        public static TextFieldResult TextField(string text, UGUIStyle style, params GUILayoutOption[] options)
            => DoTextField(text, style: style, options: options);

        public static TextFieldResult TextField(
          string text,
          int maxLength = -1,
          UGUIStyle style = null,
          params GUILayoutOption[] options)
        {
            style ??= UGUI.skin.TextField;
            return DoTextField(text, style, options, false, maxLength);
        }



        /*
        public static string PasswordField(string password, char maskChar, params GUILayoutOption[] options)
        {
            return PasswordField(password, maskChar, -1, UGUI.skin.textField, options);
        }

        public static string PasswordField( string password, char maskChar, int maxLength, params GUILayoutOption[] options)
        {
            return PasswordField(password, maskChar, maxLength, UGUI.skin.textField, options);
        }

        public static string PasswordField(
          string password,
          char maskChar,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return PasswordField(password, maskChar, -1, style, options);
        }

        public static string PasswordField(
          string password,
          char maskChar,
          int maxLength,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return UGUI.PasswordField(UGUILayoutUtility.GetRect(UGUIContent.Temp(UGUI.PasswordFieldGetStrToShow(password, maskChar)), UGUI.skin.textField, options), password, maskChar, maxLength, style);
        }





        public static IObservable<string> TextArea(string text, params GUILayoutOption[] options) => DoTextField(text, -1, true, UGUI.skin.textArea, options);

        public static IObservable<string> TextArea(string text, int maxLength, params GUILayoutOption[] options) => DoTextField(text, maxLength, true, UGUI.skin.textArea, options);

        public static IObservable<string> TextArea(string text, GUIStyle style, params GUILayoutOption[] options) => DoTextField(text, -1, true, style, options);

        public static IObservable<string> TextArea(
          string text,
          int maxLength,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoTextField(text, maxLength, true, style, options);
        }
        */



        private static TextFieldResult DoTextField(
          string text,
          UGUIStyle style,
          GUILayoutOption[] options,
          bool multiline = false,
          int maxLength = -1)
        {
            int controlID = UGUIUtility.GetControlID(FocusType.Keyboard);
            //UGUIContent.Temp(text);
            //UGUIContent content = UGUIUtility.keyboardControl == controlID ? UGUIContent.Temp(text + Input.compositionString) : UGUIContent.Temp(text);
            Rect rect = UGUILayoutUtility.GetRect(GUIContent.Temp(text), style, options);
            //if (UGUIUtility.keyboardControl == controlID)
            //    content = UGUIContent.Temp(text);

            return UGUI.DoTextField(rect, controlID, text, style, multiline, maxLength);
        }




        public static ToggleResult Toggle(
          bool value,
          UGUIContent content,
          UGUIStyle style = null,
          params GUILayoutOption[] options)
        {
            style ??= UGUI.skin.Toggle;
            return UGUI.Toggle(UGUILayoutUtility.GetRect(content, style, options), value, content, style);
        }



        /*
        public static IObservable<int> Toolbar(int selected, string[] texts, params GUILayoutOption[] options) => Toolbar(selected, UGUIContent.Cast(texts), UGUI.skin.button, options);

        public static IObservable<int> Toolbar(int selected, Texture[] images, params GUILayoutOption[] options) => Toolbar(selected, UGUIContent.Cast(images), UGUI.skin.button, options);

        public static IObservable<int> Toolbar(int selected, UGUIContent[] content, params GUILayoutOption[] options) => Toolbar(selected, content, UGUI.skin.button, options);

        public static IObservable<int> Toolbar(
          int selected,
          string[] texts,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return Toolbar(selected, UGUIContent.Cast(texts), style, options);
        }

        public static IObservable<int> Toolbar(
          int selected,
          Texture[] images,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return Toolbar(selected, UGUIContent.Cast(images), style, options);
        }

        public static IObservable<int> Toolbar(
          int selected,
          UGUIContent[] contents,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            GUIStyle firstStyle;
            GUIStyle midStyle;
            GUIStyle lastStyle;
            UGUI.FindStyles(ref style, out firstStyle, out midStyle, out lastStyle, "left", "mid", "right");
            Vector2 vector2_1 = new Vector2();
            int length = contents.Length;
            GUIStyle guiStyle1 = length <= 1 ? style : firstStyle;
            GUIStyle guiStyle2 = length <= 1 ? style : midStyle;
            GUIStyle guiStyle3 = length <= 1 ? style : lastStyle;
            int left = guiStyle1.margin.left;
            for (int index = 0; index < contents.Length; ++index)
            {
                if (index == length - 2)
                {
                    guiStyle1 = guiStyle2;
                    guiStyle2 = guiStyle3;
                }
                if (index == length - 1)
                    guiStyle1 = guiStyle3;
                Vector2 vector2_2 = guiStyle1.CalcSize(contents[index]);
                if (vector2_2.x > (double)vector2_1.x)
                    vector2_1.x = vector2_2.x;
                if (vector2_2.y > (double)vector2_1.y)
                    vector2_1.y = vector2_2.y;
                if (index == length - 1)
                    left += guiStyle1.margin.right;
                else
                    left += Mathf.Max(guiStyle1.margin.right, guiStyle2.margin.left);
            }
            vector2_1.x = vector2_1.x * contents.Length + left;
            return UGUI.Toolbar(UGUILayoutUtility.GetRect(vector2_1.x, vector2_1.y, style, options), selected, contents, style);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          string[] texts,
          int xCount,
          params GUILayoutOption[] options)
        {
            return SelectionGrid(selected, UGUIContent.Cast(texts), xCount, UGUI.skin.button, options);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          Texture[] images,
          int xCount,
          params GUILayoutOption[] options)
        {
            return SelectionGrid(selected, UGUIContent.Cast(images), xCount, UGUI.skin.button, options);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          UGUIContent[] content,
          int xCount,
          params GUILayoutOption[] options)
        {
            return SelectionGrid(selected, content, xCount, UGUI.skin.button, options);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          string[] texts,
          int xCount,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return SelectionGrid(selected, UGUIContent.Cast(texts), xCount, style, options);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          Texture[] images,
          int xCount,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return SelectionGrid(selected, UGUIContent.Cast(images), xCount, style, options);
        }

        public static IObservable<int> SelectionGrid(
          int selected,
          UGUIContent[] contents,
          int xCount,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return UGUI.SelectionGrid(GUIGridSizer.GetRect(contents, xCount, style, options), selected, contents, xCount, style);
        }

        public static IObservable<float> HorizontalSlider(
          float value,
          float leftValue,
          float rightValue,
          params GUILayoutOption[] options)
        {
            return DoHorizontalSlider(value, leftValue, rightValue, UGUI.skin.horizontalSlider, UGUI.skin.horizontalSliderThumb, options);
        }

        public static IObservable<float> HorizontalSlider(
          float value,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb,
          params GUILayoutOption[] options)
        {
            return DoHorizontalSlider(value, leftValue, rightValue, slider, thumb, options);
        }

        private static IObservable<float> DoHorizontalSlider(
          float value,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb,
          GUILayoutOption[] options)
        {
            return UGUI.HorizontalSlider(UGUILayoutUtility.GetRect(UGUIContent.Temp("mmmm"), slider, options), value, leftValue, rightValue, slider, thumb);
        }

        public static IObservable<float> VerticalSlider(
          float value,
          float leftValue,
          float rightValue,
          params GUILayoutOption[] options)
        {
            return DoVerticalSlider(value, leftValue, rightValue, UGUI.skin.verticalSlider, UGUI.skin.verticalSliderThumb, options);
        }

        public static IObservable<float> VerticalSlider(
          float value,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb,
          params GUILayoutOption[] options)
        {
            return DoVerticalSlider(value, leftValue, rightValue, slider, thumb, options);
        }

        private static IObservable<float> DoVerticalSlider(
          float value,
          float leftValue,
          float rightValue,
          GUIStyle slider,
          GUIStyle thumb,
          params GUILayoutOption[] options)
        {
            return UGUI.VerticalSlider(UGUILayoutUtility.GetRect(UGUIContent.Temp("\n\n\n\n\n"), slider, options), value, leftValue, rightValue, slider, thumb);
        }

        public static IObservable<float> HorizontalScrollbar(
          float value,
          float size,
          float leftValue,
          float rightValue,
          params GUILayoutOption[] options)
        {
            return HorizontalScrollbar(value, size, leftValue, rightValue, UGUI.skin.horizontalScrollbar, options);
        }

        public static IObservable<float> HorizontalScrollbar(
          float value,
          float size,
          float leftValue,
          float rightValue,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return UGUI.HorizontalScrollbar(UGUILayoutUtility.GetRect(UGUIContent.Temp("mmmm"), style, options), value, size, leftValue, rightValue, style);
        }

        public static IObservable<float> VerticalScrollbar(
          float value,
          float size,
          float topValue,
          float bottomValue,
          params GUILayoutOption[] options)
        {
            return VerticalScrollbar(value, size, topValue, bottomValue, UGUI.skin.verticalScrollbar, options);
        }

        public static IObservable<float> VerticalScrollbar(
          float value,
          float size,
          float topValue,
          float bottomValue,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return UGUI.VerticalScrollbar(UGUILayoutUtility.GetRect(UGUIContent.Temp("\n\n\n\n"), style, options), value, size, topValue, bottomValue, style);
        }

        public static void Space(float pixels)
        {
            UGUIUtility.CheckOnGUI();
            if (UGUILayoutUtility.current.topLevel.isVertical)
                UGUILayoutUtility.GetRect(0.0f, pixels, UGUILayoutUtility.spaceStyle, Height(pixels));
            else
                UGUILayoutUtility.GetRect(pixels, 0.0f, UGUILayoutUtility.spaceStyle, Width(pixels));
        }

        public static void FlexibleSpace()
        {
            UGUIUtility.CheckOnGUI();
            GUILayoutOption guiLayoutOption = !UGUILayoutUtility.current.topLevel.isVertical ? ExpandWidth(true) : ExpandHeight(true);
            guiLayoutOption.value = 10000;
            UGUILayoutUtility.GetRect(0.0f, 0.0f, UGUILayoutUtility.spaceStyle, guiLayoutOption);
        }

        public static void BeginHorizontal(params GUILayoutOption[] options) => BeginHorizontal(UGUIContent.none, GUIStyle.none, options);

        public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options) => BeginHorizontal(UGUIContent.none, style, options);

        public static void BeginHorizontal(
          string text,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            BeginHorizontal(UGUIContent.Temp(text), style, options);
        }

        public static void BeginHorizontal(
          Texture image,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            BeginHorizontal(UGUIContent.Temp(image), style, options);
        }

        public static void BeginHorizontal(
          UGUIContent content,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            GUILayoutGroup guiLayoutGroup = UGUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
            guiLayoutGroup.isVertical = false;
            if (style == GUIStyle.none && content == UGUIContent.none)
                return;
            UGUI.Box(guiLayoutGroup.rect, content, style);
        }

        public static void EndHorizontal() => UGUILayoutUtility.EndLayoutGroup();

        public static void BeginVertical(params GUILayoutOption[] options) => BeginVertical(UGUIContent.none, GUIStyle.none, options);

        public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options) => BeginVertical(UGUIContent.none, style, options);

        public static void BeginVertical(string text, GUIStyle style, params GUILayoutOption[] options) => BeginVertical(UGUIContent.Temp(text), style, options);

        public static void BeginVertical(
          Texture image,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            BeginVertical(UGUIContent.Temp(image), style, options);
        }

        public static void BeginVertical(
          UGUIContent content,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            GUILayoutGroup guiLayoutGroup = UGUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
            guiLayoutGroup.isVertical = true;
            if (style == GUIStyle.none && content == UGUIContent.none)
                return;
            UGUI.Box(guiLayoutGroup.rect, content, style);
        }

        public static void EndVertical() => UGUILayoutUtility.EndLayoutGroup();

        public static void BeginArea(Rect screenRect) => BeginArea(screenRect, UGUIContent.none, GUIStyle.none);

        public static void BeginArea(Rect screenRect, string text) => BeginArea(screenRect, UGUIContent.Temp(text), GUIStyle.none);

        public static void BeginArea(Rect screenRect, Texture image) => BeginArea(screenRect, UGUIContent.Temp(image), GUIStyle.none);

        public static void BeginArea(Rect screenRect, UGUIContent content) => BeginArea(screenRect, content, GUIStyle.none);

        public static void BeginArea(Rect screenRect, GUIStyle style) => BeginArea(screenRect, UGUIContent.none, style);

        public static void BeginArea(Rect screenRect, string text, GUIStyle style) => BeginArea(screenRect, UGUIContent.Temp(text), style);

        public static void BeginArea(Rect screenRect, Texture image, GUIStyle style) => BeginArea(screenRect, UGUIContent.Temp(image), style);

        public static void BeginArea(Rect screenRect, UGUIContent content, GUIStyle style)
        {
            UGUIUtility.CheckOnGUI();
            GUILayoutGroup guiLayoutGroup = UGUILayoutUtility.BeginLayoutArea(style, typeof(GUILayoutGroup));
            if (Event.current.type == EventType.Layout)
            {
                guiLayoutGroup.resetCoords = true;
                guiLayoutGroup.minWidth = guiLayoutGroup.maxWidth = screenRect.width;
                guiLayoutGroup.minHeight = guiLayoutGroup.maxHeight = screenRect.height;
                guiLayoutGroup.rect = Rect.MinMaxRect(screenRect.xMin, screenRect.yMin, guiLayoutGroup.rect.xMax, guiLayoutGroup.rect.yMax);
            }
            UGUI.BeginGroup(guiLayoutGroup.rect, content, style);
        }

        public static void EndArea()
        {
            UGUIUtility.CheckOnGUI();
            if (Event.current.type == EventType.Used)
                return;
            UGUILayoutUtility.current.layoutGroups.Pop();
            UGUILayoutUtility.current.topLevel = (GUILayoutGroup)UGUILayoutUtility.current.layoutGroups.Peek();
            UGUI.EndGroup();
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          params GUILayoutOption[] options)
        {
            return BeginScrollView(scrollPosition, false, false, UGUI.skin.horizontalScrollbar, UGUI.skin.verticalScrollbar, UGUI.skin.scrollView, options);
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          params GUILayoutOption[] options)
        {
            return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, UGUI.skin.horizontalScrollbar, UGUI.skin.verticalScrollbar, UGUI.skin.scrollView, options);
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar,
          params GUILayoutOption[] options)
        {
            return BeginScrollView(scrollPosition, false, false, horizontalScrollbar, verticalScrollbar, UGUI.skin.scrollView, options);
        }

        public static IObservable<Vector2> BeginScrollView(Vector2 scrollPosition, GUIStyle style)
        {
            GUILayoutOption[] guiLayoutOptionArray = null;
            return BeginScrollView(scrollPosition, style, guiLayoutOptionArray);
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            string name = style.name;
            GUIStyle verticalScrollbar = UGUI.skin.FindStyle(name + "VerticalScrollbar") ?? UGUI.skin.verticalScrollbar;
            GUIStyle horizontalScrollbar = UGUI.skin.FindStyle(name + "HorizontalScrollbar") ?? UGUI.skin.horizontalScrollbar;
            return BeginScrollView(scrollPosition, false, false, horizontalScrollbar, verticalScrollbar, style, options);
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar,
          params GUILayoutOption[] options)
        {
            return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, UGUI.skin.scrollView, options);
        }

        public static IObservable<Vector2> BeginScrollView(
          Vector2 scrollPosition,
          bool alwaysShowHorizontal,
          bool alwaysShowVertical,
          GUIStyle horizontalScrollbar,
          GUIStyle verticalScrollbar,
          GUIStyle background,
          params GUILayoutOption[] options)
        {
            UGUIUtility.CheckOnGUI();
            GUIScrollGroup guiScrollGroup = (GUIScrollGroup)UGUILayoutUtility.BeginLayoutGroup(background, null, typeof(GUIScrollGroup));
            if (Event.current.type == EventType.Layout)
            {
                guiScrollGroup.resetCoords = true;
                guiScrollGroup.isVertical = true;
                guiScrollGroup.stretchWidth = 1;
                guiScrollGroup.stretchHeight = 1;
                guiScrollGroup.verticalScrollbar = verticalScrollbar;
                guiScrollGroup.horizontalScrollbar = horizontalScrollbar;
                guiScrollGroup.needsVerticalScrollbar = alwaysShowVertical;
                guiScrollGroup.needsHorizontalScrollbar = alwaysShowHorizontal;
                guiScrollGroup.ApplyOptions(options);
            }
            return UGUI.BeginScrollView(guiScrollGroup.rect, scrollPosition, new Rect(0.0f, 0.0f, guiScrollGroup.clientWidth, guiScrollGroup.clientHeight), alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
        }

        public static void EndScrollView() => EndScrollView(Observable.Create(true));

        internal static void EndScrollView(IObservable<bool> handleScrollWheel)
        {
            UGUILayoutUtility.EndLayoutGroup();
            UGUI.EndScrollView(handleScrollWheel);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          string text,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, UGUIContent.Temp(text), UGUI.skin.window, options);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          Texture image,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, UGUIContent.Temp(image), UGUI.skin.window, options);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          UGUIContent content,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, content, UGUI.skin.window, options);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          string text,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, UGUIContent.Temp(text), style, options);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          Texture image,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, UGUIContent.Temp(image), style, options);
        }

        public static IObservable<Rect> Window(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          UGUIContent content,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoWindow(id, screenRect, func, content, style, options);
        }

        private static IObservable<Rect> DoWindow(
          int id,
          Rect screenRect,
          UGUI.WindowFunction func,
          UGUIContent content,
          GUIStyle style,
          GUILayoutOption[] options)
        {
            UGUIUtility.CheckOnGUI();
            LayoutedWindow layoutedWindow = new LayoutedWindow(func, screenRect, content, options, style);
            return UGUI.Window(id, screenRect, new UGUI.WindowFunction(layoutedWindow.DoWindow), content, style);
        }
        */
        


        public static GUILayoutOption Width(float width) => new GUILayoutOption(GUILayoutOption.Type.fixedWidth, width);

        public static GUILayoutOption MinWidth(float minWidth) => new GUILayoutOption(GUILayoutOption.Type.minWidth, minWidth);

        public static GUILayoutOption MaxWidth(float maxWidth) => new GUILayoutOption(GUILayoutOption.Type.maxWidth, maxWidth);

        public static GUILayoutOption Height(float height) => new GUILayoutOption(GUILayoutOption.Type.fixedHeight, height);

        public static GUILayoutOption MinHeight(float minHeight) => new GUILayoutOption(GUILayoutOption.Type.minHeight, minHeight);

        public static GUILayoutOption MaxHeight(float maxHeight) => new GUILayoutOption(GUILayoutOption.Type.maxHeight, maxHeight);

        public static GUILayoutOption ExpandWidth(bool expand) => new GUILayoutOption(GUILayoutOption.Type.stretchWidth, !expand ? 0 : 1);

        public static GUILayoutOption ExpandHeight(bool expand) => new GUILayoutOption(GUILayoutOption.Type.stretchHeight, !expand ? 0 : 1);



        /*
        private sealed class LayoutedWindow
        {
            private readonly UGUI.WindowFunction m_Func;
            private readonly Rect m_ScreenRect;
            private readonly GUILayoutOption[] m_Options;
            private readonly GUIStyle m_Style;

            internal LayoutedWindow(
              UGUI.WindowFunction f,
              Rect screenRect,
              UGUIContent content,
              GUILayoutOption[] options,
              GUIStyle style)
            {
                m_Func = f;
                m_ScreenRect = screenRect;
                m_Options = options;
                m_Style = style;
            }

            public void DoWindow(int windowID)
            {
                GUILayoutGroup topLevel = UGUILayoutUtility.current.topLevel;
                if (UGUIEvent.current.type == EventType.Layout)
                {
                    topLevel.resetCoords = true;
                    topLevel.rect = m_ScreenRect;
                    if (m_Options != null)
                        topLevel.ApplyOptions(m_Options);
                    topLevel.isWindow = true;
                    topLevel.windowID = windowID;
                    topLevel.style = m_Style;
                }
                else
                    topLevel.ResetCursor();
                this.m_Func(windowID);
            }
        }

        public class HorizontalScope : UGUI.Scope
        {
            public HorizontalScope(params GUILayoutOption[] options) => BeginHorizontal(options);

            public HorizontalScope(GUIStyle style, params GUILayoutOption[] options) => BeginHorizontal(style, options);

            public HorizontalScope(string text, GUIStyle style, params GUILayoutOption[] options) => BeginHorizontal(text, style, options);

            public HorizontalScope(Texture image, GUIStyle style, params GUILayoutOption[] options) => BeginHorizontal(image, style, options);

            public HorizontalScope(UGUIContent content, GUIStyle style, params GUILayoutOption[] options) => BeginHorizontal(content, style, options);

            protected override void CloseScope() => EndHorizontal();
        }

        public class VerticalScope : UGUI.Scope
        {
            public VerticalScope(params GUILayoutOption[] options) => BeginVertical(options);

            public VerticalScope(GUIStyle style, params GUILayoutOption[] options) => BeginVertical(style, options);

            public VerticalScope(string text, GUIStyle style, params GUILayoutOption[] options) => BeginVertical(text, style, options);

            public VerticalScope(Texture image, GUIStyle style, params GUILayoutOption[] options) => BeginVertical(image, style, options);

            public VerticalScope(UGUIContent content, GUIStyle style, params GUILayoutOption[] options) => BeginVertical(content, style, options);

            protected override void CloseScope() => EndVertical();
        }

        public class AreaScope : UGUI.Scope
        {
            public AreaScope(Rect screenRect) => BeginArea(screenRect);

            public AreaScope(Rect screenRect, string text) => BeginArea(screenRect, text);

            public AreaScope(Rect screenRect, Texture image) => BeginArea(screenRect, image);

            public AreaScope(Rect screenRect, UGUIContent content) => BeginArea(screenRect, content);

            public AreaScope(Rect screenRect, string text, GUIStyle style) => BeginArea(screenRect, text, style);

            public AreaScope(Rect screenRect, Texture image, GUIStyle style) => BeginArea(screenRect, image, style);

            public AreaScope(Rect screenRect, UGUIContent content, GUIStyle style) => BeginArea(screenRect, content, style);

            protected override void CloseScope() => EndArea();
        }

        public class ScrollViewScope : UGUI.Scope
        {
            public ScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, options);
            }

            public ScrollViewScope(
              Vector2 scrollPosition,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical,
              params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
            }

            public ScrollViewScope(
              Vector2 scrollPosition,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar,
              params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
            }

            public ScrollViewScope(
              Vector2 scrollPosition,
              GUIStyle style,
              params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, style, options);
            }

            public ScrollViewScope(
              Vector2 scrollPosition,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar,
              params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
            }

            public ScrollViewScope(
              Vector2 scrollPosition,
              bool alwaysShowHorizontal,
              bool alwaysShowVertical,
              GUIStyle horizontalScrollbar,
              GUIStyle verticalScrollbar,
              GUIStyle background,
              params GUILayoutOption[] options)
            {
                handleScrollWheel = Observable.Create(true);
                this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
            }

            public IObservable<Vector2> scrollPosition { get; private set; }

            public IObservable<bool> handleScrollWheel { get; set; }

            protected override void CloseScope() => EndScrollView(handleScrollWheel);
        }
        */
    }
}
