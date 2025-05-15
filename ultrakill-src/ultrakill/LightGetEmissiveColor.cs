using System;
using UnityEngine;

// Token: 0x020002CD RID: 717
public class LightGetEmissiveColor : MonoBehaviour
{
	// Token: 0x06000FA2 RID: 4002 RVA: 0x00074348 File Offset: 0x00072548
	private void Start()
	{
		this.lit = base.GetComponent<Light>();
		this.block = new MaterialPropertyBlock();
		this.targetRenderer.GetPropertyBlock(this.block);
		this.lit.color = this.block.GetColor(UKShaderProperties.EmissiveColor);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00074398 File Offset: 0x00072598
	private void Update()
	{
		this.targetRenderer.GetPropertyBlock(this.block);
		this.lit.color = this.block.GetColor(UKShaderProperties.EmissiveColor);
	}

	// Token: 0x04001504 RID: 5380
	private Light lit;

	// Token: 0x04001505 RID: 5381
	[SerializeField]
	private MeshRenderer targetRenderer;

	// Token: 0x04001506 RID: 5382
	private MaterialPropertyBlock block;
}
