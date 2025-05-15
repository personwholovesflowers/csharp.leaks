using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000228 RID: 552
public class GetMusicVolume : MonoBehaviour
{
	// Token: 0x06000BD9 RID: 3033 RVA: 0x000534EC File Offset: 0x000516EC
	private void Start()
	{
		this.mman = MonoSingleton<MusicManager>.Instance;
		this.auds = base.GetComponentsInChildren<AudioSource>();
		foreach (AudioSource audioSource in this.auds)
		{
			this.origVol.Add(audioSource.volume);
		}
		for (int j = 0; j < this.auds.Length; j++)
		{
			this.auds[j].volume = this.origVol[j] * MonoSingleton<AudioMixerController>.Instance.musicVolume;
		}
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00053574 File Offset: 0x00051774
	private void Update()
	{
		for (int i = 0; i < this.auds.Length; i++)
		{
			this.auds[i].volume = this.origVol[i] * MonoSingleton<AudioMixerController>.Instance.musicVolume;
		}
	}

	// Token: 0x04000F71 RID: 3953
	private MusicManager mman;

	// Token: 0x04000F72 RID: 3954
	private AudioSource[] auds;

	// Token: 0x04000F73 RID: 3955
	private List<float> origVol = new List<float>();
}
