using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class DistantThunder : MonoBehaviour
{
	// Token: 0x06000530 RID: 1328 RVA: 0x0002276C File Offset: 0x0002096C
	private void Start()
	{
		this.rend = base.GetComponent<Renderer>();
		this.aud = base.GetComponent<AudioSource>();
		this.lights = base.GetComponentsInChildren<Light>();
		if (this.lights.Length != 0)
		{
			this.lightIntensities = new float[this.lights.Length];
			for (int i = 0; i < this.lightIntensities.Length; i++)
			{
				this.lightIntensities[i] = this.lights[i].intensity;
				this.lights[i].intensity = 0f;
			}
		}
		if (this.aud)
		{
			this.originalPitch = this.aud.pitch;
		}
		this.block = new MaterialPropertyBlock();
		this.UpdateColor();
		base.Invoke("Thunder", (this.firstTimeDelay >= 0f) ? this.firstTimeDelay : (this.delay + Random.Range(-this.delayRandomizer, this.delayRandomizer)));
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0002285B File Offset: 0x00020A5B
	private void Update()
	{
		if (this.fade > 0f)
		{
			this.fade = Mathf.MoveTowards(this.fade, 0f, this.fadeSpeed * Time.deltaTime);
			this.UpdateColor();
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00022894 File Offset: 0x00020A94
	private void Thunder()
	{
		this.fade = 1f;
		this.UpdateColor();
		if (this.aud)
		{
			this.aud.pitch = this.originalPitch + Random.Range(-0.1f, 0.1f);
			this.aud.Play();
		}
		base.Invoke("Thunder", this.delay + Random.Range(-this.delayRandomizer, this.delayRandomizer));
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00022910 File Offset: 0x00020B10
	private void UpdateColor()
	{
		if (this.rend)
		{
			this.clr = Color.white * this.fade;
			this.rend.GetPropertyBlock(this.block, 0);
			this.block.SetColor(UKShaderProperties.Color, this.clr);
			this.rend.SetPropertyBlock(this.block, 0);
		}
		if (this.lights != null && this.lights.Length != 0)
		{
			for (int i = 0; i < this.lightIntensities.Length; i++)
			{
				this.lights[i].intensity = this.lightIntensities[i] * this.fade;
			}
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x000229BA File Offset: 0x00020BBA
	public void ForceThunder()
	{
		base.CancelInvoke("Thunder");
		this.Thunder();
	}

	// Token: 0x04000719 RID: 1817
	private Renderer rend;

	// Token: 0x0400071A RID: 1818
	private Light[] lights;

	// Token: 0x0400071B RID: 1819
	private float[] lightIntensities;

	// Token: 0x0400071C RID: 1820
	private AudioSource aud;

	// Token: 0x0400071D RID: 1821
	public float delay;

	// Token: 0x0400071E RID: 1822
	public float delayRandomizer;

	// Token: 0x0400071F RID: 1823
	public float firstTimeDelay = -1f;

	// Token: 0x04000720 RID: 1824
	public float fadeSpeed = 1f;

	// Token: 0x04000721 RID: 1825
	private MaterialPropertyBlock block;

	// Token: 0x04000722 RID: 1826
	private Color clr;

	// Token: 0x04000723 RID: 1827
	private float fade;

	// Token: 0x04000724 RID: 1828
	private float originalPitch;
}
