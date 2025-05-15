using System;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class SoundOn : MonoBehaviour
{
	// Token: 0x060017C9 RID: 6089 RVA: 0x000C2D54 File Offset: 0x000C0F54
	private void Awake()
	{
		this.aud = base.GetComponentInChildren<AudioSource>();
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x000C2D62 File Offset: 0x000C0F62
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.aud.volume = this.volume;
		}
	}

	// Token: 0x0400213F RID: 8511
	private AudioSource aud;

	// Token: 0x04002140 RID: 8512
	public float volume;
}
