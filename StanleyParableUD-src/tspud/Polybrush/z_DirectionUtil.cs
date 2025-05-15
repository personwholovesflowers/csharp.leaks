using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000217 RID: 535
	public static class z_DirectionUtil
	{
		// Token: 0x06000C2B RID: 3115 RVA: 0x0003653A File Offset: 0x0003473A
		public static Vector3 ToVector3(this z_Direction dir)
		{
			switch (dir)
			{
			case z_Direction.Up:
				return Vector3.up;
			case z_Direction.Right:
				return Vector3.right;
			case z_Direction.Forward:
				return Vector3.forward;
			default:
				return Vector3.zero;
			}
		}
	}
}
