using System;
using UnityEngine;

namespace Train
{
	// Token: 0x0200051F RID: 1311
	public static class PathTools
	{
		// Token: 0x06001DDF RID: 7647 RVA: 0x000F91E0 File Offset: 0x000F73E0
		public static Vector3 InterpolateAlongCircle(Vector3 start, Vector3 end, Vector3 center, float t)
		{
			Vector3 vector = start - center;
			Vector3 vector2 = end - center;
			Vector3 vector3 = Vector3.Slerp(vector, vector2, t);
			return center + vector3;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000F920C File Offset: 0x000F740C
		public static Vector3 ComputeSphericalCurveCenter(Vector3 start, Vector3 end, bool reverse = false, float angle = 45f)
		{
			Vector3 vector = (start + end) * 0.5f;
			Vector3 vector2 = vector - start;
			Vector3 vector3 = end - start;
			bool flag = vector3.x * vector2.z - vector3.z * vector2.x < 0f;
			if (reverse)
			{
				flag = !flag;
			}
			Vector3 vector4 = (flag ? new Vector3(0f, -1f, 0f) : new Vector3(0f, 1f, 0f));
			return vector + (Quaternion.AngleAxis(90f, vector4) * (start - vector)).normalized * (start - vector).magnitude / Mathf.Tan(0.017453292f * angle / 2f);
		}
	}
}
