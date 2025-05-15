using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000214 RID: 532
	public static class z_ComponentIndexUtility
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x000363AE File Offset: 0x000345AE
		public static float GetValueAtIndex(this Vector3 value, z_ComponentIndex index)
		{
			switch (index)
			{
			case z_ComponentIndex.R:
				return value.x;
			case z_ComponentIndex.G:
				return value.y;
			case z_ComponentIndex.B:
				return value.z;
			default:
				return 0f;
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x000363DE File Offset: 0x000345DE
		public static float GetValueAtIndex(this Vector4 value, z_ComponentIndex index)
		{
			switch (index)
			{
			case z_ComponentIndex.R:
				return value.x;
			case z_ComponentIndex.G:
				return value.y;
			case z_ComponentIndex.B:
				return value.z;
			case z_ComponentIndex.A:
				return value.w;
			default:
				return 0f;
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00036419 File Offset: 0x00034619
		public static float GetValueAtIndex(this Color value, z_ComponentIndex index)
		{
			switch (index)
			{
			case z_ComponentIndex.R:
				return value.r;
			case z_ComponentIndex.G:
				return value.g;
			case z_ComponentIndex.B:
				return value.b;
			case z_ComponentIndex.A:
				return value.a;
			default:
				return 0f;
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00036454 File Offset: 0x00034654
		public static uint ToFlag(this z_ComponentIndex e)
		{
			int num = (int)(e + 1);
			if (num < 3)
			{
				return (uint)num;
			}
			if (num != 3)
			{
				return 8U;
			}
			return 4U;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00036474 File Offset: 0x00034674
		public static string GetString(this z_ComponentIndex component, z_ComponentIndexType type = z_ComponentIndexType.Vector)
		{
			int num = (int)component;
			if (type == z_ComponentIndexType.Vector)
			{
				if (num == 0)
				{
					return "X";
				}
				if (num == 1)
				{
					return "Y";
				}
				if (num != 2)
				{
					return "W";
				}
				return "Z";
			}
			else
			{
				if (type != z_ComponentIndexType.Color)
				{
					return num.ToString();
				}
				if (num == 0)
				{
					return "R";
				}
				if (num == 1)
				{
					return "G";
				}
				if (num != 2)
				{
					return "A";
				}
				return "B";
			}
		}

		// Token: 0x04000B8F RID: 2959
		public static readonly GUIContent[] ComponentIndexPopupDescriptions = new GUIContent[]
		{
			new GUIContent("X (R)"),
			new GUIContent("Y (G)"),
			new GUIContent("Z (B)"),
			new GUIContent("W (A)")
		};

		// Token: 0x04000B90 RID: 2960
		public static readonly int[] ComponentIndexPopupValues = new int[] { 0, 1, 2, 3 };
	}
}
