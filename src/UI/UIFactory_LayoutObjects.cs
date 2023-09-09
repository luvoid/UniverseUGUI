using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Styles;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Models.Controls;
using System.ComponentModel;

namespace UniverseLib.UI
{
    public sealed partial class UIFactory
    {
        /// <summary>
        /// Create a simple UI object with a RectTransform. The <paramref name="parent"/> may be null.
        /// <br/><br/>
        /// <b>This method ignores context.</b> 
        /// <br/> Use <see cref="UIObject"/> for a context-aware object.
        /// </summary>
        public static GameObject CreateUIObject(GameObject parent, string name, Vector2 sizeDelta = default)
        {
            GameObject obj = new(name)
            {
                layer = 5,
#if !UNITY_EDITOR
                hideFlags = HideFlags.HideAndDontSave,
#else
                hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset,
#endif
            };

            if (parent)
                obj.transform.SetParent(parent.transform, false);

            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
            return obj;
        }

        /// <summary>
        /// Create a simple UI object with a RectTransform. The <paramref name="parent"/> may be null.
        /// <br/><br/>
        /// <b>This method is affected by context.</b> 
        /// <br/> Use <see cref="CreateUIObject(GameObject, string, Vector2)"/> to ignore context.
        /// </summary>
        public GameObject UIObject(GameObject parent, string name, Vector2 sizeDelta = default)
        {
            GameObject obj = CreateUIObject(parent, name, sizeDelta);
            SetDefaultLayoutElement(obj, null);
            return obj;
        }


        /// <summary>
        /// Create a styled UI Object with an <see cref="Image"/> component.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach this to.</param>
        /// <param name="name">The name of the frame GameObject. Useful for debugging purposes.</param>
        /// <param name="style">The style to use when creating the frame. Defaults to <see cref="Skin"/>'s Frame.</param>
        /// <returns>A <see cref="StyledFrame"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot.</returns>
        public StyledFrame Frame(GameObject parent, string name, IReadOnlyFrameStyle style = null)
        {
            style ??= (IReadOnlyFrameStyle)Skin.Frame ?? UISkin.Default.Frame;

            StyledFrame frame = new(parent, name);
            frame.ApplyStyle(style);

            SetDefaultLayoutElement(frame.GameObject, style);

            return frame;
        }



        #region Layout Groups
        /// <summary>
        /// Create a new object with a <see cref="VerticalLayoutGroup"/> component.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters. Defaults to <see cref="Skin"/>'s LayoutGroup.</param>
        /// <inheritdoc cref="SetLayoutGroup_(GameObject, bool?, bool?, bool?, bool?, int?, Vector4?, TextAnchor?)"/>
        public GameObject VerticalGroup(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            int? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            LayoutGroupStyle? style = null)
        {
            style ??= Skin.LayoutGroup;

            GameObject groupObj = Create.UIObject(parent, name);
            var layoutGroup = groupObj.AddComponent<VerticalLayoutGroup>();
            style.Value.ApplyTo(layoutGroup);

            SetLayoutGroup_(layoutGroup, forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);

            return groupObj;
        }

        public GameObject HorizontalGroup(GameObject parent, string name, LayoutGroupStyle style)
        {
            GameObject groupObj = Create.UIObject(parent, name);
            SetLayoutGroup<HorizontalLayoutGroup>(groupObj, style);
            return groupObj;
        }

        /// <summary>
        /// Create a new object with a <see cref="HorizontalLayoutGroup"/> component.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters. Defaults to <see cref="Skin"/>'s LayoutGroup.</param>
        /// <inheritdoc cref="SetLayoutGroup_{T}(GameObject, bool?, bool?, bool?, bool?, int?, Vector4?, TextAnchor?)"/>
        public GameObject HorizontalGroup(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            int? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            LayoutGroupStyle? style = null)
        {
            style ??= Skin.LayoutGroup;

            GameObject groupObj = Create.UIObject(parent, name);
            var layoutGroup = groupObj.AddComponent<HorizontalLayoutGroup>();
            style.Value.ApplyTo(layoutGroup);

            SetLayoutGroup_(layoutGroup, forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);

            return groupObj;
        }

