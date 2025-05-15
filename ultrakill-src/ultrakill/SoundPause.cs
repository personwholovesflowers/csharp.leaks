using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class SoundPause : MonoBehaviour
{
	// Token: 0x060017CF RID: 6095 RVA: 0x000C2E18 File Offset: 0x000C1018
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x000C2E28 File Offset: 0x000C1028
	private void Update()
	{
		if (this.aud)
		{
			if (this.aud.isPlaying && Time.timeScale == 0f)
			{
				this.wasPlaying = true;
				this.aud.Pause();
				return;
			}
			if (Time.timeScale != 0f && this.wasPlaying)
			{
				this.wasPlaying = false;
				this.aud.UnPause();
			}
		}
	}

	// Token: 0x04002144 RID: 8516
	private AudioSource aud;

	// Token: 0x04002145 RID: 8517
	private bool wasPlaying;
}
