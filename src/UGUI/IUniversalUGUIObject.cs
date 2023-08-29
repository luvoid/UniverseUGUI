using System.Collections.Generic;
using UnityEngine;
using UniverseLib.UGUI.Models;

namespace UniverseLib.UGUI
{
    internal interface IUniversalUGUIObject
    {
        public string Name { get; }
        public bool ActiveInHierarchy { get; }
        public bool UseUGUILayout { get; }
        public UGUIBase Owner { get; }
        public GameObject ContentRoot { get; }
        public Dictionary<int, UGUIModel> Models { get; }

        public int GetInstanceID();
        public void OnUGUIStart();
        public void OnUGUI();
    }
}
