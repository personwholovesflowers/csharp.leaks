using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class AmbientGeneric : HammerEntity
{
	// Token: 0x06000375 RID: 885 RVA: 0x00016E44 File Offset: 0x00015044
	private void Awake()
	{
		this.source = base.GetComponent<AudioSource>();
		if (this.source == null)
		{
			Debug.LogWarning("ambient_generic " + base.name + " didn't have an audio source", base.gameObject);
			this.source = this.sourceEntity.AddComponent<AudioSource>();
		}
		if (this.sourceEntity != base.gameObject)
		{
			AudioSource audioSource = this.source;
			this.source = this.sourceEntity.AddComponent<AudioSource>();
			this.source.clip = audioSource.clip;
			this.source.loop = audioSource.loop;
			this.source.volume = audioSource.volume;
			this.source.spatialBlend = audioSource.spatialBlend;
			this.source.minDistance = audioSource.minDistance;
			this.source.maxDistance = audioSource.maxDistance;
			this.source.playOnAwake = audioSource.playOnAwake;
			this.source.pitch = audioSource.pitch;
			this.source.reverbZoneMix = audioSource.reverbZoneMix;
			this.source.bypassReverbZones = audioSource.bypassReverbZones;
			this.source.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
			Object.Destroy(audioSource);
		}
		if (this.clips.Length != 0)
		{
			this.source.pitch = this.pitchRange.Random();
			this.source.volume = this.volume;
			this.source.clip = this.clips[Random.Range(0, this.clips.Length)];
			if (this.source.playOnAwake)
			{
				this.source.Play();
			}
		}
		GameMaster.OnPause += this.Pause;
		GameMaster.OnResume += this.Resume;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00017015 File Offset: 0x00015215
	private void OnDestroy()
	{
		GameMaster.OnPause -= this.Pause;
		GameMaster.OnResume -= this.Resume;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00017039 File Offset: 0x00015239
	private void Pause()
	{
		if (this.source != null)
		{
			this.source.Pause();
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00017054 File Offset: 0x00015254
	private void Resume()
	{
		if (this.source != null)
		{
			this.source.UnPause();
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00017070 File Offset: 0x00015270
	private void OnValidate()
	{
		if (this.sourceEntity == null || this.sourceEntity.name != this.sourceEntityName || this.sourceEntity != base.gameObject)
		{
			GameObject gameObject = GameObject.Find(this.sourceEntityName);
			if (gameObject)
			{
				this.sourceEntity = gameObject;
				return;
			}
			this.sourceEntity = base.gameObject;
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x000170DE File Offset: 0x000152DE
	public void Input_PlaySound()
	{
		if (this.source == null)
		{
			return;
		}
		this.source.clip = this.clips[Random.Range(0, this.clips.Length)];
		this.source.Play();
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0001711A File Offset: 0x0001531A
	public void Input_StopSound()
	{
		if (this.source == null)
		{
			return;
		}
		this.source.Stop();
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00017136 File Offset: 0x00015336
	public void Input_FadeOut(float duration)
	{
		if (this.source == null)
		{
			return;
		}
		base.StartCoroutine(this.Fade(-1f, 0f, duration));
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001715F File Offset: 0x0001535F
	public void Input_FadeIn(float duration)
	{
		if (this.source == null)
		{
			return;
		}
		base.StartCoroutine(this.Fade(0f, -1f, duration));
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00017188 File Offset: 0x00015388
	private IEnumerator Fade(float startVol, float endVol, float duration)
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		fint32 endTime = startTime + duration;
		if (startVol < 0f)
		{
			startVol = this.volume;
		}
		if (endVol < 0f)
		{
			endVol = this.volume;
		}
		while (Singleton<GameMaster>.Instance.GameTime < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			if (this.source == null)
			{
				yield break;
			}
			this.source.volume = Mathf.Lerp(startVol, endVol, num);
			yield return new WaitForEndOfFrame();
		}
		if (this.source == null)
		{
			yield break;
		}
		this.source.volume = endVol;
		if (this.source.volume == 0f)
		{
			this.source.Stop();
			this.source.volume = startVol;
		}
		yield break;
	}

	// Token: 0x0400036C RID: 876
	private AudioSource source;

	// Token: 0x0400036D RID: 877
	public float volume = 1f;

	// Token: 0x0400036E RID: 878
	public MinMax pitchRange = new MinMax(1f, 1f);

	// Token: 0x0400036F RID: 879
	public AudioClip[] clips;

	// Token: 0x04000370 RID: 880
	public string sourceEntityName = "";

	// Token: 0x04000371 RID: 881
	public GameObject sourceEntity;
}
