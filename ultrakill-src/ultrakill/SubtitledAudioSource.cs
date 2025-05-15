using System;
using UnityEngine;

// Token: 0x02000464 RID: 1124
[RequireComponent(typeof(AudioSource))]
public class SubtitledAudioSource : MonoBehaviour
{
	// Token: 0x060019BC RID: 6588 RVA: 0x000D3523 File Offset: 0x000D1723
	private void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000D3531 File Offset: 0x000D1731
	private void OnEnable()
	{
		if (this.audioSource.playOnAwake)
		{
			this.currentVoiceLine = 0;
		}
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000D3548 File Offset: 0x000D1748
	private void Update()
	{
		if (this.audioSource.time < this.lastAudioTime)
		{
			this.currentVoiceLine = 0;
		}
		if (this.audioSource.isPlaying)
		{
			if (this.subtitles.lines.Length <= this.currentVoiceLine)
			{
				return;
			}
			if (this.audioSource.time >= this.subtitles.lines[this.currentVoiceLine].time)
			{
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle(this.subtitles.lines[this.currentVoiceLine].subtitle, this.distanceAware ? this.audioSource : null, false);
				this.currentVoiceLine++;
			}
			this.lastAudioTime = this.audioSource.time;
		}
	}

	// Token: 0x04002409 RID: 9225
	[SerializeField]
	private SubtitledAudioSource.SubtitleData subtitles;

	// Token: 0x0400240A RID: 9226
	[SerializeField]
	private bool distanceAware;

	// Token: 0x0400240B RID: 9227
	private AudioSource audioSource;

	// Token: 0x0400240C RID: 9228
	private int currentVoiceLine;

	// Token: 0x0400240D RID: 9229
	private float lastAudioTime;

	// Token: 0x02000465 RID: 1125
	[Serializable]
	public class SubtitleData
	{
		// Token: 0x0400240E RID: 9230
		public SubtitledAudioSource.SubtitleDataLine[] lines;
	}

	// Token: 0x02000466 RID: 1126
	[Serializable]
	public class SubtitleDataLine
	{
		// Token: 0x0400240F RID: 9231
		public string subtitle;

		// Token: 0x04002410 RID: 9232
		public float time;
	}
}
