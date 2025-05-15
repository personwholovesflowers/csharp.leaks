using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200006D RID: 109
internal class BackSelectEvent : MonoBehaviour
{
	// Token: 0x0600020E RID: 526 RVA: 0x0000AC1C File Offset: 0x00008E1C
	public void InvokeOnBack()
	{
		UnityEvent onBack = this.m_OnBack;
		if (onBack == null)
		{
			return;
		}
		onBack.Invoke();
	}

	// Token: 0x04000244 RID: 580
	[SerializeField]
	private UnityEvent m_OnBack;
}
