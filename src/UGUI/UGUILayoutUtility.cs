using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Scripting;
using UnityEngineInternal;
using UnityEngine;

namespace UniverseLib.UGUI
{
    public class UGUILayoutUtility
    {
        private static Dictionary<int, LayoutCache> s_StoredLayouts = new Dictionary<int, LayoutCache>();
        private static Dictionary<int, LayoutCache> s_StoredWindows = new Dictionary<int, LayoutCache>();
        internal static LayoutCache current = new LayoutCache();
        internal static readonly Rect kDummyRect = new Rect(0.0f, 0.0f, 1f, 1f);
        private static GUIStyle s_SpaceStyle;

        internal static LayoutCache SelectIDList(
          int instanceID,
          bool isWindow)
        {
            Dictionary<int, LayoutCache> dictionary = !isWindow ? s_StoredLayouts : s_StoredWindows;
            LayoutCache layoutCache;
            if (!dictionary.TryGetValue(instanceID, out layoutCache))
            {
                layoutCache = new LayoutCache();
                dictionary[instanceID] = layoutCache;
            }
            current.topLevel = layoutCache.topLevel;
            current.layoutGroups = layoutCache.layoutGroups;
            current.windows = layoutCache.windows;
            return layoutCache;
        }

        internal static void Begin(IUniversalUGUIObject uGUI)
        {
            LayoutCache layoutCache = SelectIDList(uGUI.GetInstanceID(), false);
            if (UGUIEvent.current.type == EventType.Layout)
            {
                current.topLevel = layoutCache.topLevel = new GUILayoutGroup();
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = layoutCache.windows = new GUILayoutGroup();
            }
            else
            {
                current.topLevel = layoutCache.topLevel;
                current.layoutGroups = layoutCache.layoutGroups;
                current.windows = layoutCache.windows;
            }
        }

        internal static void BeginContainer(LayoutCache cache)
        {
            if (UGUIEvent.current.type == EventType.Layout)
            {
                current.topLevel = cache.topLevel = new GUILayoutGroup();
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = cache.windows = new GUILayoutGroup();
            }
            else
            {
                current.topLevel = cache.topLevel;
                current.layoutGroups = cache.layoutGroups;
                current.windows = cache.windows;
            }
        }

        internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
        {
            LayoutCache layoutCache = SelectIDList(windowID, true);
            if (UGUIEvent.current.type == EventType.Layout)
            {
                current.topLevel = layoutCache.topLevel = new GUILayoutGroup();
                current.topLevel.style = style;
                current.topLevel.windowID = windowID;
                if (options != null)
                    current.topLevel.ApplyOptions(options);
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = layoutCache.windows = new GUILayoutGroup();
            }
            else
            {
                current.topLevel = layoutCache.topLevel;
                current.layoutGroups = layoutCache.layoutGroups;
                current.windows = layoutCache.windows;
            }
        }

        [Obsolete("BeginGroup has no effect and will be removed", false)]
        public static void BeginGroup(string GroupName)
        {
        }

        [Obsolete("EndGroup has no effect and will be removed", false)]
        public static void EndGroup(string groupName)
        {
        }

        internal static void Layout()
        {
            if (current.topLevel.windowID == -1)
            {
                current.topLevel.CalcWidth();
                current.topLevel.SetHorizontal(0.0f, Mathf.Min(Screen.width / UGUIUtility.pixelsPerPoint, current.topLevel.maxWidth));
                current.topLevel.CalcHeight();
                current.topLevel.SetVertical(0.0f, Mathf.Min(Screen.height / UGUIUtility.pixelsPerPoint, current.topLevel.maxHeight));
                LayoutFreeGroup(current.windows);
            }
            else
            {
                LayoutSingleGroup(current.topLevel);
                LayoutFreeGroup(current.windows);
            }
        }

        internal static void LayoutFromEditorWindow()
        {
            current.topLevel.CalcWidth();
            current.topLevel.SetHorizontal(0.0f, Screen.width / UGUIUtility.pixelsPerPoint);
            current.topLevel.CalcHeight();
            current.topLevel.SetVertical(0.0f, Screen.height / UGUIUtility.pixelsPerPoint);
            LayoutFreeGroup(current.windows);
        }

        internal static void LayoutFromContainer(float w, float h)
        {
            current.topLevel.CalcWidth();
            current.topLevel.SetHorizontal(0.0f, w);
            current.topLevel.CalcHeight();
            current.topLevel.SetVertical(0.0f, h);
            LayoutFreeGroup(current.windows);
        }

