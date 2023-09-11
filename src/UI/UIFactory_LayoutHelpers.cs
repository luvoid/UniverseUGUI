using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;
using UniverseLib.Utility;

namespace UniverseLib.UI
{
    public sealed partial class UIFactory
    {
        /// <summary>
        /// Get and/or Add a <see cref="LayoutElement"/> component to the <paramref name="gameObject"/>, and set all of the values on it.
        /// </summary>
        public static LayoutElement SetLayoutElement(GameObject gameObject, LayoutElementStyle style)
        {
            LayoutElement layout = gameObject.GetComponent<LayoutElement>();
            if (!layout)
                layout = gameObject.AddComponent<LayoutElement>();

            style.ApplyTo(layout);

            return layout;
        }

        /// <inheritdoc cref="SetLayoutGroup_(GameObject, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public static T SetLayoutGroup<T>(GameObject gameObject, Vector4 padding,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, TextAnchor? childAlignment = null)
            where T : HorizontalOrVerticalLayoutGroup, new()
        {
            return SetLayoutGroup_<T>(gameObject,
                forceWidth, forceHeight, childControlWidth, childControlHeight, 
                spacing, padding, childAlignment);
        }

        /// <summary>
        /// Get and/or Add a <see cref="HorizontalOrVerticalLayoutGroup"/> (must pick one) to the <paramref name="gameObject"/>, and set the values on it.
        /// </summary>
        /// <param name="padding">(left, right, top, bottom)</param>
        // Ideally, this would replace the old SetLayoutGroup one day.
        internal static T SetLayoutGroup_<T>(GameObject gameObject, 
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null)
            where T : HorizontalOrVerticalLayoutGroup, new()
        {
            T group = gameObject.GetComponent<T>();
            if (group.IsNullOrDestroyed())
                group = gameObject.AddComponent<T>();

            return SetLayoutGroup_<T>(group,
                forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);
        }

        /// <inheritdoc cref="SetLayoutGroup_{T}(T, bool?, bool?, bool?, bool?, float?, Vector4?, TextAnchor?)"/>
        public static T SetLayoutGroup<T>(T group, Vector4 padding,
           bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null,
           float? spacing = null, TextAnchor? childAlignment = null)
           where T : HorizontalOrVerticalLayoutGroup
        {
            return SetLayoutGroup_<T>(group,
                forceWidth, forceHeight, childControlWidth, childControlHeight,
                spacing, padding, childAlignment);
        }

        /// <summary>
        /// Set the values on a <see cref="HorizontalOrVerticalLayoutGroup"/>.
        /// </summary>
        /// <param name="padding">(left, right, top, bottom)</param>
        // Ideally, this would replace the old SetLayoutGroup one day.
        internal static T SetLayoutGroup_<T>(T group,
            bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null, 
            float? spacing = null, Vector4? padding = null, TextAnchor? childAlignment = null)
            where T : HorizontalOrVerticalLayoutGroup
        {
            if (forceWidth.HasValue)
                group.childForceExpandWidth = forceWidth.Value;
            if (forceHeight.HasValue)
                group.childForceExpandHeight = forceHeight.Value;
            if (childControlWidth.HasValue)
                group.SetChildControlWidth(childControlWidth.Value);
            if (childControlHeight.HasValue)
                group.SetChildControlHeight(childControlHeight.Value);
            if (spacing.HasValue)
                group.spacing = spacing.Value;
            if (padding.HasValue)
                group.padding.Set(padding.Value);
            if (childAlignment.HasValue)
                group.childAlignment = childAlignment.Value;
            return group;
        }

        public static T SetLayoutGroup<T>(GameObject gameObject,
            Vector2? cellSize = null, Vector2? spacing = null, Vector4? padding = null,
            TextAnchor? childAlignment = null)
            where T : GridLayoutGroup
        {
            T group = gameObject.GetComponent<T>();
            if (group.IsNullOrDestroyed())
                group = gameObject.AddComponent<T>();

            return SetLayoutGroup(group, cellSize, spacing, padding, childAlignment);
        }


        public static T SetLayoutGroup<T>(T group, Vector2? cellSize = null, 
            Vector2? spacing = null, Vector4? padding = null,
            TextAnchor? childAlignment = null)
            where T : GridLayoutGroup
        {
            if (cellSize.HasValue)
                group.cellSize = cellSize.Value;
            if (spacing.HasValue)
                group.spacing = spacing.Value;
            if (padding.HasValue)
                group.padding.Set(padding.Value);
            if (childAlignment.HasValue)
                group.childAlignment = childAlignment.Value;
            return group;
        }

        /// <summary>
        /// Get and/or Add a <see cref="LayoutGroup"/> (must pick one) to the <paramref name="gameObject"/>, and set the values on it.
        /// </summary>
        public static T SetLayoutGroup<T>(GameObject gameObject, LayoutGroupStyle style)
            where T : LayoutGroup, new()
        {
            T group = gameObject.GetComponent<T>();
            if (!group)
                group = gameObject.AddComponent<T>();

            style.ApplyTo(group);

            return group;
        }

        /// <summary>
        /// Set the values on a <see cref="LayoutGroup"/> <paramref name="group"/>.
        /// </summary>
        public static T SetLayoutGroup<T>(T group, LayoutGroupStyle style)
            where T : LayoutGroup, new()
        {
            if (group == null) throw new System.ArgumentNullException(nameof(group));

            style.ApplyTo(group);
            return group;
        }


        /// <summary>
        /// Get and/or Add a <see cref="LayoutAutoSize"/> to the <paramref name="gameObject"/>, and set the values on it.
        /// </summary>
        /// <param name="childControlWidth">Should the children control this object's width? Defaults to true when creating a new LayoutAutoSize.</param>
        /// <param name="childControlHeight">Should the children control this object's height? Defaults to true when creating a new LayoutAutoSize.</param>
        public static LayoutAutoSize SetLayoutAutoSize(GameObject gameObject, bool? childControlWidth = null, bool? childControlHeight = null)
        {
            LayoutAutoSize layoutAutoSize = gameObject.GetComponent<LayoutAutoSize>();
            if (!layoutAutoSize)
                layoutAutoSize = gameObject.AddComponent<LayoutAutoSize>();

            if (childControlWidth.HasValue)
                layoutAutoSize.ChildControlWidth = childControlWidth.Value;

            if (childControlHeight.HasValue)
                layoutAutoSize.ChildControlWidth = childControlHeight.Value;

            return layoutAutoSize;
        }


        /// <summary>
        /// Change the <paramref name="gameObject"/>'s <see cref="RectTransform"/> anchors and offsets to fill the entire parent
        /// with the given padding and position offset.
        /// </summary>
        /// <param name="padding">(left, right, top, bottom)</param>
        public static void SetOffsets(GameObject gameObject, Vector4 padding, Vector2 positionOffset = default)
        {
            RectTransform rectTransform = gameObject.transform.TryCast<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2( padding.x,  padding.w) + positionOffset;
            rectTransform.offsetMax = new Vector2(-padding.y, -padding.z) + positionOffset;
        }
    }
}
