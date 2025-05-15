﻿using System;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000311 RID: 785
	public static class BoundsEx
	{
		// Token: 0x06001402 RID: 5122 RVA: 0x0006A8F0 File Offset: 0x00068AF0
		public static Bounds Transform(this Bounds bounds, Matrix4x4 matrix)
		{
			Vector3 vector = matrix.MultiplyPoint3x4(bounds.center);
			Vector3 extents = bounds.extents;
			Vector3 vector2 = matrix.MultiplyVector(new Vector3(extents.x, 0f, 0f));
			Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, extents.y, 0f));
			Vector3 vector4 = matrix.MultiplyVector(new Vector3(0f, 0f, extents.z));
			extents.x = Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x) + Mathf.Abs(vector4.x);
			extents.y = Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y) + Mathf.Abs(vector4.y);
			extents.z = Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z) + Mathf.Abs(vector4.z);
			return new Bounds
			{
				center = vector,
				extents = extents
			};
		}
	}
}
