using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class TriggerHurt : TriggerMultiple
{
	// Token: 0x06000A33 RID: 2611 RVA: 0x000301BA File Offset: 0x0002E3BA
	protected override void StartTouch()
	{
		if (!this.isEnabled)
		{
			return;
		}
		base.StartTouch();
		base.StartCoroutine(this.SeeRed());
		base.FireOutput(Outputs.OnHurtPlayer);
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x000301E0 File Offset: 0x0002E3E0
	private IEnumerator SeeRed()
	{
		Singleton<GameMaster>.Instance.BeginFade(this.red, 0.1f, 0.2f, false, false);
		yield return new WaitForGameSeconds(0.19f);
		Singleton<GameMaster>.Instance.CancelFade();
		Singleton<GameMaster>.Instance.BeginFade(this.red, 0.5f, 0f, true, false);
		yield break;
	}

	// Token: 0x04000A2A RID: 2602
	private Color red = new Color(0.7f, 0f, 0f, 0.4f);
}
