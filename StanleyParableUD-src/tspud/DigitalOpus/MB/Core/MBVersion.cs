using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000278 RID: 632
	public class MBVersion
	{
		// Token: 0x06000EE8 RID: 3816 RVA: 0x0004836E File Offset: 0x0004656E
		private static MBVersionInterface _CreateMBVersionConcrete()
		{
			return (MBVersionInterface)Activator.CreateInstance(typeof(MBVersionConcrete));
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00048384 File Offset: 0x00046584
		public static string version()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.version();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000483A1 File Offset: 0x000465A1
		public static int GetMajorVersion()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMajorVersion();
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000483BE File Offset: 0x000465BE
		public static int GetMinorVersion()
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMinorVersion();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000483DB File Offset: 0x000465DB
		public static bool GetActive(GameObject go)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetActive(go);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000483F9 File Offset: 0x000465F9
		public static void SetActive(GameObject go, bool isActive)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.SetActive(go, isActive);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00048418 File Offset: 0x00046618
		public static void SetActiveRecursively(GameObject go, bool isActive)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.SetActiveRecursively(go, isActive);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00048437 File Offset: 0x00046637
		public static Object[] FindSceneObjectsOfType(Type t)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.FindSceneObjectsOfType(t);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00048455 File Offset: 0x00046655
		public static bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.IsRunningAndMeshNotReadWriteable(m);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00048473 File Offset: 0x00046673
		public static Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetMeshUV3orUV4(m, get3, LOG_LEVEL);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00048493 File Offset: 0x00046693
		public static void MeshClear(Mesh m, bool t)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshClear(m, t);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000484B2 File Offset: 0x000466B2
		public static void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshAssignUV3(m, uv3s);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000484D1 File Offset: 0x000466D1
		public static void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.MeshAssignUV4(m, uv4s);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000484F0 File Offset: 0x000466F0
		public static Vector4 GetLightmapTilingOffset(Renderer r)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetLightmapTilingOffset(r);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0004850E File Offset: 0x0004670E
		public static Transform[] GetBones(Renderer r)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBones(r);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0004852C File Offset: 0x0004672C
		public static void OptimizeMesh(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.OptimizeMesh(m);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0004854A File Offset: 0x0004674A
		public static int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBlendShapeFrameCount(m, shapeIndex);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00048569 File Offset: 0x00046769
		public static float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			return MBVersion._MBVersion.GetBlendShapeFrameWeight(m, shapeIndex, frameIndex);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00048589 File Offset: 0x00046789
		public static void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.GetBlendShapeFrameVertices(m, shapeIndex, frameIndex, vs, ns, ts);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x000485AE File Offset: 0x000467AE
		public static void ClearBlendShapes(Mesh m)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.ClearBlendShapes(m);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000485CC File Offset: 0x000467CC
		public static void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
			if (MBVersion._MBVersion == null)
			{
				MBVersion._MBVersion = MBVersion._CreateMBVersionConcrete();
			}
			MBVersion._MBVersion.AddBlendShapeFrame(m, nm, wt, vs, ns, ts);
		}

		// Token: 0x04000D69 RID: 3433
		private static MBVersionInterface _MBVersion;
	}
}
