using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
public class SoundWobbler : MonoBehaviour
{
	// Token: 0x060017E7 RID: 6119 RVA: 0x000C311F File Offset: 0x000C131F
	private void Awake()
	{
		this.aud = base.GetComponentInChildren<AudioSource>();
		base.Invoke("ChangeWobble", this.wobbleTime);
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000C3140 File Offset: 0x000C1340
	private void Update()
	{
		if (this.wobbleUp)
		{
			this.aud.pitch += this.wobbleSpeed * Time.deltaTime;
			return;
		}
		this.aud.pitch -= this.wobbleSpeed * Time.deltaTime;
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000C3192 File Offset: 0x000C1392
	private void ChangeWobble()
	{
		if (this.wobbleUp)
		{
			this.wobbleUp = false;
		}
		else
		{
			this.wobbleUp = true;
		}
		base.Invoke("ChangeWobble", this.wobbleTime);
	}

	// Token: 0x04002157 RID: 8535
	private AudioSource aud;

	// Token: 0x04002158 RID: 8536
	public float wobbleTime;

	// Token: 0x04002159 RID: 8537
	public float wobbleSpeed;

	// Token: 0x0400215A RID: 8538
	public bool wobbleUp;
}
