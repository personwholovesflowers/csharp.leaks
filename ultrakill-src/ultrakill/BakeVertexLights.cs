using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class BakeVertexLights : MonoBehaviour
{
	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06001B2B RID: 6955 RVA: 0x000E228B File Offset: 0x000E048B
	// (set) Token: 0x06001B2C RID: 6956 RVA: 0x000E2293 File Offset: 0x000E0493
	public float Strength
	{
		get
		{
			return this._strength;
		}
		set
		{
			if (this._strength != value)
			{
				this._strength = value;
				this.UpdateChannelStrength(this.UVTargetChannel, this._strength);
			}
		}
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000E22B8 File Offset: 0x000E04B8
	private void Start()
	{
		this.rendPropBlocks = new MaterialPropertyBlock[this.bakedRenderers.Count];
		for (int i = 0; i < this.bakedRenderers.Count; i++)
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			this.bakedRenderers[i].GetPropertyBlock(materialPropertyBlock);
			MonoBehaviour.print(materialPropertyBlock.isEmpty);
			this.rendPropBlocks[i] = materialPropertyBlock;
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000E2324 File Offset: 0x000E0524
	private void Update()
	{
		float num = Mathf.Sin(Time.time * 10f) * 0.5f + 0.5f;
		float num2 = Mathf.Sin(Time.time * 10f + 3.14f) * 0.5f + 0.5f;
		for (int i = 0; i < this.bakedRenderers.Count; i++)
		{
			MaterialPropertyBlock materialPropertyBlock = this.rendPropBlocks[i];
			materialPropertyBlock.SetFloat("_BakedLights1Strength", num);
			materialPropertyBlock.SetFloat("_BakedLights2Strength", num2);
			this.bakedRenderers[i].SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000E23BC File Offset: 0x000E05BC
	private void UpdateChannelStrength(int targetChannel, float strength)
	{
		int num = Mathf.Clamp(targetChannel - 2, 1, 6);
		string text = string.Format("_BakedLights{0}Strength", num);
		strength = Mathf.Clamp01(strength);
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		foreach (Renderer renderer in this.bakedRenderers)
		{
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat(text, strength);
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
	}

	// Token: 0x0400265D RID: 9821
	public List<Renderer> bakedRenderers;

	// Token: 0x0400265E RID: 9822
	private MaterialPropertyBlock[] rendPropBlocks;

	// Token: 0x0400265F RID: 9823
	[HideInInspector]
	public int UVTargetChannel = 2;

	// Token: 0x04002660 RID: 9824
	private float _strength;
}
