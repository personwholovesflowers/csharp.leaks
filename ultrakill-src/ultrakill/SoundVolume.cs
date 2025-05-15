using System;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class SoundVolume : MonoBehaviour
{
	// Token: 0x060017E0 RID: 6112 RVA: 0x000C308C File Offset: 0x000C128C
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (!this.notOnEnable)
		{
			this.activated = true;
		}
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x000C30AC File Offset: 0x000C12AC
	private void Update()
	{
		if (this.aud && this.activated)
		{
			this.aud.volume = Mathf.MoveTowards(this.aud.volume, this.targetVolume, this.speed * Time.deltaTime);
		}
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x000C30FB File Offset: 0x000C12FB
	public void Activate()
	{
		this.activated = true;
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000C3104 File Offset: 0x000C1304
	public void Deactivate()
	{
		this.activated = false;
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x000C310D File Offset: 0x000C130D
	public void ChangeVolume(float newVolume)
	{
		this.targetVolume = newVolume;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000C3116 File Offset: 0x000C1316
	public void ChangeSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}

	// Token: 0x04002152 RID: 8530
	private AudioSource aud;

	// Token: 0x04002153 RID: 8531
	public float targetVolume;

	// Token: 0x04002154 RID: 8532
	public float speed;

	// Token: 0x04002155 RID: 8533
	public bool notOnEnable;

	// Token: 0x04002156 RID: 8534
	private bool activated;
}
