using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class DontBatchRenderer : MonoBehaviour
{
	// Token: 0x06001D28 RID: 7464 RVA: 0x000F4A48 File Offset: 0x000F2C48
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		int num = component.sharedMaterials.Length;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		int num2 = Random.Range(0, 99999999);
		int num3 = 0;
		if (num3 >= num)
		{
			return;
		}
		component.GetPropertyBlock(materialPropertyBlock, num3);
		materialPropertyBlock.SetInteger("_BatchingID", num2 + num3);
		component.SetPropertyBlock(materialPropertyBlock, num3);
	}
}
