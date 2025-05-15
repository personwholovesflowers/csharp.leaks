using System;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class SparkFX : MonoBehaviour
{
	// Token: 0x06000B28 RID: 2856 RVA: 0x00033D63 File Offset: 0x00031F63
	private void OnValidate()
	{
		this.particleSystemFX = base.GetComponent<ParticleSystem>();
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x04000AF6 RID: 2806
	public ParticleSystem particleSystemFX;

	// Token: 0x04000AF7 RID: 2807
	public AudioSource audioSource;

	// Token: 0x04000AF8 RID: 2808
	public AudioClip[] audioClipSet;
}
