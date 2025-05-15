using System;
using UnityEngine;

// Token: 0x02000048 RID: 72
public static class CameraUtility
{
	// Token: 0x0600017E RID: 382 RVA: 0x0000A338 File Offset: 0x00008538
	public static bool VisibleFromCamera(Renderer renderer, Camera camera)
	{
		return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A34C File Offset: 0x0000854C
	public static bool BoundsOverlap(MeshFilter nearObject, MeshFilter farObject, Camera camera)
	{
		CameraUtility.MinMax3D screenRectFromBounds = CameraUtility.GetScreenRectFromBounds(nearObject, camera);
		CameraUtility.MinMax3D screenRectFromBounds2 = CameraUtility.GetScreenRectFromBounds(farObject, camera);
		return screenRectFromBounds2.zMax > screenRectFromBounds.zMin && screenRectFromBounds2.xMax >= screenRectFromBounds.xMin && screenRectFromBounds2.xMin <= screenRectFromBounds.xMax && screenRectFromBounds2.yMax >= screenRectFromBounds.yMin && screenRectFromBounds2.yMin <= screenRectFromBounds.yMax;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A3B8 File Offset: 0x000085B8
	public static CameraUtility.MinMax3D GetScreenRectFromBounds(MeshFilter renderer, Camera mainCamera)
	{
		CameraUtility.MinMax3D minMax3D = new CameraUtility.MinMax3D(float.MaxValue, float.MinValue);
		new Vector3[8];
		Bounds bounds = renderer.sharedMesh.bounds;
		bool flag = false;
		for (int i = 0; i < 8; i++)
		{
			Vector3 vector = bounds.center + Vector3.Scale(bounds.extents, CameraUtility.cubeCornerOffsets[i]);
			Vector3 vector2 = renderer.transform.TransformPoint(vector);
			Vector3 vector3 = mainCamera.WorldToViewportPoint(vector2);
			if (vector3.z > 0f)
			{
				flag = true;
			}
			else
			{
				vector3.x = (float)((vector3.x <= 0.5f) ? 1 : 0);
				vector3.y = (float)((vector3.y <= 0.5f) ? 1 : 0);
			}
			minMax3D.AddPoint(vector3);
		}
		if (!flag)
		{
			return default(CameraUtility.MinMax3D);
		}
		return minMax3D;
	}

	// Token: 0x040001C7 RID: 455
	private static readonly Vector3[] cubeCornerOffsets = new Vector3[]
	{
		new Vector3(1f, 1f, 1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(-1f, -1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, -1f, 1f)
	};

	// Token: 0x02000362 RID: 866
	public struct MinMax3D
	{
		// Token: 0x060015E3 RID: 5603 RVA: 0x00074B96 File Offset: 0x00072D96
		public MinMax3D(float min, float max)
		{
			this.xMin = min;
			this.xMax = max;
			this.yMin = min;
			this.yMax = max;
			this.zMin = min;
			this.zMax = max;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00074BC4 File Offset: 0x00072DC4
		public void AddPoint(Vector3 point)
		{
			this.xMin = Mathf.Min(this.xMin, point.x);
			this.xMax = Mathf.Max(this.xMax, point.x);
			this.yMin = Mathf.Min(this.yMin, point.y);
			this.yMax = Mathf.Max(this.yMax, point.y);
			this.zMin = Mathf.Min(this.zMin, point.z);
			this.zMax = Mathf.Max(this.zMax, point.z);
		}

		// Token: 0x0400123F RID: 4671
		public float xMin;

		// Token: 0x04001240 RID: 4672
		public float xMax;

		// Token: 0x04001241 RID: 4673
		public float yMin;

		// Token: 0x04001242 RID: 4674
		public float yMax;

		// Token: 0x04001243 RID: 4675
		public float zMin;

		// Token: 0x04001244 RID: 4676
		public float zMax;
	}
}
