using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public static class NormalSolver
{
	// Token: 0x06001B98 RID: 7064 RVA: 0x000E4BC0 File Offset: 0x000E2DC0
	public static void RecalculateNormals(this Mesh mesh, float angle)
	{
		float num = Mathf.Cos(angle * 0.017453292f);
		Vector3[] vertices = mesh.vertices;
		Vector3[] array = new Vector3[vertices.Length];
		Vector3[][] array2 = new Vector3[mesh.subMeshCount][];
		Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary = new Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>>(vertices.Length);
		for (int i = 0; i < mesh.subMeshCount; i++)
		{
			int[] triangles = mesh.GetTriangles(i);
			array2[i] = new Vector3[triangles.Length / 3];
			for (int j = 0; j < triangles.Length; j += 3)
			{
				int num2 = triangles[j];
				int num3 = triangles[j + 1];
				int num4 = triangles[j + 2];
				Vector3 vector = vertices[num3] - vertices[num2];
				Vector3 vector2 = vertices[num4] - vertices[num2];
				Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
				int num5 = j / 3;
				array2[i][num5] = normalized;
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary2 = dictionary;
				NormalSolver.VertexKey vertexKey = new NormalSolver.VertexKey(vertices[num2]);
				List<NormalSolver.VertexEntry> list;
				if (!dictionary2.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>(4);
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num2));
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary3 = dictionary;
				vertexKey = new NormalSolver.VertexKey(vertices[num3]);
				if (!dictionary3.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>();
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num3));
				Dictionary<NormalSolver.VertexKey, List<NormalSolver.VertexEntry>> dictionary4 = dictionary;
				vertexKey = new NormalSolver.VertexKey(vertices[num4]);
				if (!dictionary4.TryGetValue(vertexKey, out list))
				{
					list = new List<NormalSolver.VertexEntry>();
					dictionary.Add(vertexKey, list);
				}
				list.Add(new NormalSolver.VertexEntry(i, num5, num4));
			}
		}
		foreach (List<NormalSolver.VertexEntry> list2 in dictionary.Values)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				Vector3 vector3 = default(Vector3);
				NormalSolver.VertexEntry vertexEntry = list2[k];
				for (int l = 0; l < list2.Count; l++)
				{
					NormalSolver.VertexEntry vertexEntry2 = list2[l];
					if (vertexEntry.VertexIndex == vertexEntry2.VertexIndex)
					{
						vector3 += array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex];
					}
					else if (Vector3.Dot(array2[vertexEntry.MeshIndex][vertexEntry.TriangleIndex], array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex]) >= num)
					{
						vector3 += array2[vertexEntry2.MeshIndex][vertexEntry2.TriangleIndex];
					}
				}
				array[vertexEntry.VertexIndex] = vector3.normalized;
			}
		}
		mesh.normals = array;
	}

	// Token: 0x020004B2 RID: 1202
	private struct VertexKey
	{
		// Token: 0x06001B99 RID: 7065 RVA: 0x000E4EC0 File Offset: 0x000E30C0
		public VertexKey(Vector3 position)
		{
			this._x = (long)Mathf.Round(position.x * 100000f);
			this._y = (long)Mathf.Round(position.y * 100000f);
			this._z = (long)Mathf.Round(position.z * 100000f);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000E4F18 File Offset: 0x000E3118
		public override bool Equals(object obj)
		{
			NormalSolver.VertexKey vertexKey = (NormalSolver.VertexKey)obj;
			return this._x == vertexKey._x && this._y == vertexKey._y && this._z == vertexKey._z;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000E4F58 File Offset: 0x000E3158
		public override int GetHashCode()
		{
			long num = (long)((ulong)(-2128831035));
			num ^= this._x;
			num *= 16777619L;
			num ^= this._y;
			num *= 16777619L;
			num ^= this._z;
			return (num * 16777619L).GetHashCode();
		}

		// Token: 0x040026D4 RID: 9940
		private readonly long _x;

		// Token: 0x040026D5 RID: 9941
		private readonly long _y;

		// Token: 0x040026D6 RID: 9942
		private readonly long _z;

		// Token: 0x040026D7 RID: 9943
		private const int Tolerance = 100000;

		// Token: 0x040026D8 RID: 9944
		private const long FNV32Init = 2166136261L;

		// Token: 0x040026D9 RID: 9945
		private const long FNV32Prime = 16777619L;
	}

	// Token: 0x020004B3 RID: 1203
	private struct VertexEntry
	{
		// Token: 0x06001B9C RID: 7068 RVA: 0x000E4FA9 File Offset: 0x000E31A9
		public VertexEntry(int meshIndex, int triIndex, int vertIndex)
		{
			this.MeshIndex = meshIndex;
			this.TriangleIndex = triIndex;
			this.VertexIndex = vertIndex;
		}

		// Token: 0x040026DA RID: 9946
		public int MeshIndex;

		// Token: 0x040026DB RID: 9947
		public int TriangleIndex;

		// Token: 0x040026DC RID: 9948
		public int VertexIndex;
	}
}
