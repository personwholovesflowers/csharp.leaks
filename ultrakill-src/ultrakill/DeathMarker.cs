using System;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class DeathMarker : MonoBehaviour
{
	// Token: 0x060004EA RID: 1258 RVA: 0x000214BF File Offset: 0x0001F6BF
	private void OnEnable()
	{
		if (!this.activated)
		{
			this.activated = true;
			base.GetComponentInParent<ActivateNextWave>().AddDeadEnemy();
		}
	}

	// Token: 0x040006B0 RID: 1712
	public bool activated;
}
