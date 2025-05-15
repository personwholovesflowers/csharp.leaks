using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class LaughingSkull : MonoBehaviour
{
	// Token: 0x06000E9D RID: 3741 RVA: 0x0006C649 File Offset: 0x0006A849
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0006C657 File Offset: 0x0006A857
	public void PlayAudio()
	{
		this.aud.Play();
	}

	// Token: 0x0400135B RID: 4955
	private AudioSource aud;
}
