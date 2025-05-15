using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class MB3_MeshBakerGrouper : MonoBehaviour
{
	// Token: 0x0600028A RID: 650 RVA: 0x00011068 File Offset: 0x0000F268
	private void OnDrawGizmosSelected()
	{
		if (this.grouper == null)
		{
			this.grouper = this.CreateGrouper(this.clusterType, this.data);
		}
		if (this.grouper.d == null)
		{
			this.grouper.d = this.data;
		}
		this.grouper.DrawGizmos(this.sourceObjectBounds);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x000110C4 File Offset: 0x0000F2C4
	public MB3_MeshBakerGrouperCore CreateGrouper(MB3_MeshBakerGrouper.ClusterType t, GrouperData data)
	{
		if (t == MB3_MeshBakerGrouper.ClusterType.grid)
		{
			this.grouper = new MB3_MeshBakerGrouperGrid(data);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.pie)
		{
			this.grouper = new MB3_MeshBakerGrouperPie(data);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.agglomerative)
		{
			MB3_TextureBaker component = base.GetComponent<MB3_TextureBaker>();
			List<GameObject> list;
			if (component != null)
			{
				list = component.GetObjectsToCombine();
			}
			else
			{
				list = new List<GameObject>();
			}
			this.grouper = new MB3_MeshBakerGrouperCluster(data, list);
		}
		if (t == MB3_MeshBakerGrouper.ClusterType.none)
		{
			this.grouper = new MB3_MeshBakerGrouperNone(data);
		}
		return this.grouper;
	}

	// Token: 0x0400028C RID: 652
	public MB3_MeshBakerGrouperCore grouper;

	// Token: 0x0400028D RID: 653
	public MB3_MeshBakerGrouper.ClusterType clusterType;

	// Token: 0x0400028E RID: 654
	public GrouperData data = new GrouperData();

	// Token: 0x0400028F RID: 655
	[HideInInspector]
	public Bounds sourceObjectBounds = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x02000376 RID: 886
	public enum ClusterType
	{
		// Token: 0x0400128C RID: 4748
		none,
		// Token: 0x0400128D RID: 4749
		grid,
		// Token: 0x0400128E RID: 4750
		pie,
		// Token: 0x0400128F RID: 4751
		agglomerative
	}
}
