using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine;
using UnityEngine.UI;
using BaseUniverseLib = UniverseLib;
using UniverseLib.Runtime;
using UniverseLib.UI.Models;
using System.Collections;
using System.Collections.Generic;
using UniverseLib.UGUI.Models;
using UniverseLib.UI;
//using static UnityEngine.GUIUtility;

namespace UniverseLib.UGUI
{
    public static class UGUIUtility
    {
        public readonly static Texture2D BlankTexture;
        static UGUIUtility()
        {
            BlankTexture = TextureHelper.NewTexture2D(1, 1);
            BlankTexture.SetPixel(0, 0, Color.white);
            BlankTexture.filterMode = FilterMode.Point;
        }

        internal static IUniversalUGUIObject s_ActiveUGUI;
        internal static GameObject s_ActiveParent;
        internal static int s_SkinMode;
        internal static int s_OriginalID;
        internal static Vector2 s_EditorScreenPointOffset = Vector2.zero;

        public static void SetRect(RectTransform rectTransform, Rect rect)
        {
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.offsetMin = new Vector2(rect.min.x, -rect.max.y);
            rectTransform.offsetMax = new Vector2(rect.max.x, -rect.min.y);
        }

        private static Font s_BuiltinDefaultFont;
        internal static Font GetDefaultFont()
        {
            // First try to get the best skin
            GUISkin skin = null;
            try
            {
                skin = GUI.skin;
            }
            catch
            { }

            if (skin != null)
            {
                // Then try to get the skin font
                var skinFont = skin.font;
                if (skinFont != null) return skinFont;
            }

            // Otherwise use the UniversalUI default font
            return UniversalUI.DefaultFont;
        }





        internal static float pixelsPerPoint => GUIUtility.Internal_GetPixelsPerPoint();


        private static HashSet<int> s_ControlIDs = new HashSet<int>();

        public static int GetControlID(FocusType focus, Rect? position = null)
            => GetControlID(0, focus, position);
        public static int GetControlID(GUIContent contents, FocusType focus, Rect? position = null)
            => GetControlID(contents.hash, focus, position);
        public static int GetControlID(int hint, FocusType focus, Rect? position = null)
        {
            return GetControlID(hint + focus.GetHashCode());
        }
        public static int GetControlID<T>(int hint = 0)
            where T : UGUIModel
        {
            return GetControlID(typeof(T).GUID.GetHashCode() + hint);
        }
        private static int GetControlID(int hint)
        {
            int controlId = hint;
            while (!s_ControlIDs.Add(controlId++)) ;

            return controlId;
        }



        public static object GetStateObject(Type t, int controlID) => GUIStateObjects.GetStateObject(t, controlID);

        public static object QueryStateObject(Type t, int controlID) => GUIStateObjects.QueryStateObject(t, controlID);

        internal static bool guiIsExiting { get; set; }

        public static int hotControl
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public static int keyboardControl
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public static void ExitUGUI()
        {
            guiIsExiting = true;
            throw new ExitGUIException();
        }

        internal static GUISkin GetDefaultSkin() => GUIUtility.Internal_GetDefaultSkin(s_SkinMode);

        internal static GUISkin GetBuiltinSkin(int skin) => GUIUtility.Internal_GetBuiltinSkin(skin) as GUISkin;

        internal static void BeginUGUI(in IUniversalUGUIObject uGUI, in UGUIEventType eventType, int skinMode = 0, bool useGUILayout = true)
        {
            s_ActiveUGUI = uGUI;
            s_ActiveParent = uGUI.ContentRoot;
            s_SkinMode = skinMode;
            s_OriginalID = uGUI.GetInstanceID();

            UGUI.skin = null;
            UGUI.color = Color.white;
            UGUI.backgroundColor = Color.white;
            UGUI.contentColor = Color.white;

            guiIsExiting = false;
            UGUIEvent.BeginUGUIEvent(eventType);
            if (useGUILayout)
                UGUILayoutUtility.Begin(uGUI);
            UGUI.changed = false;
        }

        internal static void EndUGUI()
        {
            EndUGUI(1);
        }

        internal static void EndUGUI(int layoutType)
        {
            try
            {
                if (UGUIEvent.current.type == EventType.Layout && layoutType != 0)
                {
                    if (layoutType != 1)
                    {
                        if (layoutType == 2)
                            UGUILayoutUtility.LayoutFromEditorWindow();
                    }
                    else
                        UGUILayoutUtility.Layout();
                }
                UGUILayoutUtility.SelectIDList(s_OriginalID, false);
                GUIContent.ClearStaticCache();
            }
            finally
            {
                Internal_ExitUGUI();
            }
        }

