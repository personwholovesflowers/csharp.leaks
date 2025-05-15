using System;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public class SoundLoopWithIntro : MonoBehaviour
{
	// Token: 0x060017C3 RID: 6083 RVA: 0x000C2C40 File Offset: 0x000C0E40
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (this.aud == null)
		{
			base.enabled = false;
			return;
		}
		this.aud.clip = this.intro;
		this.aud.loop = false;
		this.aud.Play();
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000C2C98 File Offset: 0x000C0E98
	private void Update()
	{
		if (this.aud == null)
		{
			base.enabled = false;
			return;
		}
		if (!this.introOver && (!this.aud.isPlaying || this.aud.time > this.aud.clip.length - 0.1f))
		{
			this.introOver = true;
			this.aud.clip = this.loop;
			this.aud.loop = true;
			this.aud.Play();
		}
	}

	// Token: 0x0400213A RID: 8506
	private AudioSource aud;

	// Token: 0x0400213B RID: 8507
	[SerializeField]
	private AudioClip intro;

	// Token: 0x0400213C RID: 8508
	[SerializeField]
	private AudioClip loop;

	// Token: 0x0400213D RID: 8509
	private bool introOver;
}
