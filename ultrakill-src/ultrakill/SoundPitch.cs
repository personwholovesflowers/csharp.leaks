using System;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class SoundPitch : MonoBehaviour
{
	// Token: 0x060017D2 RID: 6098 RVA: 0x000C2E94 File Offset: 0x000C1094
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (!this.notOnEnable)
		{
			this.activated = true;
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000C2EB4 File Offset: 0x000C10B4
	private void Update()
	{
		if (this.aud && this.activated)
		{
			this.aud.pitch = Mathf.MoveTowards(this.aud.pitch, this.targetPitch, this.speed * Time.deltaTime);
		}
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000C2F03 File Offset: 0x000C1103
	public void Activate()
	{
		this.activated = true;
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000C2F0C File Offset: 0x000C110C
	public void Deactivate()
	{
		this.activated = false;
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000C2F15 File Offset: 0x000C1115
	public void ChangePitch(float newPitch)
	{
		this.targetPitch = newPitch;
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000C2F1E File Offset: 0x000C111E
	public void ChangeSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}

	// Token: 0x04002146 RID: 8518
	private AudioSource aud;

	// Token: 0x04002147 RID: 8519
	public float targetPitch;

	// Token: 0x04002148 RID: 8520
	public float speed;

	// Token: 0x04002149 RID: 8521
	public bool notOnEnable;

	// Token: 0x0400214A RID: 8522
	private bool activated;
}