        internal static bool EndUGUIFromException(Exception exception)
        {
            Internal_ExitUGUI();
            return ShouldRethrowException(exception);
        }

        internal static void Internal_ExitUGUI()
        {
            s_ActiveUGUI = null;
            UGUIEvent.EndUGUIEvent();
            s_ControlIDs.Clear();

            UGUI.color = Color.white;
            UGUI.backgroundColor = Color.white;
            UGUI.contentColor = Color.white;
        }


        [RequiredByNativeCode]
        internal static bool EndContainerGUIFromException(Exception exception) => ShouldRethrowException(exception);

        internal static bool ShouldRethrowException(Exception exception)
        {
            while (exception is TargetInvocationException && exception.InnerException != null)
                exception = exception.InnerException;
            return exception is ExitGUIException;
        }

        internal static void CheckOnUGUI()
        {
            if (s_ActiveUGUI == null)
                throw new ArgumentException($"You can only call UGUI functions from inside {nameof(IUniversalUGUIBehaviour.OnUGUI)} or {nameof(IUniversalUGUIBehaviour.OnUGUIStart)}.");
        }

        internal static void CheckOnUGUIStart()
        {
            CheckOnUGUI();
            if (UGUIEvent.current.uRawType != UGUIEventType.InitialLayout
                && UGUIEvent.current.uRawType != UGUIEventType.InitialRepaint)
            {
                throw new ArgumentException($"You can only call UGUI functions with getters from inside {nameof(IUniversalUGUIBehaviour.OnUGUIStart)}.");
            }
        }

        public static Vector2 GUIToScreenPoint(Vector2 guiPoint) => GUIClip.Unclip(guiPoint) + s_EditorScreenPointOffset;

        internal static Rect GUIToScreenRect(Rect guiRect)
        {
            Vector2 screenPoint = GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
            guiRect.x = screenPoint.x;
            guiRect.y = screenPoint.y;
            return guiRect;
        }

        public static Vector2 ScreenToGUIPoint(Vector2 screenPoint) => GUIClip.Clip(screenPoint) - s_EditorScreenPointOffset;

        public static Rect ScreenToGUIRect(Rect screenRect)
        {
            Vector2 guiPoint = ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
            screenRect.x = guiPoint.x;
            screenRect.y = guiPoint.y;
            return screenRect;
        }

        public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
        {
            Matrix4x4 matrix = UGUI.matrix;
            UGUI.matrix = Matrix4x4.identity;
            Vector2 vector2 = GUIClip.Unclip(pivotPoint);
            UGUI.matrix = Matrix4x4.TRS((Vector3)vector2, Quaternion.Euler(0.0f, 0.0f, angle), Vector3.one) * Matrix4x4.TRS(-vector2, Quaternion.identity, Vector3.one) * matrix;
        }

        public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
        {
            Matrix4x4 matrix = UGUI.matrix;
            Vector2 vector2 = GUIClip.Unclip(pivotPoint);
            UGUI.matrix = Matrix4x4.TRS((Vector3)vector2, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector2, Quaternion.identity, Vector3.one) * matrix;
        }



        internal static T GetControlModel<T>(out int controlId, int hint = 0)
            where T : UGUIModel
        {
            controlId = GetControlID<T>(hint);
            return GetControlModel<T>(controlId);
        }
        internal static T GetControlModel<T>(int controlID)
            where T : UGUIModel
        {
            return s_ActiveUGUI.Models[controlID] as T;
        }

        internal static bool TryGetControlModel<T>(out T model, out int controlId, int hint = 0)
            where T : UGUIModel
        {
            controlId = GetControlID<T>(hint);
            return TryGetControlModel(out model, controlId);
        }
        internal static bool TryGetControlModel<T>(out T model, int controlID)
            where T : UGUIModel
        {
            if (s_ActiveUGUI.Models.TryGetValue(controlID, out UGUIModel uGUIModel))
            {
                model = uGUIModel as T;
                return true;
            }
            else
            {
                model = default;
                return false;
            }
        }

        //internal static void AddControlModel<T>(T model, out int controlId, int hint = 0)
        //	where T : UGUIModel
        //{
        //	controlId = GetControlID<T>(hint);
        //	SetControlModel(model, controlId);
        //}

        internal static void SetControlModel(UGUIModel model, int controlID)
        {
            s_ActiveUGUI.Models[controlID] = model;
        }
    }
}
