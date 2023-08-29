using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniverseLib.UGUI
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles",
        Justification = "Names match UnityEngine.MonoBehaviour properties so they don't need to be manually implemented.")]
    public interface IUniversalUGUIBehaviour
    {
        public string name { get; }
        public bool isActiveAndEnabled { get; }
        public bool useGUILayout { get; }

        public int GetInstanceID();
        public void OnUGUI();
        public void OnUGUIStart();
    }
}
