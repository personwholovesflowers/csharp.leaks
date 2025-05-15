using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class RandomSoundPlayer : MonoBehaviour
{
	// Token: 0x060014A4 RID: 5284 RVA: 0x000A6E92 File Offset: 0x000A5092
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000A6EA0 File Offset: 0x000A50A0
	private void Update()
	{
		if (this.playing && this.fade < this.volume - 0.1f)
		{
			this.fade += Time.deltaTime / 10f;
		}
		else if (this.playing && this.fade > this.volume + 0.1f)
		{
			this.fade -= Time.deltaTime / 10f;
		}
		else if (!this.playing && this.fade > 0f)
		{
			this.fade -= Time.deltaTime / 10f;
		}
		else if (!this.playing && this.fade < 0f)
		{
			this.fade = 0f;
		}
		this.aud.volume = this.fade;
		if (this.playing && this.aud.pitch > this.targetPitch + 0.1f)
		{
			this.aud.pitch -= Time.deltaTime / 10f;
			return;
		}
		if (this.playing && this.aud.pitch < this.targetPitch - 0.1f)
		{
			this.aud.pitch += Time.deltaTime / 10f;
		}
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x000A6FFC File Offset: 0x000A51FC
	public void RollForPlay()
	{
		if (Random.Range(0f, 1f) >= 0.5f && !this.playing)
		{
			this.PlayRandomSound();
			return;
		}
		if (this.playing)
		{
			if (Random.Range(0f, 1f) >= 0.7f)
			{
				this.PlayRandomSound();
			}
			if (Random.Range(0f, 1f) >= 0.7f)
			{
				this.targetPitch = Random.Range(0.2f, 3f);
			}
			if (Random.Range(0f, 1f) >= 0.7f)
			{
				this.volume = Random.Range(0.1f, 0.7f);
			}
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000A70A8 File Offset: 0x000A52A8
	private void PlayRandomSound()
	{
		if (!this.playing)
		{
			this.playing = true;
			this.aud.clip = this.sounds[Random.Range(0, this.sounds.Length)];
			this.volume = Random.Range(0.1f, 0.5f);
			this.aud.pitch = Random.Range(0.2f, 3f);
			this.targetPitch = this.aud.pitch;
			this.aud.Play();
			return;
		}
		this.playing = false;
	}

	// Token: 0x04001C6F RID: 7279
	public AudioClip[] sounds;

	// Token: 0x04001C70 RID: 7280
	private AudioSource aud;

	// Token: 0x04001C71 RID: 7281
	private float volume;

	// Token: 0x04001C72 RID: 7282
	public bool playing;

	// Token: 0x04001C73 RID: 7283
	private float fade;

	// Token: 0x04001C74 RID: 7284
	private float targetPitch;
}
