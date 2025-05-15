using System;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class ModifyMaterial : MonoBehaviour
{
	// Token: 0x0600116B RID: 4459 RVA: 0x00088034 File Offset: 0x00086234
	public void ChangeEmissionIntensity(float value)
	{
		this.SetValues();
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			this.block.SetFloat(UKShaderProperties.EmissiveIntensity, value);
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x0600116C RID: 4460 RVA: 0x00088094 File Offset: 0x00086294
	public void ChangeEmissionColor(string hex)
	{
		Color color;
		if (ColorUtility.TryParseHtmlString(hex, out color))
		{
			this.ChangeEmissionColor(color);
			return;
		}
		Debug.LogError("Failed to Change Emission Color: " + hex);
	}

	// Token: 0x0600116D RID: 4461 RVA: 0x000880C4 File Offset: 0x000862C4
	public void ChangeEmissionColor(Color clr)
	{
		this.SetValues();
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			this.block.SetColor(UKShaderProperties.EmissiveColor, clr);
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x0600116E RID: 4462 RVA: 0x00088124 File Offset: 0x00086324
	public void ChangeColor(string hex)
	{
		Color color;
		if (ColorUtility.TryParseHtmlString(hex, out color))
		{
			this.ChangeColor(color);
			return;
		}
		Debug.LogError("Failed to Change Color: " + hex);
	}

	// Token: 0x0600116F RID: 4463 RVA: 0x00088154 File Offset: 0x00086354
	public void ChangeColor(Color clr)
	{
		this.SetValues();
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			this.block.SetColor("Color", clr);
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x06001170 RID: 4464 RVA: 0x000881B4 File Offset: 0x000863B4
	public void ChangeColorToWhite()
	{
		this.ChangeColor(Color.white);
	}

	// Token: 0x06001171 RID: 4465 RVA: 0x000881C1 File Offset: 0x000863C1
	public void ChangeColorToBlack()
	{
		this.ChangeColor(Color.black);
	}

	// Token: 0x06001172 RID: 4466 RVA: 0x000881D0 File Offset: 0x000863D0
	public void ChangeAlpha(float value)
	{
		this.SetValues();
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			Color color = this.block.GetColor("Color");
			color.a = value;
			this.block.SetColor("Color", color);
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0008824C File Offset: 0x0008644C
	public void SetSandified(bool sandified)
	{
		this.SetValues();
		for (int i = 0; i < this.rend.materials.Length; i++)
		{
			this.rend.GetPropertyBlock(this.block, i);
			this.block.SetFloat(Shader.PropertyToID("_HasSandBuff"), (float)(sandified ? 1 : 0));
			this.block.SetFloat(Shader.PropertyToID("_Sanded"), (float)(sandified ? 1 : 0));
			this.rend.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x000882D5 File Offset: 0x000864D5
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.block = new MaterialPropertyBlock();
		this.rend = base.GetComponent<Renderer>();
	}

	// Token: 0x040017B8 RID: 6072
	private Renderer rend;

	// Token: 0x040017B9 RID: 6073
	private MaterialPropertyBlock block;

	// Token: 0x040017BA RID: 6074
	private bool valuesSet;
}
