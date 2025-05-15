using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F1 RID: 753
	public class ProceduralMeshUtil
	{
		// Token: 0x06001399 RID: 5017 RVA: 0x0006843C File Offset: 0x0006663C
		public static void EnsureProceduralMesh(MeshFilter aFilter, bool aCreateRestoreComponent = true)
		{
			if (!ProceduralMeshUtil.IsProceduralMesh(aFilter))
			{
				if (aCreateRestoreComponent)
				{
					RestoreMesh restoreMesh = aFilter.GetComponent<RestoreMesh>();
					if (restoreMesh == null)
					{
						restoreMesh = aFilter.gameObject.AddComponent<RestoreMesh>();
					}
					restoreMesh.OriginalMesh = aFilter.sharedMesh;
				}
				if (aFilter.sharedMesh == null)
				{
					aFilter.sharedMesh = new Mesh();
				}
				aFilter.sharedMesh = Object.Instantiate<Mesh>(aFilter.sharedMesh);
				aFilter.sharedMesh.name = ProceduralMeshUtil.MakeInstName(aFilter);
				return;
			}
			if (!ProceduralMeshUtil.IsCorrectName(aFilter))
			{
				aFilter.sharedMesh = Object.Instantiate<Mesh>(aFilter.sharedMesh);
				aFilter.sharedMesh.name = ProceduralMeshUtil.MakeInstName(aFilter);
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000684E2 File Offset: 0x000666E2
		public static bool IsProceduralMesh(MeshFilter aFilter)
		{
			return !(aFilter == null) && !(aFilter.sharedMesh == null) && aFilter.sharedMesh.name.StartsWith("FerrProcMesh_");
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00068512 File Offset: 0x00066712
		public static string MakeInstName(MeshFilter aFilter)
		{
			return string.Format("{0}{1}_{2}", "FerrProcMesh_", aFilter.gameObject.name, aFilter.GetInstanceID());
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00068539 File Offset: 0x00066739
		public static bool IsCorrectName(MeshFilter aFilter)
		{
			return !(aFilter == null) && !(aFilter.sharedMesh == null) && aFilter.sharedMesh.name == ProceduralMeshUtil.MakeInstName(aFilter);
		}

		// Token: 0x04000F4B RID: 3915
		public const string cProcMeshPrefix = "FerrProcMesh_";
	}
}
