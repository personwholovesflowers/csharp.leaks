using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class DoorUnlocker : MonoBehaviour
{
	// Token: 0x06000561 RID: 1377 RVA: 0x00024412 File Offset: 0x00022612
	private void OnEnable()
	{
		this.door.Unlock();
		if (this.open)
		{
			this.door.Open(false, true);
		}
	}

	// Token: 0x0400076F RID: 1903
	public Door door;

	// Token: 0x04000770 RID: 1904
	public bool open;
}
