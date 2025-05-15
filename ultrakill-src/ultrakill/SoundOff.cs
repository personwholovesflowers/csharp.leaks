using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class SoundOff : MonoBehaviour
{
	// Token: 0x060017C6 RID: 6086 RVA: 0x000C2D22 File Offset: 0x000C0F22
	private void Start()
	{
		this.aud = base.GetComponentInChildren<AudioSource>();
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x000C2D30 File Offset: 0x000C0F30
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.aud.volume = 0f;
		}
	}

	// Token: 0x0400213E RID: 8510
	private AudioSource aud;
}
