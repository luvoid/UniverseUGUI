using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Styles;
using UniverseLib.UI.Panels;
using UniverseLib.UI.Models.Styled;
using UniverseLib.UI.Models.Controls;

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
        /// Create a styled UI Object with a <see cref="VerticalLayoutGroup"/> and an <see cref="Image"/> component.
        /// <br /><br />See also: <see cref="SkinnedPanelBase"/>
        /// </summary>
        /// <param name="parent">The parent GameObject to attach this to.</param>
        /// <param name="name">The name of the panel GameObject, useful for debugging purposes.</param>
        /// <param name="style">The style to use when creating the panel. Defaults to <see cref="Skin"/>'s Panel.</param>
        /// <returns>A <see cref="StyledPanel"/> wrapper for the base GameObject (not for adding content to) and the ContentRoot.</returns>
        public StyledPanel Panel(GameObject parent, string name, IReadOnlyPanelStyle style = null)
        {
            style ??= (IReadOnlyPanelStyle)Skin?.Panel ?? UISkin.Default.Panel;

            StyledPanel panel = new(parent, name);
            panel.ApplyStyle(style);

            SetDefaultLayoutElement(panel.GameObject, style);

            return panel;
        }
    }
}
