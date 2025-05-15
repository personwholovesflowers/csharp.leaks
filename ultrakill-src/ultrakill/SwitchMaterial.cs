using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class SwitchMaterial : MonoBehaviour
{
	// Token: 0x060019E0 RID: 6624 RVA: 0x000D45C4 File Offset: 0x000D27C4
	private void Start()
	{
		this.mr = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x000D45D2 File Offset: 0x000D27D2
	public void Switch(int i)
	{
		this.mr.sharedMaterial = this.materials[i];
	}

	// Token: 0x0400243A RID: 9274
	public Material[] materials;

	// Token: 0x0400243B RID: 9275
	private MeshRenderer mr;
}
