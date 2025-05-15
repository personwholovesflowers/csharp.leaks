using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class MB2_UpdateSkinnedMeshBoundsFromBones : MonoBehaviour
{
	// Token: 0x06000261 RID: 609 RVA: 0x000109DC File Offset: 0x0000EBDC
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		if (this.smr == null)
		{
			Debug.LogError("Need to attach MB2_UpdateSkinnedMeshBoundsFromBones script to an object with a SkinnedMeshRenderer component attached.");
			return;
		}
		this.bones = this.smr.bones;
		bool updateWhenOffscreen = this.smr.updateWhenOffscreen;
		this.smr.updateWhenOffscreen = true;
		this.smr.updateWhenOffscreen = updateWhenOffscreen;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00010A43 File Offset: 0x0000EC43
	private void Update()
	{
		if (this.smr != null)
		{
			MB3_MeshCombiner.UpdateSkinnedMeshApproximateBoundsFromBonesStatic(this.bones, this.smr);
		}
	}

	// Token: 0x0400027A RID: 634
	private SkinnedMeshRenderer smr;

	// Token: 0x0400027B RID: 635
	private Transform[] bones;
}
