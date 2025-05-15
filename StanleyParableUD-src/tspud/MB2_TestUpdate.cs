using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class MB2_TestUpdate : MonoBehaviour
{
	// Token: 0x060002C2 RID: 706 RVA: 0x00011C38 File Offset: 0x0000FE38
	private void Start()
	{
		this.meshbaker.AddDeleteGameObjects(this.objsToMove, null, true);
		this.meshbaker.AddDeleteGameObjects(new GameObject[] { this.objWithChangingUVs }, null, true);
		MeshFilter meshFilter = this.objWithChangingUVs.GetComponent<MeshFilter>();
		this.m = meshFilter.sharedMesh;
		this.uvs = this.m.uv;
		this.meshbaker.Apply(null);
		this.multiMeshBaker.AddDeleteGameObjects(this.objsToMove, null, true);
		this.multiMeshBaker.AddDeleteGameObjects(new GameObject[] { this.objWithChangingUVs }, null, true);
		meshFilter = this.objWithChangingUVs.GetComponent<MeshFilter>();
		this.m = meshFilter.sharedMesh;
		this.uvs = this.m.uv;
		this.multiMeshBaker.Apply(null);
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00011D14 File Offset: 0x0000FF14
	private void LateUpdate()
	{
		this.meshbaker.UpdateGameObjects(this.objsToMove, false, true, true, true, false, false, false, false, false);
		Vector2[] array = this.m.uv;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Mathf.Sin(Time.time) * this.uvs[i];
		}
		this.m.uv = array;
		this.meshbaker.UpdateGameObjects(new GameObject[] { this.objWithChangingUVs }, true, true, true, true, true, false, false, false, false);
		this.meshbaker.Apply(false, true, true, true, true, false, false, false, false, false, false, null);
		this.multiMeshBaker.UpdateGameObjects(this.objsToMove, false, true, true, true, false, false, false, false, false);
		array = this.m.uv;
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = Mathf.Sin(Time.time) * this.uvs[j];
		}
		this.m.uv = array;
		this.multiMeshBaker.UpdateGameObjects(new GameObject[] { this.objWithChangingUVs }, true, true, true, true, true, false, false, false, false);
		this.multiMeshBaker.Apply(false, true, true, true, true, false, false, false, false, false, false, null);
	}

	// Token: 0x040002AA RID: 682
	public MB3_MeshBaker meshbaker;

	// Token: 0x040002AB RID: 683
	public MB3_MultiMeshBaker multiMeshBaker;

	// Token: 0x040002AC RID: 684
	public GameObject[] objsToMove;

	// Token: 0x040002AD RID: 685
	public GameObject objWithChangingUVs;

	// Token: 0x040002AE RID: 686
	private Vector2[] uvs;

	// Token: 0x040002AF RID: 687
	private Mesh m;
}
