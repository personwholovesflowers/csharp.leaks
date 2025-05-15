using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public static class VectorUtility
{
	// Token: 0x06001B20 RID: 6944 RVA: 0x000E1E58 File Offset: 0x000E0058
	public static Vector3 SmoothStep(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t), Mathf.SmoothStep(from.z, to.z, t));
	}
}
