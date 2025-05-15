using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class InfoParticleSystem : HammerEntity
{
	// Token: 0x060005C1 RID: 1473 RVA: 0x0001FF84 File Offset: 0x0001E184
	private void Awake()
	{
		this.systems = base.GetComponentsInChildren<ParticleSystem>();
		if (this.startActive)
		{
			this.Input_Start();
		}
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0001FFA0 File Offset: 0x0001E1A0
	public void Input_Start()
	{
		for (int i = 0; i < this.systems.Length; i++)
		{
			this.systems[i].Play();
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0001FFD0 File Offset: 0x0001E1D0
	public void Input_Stop()
	{
		for (int i = 0; i < this.systems.Length; i++)
		{
			this.systems[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
		}
	}

	// Token: 0x04000600 RID: 1536
	public bool startActive;

	// Token: 0x04000601 RID: 1537
	private ParticleSystem[] systems = new ParticleSystem[0];
}
