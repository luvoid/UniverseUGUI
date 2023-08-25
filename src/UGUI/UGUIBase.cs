using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniverseLib;
using UniverseLib.UGUI.Models;
using UniverseLib.UI;

namespace UniverseLib.UGUI
{
    public class UGUIBase : UIBase
    {
        private bool isConstructed = false;
        private readonly Queue<IUniversalUGUIObject> newUGUIObjects = new Queue<IUniversalUGUIObject>();
        private readonly List<IUniversalUGUIObject> uGUIObjects = new List<IUniversalUGUIObject>();

        protected internal UGUIBase(string id, IUniversalUGUIBehaviour[] behaviours)
            : base(id, CreateUpdateCallback(id))
        {
            foreach (var behaviour in behaviours)
            {
                AddBehavior(behaviour);
            }
        }

        internal void AddObject(IUniversalUGUIObject uGUIObject)
        {
            newUGUIObjects.Enqueue(uGUIObject);
        }

        public void AddBehavior(IUniversalUGUIBehaviour behaviour)
        {
            newUGUIObjects.Enqueue(new BehaviourWrapper(this, behaviour));
        }

        private static Action CreateUpdateCallback(string id)
        {
            Action callback = () =>
            {
                var @this = UniversalUI.registeredUIs[id] as UGUIBase;
                @this.OnUpdate();
            };
            return callback;
        }

        private void OnUpdate()
        {
            while (newUGUIObjects.Count > 0)
            {
                var obj = newUGUIObjects.Dequeue();
                ConstructObject(obj);
                uGUIObjects.Add(obj);
            }

            foreach (var obj in uGUIObjects)
            {
                UpdateObject(obj);
            }
        }

        private void ConstructObject(IUniversalUGUIObject uGUIObject)
        {
            InvokeUGUIMethodWithEvents(uGUIObject, uGUIObject.OnUGUIStart, UGUIEventType.InitialLayout, UGUIEventType.InitialRepaint);
            uGUIObject.Models.Clear(); // Models in OnUGUI will be different from the models in OnUGUIStart
        }

        private void UpdateObject(IUniversalUGUIObject uGUIObject)
        {
            InvokeUGUIMethodWithEvents(uGUIObject, uGUIObject.OnUGUI, UGUIEventType.Layout, UGUIEventType.Repaint);

            //UGUIUtility.BeginUGUI(uGUIObject, EventType.Layout);
            //try
            //{
            //	uGUIObject.OnUGUIUpdate();
            //}
            //catch (Exception ex)
            //{
            //	Universe.LogException(ex);
            //}
            //UGUIUtility.EndUGUI();
        }

        private static void InvokeUGUIMethodWithEvents(IUniversalUGUIObject uGUIObject, Action method, params UGUIEventType[] eventTypes)
        {
            try
            {
                foreach (var type in eventTypes)
                {
                    UGUIUtility.BeginUGUI(uGUIObject, type);
                    method.Invoke();
                    UGUIUtility.EndUGUI();
                }
            }
            catch (Exception ex)
            {
                if (!UGUIUtility.EndUGUIFromException(ex))
                    Universe.LogError(ex);
            }
        }


        private class BehaviourWrapper : IUniversalUGUIObject
        {
            private readonly UGUIBase owner;
            private readonly IUniversalUGUIBehaviour behaviour;

            public BehaviourWrapper(UGUIBase owner, IUniversalUGUIBehaviour behaviour)
            {
                if (owner == null) throw new ArgumentNullException(nameof(owner));
                if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

                this.owner = owner;
                this.behaviour = behaviour;
            }

            bool IUniversalUGUIObject.ActiveInHierarchy => behaviour.isActiveAndEnabled;
            bool IUniversalUGUIObject.UseUGUILayout => behaviour.useGUILayout;
            UGUIBase IUniversalUGUIObject.Owner => owner;
            GameObject IUniversalUGUIObject.ContentRoot => owner.RootObject;
            Dictionary<int, UGUIModel> IUniversalUGUIObject.Models { get; } = new();

            int IUniversalUGUIObject.GetInstanceID()
                => behaviour.GetInstanceID();
            void IUniversalUGUIObject.OnUGUI()
                => behaviour.OnUGUI();
            void IUniversalUGUIObject.OnUGUIStart()
                => behaviour.OnUGUIStart();
        }
    }

}
