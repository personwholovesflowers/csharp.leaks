using System;
using UnityEngine;

// Token: 0x0200035C RID: 860
public static class PreserveOriginalMeshExtensions
{
	// Token: 0x060013FF RID: 5119 RVA: 0x0009FF6C File Offset: 0x0009E16C
	public static PreservedOriginalMesh PreserveMesh(this MeshFilter mf)
	{
		if (mf == null)
		{
			return null;
		}
		PreservedOriginalMesh preservedOriginalMesh;
		if (mf.TryGetComponent<PreservedOriginalMesh>(out preservedOriginalMesh))
		{
			if (preservedOriginalMesh.mesh == null || preservedOriginalMesh.mesh != mf.sharedMesh)
			{
				preservedOriginalMesh.mesh = mf.sharedMesh;
			}
			return preservedOriginalMesh;
		}
		preservedOriginalMesh = mf.gameObject.AddComponent<PreservedOriginalMesh>();
		preservedOriginalMesh.mesh = mf.sharedMesh;
		return preservedOriginalMesh;
	}
}
