using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x0200022F RID: 559
	public static class z_Vector
	{
		// Token: 0x06000CBD RID: 3261 RVA: 0x0003ABB0 File Offset: 0x00038DB0
		public static void Cross(Vector3 a, Vector3 b, ref float x, ref float y, ref float z)
		{
			x = a.y * b.z - a.z * b.y;
			y = a.z * b.x - a.x * b.z;
			z = a.x * b.y - a.y * b.x;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003AC18 File Offset: 0x00038E18
		public static void Cross(Vector3 a, Vector3 b, ref Vector3 res)
		{
			res.x = a.y * b.z - a.z * b.y;
			res.y = a.z * b.x - a.x * b.z;
			res.z = a.x * b.y - a.y * b.x;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003AC88 File Offset: 0x00038E88
		public static void Cross(float ax, float ay, float az, float bx, float by, float bz, ref float x, ref float y, ref float z)
		{
			x = ay * bz - az * by;
			y = az * bx - ax * bz;
			z = ax * by - ay * bx;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003ACAC File Offset: 0x00038EAC
		public static void Subtract(Vector3 a, Vector3 b, ref Vector3 res)
		{
			res.x = b.x - a.x;
			res.y = b.y - a.y;
			res.z = b.z - a.z;
		}
	}
}