        internal static float LayoutFromInspector(float width)
        {
            if (current.topLevel != null && current.topLevel.windowID == -1)
            {
                current.topLevel.CalcWidth();
                current.topLevel.SetHorizontal(0.0f, width);
                current.topLevel.CalcHeight();
                current.topLevel.SetVertical(0.0f, Mathf.Min(Screen.height / UGUIUtility.pixelsPerPoint, current.topLevel.maxHeight));
                float minHeight = current.topLevel.minHeight;
                LayoutFreeGroup(current.windows);
                return minHeight;
            }
            if (current.topLevel != null)
                LayoutSingleGroup(current.topLevel);
            return 0.0f;
        }

        internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
        {
            foreach (GUILayoutGroup entry in toplevel.entries)
                LayoutSingleGroup(entry);
            toplevel.ResetCursor();
        }

        private static void LayoutSingleGroup(GUILayoutGroup i)
        {
            if (!i.isWindow)
            {
                float minWidth = i.minWidth;
                float maxWidth = i.maxWidth;
                i.CalcWidth();
                i.SetHorizontal(i.rect.x, Mathf.Clamp(i.maxWidth, minWidth, maxWidth));
                float minHeight = i.minHeight;
                float maxHeight = i.maxHeight;
                i.CalcHeight();
                i.SetVertical(i.rect.y, Mathf.Clamp(i.maxHeight, minHeight, maxHeight));
            }
            else
            {
                i.CalcWidth();
                Rect windowRect = Internal_GetWindowRect(i.windowID);
                i.SetHorizontal(windowRect.x, Mathf.Clamp(windowRect.width, i.minWidth, i.maxWidth));
                i.CalcHeight();
                i.SetVertical(windowRect.y, Mathf.Clamp(windowRect.height, i.minHeight, i.maxHeight));
                Internal_MoveWindow(i.windowID, i.rect);
            }
        }

        [SecuritySafeCritical]
        private static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(Type LayoutType) => typeof(GUILayoutGroup).IsAssignableFrom(LayoutType) ? (GUILayoutGroup)Activator.CreateInstance(LayoutType) : throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup");

