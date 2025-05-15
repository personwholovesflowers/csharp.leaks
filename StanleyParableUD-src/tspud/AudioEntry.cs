using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
[Serializable]
public class AudioEntry
{
	// Token: 0x060003A3 RID: 931 RVA: 0x00017E52 File Offset: 0x00016052
	public AudioEntry(AudioClip clip)
	{
		this.audioClip = clip;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00017E82 File Offset: 0x00016082
	public bool GetClip(out AudioClip clip)
	{
		clip = this.audioClip;
		return this.audioClip != null;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00017E98 File Offset: 0x00016098
	public float GetPitch()
	{
		return Random.Range(this.minimumPitch, this.maximumPitch);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00017EAB File Offset: 0x000160AB
	public float GetVolume()
	{
		return this.volume;
	}

	// Token: 0x04000398 RID: 920
	[SerializeField]
	private AudioClip audioClip;

	// Token: 0x04000399 RID: 921
	[SerializeField]
	[Range(0f, 1f)]
	private float volume = 1f;

	// Token: 0x0400039A RID: 922
	[SerializeField]
	private float minimumPitch = 1f;

	// Token: 0x0400039B RID: 923
	[SerializeField]
	private float maximumPitch = 1f;
}
