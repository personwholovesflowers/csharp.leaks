using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200006B RID: 107
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
[DefaultExecutionOrder(-10)]
public class AudioMixerController : MonoSingleton<AudioMixerController>
{
	// Token: 0x06000201 RID: 513 RVA: 0x0000A7F4 File Offset: 0x000089F4
	private void Start()
	{
		this.sfxVolume = MonoSingleton<PrefsManager>.Instance.GetFloat("sfxVolume", 0f);
		this.SetSFXVolume(this.sfxVolume);
		this.optionsMusicVolume = MonoSingleton<PrefsManager>.Instance.GetFloat("musicVolume", 0f);
		this.SetMusicVolume(this.optionsMusicVolume);
		this.muffleMusic = MonoSingleton<PrefsManager>.Instance.GetBool("muffleMusic", false);
		if (!this.forceOff)
		{
			this.SetSFXVolume(this.sfxVolume);
			this.musicSound.SetFloat("allVolume", this.CalculateVolume(this.optionsMusicVolume));
		}
		this.IsInWater(false);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000A89B File Offset: 0x00008A9B
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000A8C3 File Offset: 0x00008AC3
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000A8E8 File Offset: 0x00008AE8
	private void OnPrefChanged(string key, object value)
	{
		if (!(key == "allVolume"))
		{
			if (!(key == "sfxVolume"))
			{
				if (!(key == "musicVolume"))
				{
					if (!(key == "muffleMusic"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag = (bool)value;
						this.MuffleMusic(flag);
					}
				}
				else if (value is float)
				{
					float num = (float)value;
					this.optionsMusicVolume = num;
					this.SetMusicVolume(num);
					return;
				}
			}
			else if (value is float)
			{
				float num2 = (float)value;
				this.SetSFXVolume(num2);
				return;
			}
			return;
		}
		AudioListener.volume = (float)value;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000A983 File Offset: 0x00008B83
	private void Update()
	{
		if (this.musicVolume > this.optionsMusicVolume)
		{
			this.SetMusicVolume(this.optionsMusicVolume);
		}
		this.UpdateSFXVolume();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000A9A5 File Offset: 0x00008BA5
	public void SetMusicVolume(float volume)
	{
		if (!this.forceOff)
		{
			this.musicSound.SetFloat("allVolume", this.CalculateVolume(volume));
		}
		this.musicVolume = volume;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000A9CE File Offset: 0x00008BCE
	public void SetSFXVolume(float volume)
	{
		this.sfxVolume = volume;
		this.UpdateSFXVolume();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000A9E0 File Offset: 0x00008BE0
	public void UpdateSFXVolume()
	{
		float num;
		if (!this.forceOff)
		{
			this.allSound.SetFloat("allVolume", this.CalculateVolume((this.allSound.GetFloat("allPitch", out num) && num == 0f) ? 0f : this.sfxVolume) + this.temporaryDipAmount);
		}
		this.goreSound.SetFloat("allVolume", this.CalculateVolume((this.goreSound.GetFloat("allPitch", out num) && num == 0f) ? 0f : this.sfxVolume) + this.temporaryDipAmount);
		this.doorSound.SetFloat("allVolume", this.CalculateVolume((this.doorSound.GetFloat("allPitch", out num) && num == 0f) ? 0f : this.sfxVolume) + this.temporaryDipAmount);
		this.unfreezeableSound.SetFloat("allVolume", this.CalculateVolume((this.unfreezeableSound.GetFloat("allPitch", out num) && num == 0f) ? 0f : this.sfxVolume) + this.temporaryDipAmount);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000AB11 File Offset: 0x00008D11
	public void TemporaryDip(float amount)
	{
		this.temporaryDipAmount = amount;
		this.SetSFXVolume(this.sfxVolume);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000AB26 File Offset: 0x00008D26
	public float CalculateVolume(float volume)
	{
		if (volume > 0f)
		{
			return Mathf.Log10(volume) * 20f;
		}
		return -80f;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000AB44 File Offset: 0x00008D44
	public void IsInWater(bool isInWater)
	{
		float num = (float)(isInWater ? 0 : (-80));
		this.isUnderWater = isInWater;
		this.allSound.SetFloat("lowPassVolume", num);
		if (this.muffleMusic || !isInWater)
		{
			this.musicSound.SetFloat("lowPassVolume", num);
		}
		this.goreSound.SetFloat("lowPassVolume", num);
		this.doorSound.SetFloat("lowPassVolume", num);
		this.unfreezeableSound.SetFloat("lowPassVolume", num);
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000ABC8 File Offset: 0x00008DC8
	public void MuffleMusic(bool isOn)
	{
		this.muffleMusic = isOn;
		if (!isOn)
		{
			this.musicSound.SetFloat("lowPassVolume", -80f);
			return;
		}
		if (this.isUnderWater)
		{
			this.musicSound.SetFloat("lowPassVolume", 0f);
		}
	}

	// Token: 0x04000222 RID: 546
	[Header("Mixers")]
	public AudioMixer allSound;

	// Token: 0x04000223 RID: 547
	public AudioMixer goreSound;

	// Token: 0x04000224 RID: 548
	public AudioMixer musicSound;

	// Token: 0x04000225 RID: 549
	public AudioMixer doorSound;

	// Token: 0x04000226 RID: 550
	public AudioMixer unfreezeableSound;

	// Token: 0x04000227 RID: 551
	[Header("Mixer Groups")]
	public AudioMixerGroup allGroup;

	// Token: 0x04000228 RID: 552
	public AudioMixerGroup goreGroup;

	// Token: 0x04000229 RID: 553
	public AudioMixerGroup musicGroup;

	// Token: 0x0400022A RID: 554
	public AudioMixerGroup doorGroup;

	// Token: 0x0400022B RID: 555
	public AudioMixerGroup unfreezeableGroup;

	// Token: 0x0400022C RID: 556
	[HideInInspector]
	public float sfxVolume;

	// Token: 0x0400022D RID: 557
	[HideInInspector]
	public float musicVolume;

	// Token: 0x0400022E RID: 558
	[HideInInspector]
	public float optionsMusicVolume;

	// Token: 0x0400022F RID: 559
	[HideInInspector]
	public bool muffleMusic;

	// Token: 0x04000230 RID: 560
	[Space]
	public bool forceOff;

	// Token: 0x04000231 RID: 561
	private float temporaryDipAmount;

	// Token: 0x04000232 RID: 562
	private bool isUnderWater;
}
