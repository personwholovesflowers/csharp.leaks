using System;
using UnityEngine;

// Token: 0x0200033B RID: 827
public class PlatformerChecker : MonoBehaviour
{
	// Token: 0x060012FC RID: 4860 RVA: 0x000971E1 File Offset: 0x000953E1
	private void Update()
	{
		if (!this.activated && MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			this.activated = true;
			this.onPlatformer.Invoke("");
		}
	}

	// Token: 0x04001A1A RID: 6682
	public bool activated;

	// Token: 0x04001A1B RID: 6683
	public UltrakillEvent onPlatformer;
}
