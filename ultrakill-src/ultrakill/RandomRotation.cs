using System;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class RandomRotation : MonoBehaviour
{
	// Token: 0x060014A1 RID: 5281 RVA: 0x000A6E2C File Offset: 0x000A502C
	private void Awake()
	{
		if (this.materials.Length != 0)
		{
			this.mr = base.GetComponent<MeshRenderer>();
			this.mr.material = this.materials[Random.Range(0, this.materials.Length - 1)];
		}
		base.transform.Rotate(Vector3.forward * (float)Random.Range(0, 359), Space.Self);
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Update()
	{
	}

	// Token: 0x04001C6D RID: 7277
	public Material[] materials;

	// Token: 0x04001C6E RID: 7278
	private MeshRenderer mr;
}
