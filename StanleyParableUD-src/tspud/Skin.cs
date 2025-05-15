using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class Skin : MonoBehaviour
{
	// Token: 0x0600093C RID: 2364 RVA: 0x0002B774 File Offset: 0x00029974
	public void ValidateSkin()
	{
		if (!this.primary)
		{
			return;
		}
		MeshRenderer componentInChildren = base.GetComponentInChildren<MeshRenderer>();
		SkinnedMeshRenderer componentInChildren2 = base.GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren)
		{
			componentInChildren.materials = this.materials;
		}
		if (componentInChildren2)
		{
			componentInChildren2.materials = this.materials;
		}
	}

	// Token: 0x04000912 RID: 2322
	public int index;

	// Token: 0x04000913 RID: 2323
	public bool primary;

	// Token: 0x04000914 RID: 2324
	public Material[] materials;
}
