using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Controls
{
    public abstract class Property
    {
        public readonly GameObject GameObject;
        public readonly LabelModel Label;

        public UIControlModel Control => ControlModel;
        protected abstract UIControlModel ControlModel { get; }

        internal Property(GameObject parent, string propertyName)
        {
            GameObject = UIFactory.CreateUIObject(parent, propertyName.Replace(" ", ""));
            Label = new LabelModel(GameObject, "Label", propertyName);

            RectTransform labelTransform = Label.UIRoot.transform.TryCast<RectTransform>();
            labelTransform.anchorMin = new Vector2(0, 0);
            labelTransform.anchorMax = new Vector2(0.5f, 1);
            labelTransform.anchoredPosition = Vector2.zero;
            labelTransform.sizeDelta = Vector2.zero;

            UIFactory.SetLayoutAutoSize(GameObject);
        }
    }

    public class Property<T> : Property
        where T : UIControlModel
    {
        protected sealed override UIControlModel ControlModel => Control;

        public new readonly T Control;

        public Property(GameObject parent, string propertyName, T control)
            : base(parent, propertyName)
        {
            Control = control;

            RectTransform controlTransform = Control.UIRoot.transform.TryCast<RectTransform>();
            controlTransform.SetParent(GameObject.transform, false);
            controlTransform.anchorMin = new Vector2(0.5f, 0);
            controlTransform.anchorMax = new Vector2(1, 1);
            controlTransform.anchoredPosition = Vector2.zero;
            controlTransform.sizeDelta = Vector2.zero;
        }
    }
}
