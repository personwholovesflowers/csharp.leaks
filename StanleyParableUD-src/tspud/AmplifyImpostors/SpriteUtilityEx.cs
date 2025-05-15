using System;
using System.Reflection;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000313 RID: 787
	public static class SpriteUtilityEx
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x0006AD51 File Offset: 0x00068F51
		public static Type Type
		{
			get
			{
				if (!(SpriteUtilityEx.type == null))
				{
					return SpriteUtilityEx.type;
				}
				return SpriteUtilityEx.type = Type.GetType("UnityEditor.Sprites.SpriteUtility, UnityEditor");
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0006AD78 File Offset: 0x00068F78
		public static void GenerateOutline(Texture2D texture, Rect rect, float detail, byte alphaTolerance, bool holeDetection, out Vector2[][] paths)
		{
			Vector2[][] array = new Vector2[0][];
			object[] array2 = new object[] { texture, rect, detail, alphaTolerance, holeDetection, array };
			SpriteUtilityEx.Type.GetMethod("GenerateOutline", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, array2);
			paths = (Vector2[][])array2[5];
		}

		// Token: 0x04001016 RID: 4118
		private static Type type;
	}
}
