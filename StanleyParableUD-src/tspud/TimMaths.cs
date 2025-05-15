using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class TimMaths
{
	// Token: 0x06000467 RID: 1127 RVA: 0x0001A154 File Offset: 0x00018354
	public static float Vector3InverseLerp(Vector3 start, Vector3 end, Vector3 value)
	{
		Vector3 vector = end - start;
		return Mathf.Clamp01(Vector3.Dot(value - start, vector) / Vector3.Dot(vector, vector));
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001A183 File Offset: 0x00018383
	public static float SphereRandom()
	{
		return Mathf.Acos(Random.Range(-1f, 1f)) * 57.29578f - 90f;
	}
}
