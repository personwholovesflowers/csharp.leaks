using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class PortalScreen : MonoBehaviour
{
	// Token: 0x060001C8 RID: 456 RVA: 0x0000CBB9 File Offset: 0x0000ADB9
	private void OnBecameVisible()
	{
		if (this.OnVisible != null)
		{
			this.OnVisible();
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000CBCE File Offset: 0x0000ADCE
	private void OnBecameInvisible()
	{
		if (this.OnInvisible != null)
		{
			this.OnInvisible();
		}
	}

	// Token: 0x04000210 RID: 528
	public Action OnVisible;

	// Token: 0x04000211 RID: 529
	public Action OnInvisible;
}
