using System;
using UniverseLib.UI;

namespace UniverseLib.UGUI
{
    public static class UniversalUGUI
    {
        /// <inheritdoc cref="UniversalUI.RegisterUI"/>
        public static UGUIBase RegisterUGUI(string id, params IUniversalUGUIBehaviour[] behaviours)
        {
            return new UGUIBase(id, behaviours);
        }

        /// <inheritdoc cref="UniversalUI.RegisterUI{T}"/>
        public static T RegisterUGUI<T>(string id, params IUniversalUGUIBehaviour[] behaviours)
            where T : UGUIBase
        {
            return (T)Activator.CreateInstance(typeof(T), id, behaviours);
        }

    }
}
