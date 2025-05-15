using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DA RID: 1242
public class StaticSceneData : ScriptableObject
{
	// Token: 0x06001C8E RID: 7310 RVA: 0x000EFDD8 File Offset: 0x000EDFD8
	public void ClearData()
	{
		this.mainTexAtlas = null;
		this.blendTexAtlas = null;
		this.backingMeshHashes.Clear();
		this.bakedMeshes.Clear();
		this.mrLightIndices.Clear();
		this.mrMeshIndices.Clear();
		this.firstSubMesh.Clear();
		this.subMeshCount.Clear();
	}

	// Token: 0x0400285D RID: 10333
	public Texture2D mainTexAtlas;

	// Token: 0x0400285E RID: 10334
	public Texture2D blendTexAtlas;

	// Token: 0x0400285F RID: 10335
	public List<Mesh> bakedMeshes = new List<Mesh>();

	// Token: 0x04002860 RID: 10336
	public List<int> backingMeshHashes = new List<int>();

	// Token: 0x04002861 RID: 10337
	public List<int> mrLightIndices = new List<int>();

	// Token: 0x04002862 RID: 10338
	public List<ushort> mrMeshIndices = new List<ushort>();

	// Token: 0x04002863 RID: 10339
	public List<ushort> firstSubMesh = new List<ushort>();

	// Token: 0x04002864 RID: 10340
	public List<ushort> subMeshCount = new List<ushort>();
}
