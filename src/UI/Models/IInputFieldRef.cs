using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniverseLib.UI.Models
{
    public interface IInputFieldRef
    {
        InputField Component { get; }
        GameObject GameObject { get; }
        Text PlaceholderText { get; }
        bool ReachedMaxVerts { get; }
        string Text { get; set; }
        TextGenerator TextGenerator { get; }
        RectTransform Transform { get; }
        GameObject UIRoot { get; }

        event Action<string> OnValueChanged;
    }
}