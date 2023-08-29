using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI
{
    public static class GUISkinExtensions
    {
        public static GUISkin Copy(this GUISkin skin)
        {
            var newSkin = RuntimeHelper.CreateScriptable<GUISkin>();

            newSkin.name = skin.name;
            newSkin.font = skin.font;

            newSkin.box                            = new GUIStyle(skin.box                           );
            newSkin.button                         = new GUIStyle(skin.button                        );
            newSkin.toggle                         = new GUIStyle(skin.toggle                        );
            newSkin.label                          = new GUIStyle(skin.label                         );
            newSkin.textField                      = new GUIStyle(skin.textField                     );
            newSkin.textArea                       = new GUIStyle(skin.textArea                      );
            newSkin.window                         = new GUIStyle(skin.window                        );
            newSkin.horizontalSlider               = new GUIStyle(skin.horizontalSlider              );
            newSkin.horizontalSliderThumb          = new GUIStyle(skin.horizontalSliderThumb         );
            newSkin.verticalSlider                 = new GUIStyle(skin.verticalSlider                );
            newSkin.verticalSliderThumb            = new GUIStyle(skin.verticalSliderThumb           );
            newSkin.horizontalScrollbar            = new GUIStyle(skin.horizontalScrollbar           );
            newSkin.horizontalScrollbarThumb       = new GUIStyle(skin.horizontalScrollbarThumb      );
            newSkin.horizontalScrollbarLeftButton  = new GUIStyle(skin.horizontalScrollbarLeftButton );
            newSkin.horizontalScrollbarRightButton = new GUIStyle(skin.horizontalScrollbarRightButton);
            newSkin.verticalScrollbar              = new GUIStyle(skin.verticalScrollbar             );
            newSkin.verticalScrollbarThumb         = new GUIStyle(skin.verticalScrollbarThumb        );
            newSkin.verticalScrollbarUpButton      = new GUIStyle(skin.verticalScrollbarUpButton     );
            newSkin.verticalScrollbarDownButton    = new GUIStyle(skin.verticalScrollbarDownButton   );
            newSkin.scrollView                     = new GUIStyle(skin.scrollView                    );

            var newCustomStyles = new GUIStyle[skin.customStyles.Length]; 
            for (int i = 0; i < skin.customStyles.Length; i++)
            {
                newCustomStyles[i] = new GUIStyle(skin.customStyles[i]);
            }
            newSkin.customStyles = newCustomStyles;

            newSkin.settings.doubleClickSelectsWord = skin.settings.doubleClickSelectsWord;
            newSkin.settings.tripleClickSelectsLine = skin.settings.tripleClickSelectsLine;
            newSkin.settings.cursorColor            = skin.settings.cursorColor           ;
            newSkin.settings.cursorFlashSpeed       = skin.settings.cursorFlashSpeed      ;
            newSkin.settings.selectionColor         = skin.settings.selectionColor        ;

            return newSkin;
        }
    }
}
