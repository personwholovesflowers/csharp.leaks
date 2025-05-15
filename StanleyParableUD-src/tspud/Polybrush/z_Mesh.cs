using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000228 RID: 552
	public class z_Mesh
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x00037148 File Offset: 0x00035348
		public MeshTopology GetTopology(int index)
		{
			return this.meshTopology[index];
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00037152 File Offset: 0x00035352
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x0003715C File Offset: 0x0003535C
		public int subMeshCount
		{
			get
			{
				return this._subMeshCount;
			}
			set
			{
				int[][] array = new int[value][];
				MeshTopology[] array2 = new MeshTopology[value];
				if (this.indices != null)
				{
					Array.Copy(this.indices, 0, array, 0, this._subMeshCount);
				}
				Array.Copy(this.meshTopology, 0, array2, 0, this._subMeshCount);
				this.indices = array;
				this.meshTopology = array2;
				this._subMeshCount = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x000371BC File Offset: 0x000353BC
		public int vertexCount
		{
			get
			{
				if (this.vertices == null)
				{
					return 0;
				}
				return this.vertices.Length;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000371D0 File Offset: 0x000353D0
		public List<Vector4> GetUVs(int index)
		{
			if (index == 0)
			{
				return this.uv0;
			}
			if (index == 1)
			{
				return this.uv1;
			}
			if (index == 2)
			{
				return this.uv2;
			}
			if (index == 3)
			{
				return this.uv3;
			}
			return null;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000371FE File Offset: 0x000353FE
		public void SetUVs(int index, List<Vector4> uvs)
		{
			if (index == 0)
			{
				this.uv0 = uvs;
				return;
			}
			if (index == 1)
			{
				this.uv1 = uvs;
				return;
			}
			if (index == 2)
			{
				this.uv2 = uvs;
				return;
			}
			if (index == 3)
			{
				this.uv3 = uvs;
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00037230 File Offset: 0x00035430
		public void Clear()
		{
			this.subMeshCount = 0;
			this.vertices = null;
			this.normals = null;
			this.colors = null;
			this.tangents = null;
			this.uv0 = null;
			this.uv1 = null;
			this.uv2 = null;
			this.uv3 = null;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003727C File Offset: 0x0003547C
		public int[] GetTriangles()
		{
			if (this.triangles == null)
			{
				this.triangles = this.indices.SelectMany((int[] x) => x).ToArray<int>();
			}
			return this.triangles;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000372CC File Offset: 0x000354CC
		public int[] GetIndices(int index)
		{
			return this.indices[index];
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000372D6 File Offset: 0x000354D6
		public void SetTriangles(int[] triangles, int index)
		{
			this.indices[index] = triangles;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000372E4 File Offset: 0x000354E4
		public void RecalculateNormals()
		{
			Vector3[] array = new Vector3[this.vertexCount];
			int[] array2 = new int[this.vertexCount];
			int[] array3 = this.triangles;
			for (int i = 0; i < array3.Length; i += 3)
			{
				int num = array3[i];
				int num2 = array3[i + 1];
				int num3 = array3[i + 2];
				Vector3 vector = z_Math.Normal(this.vertices[num], this.vertices[num2], this.vertices[num3]);
				Vector3[] array4 = array;
				int num4 = num;
				array4[num4].x = array4[num4].x + vector.x;
				Vector3[] array5 = array;
				int num5 = num2;
				array5[num5].x = array5[num5].x + vector.x;
				Vector3[] array6 = array;
				int num6 = num3;
				array6[num6].x = array6[num6].x + vector.x;
				Vector3[] array7 = array;
				int num7 = num;
				array7[num7].y = array7[num7].y + vector.y;
				Vector3[] array8 = array;
				int num8 = num2;
				array8[num8].y = array8[num8].y + vector.y;
				Vector3[] array9 = array;
				int num9 = num3;
				array9[num9].y = array9[num9].y + vector.y;
				Vector3[] array10 = array;
				int num10 = num;
				array10[num10].z = array10[num10].z + vector.z;
				Vector3[] array11 = array;
				int num11 = num2;
				array11[num11].z = array11[num11].z + vector.z;
				Vector3[] array12 = array;
				int num12 = num3;
				array12[num12].z = array12[num12].z + vector.z;
				array2[num]++;
				array2[num2]++;
				array2[num3]++;
			}
			for (int j = 0; j < this.vertexCount; j++)
			{
				this.normals[j].x = array[j].x * (float)array2[j];
				this.normals[j].y = array[j].y * (float)array2[j];
				this.normals[j].z = array[j].z * (float)array2[j];
			}
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x000374E8 File Offset: 0x000356E8
		public void ApplyAttributesToUnityMesh(Mesh m, z_MeshChannel attrib = z_MeshChannel.All)
		{
			if (attrib == z_MeshChannel.All)
			{
				m.vertices = this.vertices;
				m.normals = this.normals;
				m.colors32 = this.colors;
				m.tangents = this.tangents;
				m.SetUVs(0, this.uv0);
				m.SetUVs(1, this.uv1);
				m.SetUVs(2, this.uv2);
				m.SetUVs(3, this.uv3);
				return;
			}
			if ((attrib & z_MeshChannel.Null) > z_MeshChannel.Null)
			{
				m.vertices = this.vertices;
			}
			if ((attrib & z_MeshChannel.Normal) > z_MeshChannel.Null)
			{
				m.normals = this.normals;
			}
			if ((attrib & z_MeshChannel.Color) > z_MeshChannel.Null)
			{
				m.colors32 = this.colors;
			}
			if ((attrib & z_MeshChannel.Tangent) > z_MeshChannel.Null)
			{
				m.tangents = this.tangents;
			}
			if ((attrib & z_MeshChannel.UV0) > z_MeshChannel.Null)
			{
				m.SetUVs(0, this.uv0);
			}
			if ((attrib & z_MeshChannel.UV2) > z_MeshChannel.Null)
			{
				m.SetUVs(1, this.uv1);
			}
			if ((attrib & z_MeshChannel.UV3) > z_MeshChannel.Null)
			{
				m.SetUVs(2, this.uv2);
			}
			if ((attrib & z_MeshChannel.UV4) > z_MeshChannel.Null)
			{
				m.SetUVs(3, this.uv3);
			}
		}

		// Token: 0x04000BD5 RID: 3029
		public string name = "";

		// Token: 0x04000BD6 RID: 3030
		public Vector3[] vertices;

		// Token: 0x04000BD7 RID: 3031
		public Vector3[] normals;

		// Token: 0x04000BD8 RID: 3032
		public Color32[] colors;

		// Token: 0x04000BD9 RID: 3033
		public Vector4[] tangents;

		// Token: 0x04000BDA RID: 3034
		public List<Vector4> uv0;

		// Token: 0x04000BDB RID: 3035
		public List<Vector4> uv1;

		// Token: 0x04000BDC RID: 3036
		public List<Vector4> uv2;

		// Token: 0x04000BDD RID: 3037
		public List<Vector4> uv3;

		// Token: 0x04000BDE RID: 3038
		private int _subMeshCount;

		// Token: 0x04000BDF RID: 3039
		private int[] triangles;

		// Token: 0x04000BE0 RID: 3040
		private int[][] indices;

		// Token: 0x04000BE1 RID: 3041
		private MeshTopology[] meshTopology = new MeshTopology[1];
	}
}
