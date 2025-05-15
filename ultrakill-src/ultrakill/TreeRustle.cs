using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000501 RID: 1281
public class TreeRustle : MonoBehaviour
{
	// Token: 0x06001D40 RID: 7488 RVA: 0x000F502C File Offset: 0x000F322C
	private void Start()
	{
		this.propertyBlock = new MaterialPropertyBlock();
		Material sharedMaterial = this.leafRenderer.sharedMaterial;
		this.baseRustleStrength = sharedMaterial.GetFloat("_VertexNoiseAmplitude");
		this.baseRustleSpeed = sharedMaterial.GetFloat("_VertexNoiseSpeed");
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x000F5074 File Offset: 0x000F3274
	private void Update()
	{
		if (this.time <= 1f)
		{
			this.time += Time.deltaTime / this.rustleDuration;
			this.leafRenderer.GetPropertyBlock(this.propertyBlock);
			this.propertyBlock.SetFloat("_VertexNoiseAmplitude", this.baseRustleStrength * Mathf.Lerp(this.rustleStrengthScale, 1f, this.time));
			this.propertyBlock.SetFloat("_VertexNoiseSpeed", this.baseRustleSpeed * Mathf.Lerp(this.rustleSpeedScale, 1f, this.time));
			this.leafRenderer.SetPropertyBlock(this.propertyBlock);
		}
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000F5128 File Offset: 0x000F3328
	public void DoRustle()
	{
		this.time = 0f;
		this.audioClips[Random.Range(0, this.audioClips.Length)].PlayClipAtPoint(this.audioGroup, base.transform.position, 128, 1f, 0.5f, Random.Range(0.8f, 1.1f), AudioRolloffMode.Linear, 1f, 100f);
	}

	// Token: 0x04002971 RID: 10609
	public MeshRenderer leafRenderer;

	// Token: 0x04002972 RID: 10610
	public AudioClip[] audioClips;

	// Token: 0x04002973 RID: 10611
	public AudioMixerGroup audioGroup;

	// Token: 0x04002974 RID: 10612
	public float rustleDuration = 1f;

	// Token: 0x04002975 RID: 10613
	public float rustleStrengthScale = 2f;

	// Token: 0x04002976 RID: 10614
	public float rustleSpeedScale = 2f;

	// Token: 0x04002977 RID: 10615
	private float baseRustleStrength;

	// Token: 0x04002978 RID: 10616
	private float baseRustleSpeed;

	// Token: 0x04002979 RID: 10617
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x0400297A RID: 10618
	private float time = 1f;
}
