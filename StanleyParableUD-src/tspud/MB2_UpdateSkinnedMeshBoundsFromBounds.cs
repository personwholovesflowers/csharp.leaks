using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class MB2_UpdateSkinnedMeshBoundsFromBounds : MonoBehaviour
{
	// Token: 0x06000264 RID: 612 RVA: 0x00010A64 File Offset: 0x0000EC64
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		if (this.smr == null)
		{
			Debug.LogError("Need to attach MB2_UpdateSkinnedMeshBoundsFromBounds script to an object with a SkinnedMeshRenderer component attached.");
			return;
		}
		if (this.objects == null || this.objects.Count == 0)
		{
			Debug.LogWarning("The MB2_UpdateSkinnedMeshBoundsFromBounds had no Game Objects. It should have the same list of game objects that the MeshBaker does.");
			this.smr = null;
			return;
		}
		for (int i = 0; i < this.objects.Count; i++)
		{
			if (this.objects[i] == null || this.objects[i].GetComponent<Renderer>() == null)
			{
				Debug.LogError("The list of objects had nulls or game objects without a renderer attached at position " + i);
				this.smr = null;
				return;
			}
		}
		bool updateWhenOffscreen = this.smr.updateWhenOffscreen;
		this.smr.updateWhenOffscreen = true;
		this.smr.updateWhenOffscreen = updateWhenOffscreen;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00010B41 File Offset: 0x0000ED41
	private void Update()
	{
		if (this.smr != null && this.objects != null)
		{
			MB3_MeshCombiner.UpdateSkinnedMeshApproximateBoundsFromBoundsStatic(this.objects, this.smr);
		}
	}

	// Token: 0x0400027C RID: 636
	public List<GameObject> objects;

	// Token: 0x0400027D RID: 637
	private SkinnedMeshRenderer smr;
}
