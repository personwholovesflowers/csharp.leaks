using System;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class RandomPitch : MonoBehaviour
{
	// Token: 0x0600149D RID: 5277 RVA: 0x000A6D2A File Offset: 0x000A4F2A
	private void Start()
	{
		if (this.nailgunOverheatFix)
		{
			this.Randomize();
		}
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000A6D3A File Offset: 0x000A4F3A
	private void OnEnable()
	{
		if (!this.nailgunOverheatFix)
		{
			this.Randomize();
		}
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000A6D4C File Offset: 0x000A4F4C
	public void Randomize()
	{
		if (this.oneTime && this.beenPlayed)
		{
			return;
		}
		this.beenPlayed = true;
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.aud != null)
		{
			if (this.pitchVariation == 0f)
			{
				this.aud.pitch = Random.Range(0.8f, 1.2f);
			}
			else
			{
				this.aud.pitch = Random.Range(this.defaultPitch - this.pitchVariation, this.defaultPitch + this.pitchVariation);
			}
			if (this.playOnEnable)
			{
				this.aud.Play();
			}
		}
	}

	// Token: 0x04001C66 RID: 7270
	public float defaultPitch = 1f;

	// Token: 0x04001C67 RID: 7271
	public float pitchVariation = 0.1f;

	// Token: 0x04001C68 RID: 7272
	public bool oneTime = true;

	// Token: 0x04001C69 RID: 7273
	public bool playOnEnable = true;

	// Token: 0x04001C6A RID: 7274
	public bool nailgunOverheatFix;

	// Token: 0x04001C6B RID: 7275
	[HideInInspector]
	public bool beenPlayed;

	// Token: 0x04001C6C RID: 7276
	public AudioSource aud;
}
