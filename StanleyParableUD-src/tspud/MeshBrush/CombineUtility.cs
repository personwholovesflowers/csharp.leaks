using System;
using UnityEngine;

namespace MeshBrush
{
	// Token: 0x02000255 RID: 597
	public static class CombineUtility
	{
		// Token: 0x06000E3A RID: 3642 RVA: 0x000438C0 File Offset: 0x00041AC0
		public static Mesh Combine(CombineUtility.MeshInstance[] combines, bool generateStrips)
		{
			CombineUtility.vertexCount = 0;
			CombineUtility.triangleCount = 0;
			CombineUtility.stripCount = 0;
			foreach (CombineUtility.MeshInstance meshInstance in combines)
			{
				if (meshInstance.mesh != null)
				{
					CombineUtility.vertexCount += meshInstance.mesh.vertexCount;
					if (generateStrips)
					{
						CombineUtility.curStripCount = meshInstance.mesh.GetTriangles(meshInstance.subMeshIndex).Length;
						if (CombineUtility.curStripCount != 0)
						{
							if (CombineUtility.stripCount != 0)
							{
								if ((CombineUtility.stripCount & 1) == 1)
								{
									CombineUtility.stripCount += 3;
								}
								else
								{
									CombineUtility.stripCount += 2;
								}
							}
							CombineUtility.stripCount += CombineUtility.curStripCount;
						}
						else
						{
							generateStrips = false;
						}
					}
				}
			}
			if (!generateStrips)
			{
				foreach (CombineUtility.MeshInstance meshInstance2 in combines)
				{
					if (meshInstance2.mesh != null)
					{
						CombineUtility.triangleCount += meshInstance2.mesh.GetTriangles(meshInstance2.subMeshIndex).Length;
					}
				}
			}
			CombineUtility.vertices = new Vector3[CombineUtility.vertexCount];
			CombineUtility.normals = new Vector3[CombineUtility.vertexCount];
			CombineUtility.tangents = new Vector4[CombineUtility.vertexCount];
			CombineUtility.uv = new Vector2[CombineUtility.vertexCount];
			CombineUtility.uv1 = new Vector2[CombineUtility.vertexCount];
			CombineUtility.colors = new Color[CombineUtility.vertexCount];
			CombineUtility.triangles = new int[CombineUtility.triangleCount];
			CombineUtility.strip = new int[CombineUtility.stripCount];
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance3 in combines)
			{
				if (meshInstance3.mesh != null)
				{
					CombineUtility.Copy(meshInstance3.mesh.vertexCount, meshInstance3.mesh.vertices, CombineUtility.vertices, ref CombineUtility.offset, meshInstance3.transform);
				}
			}
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance4 in combines)
			{
				if (meshInstance4.mesh != null)
				{
					CombineUtility.invTranspose = meshInstance4.transform;
					CombineUtility.invTranspose = CombineUtility.invTranspose.inverse.transpose;
					CombineUtility.CopyNormal(meshInstance4.mesh.vertexCount, meshInstance4.mesh.normals, CombineUtility.normals, ref CombineUtility.offset, CombineUtility.invTranspose);
				}
			}
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance5 in combines)
			{
				if (meshInstance5.mesh != null)
				{
					CombineUtility.invTranspose = meshInstance5.transform;
					CombineUtility.invTranspose = CombineUtility.invTranspose.inverse.transpose;
					CombineUtility.CopyTangents(meshInstance5.mesh.vertexCount, meshInstance5.mesh.tangents, CombineUtility.tangents, ref CombineUtility.offset, CombineUtility.invTranspose);
				}
			}
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance6 in combines)
			{
				if (meshInstance6.mesh != null)
				{
					CombineUtility.Copy(meshInstance6.mesh.vertexCount, meshInstance6.mesh.uv, CombineUtility.uv, ref CombineUtility.offset);
				}
			}
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance7 in combines)
			{
				if (meshInstance7.mesh != null)
				{
					CombineUtility.Copy(meshInstance7.mesh.vertexCount, meshInstance7.mesh.uv2, CombineUtility.uv1, ref CombineUtility.offset);
				}
			}
			CombineUtility.offset = 0;
			foreach (CombineUtility.MeshInstance meshInstance8 in combines)
			{
				if (meshInstance8.mesh != null)
				{
					CombineUtility.CopyColors(meshInstance8.mesh.vertexCount, meshInstance8.mesh.colors, CombineUtility.colors, ref CombineUtility.offset);
				}
			}
			CombineUtility.triangleOffset = 0;
			CombineUtility.stripOffset = 0;
			CombineUtility.vertexOffset = 0;
			foreach (CombineUtility.MeshInstance meshInstance9 in combines)
			{
				if (meshInstance9.mesh != null)
				{
					if (generateStrips)
					{
						int[] array = meshInstance9.mesh.GetTriangles(meshInstance9.subMeshIndex);
						if (CombineUtility.stripOffset != 0)
						{
							if ((CombineUtility.stripOffset & 1) == 1)
							{
								CombineUtility.strip[CombineUtility.stripOffset] = CombineUtility.strip[CombineUtility.stripOffset - 1];
								CombineUtility.strip[CombineUtility.stripOffset + 1] = array[0] + CombineUtility.vertexOffset;
								CombineUtility.strip[CombineUtility.stripOffset + 2] = array[0] + CombineUtility.vertexOffset;
								CombineUtility.stripOffset += 3;
							}
							else
							{
								CombineUtility.strip[CombineUtility.stripOffset] = CombineUtility.strip[CombineUtility.stripOffset - 1];
								CombineUtility.strip[CombineUtility.stripOffset + 1] = array[0] + CombineUtility.vertexOffset;
								CombineUtility.stripOffset += 2;
							}
						}
						for (int j = 0; j < array.Length; j++)
						{
							CombineUtility.strip[j + CombineUtility.stripOffset] = array[j] + CombineUtility.vertexOffset;
						}
						CombineUtility.stripOffset += array.Length;
					}
					else
					{
						int[] array2 = meshInstance9.mesh.GetTriangles(meshInstance9.subMeshIndex);
						for (int k = 0; k < array2.Length; k++)
						{
							CombineUtility.triangles[k + CombineUtility.triangleOffset] = array2[k] + CombineUtility.vertexOffset;
						}
						CombineUtility.triangleOffset += array2.Length;
					}
					CombineUtility.vertexOffset += meshInstance9.mesh.vertexCount;
				}
			}
			Mesh mesh = new Mesh();
			mesh.name = "Combined Mesh";
			mesh.vertices = CombineUtility.vertices;
			mesh.normals = CombineUtility.normals;
			mesh.colors = CombineUtility.colors;
			mesh.uv = CombineUtility.uv;
			mesh.uv2 = CombineUtility.uv1;
			mesh.tangents = CombineUtility.tangents;
			if (generateStrips)
			{
				mesh.SetTriangles(CombineUtility.strip, 0);
			}
			else
			{
				mesh.triangles = CombineUtility.triangles;
			}
			return mesh;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00043EB4 File Offset: 0x000420B4
		private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i + offset] = transform.MultiplyPoint(src[i]);
			}
			offset += vertexcount;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00043EF0 File Offset: 0x000420F0
		private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
			}
			offset += vertexcount;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00043F34 File Offset: 0x00042134
		private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i + offset] = src[i];
			}
			offset += vertexcount;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00043F68 File Offset: 0x00042168
		private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
		{
			for (int i = 0; i < src.Length; i++)
			{
				dst[i + offset] = src[i];
			}
			offset += vertexcount;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00043F9C File Offset: 0x0004219C
		private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
		{
			for (int i = 0; i < src.Length; i++)
			{
				CombineUtility.p4 = src[i];
				CombineUtility.p = new Vector3(CombineUtility.p4.x, CombineUtility.p4.y, CombineUtility.p4.z);
				CombineUtility.p = transform.MultiplyVector(CombineUtility.p).normalized;
				dst[i + offset] = new Vector4(CombineUtility.p.x, CombineUtility.p.y, CombineUtility.p.z, CombineUtility.p4.w);
			}
			offset += vertexcount;
		}

		// Token: 0x04000CF9 RID: 3321
		private static int vertexCount;

		// Token: 0x04000CFA RID: 3322
		private static int triangleCount;

		// Token: 0x04000CFB RID: 3323
		private static int stripCount;

		// Token: 0x04000CFC RID: 3324
		private static int curStripCount;

		// Token: 0x04000CFD RID: 3325
		private static Vector3[] vertices;

		// Token: 0x04000CFE RID: 3326
		private static Vector3[] normals;

		// Token: 0x04000CFF RID: 3327
		private static Vector4[] tangents;

		// Token: 0x04000D00 RID: 3328
		private static Vector2[] uv;

		// Token: 0x04000D01 RID: 3329
		private static Vector2[] uv1;

		// Token: 0x04000D02 RID: 3330
		private static Color[] colors;

		// Token: 0x04000D03 RID: 3331
		private static int[] triangles;

		// Token: 0x04000D04 RID: 3332
		private static int[] strip;

		// Token: 0x04000D05 RID: 3333
		private static int offset;

		// Token: 0x04000D06 RID: 3334
		private static int triangleOffset;

		// Token: 0x04000D07 RID: 3335
		private static int stripOffset;

		// Token: 0x04000D08 RID: 3336
		private static int vertexOffset;

		// Token: 0x04000D09 RID: 3337
		private static Matrix4x4 invTranspose;

		// Token: 0x04000D0A RID: 3338
		public const string combinedMeshName = "Combined Mesh";

		// Token: 0x04000D0B RID: 3339
		private static Vector4 p4;

		// Token: 0x04000D0C RID: 3340
		private static Vector3 p;

		// Token: 0x0200044A RID: 1098
		public struct MeshInstance
		{
			// Token: 0x040015F9 RID: 5625
			public Mesh mesh;

			// Token: 0x040015FA RID: 5626
			public int subMeshIndex;

			// Token: 0x040015FB RID: 5627
			public Matrix4x4 transform;
		}
	}
}