        public GameObject GridGroup(GameObject parent, string name, LayoutGroupStyle style)
        {
            GameObject groupObj = Create.UIObject(parent, name);
            SetLayoutGroup<GridLayoutGroup>(groupObj, style);
            return groupObj;
        }

        /// <summary>
        /// Create a new object with a <see cref="GridLayoutGroup"/> component.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters. Defaults to <see cref="Skin"/>'s LayoutGroup.</param>
        /// <inheritdoc cref="SetLayoutGroup{T}(T, Vector2?, Vector2?, Vector4?, TextAnchor?)"/>
        public GameObject GridGroup(GameObject parent, string name, Vector2 cellSize,
            Vector2? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            LayoutGroupStyle? style = null)
        {
            style ??= Skin.LayoutGroup;

            GameObject groupObj = Create.UIObject(parent, name);
            var layoutGroup = groupObj.AddComponent<GridLayoutGroup>();
            style.Value.ApplyTo(layoutGroup);

            SetLayoutGroup(layoutGroup, cellSize, spacing, padding, childAlignment);

            return groupObj;
        }

        #endregion // Layout Groups



        #region Layout Frames

        /// <summary>
        /// Create a styled frame with a <see cref="VerticalLayoutGroup"/>.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters.</param>
        /// <returns>A <see cref="StyledLayoutFrame{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="SetLayoutGroup_(GameObject, bool?, bool?, bool?, bool?, int?, Vector4?, TextAnchor?)"/>
        public StyledLayoutFrame<VerticalLayoutGroup> VerticalFrame(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            int? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            IReadOnlyFrameStyle style = null)
        {
            var layoutFrame = LayoutFrame<VerticalLayoutGroup>(parent, name, style);

            SetLayoutGroup_(layoutFrame.Component, forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);

            return layoutFrame;
        }

        /// <summary>
        /// Create a styled frame with a <see cref="HorizontalLayoutGroup"/>.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters.</param>
        /// <returns>A <see cref="StyledLayoutFrame{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="SetLayoutGroup_{T}(GameObject, bool?, bool?, bool?, bool?, int?, Vector4?, TextAnchor?)"/>
        public StyledLayoutFrame<HorizontalLayoutGroup> HorizontalFrame(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            int? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            IReadOnlyFrameStyle style = null)
        {
            var layoutFrame = LayoutFrame<HorizontalLayoutGroup>(parent, name, style);

            SetLayoutGroup_(layoutFrame.Component, forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);

            return layoutFrame;
        }

        public GameObject GridFrame(GameObject parent, string name, LayoutGroupStyle style)
        {
            GameObject groupObj = Create.UIObject(parent, name);
            SetLayoutGroup<GridLayoutGroup>(groupObj, style);
            return groupObj;
        }

        /// <summary>
        /// Create a styled frame with a <see cref="GridLayoutGroup"/>.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters.</param>
        /// <returns>A <see cref="StyledLayoutFrame{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="GridFrame(GameObject, string, LayoutGroupStyle)"/>
        /// <inheritdoc cref="SetLayoutGroup{T}(T, Vector2?, Vector2?, Vector4?, TextAnchor?)"/>
        public StyledLayoutFrame<GridLayoutGroup> GridFrame(GameObject parent, string name,
            Vector2? cellSize = null, Vector2? spacing = null, Vector4? padding = null,
            TextAnchor? childAlignment = null,
            IReadOnlyFrameStyle style = null)
        {
            var layoutFrame = LayoutFrame<GridLayoutGroup>(parent, name, style);

            SetLayoutGroup(layoutFrame.Component, cellSize, spacing, padding, childAlignment);

            return layoutFrame;
        }

        /// <summary>
        /// Create a styled frame with a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters.</param>
        /// <returns>A <see cref="StyledLayoutFrame{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <typeparamref name="T"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public StyledLayoutFrame<T> LayoutFrame<T>(GameObject parent, string name, IReadOnlyFrameStyle style)
            where T : LayoutGroup, new()
        {
            style ??= (IReadOnlyFrameStyle)Skin?.Frame ?? UISkin.Default.Frame;

            StyledLayoutFrame<T> layoutFrame = new(parent, name);
            layoutFrame.ApplyStyle(style);

            SetDefaultLayoutElement(layoutFrame.GameObject, style);

            return layoutFrame;
        }

        #endregion // Layout Frames
    }
}
