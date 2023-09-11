using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Styles;
using UniverseLib.UI.Panels;
using System.ComponentModel;
using UniverseLib.UI.Components;
using UniverseLib.UI.Models;

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
        /// <returns>A <see cref="FrameModel"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot.</returns>
        public FrameModel Frame(GameObject parent, string name, Vector2 sizeDelta = default, IReadOnlyFrameStyle style = null)
        {
            style ??= (IReadOnlyFrameStyle)Skin.Frame ?? UISkin.Default.Frame;

            FrameModel frame = new(parent, name, sizeDelta);
            frame.ApplyStyle(style);

            SetDefaultLayoutElement(frame.GameObject, style);

            return frame;
        }


        /// <summary>
        /// Create a styled UI Object with an <see cref="Image"/> component, and a ContentRoot with a <see cref="LayoutAutoSize"/> component.
        /// <br/><br/>
        /// The ContentRoot's offsets will be infulenced by the style's padding, but otherwise it's children will not be altered in any way.
        /// You can manually place objects in this frame, and it will try to resize until all the children's ILayoutElements are satisfied.
        /// </summary>
        /// <returns>A <see cref="AutoFrameModel"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot.</returns>
        /// <inheritdoc cref="Frame"/>
        public AutoFrameModel AutoFrame(GameObject parent, string name, IReadOnlyFrameStyle style = null)
        {
            style ??= (IReadOnlyFrameStyle)Skin.Frame ?? UISkin.Default.Frame;

            AutoFrameModel frame = new(parent, name);
            frame.ApplyStyle(style);

            SetDefaultLayoutElement(frame.GameObject, style);

            return frame;
        }



        #region Layout Groups
        /// <summary>
        /// Create a new object with a <see cref="VerticalLayoutGroup"/> component.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters. Defaults to <see cref="Skin"/>'s LayoutGroup.</param>
        /// <inheritdoc cref="SetLayoutGroup_(GameObject, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public GameObject VerticalGroup(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
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

        /// <summary>
        /// Create a new object with a <see cref="HorizontalLayoutGroup"/> component.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters. Defaults to <see cref="Skin"/>'s LayoutGroup.</param>
        /// <inheritdoc cref="SetLayoutGroup_{T}(GameObject, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public GameObject HorizontalGroup(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
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
        /// <returns>A <see cref="LayoutFrameModel{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="SetLayoutGroup_(GameObject, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public LayoutFrameModel<VerticalLayoutGroup> VerticalFrame(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
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
        /// <returns>A <see cref="LayoutFrameModel{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="SetLayoutGroup_{T}(GameObject, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public LayoutFrameModel<HorizontalLayoutGroup> HorizontalFrame(GameObject parent, string name,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null,
            IReadOnlyFrameStyle style = null)
        {
            var layoutFrame = LayoutFrame<HorizontalLayoutGroup>(parent, name, style);

            SetLayoutGroup_(layoutFrame.Component, forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);

            return layoutFrame;
        }

        /// <summary>
        /// Create a styled frame with a <see cref="GridLayoutGroup"/>.
        /// </summary>
        /// <param name="style">This is applied before the layout parameters.</param>
        /// <returns>A <see cref="LayoutFrameModel{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <see cref="VerticalLayoutGroup"/>.</returns>
        /// <inheritdoc cref="SetLayoutGroup{T}(T, Vector2?, Vector2?, Vector4?, TextAnchor?)"/>
        public LayoutFrameModel<GridLayoutGroup> GridFrame(GameObject parent, string name,
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
        /// <returns>A <see cref="LayoutFrameModel{T}"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot with a <typeparamref name="T"/>.</returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public LayoutFrameModel<T> LayoutFrame<T>(GameObject parent, string name, IReadOnlyFrameStyle style)
            where T : LayoutGroup, new()
        {
            style ??= (IReadOnlyFrameStyle)Skin?.Frame ?? UISkin.Default.Frame;

            LayoutFrameModel<T> layoutFrame = new(parent, name);
            layoutFrame.ApplyStyle(style);

            SetDefaultLayoutElement(layoutFrame.GameObject, style);

            return layoutFrame;
        }

        #endregion // Layout Frames
    }
}
