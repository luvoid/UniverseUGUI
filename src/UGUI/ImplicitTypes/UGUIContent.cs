using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI.ImplicitTypes
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles",
        Justification = "Unity's naming style must be preserved for backwards compatibility with IMGUI users.")]
    public ref struct UGUIContent
    {
        //internal GUIContent GUIContent = null;

        public string text = string.Empty;
        public Texture image = null;
        public string tooltip = string.Empty;
        public int hash
        {
            get
            {
                int result = 0;
                if (!string.IsNullOrEmpty(text))
                {
                    result = text.GetHashCode() * 37;
                }
                return result;
            }
        }

        public UGUIContent()
        { }

        public UGUIContent(string text, Texture image = null, string tooltip = "")
        {
            this.text = text;
            this.image = image;
            this.tooltip = tooltip;
        }

        public UGUIContent(Texture image, string tooltip = "")
        {
            this.image = image;
            this.tooltip = tooltip;
        }

        public UGUIContent(string text, string tooltip)
        {
            this.text = text;
            this.tooltip = tooltip;
        }

        public static implicit operator UGUIContent(string text)
        {
            return new UGUIContent(text);
        }

        public static implicit operator UGUIContent(Texture image)
        {
            return new UGUIContent(image);
        }

        //public static UGUIContent[] Cast(string[] labels)
        //{
        //    UGUIContent[] uiContents = new UGUIContent[labels.Length];
        //    for (int i = 0; i < labels.Length; i++)
        //    {
        //        uiContents[i] = labels[i];
        //    }
        //    return uiContents;
        //}
        //
        //public static UGUIContent[] Cast(Texture[] images)
        //{
        //    UGUIContent[] uiContents = new UGUIContent[images.Length];
        //    for (int i = 0; i < images.Length; i++)
        //    {
        //        uiContents[i] = images[i];
        //    }
        //    return uiContents;
        //}
    }
}
