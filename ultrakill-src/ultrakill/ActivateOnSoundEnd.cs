using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class ActivateOnSoundEnd : MonoBehaviour
{
	// Token: 0x060000ED RID: 237 RVA: 0x00005E91 File Offset: 0x00004091
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00005EA0 File Offset: 0x000040A0
	private void Update()
	{
		if (this.aud.isPlaying || this.dontWaitForStart)
		{
			this.hasStarted = true;
		}
		if (this.hasStarted && (!this.activated || this.oneTime) && !this.aud.isPlaying && this.aud.time == 0f)
		{
			this.activated = true;
			this.hasStarted = false;
			this.events.Invoke("");
			if (this.oneTime)
			{
				base.enabled = false;
			}
		}
	}

	// Token: 0x04000098 RID: 152
	private AudioSource aud;

	// Token: 0x04000099 RID: 153
	private bool hasStarted;

	// Token: 0x0400009A RID: 154
	[SerializeField]
	private UltrakillEvent events;

	// Token: 0x0400009B RID: 155
	[SerializeField]
	private bool dontWaitForStart;

	// Token: 0x0400009C RID: 156
	[SerializeField]
	private bool oneTime;

	// Token: 0x0400009D RID: 157
	private bool activated;
}
