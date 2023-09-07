﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UGUI;

namespace UniverseLib.UI
{
	public sealed partial class UIFactory
	{
		/// <summary>
		/// Create a simple UI Object that populates itself with content generated by UGUI functions.
		/// <br /><br />See also: <see cref="UGUI.Panels.UGUIPanelBase"/>
		/// </summary>
		/// <param name="name">The name of the GameObject, useful for debugging purposes</param>
		/// <param name="parent">The parent GameObject to attach this to</param>
		/// <param name="owner">The owner of the UGUIObject for registering callbacks</param>
		/// <param name="onUGUIStart">The function that generates the UGUI at the start</param>
		/// <param name="onUGUI">The function that generates and updates the UGUI every frame</param>
		/// <param name="sizeDelta">The size of the object. If null, will be automatically determine size and layout by the UGUI content</param>
		/// <returns>The base GameObject that will contain the UGUI.</returns>
		public static GameObject CreateUGUIObject(string name, GameObject parent, UGUIBase owner, UnityAction onUGUIStart = null, UnityAction onUGUI = null,
			 Vector2? sizeDelta = null)
		{
			bool autoSize = sizeDelta.HasValue;
			GameObject obj = CreateUIObject(name, parent, sizeDelta ?? default);

			var behaviour = obj.AddComponent<UGUIMonoBehaviour>();
			behaviour.OnUGUIStart = onUGUIStart;
			behaviour.OnUGUI = onUGUI;

			Action postUGUI = !autoSize ? null : () =>
			{
				if (!behaviour.useGUILayout) return;
				UIFactory.SetLayoutElement(obj,
					minWidth: (int)UGUILayoutUtility.topLevel.minWidth,
					minHeight: (int)UGUILayoutUtility.topLevel.minHeight,
					preferredWidth: (int)UGUILayoutUtility.topLevel.maxWidth,
					preferredHeight: (int)UGUILayoutUtility.topLevel.maxHeight
				);
			};

			var uguiObj = new UGUIWrapperObject(owner, behaviour, obj, postUGUI);
			owner.AddObject(uguiObj);

			return obj;
		}
	}
}