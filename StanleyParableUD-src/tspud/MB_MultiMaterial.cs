using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
[Serializable]
public class MB_MultiMaterial
{
	// Token: 0x04000269 RID: 617
	public Material combinedMaterial;

	// Token: 0x0400026A RID: 618
	public bool considerMeshUVs;

	// Token: 0x0400026B RID: 619
	public List<Material> sourceMaterials = new List<Material>();
}
