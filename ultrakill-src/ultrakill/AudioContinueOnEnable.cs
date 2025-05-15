using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class AudioContinueOnEnable : MonoBehaviour
{
	// Token: 0x060001FE RID: 510 RVA: 0x0000A754 File Offset: 0x00008954
	private void OnEnable()
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (!this.aud.isPlaying && (this.autoStartIfNotPlaying || this.wasPlaying))
		{
			this.aud.Play();
			this.aud.time = this.currentTime;
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000A7B3 File Offset: 0x000089B3
	private void Update()
	{
		if (this.aud.isPlaying)
		{
			this.currentTime = this.aud.time;
			this.wasPlaying = true;
			return;
		}
		this.wasPlaying = false;
	}

	// Token: 0x0400021E RID: 542
	public bool autoStartIfNotPlaying = true;

	// Token: 0x0400021F RID: 543
	private bool wasPlaying;

	// Token: 0x04000220 RID: 544
	private float currentTime;

	// Token: 0x04000221 RID: 545
	private AudioSource aud;
}
