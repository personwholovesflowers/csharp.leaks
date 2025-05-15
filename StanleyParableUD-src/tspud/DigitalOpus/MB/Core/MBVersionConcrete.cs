using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200025C RID: 604
	public class MBVersionConcrete : MBVersionInterface
	{
		// Token: 0x06000E52 RID: 3666 RVA: 0x00045850 File Offset: 0x00043A50
		public string version()
		{
			return "3.23.0";
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00045857 File Offset: 0x00043A57
		public int GetMajorVersion()
		{
			return int.Parse(Application.unityVersion.Split(new char[] { '.' })[0]);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00045875 File Offset: 0x00043A75
		public int GetMinorVersion()
		{
			return int.Parse(Application.unityVersion.Split(new char[] { '.' })[1]);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00045893 File Offset: 0x00043A93
		public bool GetActive(GameObject go)
		{
			return go.activeInHierarchy;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0004589B File Offset: 0x00043A9B
		public void SetActive(GameObject go, bool isActive)
		{
			go.SetActive(isActive);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0004589B File Offset: 0x00043A9B
		public void SetActiveRecursively(GameObject go, bool isActive)
		{
			go.SetActive(isActive);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000458A4 File Offset: 0x00043AA4
		public Object[] FindSceneObjectsOfType(Type t)
		{
			return Object.FindObjectsOfType(t);
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00005444 File Offset: 0x00003644
		public void OptimizeMesh(Mesh m)
		{
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000458AC File Offset: 0x00043AAC
		public bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			return Application.isPlaying && !m.isReadable;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000458C0 File Offset: 0x00043AC0
		public Vector2[] GetMeshUV1s(Mesh m, MB2_LogLevel LOG_LEVEL)
		{
			if (LOG_LEVEL >= MB2_LogLevel.warn)
			{
				MB2_Log.LogDebug("UV1 does not exist in Unity 5+", Array.Empty<object>());
			}
			Vector2[] array = m.uv;
			if (array.Length == 0)
			{
				if (LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Mesh " + m + " has no uv1s. Generating", Array.Empty<object>());
				}
				if (LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Mesh " + m + " didn't have uv1s. Generating uv1s.");
				}
				array = new Vector2[m.vertexCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._HALF_UV;
				}
			}
			return array;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004594C File Offset: 0x00043B4C
		public Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			Vector2[] array;
			if (get3)
			{
				array = m.uv3;
			}
			else
			{
				array = m.uv4;
			}
			if (array.Length == 0)
			{
				if (LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Concat(new object[]
					{
						"Mesh ",
						m,
						" has no uv",
						get3 ? "3" : "4",
						". Generating"
					}), Array.Empty<object>());
				}
				array = new Vector2[m.vertexCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._HALF_UV;
				}
			}
			return array;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000459E0 File Offset: 0x00043BE0
		public void MeshClear(Mesh m, bool t)
		{
			m.Clear(t);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000459E9 File Offset: 0x00043BE9
		public void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
			m.uv3 = uv3s;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000459F2 File Offset: 0x00043BF2
		public void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
			m.uv4 = uv4s;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000459FB File Offset: 0x00043BFB
		public Vector4 GetLightmapTilingOffset(Renderer r)
		{
			return r.lightmapScaleOffset;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00045A03 File Offset: 0x00043C03
		public Transform[] GetBones(Renderer r)
		{
			if (r is SkinnedMeshRenderer)
			{
				return ((SkinnedMeshRenderer)r).bones;
			}
			if (r is MeshRenderer)
			{
				return new Transform[] { r.transform };
			}
			Debug.LogError("Could not getBones. Object does not have a renderer");
			return null;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00045A3C File Offset: 0x00043C3C
		public int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			return m.GetBlendShapeFrameCount(shapeIndex);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00045A45 File Offset: 0x00043C45
		public float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			return m.GetBlendShapeFrameWeight(shapeIndex, frameIndex);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00045A4F File Offset: 0x00043C4F
		public void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			m.GetBlendShapeFrameVertices(shapeIndex, frameIndex, vs, ns, ts);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00045A5F File Offset: 0x00043C5F
		public void ClearBlendShapes(Mesh m)
		{
			m.ClearBlendShapes();
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00045A67 File Offset: 0x00043C67
		public void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			m.AddBlendShapeFrame(nm, wt, vs, ns, ts);
		}

		// Token: 0x04000D17 RID: 3351
		private Vector2 _HALF_UV = new Vector2(0.5f, 0.5f);
	}
}
