using System;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
public class DebugBakedMesh : MonoBehaviour
{
	// Token: 0x06001C8A RID: 7306 RVA: 0x000EFD3C File Offset: 0x000EDF3C
	private void Update()
	{
		if (this.sceneData == null)
		{
			return;
		}
		if (this.lookupIndex == this.previousLookupIndex && this.firstSubmesh == this.previousFirstSubmesh && this.subMeshCount == this.previousSubmeshCount)
		{
			return;
		}
		this.SetData();
		this.previousLookupIndex = this.lookupIndex;
		this.previousFirstSubmesh = this.firstSubmesh;
		this.previousSubmeshCount = this.subMeshCount;
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000EFDAD File Offset: 0x000EDFAD
	private void SetData()
	{
		base.GetComponent<MeshFilter>().mesh = this.sceneData.bakedMeshes[this.lookupIndex];
		base.GetComponent<MeshRenderer>();
	}

	// Token: 0x04002856 RID: 10326
	public int lookupIndex;

	// Token: 0x04002857 RID: 10327
	public ushort firstSubmesh;

	// Token: 0x04002858 RID: 10328
	public ushort subMeshCount;

	// Token: 0x04002859 RID: 10329
	public StaticSceneData sceneData;

	// Token: 0x0400285A RID: 10330
	private int previousLookupIndex;

	// Token: 0x0400285B RID: 10331
	private ushort previousFirstSubmesh;

	// Token: 0x0400285C RID: 10332
	private ushort previousSubmeshCount;
}
