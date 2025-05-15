using System;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class PulsateEmissiveMaterial : MonoBehaviour
{
	// Token: 0x0600141B RID: 5147 RVA: 0x000A1A3F File Offset: 0x0009FC3F
	private void Start()
	{
		if (!this.valuesSet)
		{
			this.SetValues();
		}
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000A1A50 File Offset: 0x0009FC50
	private void Update()
	{
		this.currentIntensity = Mathf.MoveTowards(this.currentIntensity, this.targetIntensity, this.pulseSpeed * Time.deltaTime);
		if (this.currentIntensity == this.targetIntensity)
		{
			this.targetIntensity = ((this.targetIntensity > this.defaultIntensity) ? (this.defaultIntensity - this.intensityRange) : (this.defaultIntensity + this.intensityRange));
		}
		for (int i = 0; i < this.sharedMaterials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			this.block.SetFloat(this.emissiveID, this.currentIntensity);
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000A1B0C File Offset: 0x0009FD0C
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.rend = base.GetComponent<MeshRenderer>();
		this.sharedMaterials = this.rend.sharedMaterials;
		if (this.block == null)
		{
			this.block = new MaterialPropertyBlock();
		}
		this.emissiveID = Shader.PropertyToID("_EmissiveIntensity");
		for (int i = 0; i < this.sharedMaterials.Length; i++)
		{
			if (this.sharedMaterials[i] != null && this.sharedMaterials[i].HasProperty(this.emissiveID))
			{
				this.defaultIntensity = this.sharedMaterials[i].GetFloat(this.emissiveID);
				this.currentIntensity = this.defaultIntensity;
				this.targetIntensity = ((Random.Range(0, 2) == 1) ? (this.targetIntensity + this.intensityRange) : (this.targetIntensity - this.intensityRange));
				return;
			}
		}
	}

	// Token: 0x04001B99 RID: 7065
	[HideInInspector]
	public bool valuesSet;

	// Token: 0x04001B9A RID: 7066
	[HideInInspector]
	public MeshRenderer rend;

	// Token: 0x04001B9B RID: 7067
	[HideInInspector]
	public Material[] sharedMaterials;

	// Token: 0x04001B9C RID: 7068
	[HideInInspector]
	public MaterialPropertyBlock block;

	// Token: 0x04001B9D RID: 7069
	[HideInInspector]
	public int emissiveID;

	// Token: 0x04001B9E RID: 7070
	[HideInInspector]
	public float defaultIntensity;

	// Token: 0x04001B9F RID: 7071
	[HideInInspector]
	public float targetIntensity;

	// Token: 0x04001BA0 RID: 7072
	[HideInInspector]
	public float currentIntensity;

	// Token: 0x04001BA1 RID: 7073
	public float intensityRange;

	// Token: 0x04001BA2 RID: 7074
	public float pulseSpeed;
}
