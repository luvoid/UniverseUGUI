using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Scripting;
using UnityEngineInternal;
using UnityEngine;
using UniverseLib.UGUI.ImplicitTypes;

namespace UniverseLib.UGUI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles",
        Justification = "Unity's naming style must be preserved for backwards compatibility with IMGUI users.")]
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
                current.topLevel = layoutCache.topLevel = new UGUILayoutGroup();
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = layoutCache.windows = new UGUILayoutGroup();
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
                current.topLevel = cache.topLevel = new UGUILayoutGroup();
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = cache.windows = new UGUILayoutGroup();
            }
            else
            {
                current.topLevel = cache.topLevel;
                current.layoutGroups = cache.layoutGroups;
                current.windows = cache.windows;
            }
        }

        internal static void BeginWindow(int windowID, int instanceID, Rect contentRect, UGUIStyle style, GUILayoutOption[] options)
        {
            LayoutCache layoutCache = SelectIDList(instanceID, true);
            if (UGUIEvent.current.type == EventType.Layout)
            {
                current.topLevel = layoutCache.topLevel = new UGUILayoutGroup();
                current.topLevel.style = GUIStyle.none;
                current.topLevel.windowID = windowID;
                current.topLevel.contentRect = contentRect;
                if (options != null)
                    current.topLevel.ApplyOptions(options);
                current.layoutGroups.Clear();
                current.layoutGroups.Push(current.topLevel);
                current.windows = layoutCache.windows = new UGUILayoutGroup();
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

        internal static void LayoutFreeGroup(UGUILayoutGroup toplevel)
        {
            foreach (UGUILayoutGroup entry in toplevel.entries)
                LayoutSingleGroup(entry);
            toplevel.ResetCursor();
        }

        private static void LayoutSingleGroup(UGUILayoutGroup i)
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
                Rect windowRect = i.contentRect;
                i.SetHorizontal(windowRect.x, Mathf.Clamp(windowRect.width, i.minWidth, i.maxWidth));
                i.CalcHeight();
                i.SetVertical(windowRect.y, Mathf.Clamp(windowRect.height, i.minHeight, i.maxHeight));
                Internal_MoveWindow(i.windowID, i.rect);
            }
        }

        [SecuritySafeCritical]
        private static UGUILayoutGroup CreateGUILayoutGroupInstanceOfType(Type LayoutType) 
            => typeof(UGUILayoutGroup).IsAssignableFrom(LayoutType) ? (UGUILayoutGroup)Activator.CreateInstance(LayoutType) : throw new ArgumentException("LayoutType needs to be of type UGUILayoutGroup");

        internal static UGUILayoutGroup BeginLayoutGroup(
          GUIStyle style,
          GUILayoutOption[] options,
          Type layoutType)
        {
            UGUILayoutGroup guiLayoutGroup = null;
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
                    if (!(current.topLevel.GetNext() is UGUILayoutGroup castGuiLayoutGroup))
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
            current.topLevel = 0 >= current.layoutGroups.Count ? null : (UGUILayoutGroup)current.layoutGroups.Peek();
        }

        internal static UGUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
        {
            UGUILayoutGroup guiLayoutGroup = null;
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                case EventType.Used:
                    guiLayoutGroup = CreateGUILayoutGroupInstanceOfType(layoutType);
                    guiLayoutGroup.style = style;
                    current.windows.Add(guiLayoutGroup);
                    break;
                default:
                    if (!(current.windows.GetNext() is UGUILayoutGroup castGuiLayoutGroup))
                        throw new ArgumentException("GUILayout: Mismatched LayoutGroup." + UGUIEvent.current.type);
                    guiLayoutGroup = castGuiLayoutGroup;
                    guiLayoutGroup.ResetCursor();
                    break;
            }
            current.layoutGroups.Push(guiLayoutGroup);
            current.topLevel = guiLayoutGroup;
            return guiLayoutGroup;
        }

        internal static UGUILayoutGroup DoBeginLayoutArea(GUIStyle style, Type layoutType) => BeginLayoutArea(style, layoutType);

        internal static UGUILayoutGroup topLevel => current.topLevel;

        public static Rect GetRect(UGUIContent content, UGUIStyle style) 
            => DoGetRect(content, style, null);

        public static Rect GetRect(
          UGUIContent content,
          UGUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetRect(content, style, options);
        }

        private static Rect DoGetRect(
          UGUIContent content,
          UGUIStyle style,
          GUILayoutOption[] options)
        {
            UGUIUtility.CheckOnUGUI();
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    if (style.isHeightDependantOnWidth)
                    {
                        current.topLevel.Add(new GUIWordWrapSizer(style.InternalGUIStyle, GUIContent.Temp(content.text, content.image), options));
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
                        Vector2 size = style.CalcSizeWithConstraints(GUIContent.Temp(content.text, content.image), constraints);
                        current.topLevel.Add(new GUILayoutEntry(size.x, size.x, size.y, size.y, style.InternalGUIStyle, options));
                    }
                    return kDummyRect;
                case EventType.Used:
                    return kDummyRect;
                default:
                    return current.topLevel.GetNext().rect;
            }
        }

        public static Rect GetRect(float width, float height) => DoGetRect(width, width, height, height, GUIStyle.none, null);

        public static Rect GetRect(float width, float height, UGUIStyle style) => DoGetRect(width, width, height, height, style, null);

        public static Rect GetRect(float width, float height, params GUILayoutOption[] options) => DoGetRect(width, width, height, height, GUIStyle.none, options);

        public static Rect GetRect(
          float width,
          float height,
          UGUIStyle style,
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
          UGUIStyle style)
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
          UGUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
        }

        private static Rect DoGetRect(
          float minWidth,
          float maxWidth,
          float minHeight,
          float maxHeight,
          UGUIStyle style,
          GUILayoutOption[] options)
        {
            switch (UGUIEvent.current.type)
            {
                case EventType.Layout:
                    current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style.InternalGUIStyle, options));
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

        public static Rect GetAspectRect(float aspect, UGUIStyle style) => DoGetAspectRect(aspect, style, null);

        public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options) => DoGetAspectRect(aspect, GUIStyle.none, options);

        public static Rect GetAspectRect(
          float aspect,
          UGUIStyle style,
          params GUILayoutOption[] options)
        {
            return DoGetAspectRect(aspect, GUIStyle.none, options);
        }

        private static Rect DoGetAspectRect(
          float aspect,
          UGUIStyle style,
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

        internal static UGUIStyle spaceStyle
        {
            get
            {
                if (s_SpaceStyle == null)
                    s_SpaceStyle = new GUIStyle();
                s_SpaceStyle.stretchWidth = false;
                return s_SpaceStyle;
            }
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
            internal UGUILayoutGroup topLevel = new UGUILayoutGroup();
            internal GenericStack layoutGroups = new GenericStack();
            internal UGUILayoutGroup windows = new UGUILayoutGroup();

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
