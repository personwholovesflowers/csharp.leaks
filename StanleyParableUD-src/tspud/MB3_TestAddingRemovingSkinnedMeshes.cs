using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class MB3_TestAddingRemovingSkinnedMeshes : MonoBehaviour
{
	// Token: 0x060002C5 RID: 709 RVA: 0x00011E59 File Offset: 0x00010059
	private void Start()
	{
		base.StartCoroutine(this.TestScript());
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00011E68 File Offset: 0x00010068
	private IEnumerator TestScript()
	{
		Debug.Log("Test 1 adding 0,1,2");
		GameObject[] array = new GameObject[]
		{
			this.g[0],
			this.g[1],
			this.g[2]
		};
		this.meshBaker.AddDeleteGameObjects(array, null, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 2 remove 1 and add 3,4,5");
		GameObject[] array2 = new GameObject[] { this.g[1] };
		array = new GameObject[]
		{
			this.g[3],
			this.g[4],
			this.g[5]
		};
		this.meshBaker.AddDeleteGameObjects(array, array2, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 remove 0,2,5 and add 1");
		array2 = new GameObject[]
		{
			this.g[3],
			this.g[4],
			this.g[5]
		};
		array = new GameObject[] { this.g[1] };
		this.meshBaker.AddDeleteGameObjects(array, array2, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 remove all remaining");
		array2 = new GameObject[]
		{
			this.g[0],
			this.g[1],
			this.g[2]
		};
		this.meshBaker.AddDeleteGameObjects(null, array2, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(3f);
		Debug.Log("Test 3 add all");
		this.meshBaker.AddDeleteGameObjects(this.g, null, true);
		this.meshBaker.Apply(null);
		this.meshBaker.meshCombiner.CheckIntegrity();
		yield return new WaitForSeconds(1f);
		Debug.Log("Done");
		yield break;
	}

	// Token: 0x040002B0 RID: 688
	public MB3_MeshBaker meshBaker;

	// Token: 0x040002B1 RID: 689
	public GameObject[] g;
}
