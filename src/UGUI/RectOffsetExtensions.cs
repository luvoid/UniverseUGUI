using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UniverseLib.UGUI
{
	internal static class RectOffsetExtensions
	{
		public static RectOffset Negative(this RectOffset rectOffset)
		{
			return new RectOffset(
				-rectOffset.left,
				-rectOffset.right,
				-rectOffset.top,
				-rectOffset.bottom
			);
		}
	}
}
