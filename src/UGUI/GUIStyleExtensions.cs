using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Runtime;
using UniverseLib.UI.Models;
using UniverseLib.UGUI.Models;
using System.ComponentModel;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UGUI.Components;

namespace UniverseLib.UGUI
{
    internal static class GUIStyleExtensions
    {
        public static void ApplyToText(this GUIStyle style, Text text, Font defaultFont = null, GUIStyleState styleState = null)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));
            if (text == null) throw new System.ArgumentNullException(nameof(text));

            styleState ??= style.normal;

            text.alignment = style.alignment;
            text.color = styleState.textColor * UGUI.contentColor * UGUI.color;
            text.font = style.font ?? defaultFont ?? UGUIUtility.GetDefaultFont();

            text.fontSize = style.fontSize != 0 ? style.fontSize : text.font.fontSize;
            text.fontStyle = style.fontStyle;
            //text.lineSpacing = text.font.lineHeight - style.lineHeight;
            text.supportRichText = style.richText;

            text.horizontalOverflow = style.wordWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        public static void ApplyToBackground(this GUIStyle style, Graphic background)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));


            if (background is Image image)
            {
                image.sprite = style.GetBackgroundSprite(image.sprite);
                image.type = Image.Type.Sliced;
                image.color = UGUI.backgroundColor * UGUI.color;
            }
        }

        public static void ApplyToSelectable(this GUIStyle style, Selectable selectable, bool useSprites = true)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));
            if (selectable == null) throw new System.ArgumentNullException(nameof(selectable));

            if (useSprites &&
                (style.active.background != null
                || style.hover.background != null)
                && selectable.targetGraphic is Image)
            {
                selectable.transition = Selectable.Transition.SpriteSwap;
                selectable.spriteState = style.GetSpriteState(selectable.spriteState);
            }
            else
            {
                selectable.transition = Selectable.Transition.ColorTint;
                RuntimeHelper.SetColorBlock(selectable, new ColorBlock()
                {
                    normalColor = style.normal.textColor,
                    highlightedColor = style.hover.textColor,
                    pressedColor = style.active.textColor,
                    disabledColor = UniversalUI.DisabledButtonColor,
                    colorMultiplier = 1f,
                    fadeDuration = 0.1f,
                });
            }
        }


        public static Sprite GetBackgroundSprite(this GUIStyle style, Sprite currentSprite)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));

            return Internal_GetBackgroundSprite(style, style.normal, currentSprite);
        }

        public static SpriteState GetSpriteState(this GUIStyle style, SpriteState currentSpriteState)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));

            return new SpriteState()
            {
                highlightedSprite = Internal_GetBackgroundSprite(style, style.hover, currentSpriteState.highlightedSprite),
                pressedSprite = Internal_GetBackgroundSprite(style, style.active, currentSpriteState.pressedSprite),
                disabledSprite = Internal_GetBackgroundSprite(style, style.normal, currentSpriteState.disabledSprite),
            };
        }

        public static ColorBlock GetTextColors(this GUIStyle style)
        {
            if (style == null) throw new System.ArgumentNullException(nameof(style));

            return default;
        }


        /*
        internal static void DrawUGUI(this GUIStyle style, Rect position, UGUIContent content, bool on = false)
        {
            if (content != null)
                Internal_DrawUGUI(style, position, content, on);
            else
                Universe.LogError($"GUIStyle.{nameof(DrawUGUI)} may not be called with {nameof(UGUIContent)} that is null.");
        }

        internal static void DrawUGUI(this GUIStyle style, Rect position, UGUIContent content, int controlID, bool on = false)
        {
            if (content != null)
                Internal_DrawUGUI(style, position, content, controlID, on);
            else
                Universe.LogError($"GUIStyle.{nameof(DrawUGUI)} may not be called with {nameof(UGUIContent)} that is null.");
        }

        internal static void DrawUGUI(this GUIStyle style, Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            Internal_DrawUGUI(style, position, UGUIContent.none, isHover, isActive, on, hasKeyboardFocus);
        }

        internal static void DrawUGUI(this GUIStyle style, Rect position, UGUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            Internal_DrawUGUI(style, position, content, isHover, isActive, on, hasKeyboardFocus);
        }
        */


        internal static GUIStyleComponent AddStyleComponentTo(this GUIStyle style, GameObject gameObject)
        {
            var styleComponent = gameObject.GetComponent<GUIStyleComponent>();
            if (styleComponent == null)
            {
                styleComponent = gameObject.AddComponent<GUIStyleComponent>();
            }
            styleComponent.Style = style;
            return styleComponent;
        }

        /*
        private static void Internal_DrawUGUI(GUIStyle style, Rect position, UGUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
        {
            Internal_DrawUGUI(style, position, content, on);
        }

        private static void Internal_DrawUGUI(GUIStyle style, Rect position, UGUIContent content, int controlID, bool on)
        {
            Internal_DrawUGUI(style, position, content, on);
        }

        private static void Internal_DrawUGUI(GUIStyle style, Rect position, UGUIContent content, bool on)
        {
            //Universe.Log($"Internal_DrawUGUI on = {on}");
            var rectTransform = UIFactory.CreateUIObject("uGUIObject", UGUIUtility.s_ActiveUGUI.ContentHolder).GetComponent<RectTransform>();

            var bgSprite = style.GetBackgroundSprite(null);
            if (bgSprite != null)
            {
                var background = rectTransform.gameObject.AddComponent<Image>();
                background.sprite = bgSprite;
                background.type = Image.Type.Sliced;
            }

            if (content.text != null)
            {
                Text text = UIFactory.CreateLabel(rectTransform.gameObject, "text", content.text);
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.rectTransform.anchorMin = Vector2.zero;
                text.rectTransform.anchorMax = Vector2.one;
                text.rectTransform.offsetMin = new Vector2(style.margin.left, style.margin.bottom);
                text.rectTransform.offsetMax = new Vector2(-style.margin.right, -style.margin.top);
                style.ApplyTo(text);
            }

            if (content.image != null)
            {
                // TODO
            }

            UGUIUtility.SetRect(rectTransform, position);
        }
        */


        private static MethodInfo s_Internal_CreateSprite;
        private static Sprite Internal_GetBackgroundSprite(GUIStyle style, GUIStyleState state, Sprite currentSprite)
        {
            Texture2D texture = state.background;
            if (texture == null) return null;

            Rect rect = new(0f, 0f, texture.width, texture.height);
            Vector2 pivot = Vector2.zero;
            Vector4 border = new(
                style.border.right,
                style.border.left,
                style.border.bottom,
                style.border.top
            );

            if (currentSprite != null
                && currentSprite.texture == texture
                && currentSprite.rect == rect
                && currentSprite.border == border)
            {
                return currentSprite;
            }

            if (s_Internal_CreateSprite == null)
            {
                s_Internal_CreateSprite = AccessTools.DeclaredMethod(typeof(TextureHelper), "Internal_CreateSprite",
                    new System.Type[] { typeof(Texture2D), typeof(Rect), typeof(Vector2), typeof(float), typeof(uint), typeof(Vector4) }
                );
            }

            Sprite sprite = s_Internal_CreateSprite.Invoke(TextureHelper.Instance, new object[] {
                state.background, rect, pivot, (float)100, (uint)0, border
            }) as Sprite;

            //Universe.Log($"Created background sprite: rect = {rect}; border = {border}");

            return sprite;
        }
    }
}
