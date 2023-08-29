using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace UniverseLib.UGUI
{
    /// <summary>
    /// An implementatio of <see cref="IUniversalUGUIBehaviour"/> using a <see cref="MonoBehaviour"/>
    /// </summary>
    internal class UGUIMonoBehaviour : MonoBehaviour, IUniversalUGUIBehaviour
    {
        public UnityAction OnUGUI;
        public UnityAction OnUGUIStart;

        void IUniversalUGUIBehaviour.OnUGUI()
        {
            throw new NotImplementedException();
        }

        void IUniversalUGUIBehaviour.OnUGUIStart()
        {
            throw new NotImplementedException();
        }
    }
}
