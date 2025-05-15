using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class Soundscape : HammerEntity
{
	// Token: 0x06000399 RID: 921 RVA: 0x00017BE0 File Offset: 0x00015DE0
	private void Awake()
	{
		if (this.clip)
		{
			this.source = base.gameObject.AddComponent<AudioSource>();
			this.source.volume = this.volume;
			this.source.pitch = this.pitch;
			this.source.clip = this.clip;
			this.source.playOnAwake = false;
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00017C4A File Offset: 0x00015E4A
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(base.transform.position, this.radius);
	}

	// Token: 0x04000387 RID: 903
	private AudioSource source;

	// Token: 0x04000388 RID: 904
	public float volume = 1f;

	// Token: 0x04000389 RID: 905
	public float radius = 1f;

	// Token: 0x0400038A RID: 906
	public float fadetime = 1f;

	// Token: 0x0400038B RID: 907
	public float pitch = 1f;

	// Token: 0x0400038C RID: 908
	public AudioClip clip;
}