        internal static GUILayoutGroup BeginLayoutGroup(
          GUIStyle style,
          GUILayoutOption[] options,
          Type layoutType)
        {
            GUILayoutGroup guiLayoutGroup = null;
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                case EventType.Used:
                    guiLayoutGroup = CreateGUILayoutGroupInstanceOfType(layoutType);
                    guiLayoutGroup.style = style;
                    if (options != null)
                        guiLayoutGroup.ApplyOptions(options);
                    current.topLevel.Add(guiLayoutGroup);
                    break;
                default:
                    if (!(current.topLevel.GetNext() is GUILayoutGroup castGuiLayoutGroup))
                        throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + UGUIEvent.current.type);
                    guiLayoutGroup = castGuiLayoutGroup;
                    guiLayoutGroup.ResetCursor();
                    break;
            }
            current.layoutGroups.Push(guiLayoutGroup);
            current.topLevel = guiLayoutGroup;
            return guiLayoutGroup;
        }

        internal static void EndLayoutGroup()
        {
            int type = (int)UGUIEvent.current.type;
            current.layoutGroups.Pop();
            current.topLevel = 0 >= current.layoutGroups.Count ? null : (GUILayoutGroup)current.layoutGroups.Peek();
        }

        internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
        {
            GUILayoutGroup guiLayoutGroup = null;
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                case EventType.Used:
                    guiLayoutGroup = CreateGUILayoutGroupInstanceOfType(layoutType);
                    guiLayoutGroup.style = style;
                    current.windows.Add(guiLayoutGroup);
                    break;
                default:
                    if (!(current.windows.GetNext() is GUILayoutGroup castGuiLayoutGroup))
                        throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + UGUIEvent.current.type);
                    guiLayoutGroup = castGuiLayoutGroup;
                    guiLayoutGroup.ResetCursor();
                    break;
            }
            current.layoutGroups.Push(guiLayoutGroup);
            current.topLevel = guiLayoutGroup;
            return guiLayoutGroup;
        }

        internal static GUILayoutGroup DoBeginLayoutArea(GUIStyle style, Type layoutType) => BeginLayoutArea(style, layoutType);

        internal static GUILayoutGroup topLevel => current.topLevel;

        public static Rect GetRect(GUIContent content, GUIStyle style) => DoGetRect(content, style, null);

        public static Rect GetRect(
          GUIContent content,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetRect(content, style, options);
        }

        private static Rect DoGetRect(
          GUIContent content,
          GUIStyle style,
          GUILayoutOption[] options)
        {
            UGUIUtility.CheckOnUGUI();
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    if (style.isHeightDependantOnWidth)
                    {
                        current.topLevel.Add(new GUIWordWrapSizer(style, content, options));
                    }
                    else
                    {
                        Vector2 constraints = new Vector2(0.0f, 0.0f);
                        if (options != null)
                        {
                            foreach (GUILayoutOption option in options)
                            {
                                switch (option.type)
                                {
                                    case GUILayoutOption.Type.maxWidth:
                                        constraints.x = (float)option.value;
                                        break;
                                    case GUILayoutOption.Type.maxHeight:
                                        constraints.y = (float)option.value;
                                        break;
                                }
                            }
                        }
                        Vector2 vector2 = style.CalcSizeWithConstraints(content, constraints);
                        current.topLevel.Add(new GUILayoutEntry(vector2.x, vector2.x, vector2.y, vector2.y, style, options));
                    }
                    return kDummyRect;
                case EventType.Used:
                    return kDummyRect;
                default:
                    return current.topLevel.GetNext().rect;
            }
        }

        public static Rect GetRect(float width, float height) => DoGetRect(width, width, height, height, GUIStyle.none, null);

        public static Rect GetRect(float width, float height, GUIStyle style) => DoGetRect(width, width, height, height, style, null);

        public static Rect GetRect(float width, float height, params GUILayoutOption[] options) => DoGetRect(width, width, height, height, GUIStyle.none, options);

        public static Rect GetRect(
          float width,
          float height,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetRect(width, width, height, height, style, options);
        }

        public static Rect GetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight)
        {
            return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null);
        }

        public static Rect GetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight,
          GUIStyle style)
        {
            return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, null);
        }

        public static Rect GetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight,
          params GUILayoutOption[] options)
        {
            return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
        }

        public static Rect GetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
        }

        private static Rect DoGetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight,
          GUIStyle style,
          GUILayoutOption[] options)
        {
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
                    return kDummyRect;
                case EventType.Used:
                    return kDummyRect;
                default:
                    return current.topLevel.GetNext().rect;
            }
        }

        public static Rect GetLastRect()
        {
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    return kDummyRect;
                case EventType.Used:
                    return kDummyRect;
                default:
                    return current.topLevel.GetLast();
            }
        }

        public static Rect GetAspectRect(float aspect) => DoGetAspectRect(aspect, GUIStyle.none, null);

        public static Rect GetAspectRect(float aspect, GUIStyle style) => DoGetAspectRect(aspect, style, null);

        public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options) => DoGetAspectRect(aspect, GUIStyle.none, options);

        public static Rect GetAspectRect(
          float aspect,
          GUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetAspectRect(aspect, GUIStyle.none, options);
        }

        private static Rect DoGetAspectRect(
          float aspect,
          GUIStyle style,
          GUILayoutOption[] options)
        {
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    current.topLevel.Add(new GUIAspectSizer(aspect, options));
                    return kDummyRect;
                case EventType.Used:
                    return kDummyRect;
                default:
                    return current.topLevel.GetNext().rect;
            }
        }

        internal static GUIStyle spaceStyle
        {
            get
            {
                if (s_SpaceStyle == null)
                    s_SpaceStyle = new GUIStyle();
                s_SpaceStyle.stretchWidth = false;
                return s_SpaceStyle;
            }
        }

        private static Rect Internal_GetWindowRect(int windowID)
        {
            Rect rect = default;
            return rect;
        }

        private static void Internal_MoveWindow(int windowID, Rect r)
        { }

        internal static Rect GetWindowsBounds()
        {
            Rect rect = default;
            return rect;
        }

        internal sealed class LayoutCache
        {
            internal GUILayoutGroup topLevel = new GUILayoutGroup();
            internal GenericStack layoutGroups = new GenericStack();
            internal GUILayoutGroup windows = new GUILayoutGroup();

            internal LayoutCache() => layoutGroups.Push(topLevel);

            internal LayoutCache(LayoutCache other)
            {
                topLevel = other.topLevel;
                layoutGroups = other.layoutGroups;
                windows = other.windows;
            }
        }
    }
}