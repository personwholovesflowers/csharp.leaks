using System;
using UnityEngine;

// Token: 0x02000168 RID: 360
[RequireComponent(typeof(AudioSource))]
public class PlaySoundFromAudioCollection : MonoBehaviour
{
	// Token: 0x06000865 RID: 2149 RVA: 0x00027FC1 File Offset: 0x000261C1
	private void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00027FD0 File Offset: 0x000261D0
	public void Play()
	{
		if (this.waitForAverageTime && this.collection != null && Time.realtimeSinceStartup - this.timeStamp <= this.collection.AverageDuration)
		{
			return;
		}
		if (this.audioSource != null && this.collection != null && this.collection.SetVolumeAndPitchAndPlayClip(this.audioSource))
		{
			this.timeStamp = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x04000837 RID: 2103
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000838 RID: 2104
	private AudioSource audioSource1;

	// Token: 0x04000839 RID: 2105
	private AudioSource audioSource2;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private AudioCollection collection;

	// Token: 0x0400083B RID: 2107
	private float timeStamp;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	private bool waitForAverageTime;
}
