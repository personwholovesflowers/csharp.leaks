using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class DetachFromParent : MonoBehaviour
{
	// Token: 0x0600050E RID: 1294 RVA: 0x000220A8 File Offset: 0x000202A8
	private void Start()
	{
		if (this.detachOnStart)
		{
			this.Detach();
		}
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x000220B8 File Offset: 0x000202B8
	public void Detach()
	{
		base.transform.SetParent(null, true);
	}

	// Token: 0x040006F6 RID: 1782
	public bool detachOnStart;
}
