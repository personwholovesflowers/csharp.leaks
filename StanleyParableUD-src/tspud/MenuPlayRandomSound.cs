using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class MenuPlayRandomSound : MonoBehaviour
{
	// Token: 0x06000739 RID: 1849 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00025954 File Offset: 0x00023B54
	private void Update()
	{
		this.musicSource.pitch = Mathf.Lerp(this.musicSource.pitch, 1f, this.tapeStopRate * Time.deltaTime);
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00025982 File Offset: 0x00023B82
	public void PlayRandomHoverSfx()
	{
		this.PlayRandomSfx(this.hoverSounds);
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00025990 File Offset: 0x00023B90
	public void PlayRandomClickSfx()
	{
		this.PlayRandomSfx(this.clickSounds);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0002599E File Offset: 0x00023B9E
	public void PlayRandomReturnSfx()
	{
		this.PlayRandomSfx(this.returnSounds);
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000259AC File Offset: 0x00023BAC
	private void PlayRandomSfx(SoundCollection collection)
	{
		if (this.randomizePitch)
		{
			this.sfxSource.pitch = Random.Range(1f, 1f + this.pitchRange);
		}
		AudioClip randomClip = collection.GetRandomClip();
		if (randomClip != null)
		{
			this.sfxSource.clip = randomClip;
			this.sfxSource.Play();
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x00025A0C File Offset: 0x00023C0C
	public void TapeStop()
	{
		float pitch = this.musicSource.pitch;
		float num = 1f - this.tapeStopIntensity;
		this.musicSource.pitch = num;
	}

	// Token: 0x04000760 RID: 1888
	public SoundCollection hoverSounds;

	// Token: 0x04000761 RID: 1889
	public SoundCollection clickSounds;

	// Token: 0x04000762 RID: 1890
	public SoundCollection returnSounds;

	// Token: 0x04000763 RID: 1891
	public AudioSource sfxSource;

	// Token: 0x04000764 RID: 1892
	public AudioSource musicSource;

	// Token: 0x04000765 RID: 1893
	public float pitchRange;

	// Token: 0x04000766 RID: 1894
	public bool randomizePitch;

	// Token: 0x04000767 RID: 1895
	public float tapeStopIntensity = 1f;

	// Token: 0x04000768 RID: 1896
	public float tapeStopRate = 1f;
}
