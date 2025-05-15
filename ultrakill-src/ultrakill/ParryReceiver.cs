using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000330 RID: 816
public class ParryReceiver : MonoBehaviour
{
	// Token: 0x060012DE RID: 4830 RVA: 0x00096473 File Offset: 0x00094673
	public void Parry()
	{
		this.onParry.Invoke();
		if (this.disappearOnParry)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Update()
	{
	}

	// Token: 0x040019D4 RID: 6612
	public bool parryHeal;

	// Token: 0x040019D5 RID: 6613
	public bool disappearOnParry;

	// Token: 0x040019D6 RID: 6614
	[Space]
	public UnityEvent onParry;
}
