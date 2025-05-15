using System;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class FishingExitDoor : MonoBehaviour
{
	// Token: 0x060009C4 RID: 2500 RVA: 0x00043C6B File Offset: 0x00041E6B
	private void Update()
	{
		if (!this.isLocked)
		{
			return;
		}
		if (this.manager.RemainingFishes <= 0)
		{
			this.isLocked = false;
			this.onUnlock.Invoke("");
		}
	}

	// Token: 0x04000CB7 RID: 3255
	[SerializeField]
	private FishManager manager;

	// Token: 0x04000CB8 RID: 3256
	[SerializeField]
	private UltrakillEvent onUnlock;

	// Token: 0x04000CB9 RID: 3257
	private bool isLocked = true;
}
