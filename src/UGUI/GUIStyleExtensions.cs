using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Runtime;
using UniverseLib.UI;
using UniverseLib.UGUI.Components;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UniverseLib.UGUI
{
    internal static class GUIStyleExtensions
    {
        private static readonly Dictionary<GUIStyleState, Sprite> s_StyleStateSprites = new();
        private static readonly Dictionary<int, Sprite> s_SpriteCache = new();
        private struct SpriteCacheKey
        {
            public Texture2D Texture;
            public Vector4 Border;
            public SpriteCacheKey(Texture2D texture, Vector4 border)
            {
                Texture = texture;
                Border = new Vector4((int)border.x, (int)border.y, (int)border.z, (int)border.w);
            }
        }

        public static void SetBorder(this GUIStyle style, Vector4 border)
        {
            style.border.right  = (int)border.x;
            style.border.left   = (int)border.y;
            style.border.bottom = (int)border.z;
            style.border.top    = (int)border.w;
        }

        public static void SetBackgroundSprite(this GUIStyle style, Sprite sprite, StyleState state)
        {
            Rect rect = new(0f, 0f, sprite.texture.width, sprite.texture.height);
            if (sprite.rect != rect)
            {
                Universe.Logger.LogWarning("Attempting to set a multi-sprite texture to a GUIStyle. It may not display correctly in IMGUI.");
            }

            int key = new SpriteCacheKey(sprite.texture, sprite.border).GetHashCode();
            if (s_SpriteCache.ContainsKey(key) && !s_SpriteCache.ContainsValue(sprite))
            {
                Universe.Logger.LogWarning("Attempt to set sprite to style that has the same texture and border as a different sprite that was previously set. This leades to undefined behavior.");
            }

            s_SpriteCache[key] = sprite;

            GUIStyleState styleState;

            switch (state)
            {
                case StyleState.Normal:
                    styleState = style.normal;
                    break;
                case StyleState.Hover:
                    styleState = style.hover;
                    break;
                case StyleState.Active:
                    styleState = style.active;
                    break;
                case StyleState.Focused:
                    styleState = style.focused;
                    break;
                case StyleState.OnNormal:
                    styleState = style.onNormal;
                    break;
                case StyleState.OnHover:
                    styleState = style.onHover;
                    break;
                case StyleState.OnActive:
                    styleState = style.onActive;
                    break;
                case StyleState.OnFocused:
                    styleState = style.onFocused;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
            styleState.background = sprite.texture;
            style.SetBorder(sprite.border);
            s_StyleStateSprites[styleState] = sprite;
        }

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

        private static Sprite Internal_GetBackgroundSprite(GUIStyle style, GUIStyleState state, Sprite currentSprite)
        {
            Texture2D texture = state.background;
            if (texture == null) return null;

            Vector4 border = new(
                style.border.right,
                style.border.left,
                style.border.bottom,
                style.border.top
            );

            if (s_StyleStateSprites.TryGetValue(state, out Sprite specifiedSprite) && specifiedSprite.texture == texture)
            {
                return specifiedSprite;
            }

            int key = new SpriteCacheKey(texture, border).GetHashCode();
            if (s_SpriteCache.ContainsKey(key)) 
            {
                return s_SpriteCache[key] as Sprite;
            }

            Rect rect = new(0f, 0f, texture.width, texture.height);
            Vector2 pivot = Vector2.zero;
            if (currentSprite != null
                && currentSprite.texture == texture
                && currentSprite.rect == rect
                && currentSprite.border == border)
            {
                return currentSprite;
            }

            Sprite sprite = TextureHelper.CreateSprite(state.background, rect, pivot, 100, 0, border);
            s_SpriteCache.Add(key, sprite);

            //Universe.Log($"Created background sprite: rect = {rect}; border = {border}");

            return sprite;
        }
    }
}
