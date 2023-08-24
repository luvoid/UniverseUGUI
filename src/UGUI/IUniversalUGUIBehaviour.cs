using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.UGUI
{
    public interface IUniversalUGUIBehaviour
    {
        public bool isActiveAndEnabled { get; }
        public bool useGUILayout { get; }

        public int GetInstanceID();
        public void OnUGUI();
        public void OnUGUIStart();
    }
}
