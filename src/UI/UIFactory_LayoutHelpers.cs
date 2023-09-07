using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Components;
using UniverseLib.UI.Styles;

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

        /// <summary>
        /// Get and/or Add a <see cref="HorizontalOrVerticalLayoutGroup"/> (must pick one) to the <paramref name="gameObject"/>, and set the values on it.
        /// </summary>
        /// <param name="padding">(left, right, top, bottom)</param>
        public static T SetLayoutGroup<T>(GameObject gameObject, Vector4 padding, bool? forceWidth = null, bool? forceHeight = null,
            bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, 
            TextAnchor? childAlignment = null)
            where T : HorizontalOrVerticalLayoutGroup, new()
        {
            return SetLayoutGroup<T>(gameObject, forceWidth, forceHeight, childControlWidth, childControlHeight, spacing,
                (int)padding.z, (int)padding.w, (int)padding.x, (int)padding.y, childAlignment);
        }

        /// <summary>
        /// Set the values on a <see cref="HorizontalOrVerticalLayoutGroup"/>.
        /// </summary>
        /// <param name="padding">(left, right, top, bottom)</param>
        public static T SetLayoutGroup<T>(T group, Vector4 padding, bool? forceWidth = null, bool? forceHeight = null,
            bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, TextAnchor? childAlignment = null)
            where T : HorizontalOrVerticalLayoutGroup, new()
        {
            return SetLayoutGroup(group, forceWidth, forceHeight, childControlWidth, childControlHeight, spacing,
                (int)padding.z, (int)padding.w, (int)padding.x, (int)padding.y, childAlignment);
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
    }
}
