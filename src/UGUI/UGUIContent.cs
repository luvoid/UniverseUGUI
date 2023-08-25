using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI
{
    public class UGUIContent : GUIContent
    {
        internal static readonly new UGUIContent none;

        public UGUIContent()
        { }

        public UGUIContent(string text)
            : base(text)
        { }

        public UGUIContent(Texture image)
            : base(image)
        { }

        public UGUIContent(string text, Texture image)
            : base(text, image)
        { }

        public UGUIContent(string text, string tooltip)
            : base(text, tooltip)
        { }

        public UGUIContent(Texture image, string tooltip)
            : base(image, tooltip)
        { }

        public UGUIContent(string text, Texture image, string tooltip)
            : base(text, image, tooltip)
        { }

        public UGUIContent(GUIContent src)
            : base(src)
        { }

        public static implicit operator UGUIContent(string label)
        {
            return new UGUIContent(label);
        }

        public static implicit operator UGUIContent(Texture image)
        {
            return new UGUIContent(image);
        }

        public static UGUIContent[] Cast(string[] labels)
        {
            UGUIContent[] uiContents = new UGUIContent[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                uiContents[i] = labels[i];
            }
            return uiContents;
        }

        public static UGUIContent[] Cast(Texture[] images)
        {
            UGUIContent[] uiContents = new UGUIContent[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                uiContents[i] = images[i];
            }
            return uiContents;
        }
    }
}
