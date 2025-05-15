using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class MB3_MeshBaker : MB3_MeshBakerCommon
{
	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600026D RID: 621 RVA: 0x00010C50 File Offset: 0x0000EE50
	public override MB3_MeshCombiner meshCombiner
	{
		get
		{
			return this._meshCombiner;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00010C58 File Offset: 0x0000EE58
	public void BuildSceneMeshObject()
	{
		this._meshCombiner.BuildSceneMeshObject(null, false);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00010C67 File Offset: 0x0000EE67
	public virtual bool ShowHide(GameObject[] gos, GameObject[] deleteGOs)
	{
		return this._meshCombiner.ShowHideGameObjects(gos, deleteGOs);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00010C76 File Offset: 0x0000EE76
	public virtual void ApplyShowHide()
	{
		this._meshCombiner.ApplyShowHide();
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00010C83 File Offset: 0x0000EE83
	public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource)
	{
		this._meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjects(gos, deleteGOs, disableRendererInSource);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00010CAE File Offset: 0x0000EEAE
	public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource)
	{
		this._meshCombiner.name = base.name + "-mesh";
		return this._meshCombiner.AddDeleteGameObjectsByID(gos, deleteGOinstanceIDs, disableRendererInSource);
	}

	// Token: 0x04000286 RID: 646
	[SerializeField]
	protected MB3_MeshCombinerSingle _meshCombiner = new MB3_MeshCombinerSingle();
}
