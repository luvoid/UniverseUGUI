using UnityEngine;
using UniverseLib.UGUI.Collections.Generic;
using UniverseLib.UGUI.ImplicitTypes;
using UniverseLib.UI.Styles;

namespace UniverseLib.UGUI
{
    public abstract class UGUISkin
    {
        private static readonly Hashtable<IReadOnlyUISkin, ConvertedUISkin> s_ConvertedUISkinCache = new();
        private static readonly Hashtable<GUISkin, ConvertedGUISkin> s_ConvertedGUISkinCache = new();

        public static explicit operator UGUISkin(UISkin uiSkin)
        {
            if (uiSkin == null) return null;

            if (!s_ConvertedUISkinCache.TryGetValue(uiSkin, out ConvertedUISkin uguiSkin))
            {
                uguiSkin = new ConvertedUISkin(uiSkin);
            }
            return uguiSkin;
        }

        public static explicit operator UGUISkin(ReadOnlyUISkin uiSkin)
        {
            if (uiSkin == null) return null;

            if (!s_ConvertedUISkinCache.TryGetValue(uiSkin, out ConvertedUISkin uguiSkin))
            {
                uguiSkin = new ConvertedUISkin(uiSkin);
            }
            return uguiSkin;
        }

        public static explicit operator UGUISkin(GUISkin guiSkin)
        {
            if (guiSkin == null) return null;

            if (!s_ConvertedGUISkinCache.TryGetValue(guiSkin, out ConvertedGUISkin uguiSkin))
            {
                uguiSkin = new ConvertedGUISkin(guiSkin);
            }
            return uguiSkin;
        }

        internal abstract string    Name      { get; }
        internal abstract Font      Font      { get; }
        internal abstract UGUIStyle Box       { get; }
        internal abstract UGUIStyle Button    { get; }
        internal abstract UGUIStyle Toggle    { get; }
        internal abstract UGUIStyle Label     { get; }
        internal abstract UGUIStyle TextField { get; }
        internal abstract UGUIStyle TextArea  { get; }
        internal abstract UGUIStyle Window    { get; }

        private class ConvertedUISkin : UGUISkin
        {
            private readonly IReadOnlyUISkin skin;
            public ConvertedUISkin(IReadOnlyUISkin skin)
            {
                if (skin == null) throw new System.ArgumentNullException(nameof(skin));
                this.skin = skin;
            }
            internal override string    Name      => skin.Name            ;
            internal override Font      Font      => skin.Text.Font       ;
            internal override UGUIStyle Box       => skin.Box             ;
            internal override UGUIStyle Button    => skin.Button          ;
            internal override UGUIStyle Toggle    => skin.Toggle          ;
            internal override UGUIStyle Label     => skin.Label           ;
            internal override UGUIStyle TextField => skin.InputField      ;
            internal override UGUIStyle TextArea  => skin.ScrollInputField;
            internal override UGUIStyle Window    => skin.Window          ;
        }

        private class ConvertedGUISkin : UGUISkin
        {
            private readonly GUISkin skin;
            public ConvertedGUISkin(GUISkin skin)
            {
                if (skin == null) throw new System.ArgumentNullException(nameof(skin));
                this.skin = skin;
            }
            internal override string    Name      => skin.name     ;
            internal override Font      Font      => skin.font     ;
            internal override UGUIStyle Box       => skin.box      ;
            internal override UGUIStyle Button    => skin.button   ;
            internal override UGUIStyle Toggle    => skin.toggle   ;
            internal override UGUIStyle Label     => skin.label    ;
            internal override UGUIStyle TextField => skin.textField;
            internal override UGUIStyle TextArea  => skin.textArea ;
            internal override UGUIStyle Window    => skin.window   ;
        }
    }
}
