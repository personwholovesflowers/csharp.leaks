using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class LightPillar : MonoBehaviour
{
	// Token: 0x06000FB7 RID: 4023 RVA: 0x00075070 File Offset: 0x00073270
	private void Start()
	{
		this.block = new MaterialPropertyBlock();
		if (!this.activated && !this.completed)
		{
			if (!this.gotValues)
			{
				this.gotValues = true;
				this.lights = base.GetComponentsInChildren<Light>();
				this.aud = base.GetComponentInChildren<AudioSource>();
				this.origScale = base.transform.localScale;
				this.origPitch = this.aud.pitch + Random.Range(-0.1f, 0.1f);
				if (this.lights.Length != 0)
				{
					this.lightRange = this.lights[0].range;
					foreach (Light light in this.lights)
					{
						if (light.gameObject != this.tipGlow)
						{
							light.range = 0f;
						}
					}
				}
				for (int j = 0; j < this.emissivesToLightUp.Length; j++)
				{
					for (int k = 0; k < this.emissivesToLightUp[j].sharedMaterials.Length; k++)
					{
						if (this.emissivesToLightUp[j].sharedMaterials[k].HasFloat(UKShaderProperties.EmissiveIntensity))
						{
							this.emissiveStrengths.Add(this.emissivesToLightUp[j].sharedMaterials[k].GetFloat(UKShaderProperties.EmissiveIntensity));
							this.emissivesToLightUp[j].GetPropertyBlock(this.block, k);
							this.block.SetFloat(UKShaderProperties.EmissiveIntensity, 0f);
							this.emissivesToLightUp[j].SetPropertyBlock(this.block, k);
						}
					}
				}
			}
			else if (this.emissivesToLightUp.Length != 0)
			{
				this.SetEmissives(0f);
			}
			this.aud.pitch = 0f;
			base.transform.localScale = Vector3.zero;
		}
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x00075240 File Offset: 0x00073440
	private void Update()
	{
		if (this.activated)
		{
			if (!this.heightDone)
			{
				base.transform.localScale = new Vector3(this.origScale.x / 10f, base.transform.localScale.y + Mathf.Min(this.speed * 5f, this.speed * (this.origScale.y - base.transform.localScale.y) + 0.1f) * Time.deltaTime, this.origScale.z / 10f);
				if (base.transform.localScale.y <= this.origScale.y - 0.1f)
				{
					return;
				}
				base.transform.localScale = new Vector3(this.origScale.x / 10f, this.origScale.y, this.origScale.z / 10f);
				this.heightDone = true;
				this.tipGlow.SetActive(false);
				UltrakillEvent ultrakillEvent = this.onHeightDone;
				if (ultrakillEvent != null)
				{
					ultrakillEvent.Invoke("");
				}
			}
			if (base.transform.localScale != this.origScale)
			{
				base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.origScale, Mathf.Min(this.speed, this.speed * Vector3.Distance(base.transform.localScale, this.origScale) + 0.01f) * Time.deltaTime);
			}
			if (this.lights != null && this.lights.Length != 0 && this.lights[0].range != this.lightRange)
			{
				foreach (Light light in this.lights)
				{
					light.range = Mathf.MoveTowards(light.range, this.lightRange, this.speed * 4f * Time.deltaTime);
				}
				if (this.emissivesToLightUp.Length != 0)
				{
					float num = this.lights[0].range / this.lightRange;
					this.SetEmissives(num);
				}
			}
			if (this.aud.pitch != this.origPitch)
			{
				this.aud.pitch = Mathf.MoveTowards(this.aud.pitch, this.origPitch, this.speed / 3f * this.origPitch * Time.deltaTime);
				return;
			}
			if (base.transform.localScale == this.origScale && this.lights[0].range == this.lightRange)
			{
				this.activated = false;
				this.completed = true;
			}
		}
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x000754F8 File Offset: 0x000736F8
	private void SetEmissives(float lerpAmount)
	{
		int num = 0;
		for (int i = 0; i < this.emissivesToLightUp.Length; i++)
		{
			for (int j = 0; j < this.emissivesToLightUp[i].materials.Length; j++)
			{
				this.emissivesToLightUp[i].GetPropertyBlock(this.block, j);
				if (this.block.HasFloat(UKShaderProperties.EmissiveIntensity))
				{
					this.block.SetFloat(UKShaderProperties.EmissiveIntensity, Mathf.Lerp(0f, this.emissiveStrengths[num], lerpAmount));
					this.emissivesToLightUp[i].SetPropertyBlock(this.block, j);
					num++;
				}
			}
		}
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0007559F File Offset: 0x0007379F
	public void ActivatePillar()
	{
		this.activated = true;
		this.tipGlow.SetActive(true);
	}

	// Token: 0x04001535 RID: 5429
	[HideInInspector]
	public bool gotValues;

	// Token: 0x04001536 RID: 5430
	[HideInInspector]
	public bool activated;

	// Token: 0x04001537 RID: 5431
	[HideInInspector]
	public bool completed;

	// Token: 0x04001538 RID: 5432
	[HideInInspector]
	public Light[] lights;

	// Token: 0x04001539 RID: 5433
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x0400153A RID: 5434
	[HideInInspector]
	public Vector3 origScale;

	// Token: 0x0400153B RID: 5435
	[HideInInspector]
	public float lightRange;

	// Token: 0x0400153C RID: 5436
	[HideInInspector]
	public float origPitch;

	// Token: 0x0400153D RID: 5437
	public float speed;

	// Token: 0x0400153E RID: 5438
	public GameObject tipGlow;

	// Token: 0x0400153F RID: 5439
	public Renderer[] emissivesToLightUp;

	// Token: 0x04001540 RID: 5440
	[HideInInspector]
	public List<float> emissiveStrengths = new List<float>();

	// Token: 0x04001541 RID: 5441
	private MaterialPropertyBlock block;

	// Token: 0x04001542 RID: 5442
	private bool heightDone;

	// Token: 0x04001543 RID: 5443
	public UltrakillEvent onHeightDone;
}
