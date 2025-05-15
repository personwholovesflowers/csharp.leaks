using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B8 RID: 184
public class EventButton : MenuButton
{
	// Token: 0x06000444 RID: 1092 RVA: 0x00019D6F File Offset: 0x00017F6F
	public override void OnClick(Vector3 point = default(Vector3))
	{
		this.OnTrigger.Invoke();
	}

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private UnityEvent OnTrigger;
}
