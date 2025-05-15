using System;
using UnityEngine;

// Token: 0x020003FA RID: 1018
public class ShotgunAnimationReceiver : MonoBehaviour
{
	// Token: 0x060016E0 RID: 5856 RVA: 0x000B85C8 File Offset: 0x000B67C8
	private void Start()
	{
		this.sgun = base.GetComponentInParent<Shotgun>();
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x000B85D6 File Offset: 0x000B67D6
	public void ReleaseHeat()
	{
		this.sgun.ReleaseHeat();
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000B85E3 File Offset: 0x000B67E3
	public void ClickSound()
	{
		this.sgun.ClickSound();
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x000B85F0 File Offset: 0x000B67F0
	public void ReadyGun()
	{
		this.sgun.ReadyGun();
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000B85FD File Offset: 0x000B67FD
	public void Smack()
	{
		this.sgun.Smack();
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x000B860A File Offset: 0x000B680A
	public void Pump1Sound()
	{
		this.sgun.Pump1Sound();
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x000B8617 File Offset: 0x000B6817
	public void Pump2Sound()
	{
		this.sgun.Pump2Sound();
	}

	// Token: 0x04001FCA RID: 8138
	private Shotgun sgun;
}
