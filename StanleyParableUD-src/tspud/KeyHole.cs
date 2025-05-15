using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000F7 RID: 247
public class KeyHole : MonoBehaviour
{
	// Token: 0x060005FB RID: 1531 RVA: 0x00020BCD File Offset: 0x0001EDCD
	private void Start()
	{
		if (this.m_MyEvent == null)
		{
			this.m_MyEvent = new UnityEvent();
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x00020BE2 File Offset: 0x0001EDE2
	private void Update()
	{
		if (GameMaster.PAUSEMENUACTIVE)
		{
			return;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.HoleTeleportAction.WasPressed)
		{
			this.m_MyEvent.Invoke();
		}
	}

	// Token: 0x0400063F RID: 1599
	public UnityEvent m_MyEvent;
}
