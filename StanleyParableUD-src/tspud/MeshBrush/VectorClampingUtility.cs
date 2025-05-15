using System;
using UnityEngine;

namespace MeshBrush
{
	// Token: 0x02000259 RID: 601
	public static class VectorClampingUtility
	{
		// Token: 0x06000E4C RID: 3660 RVA: 0x00045640 File Offset: 0x00043840
		public static void ClampVector(ref Vector2 vector, float minX, float maxX, float minY, float maxY)
		{
			vector.x = Mathf.Clamp(vector.x, minX, maxX);
			vector.y = Mathf.Clamp(vector.y, minY, maxY);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00045669 File Offset: 0x00043869
		public static void ClampVector(ref Vector3 vector, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
		{
			vector.x = Mathf.Clamp(vector.x, minX, maxX);
			vector.y = Mathf.Clamp(vector.y, minY, maxY);
			vector.z = Mathf.Clamp(vector.z, minZ, maxZ);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000456A8 File Offset: 0x000438A8
		public static void ClampVector(ref Vector4 vector, float minX, float maxX, float minY, float maxY, float minZ, float maxZ, float minW, float maxW)
		{
			vector.x = Mathf.Clamp(vector.x, minX, maxX);
			vector.y = Mathf.Clamp(vector.y, minY, maxY);
			vector.z = Mathf.Clamp(vector.z, minZ, maxZ);
			vector.w = Mathf.Clamp(vector.w, minW, maxW);
		}
	}
}
