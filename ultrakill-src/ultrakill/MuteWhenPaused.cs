using System;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class MuteWhenPaused : MonoBehaviour
{
	// Token: 0x060011CF RID: 4559 RVA: 0x0008AA6E File Offset: 0x00088C6E
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.aud.mute = Time.deltaTime == 0f;
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0008AA93 File Offset: 0x00088C93
	private void Update()
	{
		this.aud.mute = Time.deltaTime == 0f;
	}

	// Token: 0x04001840 RID: 6208
	private AudioSource aud;
}
