using System;
using UnityEngine;

// Token: 0x02000374 RID: 884
public class RandomAudioclip : MonoBehaviour
{
	// Token: 0x0600148C RID: 5260 RVA: 0x000A6A70 File Offset: 0x000A4C70
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x000A6A7E File Offset: 0x000A4C7E
	private void OnEnable()
	{
		if (this.activateOnEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x000A6A8E File Offset: 0x000A4C8E
	public void Activate()
	{
		this.aud.clip = this.clips[Random.Range(0, this.clips.Length)];
		if (this.playOnChange)
		{
			this.aud.Play();
		}
	}

	// Token: 0x04001C4E RID: 7246
	public AudioClip[] clips;

	// Token: 0x04001C4F RID: 7247
	private AudioSource aud;

	// Token: 0x04001C50 RID: 7248
	public bool playOnChange;

	// Token: 0x04001C51 RID: 7249
	public bool activateOnEnable = true;
}
