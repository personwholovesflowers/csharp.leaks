using System;
using UnityEngine;

// Token: 0x0200030D RID: 781
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
[DefaultExecutionOrder(600)]
public class MusicManager : MonoSingleton<MusicManager>
{
	// Token: 0x060011BE RID: 4542 RVA: 0x0008A328 File Offset: 0x00088528
	private new void OnEnable()
	{
		if (this.fadeSpeed == 0f)
		{
			this.fadeSpeed = 1f;
		}
		this.allThemes = base.GetComponentsInChildren<AudioSource>();
		this.defaultVolume = this.volume;
		if (!this.off)
		{
			AudioSource[] array = this.allThemes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
			this.cleanTheme.volume = this.volume;
			this.targetTheme = this.cleanTheme;
		}
		else
		{
			this.targetTheme = base.GetComponent<AudioSource>();
		}
		if (MonoSingleton<AudioMixerController>.Instance.musicSound)
		{
			MonoSingleton<AudioMixerController>.Instance.musicSound.FindSnapshot("Unpaused").TransitionTo(0f);
		}
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0008A3E4 File Offset: 0x000885E4
	private void Update()
	{
		if (!this.off && this.targetTheme.volume != this.volume)
		{
			foreach (AudioSource audioSource in this.allThemes)
			{
				if (audioSource == this.targetTheme)
				{
					if (audioSource.volume > this.volume)
					{
						audioSource.volume = this.volume;
					}
					if (Time.timeScale == 0f)
					{
						audioSource.volume = this.volume;
					}
					else
					{
						audioSource.volume = Mathf.MoveTowards(audioSource.volume, this.volume, this.fadeSpeed * Time.deltaTime);
					}
				}
				else if (Time.timeScale == 0f)
				{
					audioSource.volume = 0f;
				}
				else
				{
					audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, this.fadeSpeed * Time.deltaTime);
				}
			}
			if (this.targetTheme.volume == this.volume)
			{
				foreach (AudioSource audioSource2 in this.allThemes)
				{
					if (audioSource2 != this.targetTheme)
					{
						audioSource2.volume = 0f;
					}
				}
			}
		}
		if (this.filtering)
		{
			float num;
			MonoSingleton<AudioMixerController>.Instance.musicSound.GetFloat("highPassVolume", out num);
			num = Mathf.MoveTowards(num, 0f, 1200f * Time.unscaledDeltaTime);
			MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("highPassVolume", num);
			if (num == 0f)
			{
				this.filtering = false;
			}
		}
		if (this.volume == 0f || this.off)
		{
			foreach (AudioSource audioSource3 in this.allThemes)
			{
				audioSource3.volume = Mathf.MoveTowards(audioSource3.volume, 0f, Time.deltaTime / 5f * this.fadeSpeed);
			}
		}
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0008A5CB File Offset: 0x000887CB
	public void ForceStartBattleMusic()
	{
		this.forcedOff = false;
		this.ArenaMusicStart();
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0008A5DA File Offset: 0x000887DA
	public void ForceStartMusic()
	{
		this.forcedOff = false;
		this.StartMusic();
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0008A5EC File Offset: 0x000887EC
	public void StartMusic()
	{
		if (this.forcedOff || !this.off)
		{
			return;
		}
		foreach (AudioSource audioSource in this.allThemes)
		{
			if (audioSource.clip != null)
			{
				audioSource.Play();
				if (this.off && audioSource.time != 0f)
				{
					audioSource.time = 0f;
				}
			}
		}
		this.off = false;
		if (!this.arenaMode && this.requestedThemes <= 0f)
		{
			this.cleanTheme.volume = this.volume;
			this.targetTheme = this.cleanTheme;
			this.battleTheme.volume = 0f;
			this.bossTheme.volume = 0f;
			return;
		}
		this.battleTheme.volume = this.volume;
		this.targetTheme = this.battleTheme;
		this.cleanTheme.volume = 0f;
		this.bossTheme.volume = 0f;
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x0008A6F0 File Offset: 0x000888F0
	public void PlayBattleMusic()
	{
		if (!this.dontMatch && this.targetTheme != this.battleTheme)
		{
			this.battleTheme.time = this.cleanTheme.time;
		}
		if (this.targetTheme != this.bossTheme)
		{
			this.targetTheme = this.battleTheme;
		}
		this.requestedThemes += 1f;
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x0008A760 File Offset: 0x00088960
	public void PlayCleanMusic()
	{
		this.requestedThemes -= 1f;
		if (this.requestedThemes <= 0f && !this.arenaMode)
		{
			this.requestedThemes = 0f;
			if (!this.dontMatch && this.targetTheme != this.cleanTheme)
			{
				this.cleanTheme.time = this.battleTheme.time;
			}
			if (this.battleTheme.volume == this.volume)
			{
				this.cleanTheme.time = this.battleTheme.time;
			}
			this.targetTheme = this.cleanTheme;
		}
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0008A805 File Offset: 0x00088A05
	public void PlayBossMusic()
	{
		if (this.targetTheme != this.bossTheme)
		{
			this.bossTheme.time = this.cleanTheme.time;
		}
		this.targetTheme = this.bossTheme;
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x0008A83C File Offset: 0x00088A3C
	public void ArenaMusicStart()
	{
		if (this.forcedOff)
		{
			return;
		}
		if (this.off)
		{
			foreach (AudioSource audioSource in this.allThemes)
			{
				if (audioSource.clip != null)
				{
					audioSource.Play();
					if (this.off && audioSource.time != 0f)
					{
						audioSource.time = 0f;
					}
				}
			}
			this.off = false;
			this.battleTheme.volume = this.volume;
			this.targetTheme = this.battleTheme;
		}
		if (!this.battleTheme.isPlaying)
		{
			AudioSource[] array = this.allThemes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
			this.battleTheme.volume = this.volume;
		}
		if (this.targetTheme != this.bossTheme)
		{
			this.targetTheme = this.battleTheme;
		}
		this.arenaMode = true;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x0008A92C File Offset: 0x00088B2C
	public void ArenaMusicEnd()
	{
		this.requestedThemes = 0f;
		this.targetTheme = this.cleanTheme;
		this.arenaMode = false;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x0008A94C File Offset: 0x00088B4C
	public void ForceStopMusic()
	{
		this.forcedOff = true;
		this.StopMusic();
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x0008A95C File Offset: 0x00088B5C
	public void StopMusic()
	{
		this.off = true;
		foreach (AudioSource audioSource in this.allThemes)
		{
			audioSource.volume = 0f;
			audioSource.Stop();
		}
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0008A998 File Offset: 0x00088B98
	public void FilterMusic()
	{
		MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("highPassVolume", -80f);
		base.CancelInvoke("RemoveHighPass");
		MonoSingleton<AudioMixerController>.Instance.musicSound.FindSnapshot("Paused").TransitionTo(0f);
		this.filtering = true;
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0008A9EF File Offset: 0x00088BEF
	public void UnfilterMusic()
	{
		this.filtering = false;
		MonoSingleton<AudioMixerController>.Instance.musicSound.FindSnapshot("Unpaused").TransitionTo(0.5f);
		base.Invoke("RemoveHighPass", 0.5f);
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0008AA26 File Offset: 0x00088C26
	private void RemoveHighPass()
	{
		MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("highPassVolume", -80f);
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x0008AA42 File Offset: 0x00088C42
	public bool IsInBattle()
	{
		return this.arenaMode || this.requestedThemes > 0f;
	}

	// Token: 0x04001831 RID: 6193
	public bool off;

	// Token: 0x04001832 RID: 6194
	public bool dontMatch;

	// Token: 0x04001833 RID: 6195
	public bool useBossTheme;

	// Token: 0x04001834 RID: 6196
	public AudioSource battleTheme;

	// Token: 0x04001835 RID: 6197
	public AudioSource cleanTheme;

	// Token: 0x04001836 RID: 6198
	public AudioSource bossTheme;

	// Token: 0x04001837 RID: 6199
	public AudioSource targetTheme;

	// Token: 0x04001838 RID: 6200
	private AudioSource[] allThemes;

	// Token: 0x04001839 RID: 6201
	public float volume = 1f;

	// Token: 0x0400183A RID: 6202
	public float requestedThemes;

	// Token: 0x0400183B RID: 6203
	private bool arenaMode;

	// Token: 0x0400183C RID: 6204
	private float defaultVolume;

	// Token: 0x0400183D RID: 6205
	public float fadeSpeed;

	// Token: 0x0400183E RID: 6206
	public bool forcedOff;

	// Token: 0x0400183F RID: 6207
	private bool filtering;
}
