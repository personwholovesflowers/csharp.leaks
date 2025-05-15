using System;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class SoundPitchDip : MonoBehaviour
{
	// Token: 0x060017D9 RID: 6105 RVA: 0x000C2F27 File Offset: 0x000C1127
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Dip(0f);
		}
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000C2F3C File Offset: 0x000C113C
	public void Dip(float pitch)
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
			if (!this.aud)
			{
				return;
			}
			this.origPitch = this.aud.pitch;
			this.aud.pitch = pitch;
			this.target = this.origPitch;
			this.dipping = true;
		}
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x000C2FA0 File Offset: 0x000C11A0
	public void DipToZero()
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
			if (!this.aud)
			{
				return;
			}
			this.origPitch = this.aud.pitch;
			this.target = 0f;
			this.dipping = true;
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x000C2FF8 File Offset: 0x000C11F8
	private void Update()
	{
		if (this.dipping)
		{
			this.aud.pitch = Mathf.MoveTowards(this.aud.pitch, this.target, Time.deltaTime * this.speed);
			if (this.aud.pitch == this.target)
			{
				this.dipping = false;
				if (this.aud.pitch == 0f)
				{
					this.aud.mute = true;
					return;
				}
			}
			else
			{
				this.aud.mute = false;
			}
		}
	}

	// Token: 0x0400214B RID: 8523
	private AudioSource aud;

	// Token: 0x0400214C RID: 8524
	private bool dipping;

	// Token: 0x0400214D RID: 8525
	private float origPitch;

	// Token: 0x0400214E RID: 8526
	private float target;

	// Token: 0x0400214F RID: 8527
	public float speed;

	// Token: 0x04002150 RID: 8528
	public bool onEnable;
}
