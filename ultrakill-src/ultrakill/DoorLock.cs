using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class DoorLock : MonoBehaviour
{
	// Token: 0x06000559 RID: 1369 RVA: 0x0002436F File Offset: 0x0002256F
	public void Open()
	{
		if (this.parentDoor != null)
		{
			this.parentDoor.LockOpen();
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002438A File Offset: 0x0002258A
	public void Close()
	{
		if (this.parentDoor != null)
		{
			this.parentDoor.LockClose();
		}
	}

	// Token: 0x0400076B RID: 1899
	[HideInInspector]
	public Door parentDoor;
}
