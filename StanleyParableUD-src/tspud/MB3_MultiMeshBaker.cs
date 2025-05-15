using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class MB3_MultiMeshBaker : MB3_MeshBakerCommon
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000292 RID: 658 RVA: 0x000113AB File Offset: 0x0000F5AB
	public override MB3_MeshCombiner meshCombiner
	{
		get
		{
			return this._meshCombiner;
		}
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000113B4 File Offset: 0x0000F5B4
	public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource)
	{
		if (this._meshCombiner.resultSceneObject == null)
		{
			this._meshCombiner.resultSceneObject = new GameObject("CombinedMesh-" + base.name);
		}
		this.meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjects(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00011420 File Offset: 0x0000F620
	public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOs, bool disableRendererInSource)
	{
		if (this._meshCombiner.resultSceneObject == null)
		{
			this._meshCombiner.resultSceneObject = new GameObject("CombinedMesh-" + base.name);
		}
		this.meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjectsByID(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x04000291 RID: 657
	[SerializeField]
	protected MB3_MultiMeshCombiner _meshCombiner = new MB3_MultiMeshCombiner();
}
