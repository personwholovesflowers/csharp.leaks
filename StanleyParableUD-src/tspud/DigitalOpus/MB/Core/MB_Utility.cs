using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000286 RID: 646
	public class MB_Utility
	{
		// Token: 0x0600103A RID: 4154 RVA: 0x0005527C File Offset: 0x0005347C
		public static Texture2D createTextureCopy(Texture2D source)
		{
			Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
			texture2D.SetPixels(source.GetPixels());
			return texture2D;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000552A0 File Offset: 0x000534A0
		public static bool ArrayBIsSubsetOfA(object[] a, object[] b)
		{
			for (int i = 0; i < b.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < a.Length; j++)
				{
					if (a[j] == b[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x000552E0 File Offset: 0x000534E0
		public static Material[] GetGOMaterials(GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			Material[] array = null;
			Mesh mesh = null;
			MeshRenderer component = go.GetComponent<MeshRenderer>();
			if (component != null)
			{
				array = component.sharedMaterials;
				MeshFilter component2 = go.GetComponent<MeshFilter>();
				if (component2 == null)
				{
					throw new Exception("Object " + go + " has a MeshRenderer but no MeshFilter.");
				}
				mesh = component2.sharedMesh;
			}
			SkinnedMeshRenderer component3 = go.GetComponent<SkinnedMeshRenderer>();
			if (component3 != null)
			{
				array = component3.sharedMaterials;
				mesh = component3.sharedMesh;
			}
			if (array == null)
			{
				Debug.LogError("Object " + go.name + " does not have a MeshRenderer or a SkinnedMeshRenderer component");
				return new Material[0];
			}
			if (mesh == null)
			{
				Debug.LogError("Object " + go.name + " has a MeshRenderer or SkinnedMeshRenderer but no mesh.");
				return new Material[0];
			}
			if (mesh.subMeshCount < array.Length)
			{
				Debug.LogWarning(string.Concat(new object[] { "Object ", go, " has only ", mesh.subMeshCount, " submeshes and has ", array.Length, " materials. Extra materials do nothing." }));
				Material[] array2 = new Material[mesh.subMeshCount];
				Array.Copy(array, array2, array2.Length);
				array = array2;
			}
			return array;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00055420 File Offset: 0x00053620
		public static Mesh GetMesh(GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			MeshFilter component = go.GetComponent<MeshFilter>();
			if (component != null)
			{
				return component.sharedMesh;
			}
			SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				return component2.sharedMesh;
			}
			Debug.LogError("Object " + go.name + " does not have a MeshFilter or a SkinnedMeshRenderer component");
			return null;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00055484 File Offset: 0x00053684
		public static void SetMesh(GameObject go, Mesh m)
		{
			if (go == null)
			{
				return;
			}
			MeshFilter component = go.GetComponent<MeshFilter>();
			if (component != null)
			{
				component.sharedMesh = m;
				return;
			}
			SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				component2.sharedMesh = m;
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000554CC File Offset: 0x000536CC
		public static Renderer GetRenderer(GameObject go)
		{
			if (go == null)
			{
				return null;
			}
			MeshRenderer component = go.GetComponent<MeshRenderer>();
			if (component != null)
			{
				return component;
			}
			SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				return component2;
			}
			return null;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0005550C File Offset: 0x0005370C
		public static void DisableRendererInSource(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			MeshRenderer component = go.GetComponent<MeshRenderer>();
			if (component != null)
			{
				component.enabled = false;
				return;
			}
			SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
			if (component2 != null)
			{
				component2.enabled = false;
				return;
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00055554 File Offset: 0x00053754
		public static bool hasOutOfBoundsUVs(Mesh m, ref Rect uvBounds)
		{
			MB_Utility.MeshAnalysisResult meshAnalysisResult = default(MB_Utility.MeshAnalysisResult);
			bool flag = MB_Utility.hasOutOfBoundsUVs(m, ref meshAnalysisResult, -1, 0);
			uvBounds = meshAnalysisResult.uvRect;
			return flag;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00055580 File Offset: 0x00053780
		public static bool hasOutOfBoundsUVs(Mesh m, ref MB_Utility.MeshAnalysisResult putResultHere, int submeshIndex = -1, int uvChannel = 0)
		{
			if (m == null)
			{
				putResultHere.hasOutOfBoundsUVs = false;
				return putResultHere.hasOutOfBoundsUVs;
			}
			Vector2[] array;
			if (uvChannel == 0)
			{
				array = m.uv;
			}
			else if (uvChannel == 1)
			{
				array = m.uv2;
			}
			else if (uvChannel == 2)
			{
				array = m.uv3;
			}
			else
			{
				array = m.uv4;
			}
			return MB_Utility.hasOutOfBoundsUVs(array, m, ref putResultHere, submeshIndex);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000555DC File Offset: 0x000537DC
		public static bool hasOutOfBoundsUVs(Vector2[] uvs, Mesh m, ref MB_Utility.MeshAnalysisResult putResultHere, int submeshIndex = -1)
		{
			putResultHere.hasUVs = true;
			if (uvs.Length == 0)
			{
				putResultHere.hasUVs = false;
				putResultHere.hasOutOfBoundsUVs = false;
				putResultHere.uvRect = default(Rect);
				return putResultHere.hasOutOfBoundsUVs;
			}
			if (submeshIndex >= m.subMeshCount)
			{
				putResultHere.hasOutOfBoundsUVs = false;
				putResultHere.uvRect = default(Rect);
				return putResultHere.hasOutOfBoundsUVs;
			}
			float num2;
			float num;
			float num4;
			float num3;
			if (submeshIndex >= 0)
			{
				int[] triangles = m.GetTriangles(submeshIndex);
				if (triangles.Length == 0)
				{
					putResultHere.hasOutOfBoundsUVs = false;
					putResultHere.uvRect = default(Rect);
					return putResultHere.hasOutOfBoundsUVs;
				}
				num = (num2 = uvs[triangles[0]].x);
				num3 = (num4 = uvs[triangles[0]].y);
				foreach (int num5 in triangles)
				{
					if (uvs[num5].x < num2)
					{
						num2 = uvs[num5].x;
					}
					if (uvs[num5].x > num)
					{
						num = uvs[num5].x;
					}
					if (uvs[num5].y < num4)
					{
						num4 = uvs[num5].y;
					}
					if (uvs[num5].y > num3)
					{
						num3 = uvs[num5].y;
					}
				}
			}
			else
			{
				num = (num2 = uvs[0].x);
				num3 = (num4 = uvs[0].y);
				for (int j = 0; j < uvs.Length; j++)
				{
					if (uvs[j].x < num2)
					{
						num2 = uvs[j].x;
					}
					if (uvs[j].x > num)
					{
						num = uvs[j].x;
					}
					if (uvs[j].y < num4)
					{
						num4 = uvs[j].y;
					}
					if (uvs[j].y > num3)
					{
						num3 = uvs[j].y;
					}
				}
			}
			Rect rect = default(Rect);
			rect.x = num2;
			rect.y = num4;
			rect.width = num - num2;
			rect.height = num3 - num4;
			if (num > 1f || num2 < 0f || num3 > 1f || num4 < 0f)
			{
				putResultHere.hasOutOfBoundsUVs = true;
			}
			else
			{
				putResultHere.hasOutOfBoundsUVs = false;
			}
			putResultHere.uvRect = rect;
			return putResultHere.hasOutOfBoundsUVs;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00055840 File Offset: 0x00053A40
		public static void setSolidColor(Texture2D t, Color c)
		{
			Color[] pixels = t.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = c;
			}
			t.SetPixels(pixels);
			t.Apply();
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00055878 File Offset: 0x00053A78
		public static Texture2D resampleTexture(Texture2D source, int newWidth, int newHeight)
		{
			TextureFormat format = source.format;
			if (format == TextureFormat.ARGB32 || format == TextureFormat.RGBA32 || format == TextureFormat.BGRA32 || format == TextureFormat.RGB24 || format == TextureFormat.Alpha8 || format == TextureFormat.DXT1)
			{
				Texture2D texture2D = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, true);
				float num = (float)newWidth;
				float num2 = (float)newHeight;
				for (int i = 0; i < newWidth; i++)
				{
					for (int j = 0; j < newHeight; j++)
					{
						float num3 = (float)i / num;
						float num4 = (float)j / num2;
						texture2D.SetPixel(i, j, source.GetPixelBilinear(num3, num4));
					}
				}
				texture2D.Apply();
				return texture2D;
			}
			Debug.LogError("Can only resize textures in formats ARGB32, RGBA32, BGRA32, RGB24, Alpha8 or DXT");
			return null;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0005590C File Offset: 0x00053B0C
		public static bool AreAllSharedMaterialsDistinct(Material[] sharedMaterials)
		{
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				for (int j = i + 1; j < sharedMaterials.Length; j++)
				{
					if (sharedMaterials[i] == sharedMaterials[j])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00055948 File Offset: 0x00053B48
		public static int doSubmeshesShareVertsOrTris(Mesh m, ref MB_Utility.MeshAnalysisResult mar)
		{
			MB_Utility.MB_Triangle mb_Triangle = new MB_Utility.MB_Triangle();
			MB_Utility.MB_Triangle mb_Triangle2 = new MB_Utility.MB_Triangle();
			int[][] array = new int[m.subMeshCount][];
			for (int i = 0; i < m.subMeshCount; i++)
			{
				array[i] = m.GetTriangles(i);
			}
			bool flag = false;
			bool flag2 = false;
			for (int j = 0; j < m.subMeshCount; j++)
			{
				int[] array2 = array[j];
				for (int k = j + 1; k < m.subMeshCount; k++)
				{
					int[] array3 = array[k];
					for (int l = 0; l < array2.Length; l += 3)
					{
						mb_Triangle.Initialize(array2, l, j);
						for (int n = 0; n < array3.Length; n += 3)
						{
							mb_Triangle2.Initialize(array3, n, k);
							if (mb_Triangle.isSame(mb_Triangle2))
							{
								flag2 = true;
								break;
							}
							if (mb_Triangle.sharesVerts(mb_Triangle2))
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			if (flag2)
			{
				mar.hasOverlappingSubmeshVerts = true;
				mar.hasOverlappingSubmeshTris = true;
				return 2;
			}
			if (flag)
			{
				mar.hasOverlappingSubmeshVerts = true;
				mar.hasOverlappingSubmeshTris = false;
				return 1;
			}
			mar.hasOverlappingSubmeshTris = false;
			mar.hasOverlappingSubmeshVerts = false;
			return 0;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00055A64 File Offset: 0x00053C64
		public static bool GetBounds(GameObject go, out Bounds b)
		{
			if (go == null)
			{
				Debug.LogError("go paramater was null");
				b = new Bounds(Vector3.zero, Vector3.zero);
				return false;
			}
			Renderer renderer = MB_Utility.GetRenderer(go);
			if (renderer == null)
			{
				Debug.LogError("GetBounds must be called on an object with a Renderer");
				b = new Bounds(Vector3.zero, Vector3.zero);
				return false;
			}
			if (renderer is MeshRenderer)
			{
				b = renderer.bounds;
				return true;
			}
			if (renderer is SkinnedMeshRenderer)
			{
				b = renderer.bounds;
				return true;
			}
			Debug.LogError("GetBounds must be called on an object with a MeshRender or a SkinnedMeshRenderer.");
			b = new Bounds(Vector3.zero, Vector3.zero);
			return false;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00055B18 File Offset: 0x00053D18
		public static void Destroy(Object o)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(o);
				return;
			}
			Object.DestroyImmediate(o, false);
		}

		// Token: 0x0200047C RID: 1148
		public struct MeshAnalysisResult
		{
			// Token: 0x0400170A RID: 5898
			public Rect uvRect;

			// Token: 0x0400170B RID: 5899
			public bool hasOutOfBoundsUVs;

			// Token: 0x0400170C RID: 5900
			public bool hasOverlappingSubmeshVerts;

			// Token: 0x0400170D RID: 5901
			public bool hasOverlappingSubmeshTris;

			// Token: 0x0400170E RID: 5902
			public bool hasUVs;

			// Token: 0x0400170F RID: 5903
			public float submeshArea;
		}

		// Token: 0x0200047D RID: 1149
		private class MB_Triangle
		{
			// Token: 0x0600197A RID: 6522 RVA: 0x00081770 File Offset: 0x0007F970
			public bool isSame(object obj)
			{
				MB_Utility.MB_Triangle mb_Triangle = (MB_Utility.MB_Triangle)obj;
				return this.vs[0] == mb_Triangle.vs[0] && this.vs[1] == mb_Triangle.vs[1] && this.vs[2] == mb_Triangle.vs[2] && this.submeshIdx != mb_Triangle.submeshIdx;
			}

			// Token: 0x0600197B RID: 6523 RVA: 0x000817CC File Offset: 0x0007F9CC
			public bool sharesVerts(MB_Utility.MB_Triangle obj)
			{
				return ((this.vs[0] == obj.vs[0] || this.vs[0] == obj.vs[1] || this.vs[0] == obj.vs[2]) && this.submeshIdx != obj.submeshIdx) || ((this.vs[1] == obj.vs[0] || this.vs[1] == obj.vs[1] || this.vs[1] == obj.vs[2]) && this.submeshIdx != obj.submeshIdx) || ((this.vs[2] == obj.vs[0] || this.vs[2] == obj.vs[1] || this.vs[2] == obj.vs[2]) && this.submeshIdx != obj.submeshIdx);
			}

			// Token: 0x0600197C RID: 6524 RVA: 0x000818AC File Offset: 0x0007FAAC
			public void Initialize(int[] ts, int idx, int sIdx)
			{
				this.vs[0] = ts[idx];
				this.vs[1] = ts[idx + 1];
				this.vs[2] = ts[idx + 2];
				this.submeshIdx = sIdx;
				Array.Sort<int>(this.vs);
			}

			// Token: 0x04001710 RID: 5904
			private int submeshIdx;

			// Token: 0x04001711 RID: 5905
			private int[] vs = new int[3];
		}
	}
}
