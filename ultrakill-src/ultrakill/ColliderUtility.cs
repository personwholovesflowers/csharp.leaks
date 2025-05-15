using System;
using System.Collections.Generic;
using NewBlood.Rendering;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020000CB RID: 203
public static class ColliderUtility
{
	// Token: 0x06000409 RID: 1033 RVA: 0x0001BF54 File Offset: 0x0001A154
	private static Triangle<Vector3> GetTriangle(int index)
	{
		Vector3 vector = ColliderUtility.s_Vertices[ColliderUtility.s_Triangles[3 * index]];
		Vector3 vector2 = ColliderUtility.s_Vertices[ColliderUtility.s_Triangles[3 * index + 1]];
		Vector3 vector3 = ColliderUtility.s_Vertices[ColliderUtility.s_Triangles[3 * index + 2]];
		return new Triangle<Vector3>(vector, vector2, vector3);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001BFB3 File Offset: 0x0001A1B3
	private static Plane GetTrianglePlane(Transform collider, int index)
	{
		return ColliderUtility.GetTrianglePlane(ColliderUtility.GetTriangle(index));
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001BFC0 File Offset: 0x0001A1C0
	private static Plane GetTrianglePlane(Triangle<Vector3> source)
	{
		return new Plane(source.Index0, source.Index1, source.Index2);
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0001BFDC File Offset: 0x0001A1DC
	private static bool InTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = c - a;
		Vector3 vector3 = p - a;
		float num = Vector3.Dot(vector, vector);
		float num2 = Vector3.Dot(vector, vector2);
		float num3 = Vector3.Dot(vector, vector3);
		float num4 = Vector3.Dot(vector2, vector2);
		float num5 = Vector3.Dot(vector2, vector3);
		float num6 = 1f / (num * num4 - num2 * num2);
		float num7 = (num4 * num3 - num2 * num5) * num6;
		float num8 = (num * num5 - num2 * num3) * num6;
		return num7 >= 0f && num8 >= 0f && num7 + num8 < 1f;
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0001C077 File Offset: 0x0001A277
	public static Vector3 FindClosestPoint(Collider collider, Vector3 position)
	{
		return ColliderUtility.FindClosestPoint(collider, position, false);
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0001C084 File Offset: 0x0001A284
	public static Vector3 FindClosestPoint(Collider collider, Vector3 position, bool ignoreVerticalTriangles)
	{
		if (NonConvexJumpDebug.Active)
		{
			NonConvexJumpDebug.Reset();
		}
		MeshCollider meshCollider = collider as MeshCollider;
		if (meshCollider != null && !meshCollider.convex)
		{
			Mesh sharedMesh = meshCollider.sharedMesh;
			sharedMesh.GetVertices(ColliderUtility.s_Vertices);
			Vector3 vector = Vector3.zero;
			float num = float.PositiveInfinity;
			Transform transform = meshCollider.transform;
			position = transform.InverseTransformPoint(position);
			Vector3 vector2 = transform.InverseTransformDirection(Vector3.up);
			for (int i = 0; i < sharedMesh.subMeshCount; i++)
			{
				sharedMesh.GetTriangles(ColliderUtility.s_Triangles, i);
				int j = 0;
				int num2 = ColliderUtility.s_Triangles.Count / 3;
				while (j < num2)
				{
					Triangle<Vector3> triangle = ColliderUtility.GetTriangle(j);
					Vector3 vector3 = Vector3.Normalize(Vector3.Cross(triangle.Index1 - triangle.Index0, triangle.Index2 - triangle.Index0));
					float num3 = 0f - Vector3.Dot(vector3, triangle.Index0);
					if (!ignoreVerticalTriangles || Mathf.Abs(Vector3.Dot(vector3, vector2)) < 0.9f)
					{
						float num4 = Vector3.Dot(vector3, position) + num3;
						float num5 = Mathf.Abs(num4);
						if (num5 < num)
						{
							Vector3 vector4 = position - vector3 * num4;
							bool flag = ColliderUtility.InTriangle(triangle.Index0, triangle.Index1, triangle.Index2, vector4);
							if (NonConvexJumpDebug.Active)
							{
								Triangle<Vector3> triangle2 = new Triangle<Vector3>(transform.TransformPoint(triangle.Index0), transform.TransformPoint(triangle.Index1), transform.TransformPoint(triangle.Index2));
								NonConvexJumpDebug.CreateTri(vector3, triangle2, flag ? new Color(0f, 0f, 1f) : new Color(0f, 0f, Random.Range(0.1f, 0.4f)));
							}
							if (flag)
							{
								vector = vector4;
								num = num5;
							}
						}
					}
					j++;
				}
			}
			vector = transform.TransformPoint(vector);
			if (NonConvexJumpDebug.Active)
			{
				NonConvexJumpDebug.CreateBall(Color.green, vector, 1f);
			}
			return vector;
		}
		return collider.ClosestPoint(position);
	}

	// Token: 0x040004F8 RID: 1272
	private static readonly List<Vector3> s_Vertices = new List<Vector3>();

	// Token: 0x040004F9 RID: 1273
	private static readonly List<int> s_Triangles = new List<int>();
}
