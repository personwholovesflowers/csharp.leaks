using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class SimpleGPUInstancingExample : MonoBehaviour
{
	// Token: 0x06000052 RID: 82 RVA: 0x000047A8 File Offset: 0x000029A8
	private void Awake()
	{
		this.InstancedMaterial.enableInstancing = true;
		float num = 4f;
		for (int i = 0; i < 1000; i++)
		{
			Component component = Object.Instantiate<Transform>(this.Prefab, new Vector3(Random.Range(-num, num), num + Random.Range(-num, num), Random.Range(-num, num)), Quaternion.identity);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			materialPropertyBlock.SetColor("_Color", color);
			component.GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x04000072 RID: 114
	public Transform Prefab;

	// Token: 0x04000073 RID: 115
	public Material InstancedMaterial;
}
