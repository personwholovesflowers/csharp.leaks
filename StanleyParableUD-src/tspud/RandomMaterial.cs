using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x0600007C RID: 124 RVA: 0x000055D1 File Offset: 0x000037D1
	public void Start()
	{
		this.ChangeMaterial();
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000055D9 File Offset: 0x000037D9
	public void ChangeMaterial()
	{
		this.targetRenderer.sharedMaterial = this.materials[Random.Range(0, this.materials.Length)];
	}

	// Token: 0x0400009F RID: 159
	public Renderer targetRenderer;

	// Token: 0x040000A0 RID: 160
	public Material[] materials;
}
