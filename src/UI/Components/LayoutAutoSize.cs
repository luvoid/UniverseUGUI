using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniverseLib.UI.ObjectPool;
using UniverseLib.UI.Styles;

namespace UniverseLib.UI.Components
{
    /// <summary>
    /// A <see cref="ILayoutElement"/> that defines its layout based on its children.
    /// This is similar to a <see cref="LayoutGroup"/>, except it does not control
    /// the layout of the children. A <see cref="LayoutElement"/> can still override settings.
    /// </summary>
    /// <remarks>
    /// Like other <see cref="ILayoutElement"/> components, this wont be resized unless
    /// parented to a <see cref="ILayoutController"/>, such as a <see cref="VerticalLayoutGroup"/>.
    /// </remarks>
    public class LayoutAutoSize : UIBehaviour, ILayoutElement, ILayoutGroup
    {
        public RectSizeBool ChildControl = new(true, true);

        public bool ChildControlWidth { get => ChildControl.Width; set => ChildControl.Width = value; }
        public bool ChildControlHeight { get => ChildControl.Height; set => ChildControl.Height = value; }


        private DrivenRectTransformTracker m_Tracker;

        private Vector2 m_TotalMinSize       = Vector2.zero;
        private Vector2 m_TotalPreferredSize = Vector2.zero;
        private Vector2 m_TotalFlexibleSize  = Vector2.zero;

        [System.NonSerialized]
        private LayoutElement m_LayoutElement = null;
        protected LayoutElement LayoutElement
        {
            get
            {
                if (m_LayoutElement == null)
                {
                    m_LayoutElement = GetComponent<LayoutElement>();
                }

                return m_LayoutElement;
            }
        }


        [System.NonSerialized]
        private RectTransform m_Rect;

        [System.NonSerialized]
        protected readonly List<RectTransform> rectChildren = new List<RectTransform>();
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }

                return m_Rect;
            }
        }

        public float minWidth        => (LayoutElement?.minWidth        >= 0) ? -1 : m_TotalMinSize.x;
        public float minHeight       => (LayoutElement?.minHeight       >= 0) ? -1 : m_TotalMinSize.y;
        public float preferredWidth  => (LayoutElement?.preferredWidth  >= 0) ? -1 : m_TotalPreferredSize.x;
        public float preferredHeight => (LayoutElement?.preferredHeight >= 0) ? -1 : m_TotalPreferredSize.y;
        public float flexibleWidth   => (LayoutElement?.flexibleWidth   >= 0) ? -1 : m_TotalFlexibleSize.x;
        public float flexibleHeight  => (LayoutElement?.flexibleHeight  >= 0) ? -1 : m_TotalFlexibleSize.y;

        public int layoutPriority { get { return 1; } }


        public void CalculateLayoutInputHorizontal()
        {
            CollectRectChildren();
            CalcAlongAxis(0);
        }

        public void CalculateLayoutInputVertical()
        {
            CollectRectChildren();
            CalcAlongAxis(1);
        }

#if MONO
        public override void OnEnable()
#else
        protected override void OnEnable()
#endif
        {
            base.OnEnable();
            SetDirty();
        }

#if MONO
        public override void OnDisable()
#else
        protected override void OnDisable()
#endif
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

#if MONO
        public override void OnDidApplyAnimationProperties()
#else
        protected override void OnDidApplyAnimationProperties()
#endif
        {
            SetDirty();
        }

        protected virtual void OnTransformChildrenChanged()
        {
            SetDirty();
        }


        /// <summary>
        /// Mark the <see cref="LayoutAutoSize"/> as dirty.
        /// </summary>
        protected void SetDirty()
        {
            if (IsActive())
            {
                if (!CanvasUpdateRegistry.IsRebuildingLayout())
                {
                    LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
                }
                else
                {
                    StartCoroutine(DelayedSetDirty(rectTransform));
                }
            }
        }

        private IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }


        protected void CollectRectChildren()
        {
            rectChildren.Clear();
            List<ILayoutIgnorer> ignoreList = ListPool<ILayoutIgnorer>.Get();
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                RectTransform rectTransform = this.rectTransform.GetChild(i) as RectTransform;
                if (rectTransform == null || !rectTransform.gameObject.activeInHierarchy)
                {
                    continue;
                }

                rectTransform.GetComponents(ignoreList);
                if (ignoreList.Count == 0 || ignoreList.Any((x) => !x.ignoreLayout))
                {
                    rectChildren.Add(rectTransform);
                }
            }

            ListPool<ILayoutIgnorer>.Release(ignoreList);
            m_Tracker.Clear();
        }

        /// <summary>
        ///     Calculate the layout element properties for this layout element along the given
        ///     axis.
        /// </summary>
        /// <param name="axis">The axis to calculate for. 0 is horizontal and 1 is vertical.</param>
        protected void CalcAlongAxis(int axis)
        {
            float padding = 0;
            bool controlSize = true;
            bool childForceExpand = false;
            float totalMin = padding;
            float totalPreferred = padding;
            float totalFlexible = 0f;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                GetChildSizes(child, axis, controlSize, childForceExpand, 
                    out float childMin, out float childPreferred, out float childFlexible);

                totalMin = Mathf.Max(childMin + padding, totalMin);
                totalPreferred = Mathf.Max(childPreferred + padding, totalPreferred);
                totalFlexible = Mathf.Max(childFlexible, totalFlexible);
            }

            totalPreferred = Mathf.Max(totalMin, totalPreferred);
            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
        }

        /// <summary>
        /// Used to set the calculated layout properties for the given axis.
        /// </summary>
        /// <param name="totalMin">The min size for the layout group.</param>
        /// <param name="totalPreferred">The preferred size for the layout group.</param>
        /// <param name="totalFlexible">The flexible size for the layout group.</param>
        /// <param name="axis">The axis to set sizes for. 0 is horizontal and 1 is vertical.</param>
        protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
        {
            m_TotalMinSize      [axis] = totalMin;
            m_TotalPreferredSize[axis] = totalPreferred;
            m_TotalFlexibleSize [axis] = totalFlexible;
        }

        private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand, out float min, out float preferred, out float flexible)
        {
            float sizeInfluence = child.anchorMax[axis] - child.anchorMin[axis];

            if (!controlSize || sizeInfluence <= 0)
            {
                min       = -1f;
                preferred = -1f;
                flexible  = 0f;
            }
            // Fairly certain this isn't needed.
            //else if (!controlSize)
            //{
            //    min       = child.sizeDelta[axis];
            //    preferred = min;
            //    flexible  = 0f;
            //}
            else
            {
                float childMin       = LayoutUtility.GetMinSize      (child, axis);
                float childPreferred = LayoutUtility.GetPreferredSize(child, axis);
                float childFlexible  = LayoutUtility.GetFlexibleSize (child, axis);

                min       = childMin       / sizeInfluence - child.sizeDelta[axis];
                preferred = childPreferred / sizeInfluence - child.sizeDelta[axis];
                flexible  = childFlexible ;
            }

            if (childForceExpand)
            {
                flexible = Mathf.Max(flexible, 1f);
            }
        }



        // Implementing ILayoutGroup means this ILayoutElement is marked for layout rebuild
        // whenever children are marked for rebuild.
        // This component does not actually control the children's layout though,
        // so these methods do nothing.
        void ILayoutController.SetLayoutHorizontal() { }

        void ILayoutController.SetLayoutVertical() { }
    }
}
