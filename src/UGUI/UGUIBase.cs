using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniverseLib;
using UniverseLib.UGUI.ImplicitTypes;
using UniverseLib.UGUI.Models;
using UniverseLib.UI;

namespace UniverseLib.UGUI
{
    public class UGUIBase : UIBase
    {
        /// <summary>
        /// The default skin used by <see cref="IUniversalUGUIBehaviour"/>s assigned to this <see cref="UGUIBase"/>.
        /// </summary>
        public UGUISkin Skin = null;

        private bool isConstructed = false;
        private readonly Queue<IUniversalUGUIObject> newUGUIObjects = new Queue<IUniversalUGUIObject>();
        private readonly List<IUniversalUGUIObject> uGUIObjects = new List<IUniversalUGUIObject>();
        private readonly Action earlyUpdateMethod = null;

        protected internal UGUIBase(string id, Action earlyUpdateMethod, IUniversalUGUIBehaviour[] behaviours)
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
            newUGUIObjects.Enqueue(new UGUIWrapperObject(this, behaviour, RootObject));
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
            try
            {
                earlyUpdateMethod?.Invoke();
            }
            catch (Exception ex)
            {
                Universe.LogError($"Exception invoking early update method for {ID}: {ex}");
            }

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

        private void InvokeUGUIMethodWithEvents(IUniversalUGUIObject uGUIObject, Action method, params UGUIEventType[] eventTypes)
        {
            try
            {
                foreach (var type in eventTypes)
                {
                    UGUIUtility.BeginUGUI(uGUIObject, type);
                    UGUI.skin = Skin;
                    method.Invoke();
                    UGUIUtility.EndUGUI();
                }
            }
            catch (Exception ex)
            {
                if (!UGUIUtility.EndUGUIFromException(ex))
                    Universe.LogError($"Exception invoking OnUGUI / OnUGUIStart for {ID} {uGUIObject}: {ex}");
            }
        }
    }
}
