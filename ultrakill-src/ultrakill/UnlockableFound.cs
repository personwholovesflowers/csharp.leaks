using System;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class UnlockableFound : MonoBehaviour
{
	// Token: 0x06001AD5 RID: 6869 RVA: 0x000DCF5D File Offset: 0x000DB15D
	private void OnEnable()
	{
		if (this.unlockOnEnable)
		{
			this.Unlock();
		}
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000DCF6D File Offset: 0x000DB16D
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		if (this.unlockOnTriggerEnter)
		{
			this.Unlock();
		}
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x000DCF8B File Offset: 0x000DB18B
	public void Unlock()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		MonoSingleton<UnlockablesData>.Instance.SetUnlocked(this.unlockableType, true);
	}

	// Token: 0x040025CE RID: 9678
	[SerializeField]
	private UnlockableType unlockableType;

	// Token: 0x040025CF RID: 9679
	[SerializeField]
	private bool unlockOnEnable = true;

	// Token: 0x040025D0 RID: 9680
	[SerializeField]
	private bool unlockOnTriggerEnter;
}
