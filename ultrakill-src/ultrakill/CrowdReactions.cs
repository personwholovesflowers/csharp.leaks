using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CrowdReactions : MonoSingleton<CrowdReactions>
{
	// Token: 0x060004CB RID: 1227 RVA: 0x00020E5B File Offset: 0x0001F05B
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00020E6C File Offset: 0x0001F06C
	public void React(AudioClip clip)
	{
		if (this.aud.clip != this.cheerLong || !this.aud.isPlaying)
		{
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.clip = clip;
			this.aud.Play();
		}
	}

	// Token: 0x0400067F RID: 1663
	private AudioSource aud;

	// Token: 0x04000680 RID: 1664
	public AudioClip cheer;

	// Token: 0x04000681 RID: 1665
	public AudioClip cheerLong;

	// Token: 0x04000682 RID: 1666
	public AudioClip aww;
}
