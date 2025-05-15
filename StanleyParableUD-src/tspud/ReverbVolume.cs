using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class ReverbVolume : VolumeBase
{
	// Token: 0x06000332 RID: 818 RVA: 0x00015E8E File Offset: 0x0001408E
	private void OnTriggerEnter(Collider other)
	{
		if (this.Profile != null && other.GetComponentInChildren<ReverbController>() != null && ReverbVolume.OnEnterReverbVolume != null)
		{
			ReverbVolume.OnEnterReverbVolume(this);
		}
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00015EBE File Offset: 0x000140BE
	private void OnTriggerExit(Collider other)
	{
		if (this.Profile != null && other.GetComponentInChildren<ReverbController>() != null && ReverbVolume.OnExitReverbVolume != null && ReverbVolume.OnExitReverbVolume != null)
		{
			ReverbVolume.OnExitReverbVolume(this);
		}
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00015EF5 File Offset: 0x000140F5
	public override ProfileBase GetProfile()
	{
		return this.Profile;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00015EFD File Offset: 0x000140FD
	protected override Color GetVolumeBaseColor()
	{
		return Color.cyan * new Color(1f, 1f, 1f, 0.25f);
	}

	// Token: 0x0400032F RID: 815
	[Space]
	public ReverbProfile Profile;

	// Token: 0x04000330 RID: 816
	public static Action<ReverbVolume> OnEnterReverbVolume;

	// Token: 0x04000331 RID: 817
	public static Action<ReverbVolume> OnExitReverbVolume;
}
