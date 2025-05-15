using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000113 RID: 275
public class DisabledEnemiesChecker : MonoBehaviour
{
	// Token: 0x0600051E RID: 1310 RVA: 0x000224CC File Offset: 0x000206CC
	private void Update()
	{
		if (!this.activated && MonoSingleton<StatsManager>.Instance && MonoSingleton<StatsManager>.Instance.levelStarted && DisableEnemySpawns.DisableArenaTriggers)
		{
			this.activated = true;
			base.Invoke("Activate", this.delay);
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00022518 File Offset: 0x00020718
	private void Activate()
	{
		UnityEvent unityEvent = this.onDisabledEnemies;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0400070A RID: 1802
	private bool activated;

	// Token: 0x0400070B RID: 1803
	public float delay;

	// Token: 0x0400070C RID: 1804
	public UnityEvent onDisabledEnemies;
}
