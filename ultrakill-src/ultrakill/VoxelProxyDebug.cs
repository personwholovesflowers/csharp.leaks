using System;
using ULTRAKILL.Cheats.UnityEditor;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class VoxelProxyDebug : MonoBehaviour
{
	// Token: 0x060018D0 RID: 6352 RVA: 0x000C9017 File Offset: 0x000C7217
	private void Awake()
	{
		this.voxelProxy = base.GetComponent<VoxelProxy>();
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000C9028 File Offset: 0x000C7228
	private void OnDrawGizmos()
	{
		if (this.voxelProxy == null)
		{
			return;
		}
		if (!NapalmDebugVoxels.Enabled)
		{
			Object.Destroy(this);
			return;
		}
		Gizmos.color = (this.voxelProxy.isStatic ? Color.blue : Color.green);
		Gizmos.DrawWireCube(base.transform.position, Vector3.one * 2.75f);
		if (!this.voxelProxy.isStatic)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireCube(this.voxelProxy.voxel.RoundedWorldPosition, Vector3.one * 2.75f);
		}
	}

	// Token: 0x040022A7 RID: 8871
	private VoxelProxy voxelProxy;
}
