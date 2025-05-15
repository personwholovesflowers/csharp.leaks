using System;
using UnityEngine;

// Token: 0x02000370 RID: 880
public class Radio : MonoBehaviour
{
	// Token: 0x06001473 RID: 5235 RVA: 0x000A5A04 File Offset: 0x000A3C04
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (this.randomizeOrder)
		{
			for (int i = this.songs.Length - 1; i >= 0; i--)
			{
				int num = Random.Range(0, this.songs.Length);
				AudioClip audioClip = this.songs[i];
				this.songs[i] = this.songs[num];
				this.songs[num] = audioClip;
			}
		}
		this.currentSong = Random.Range(0, this.songs.Length);
		this.aud.clip = this.songs[this.currentSong];
		this.aud.Play();
		if (!this.dontStartFromMiddle)
		{
			this.aud.time = Random.Range(0f, this.aud.clip.length);
		}
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x000A5ACF File Offset: 0x000A3CCF
	private void Update()
	{
		if (this.aud.time >= this.aud.clip.length - 0.01f || !this.aud.isPlaying)
		{
			this.NextSong();
		}
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000A5B08 File Offset: 0x000A3D08
	public void NextSong()
	{
		this.currentSong++;
		if (this.currentSong >= this.songs.Length)
		{
			this.currentSong = 0;
		}
		this.aud.clip = this.songs[this.currentSong];
		this.aud.time = 0f;
		this.aud.Play();
	}

	// Token: 0x04001C1A RID: 7194
	public AudioClip[] songs;

	// Token: 0x04001C1B RID: 7195
	private AudioSource aud;

	// Token: 0x04001C1C RID: 7196
	private int currentSong;

	// Token: 0x04001C1D RID: 7197
	public bool dontStartFromMiddle;

	// Token: 0x04001C1E RID: 7198
	public bool randomizeOrder;
}
