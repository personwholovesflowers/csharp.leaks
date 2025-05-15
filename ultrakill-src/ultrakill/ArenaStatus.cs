using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class ArenaStatus : MonoBehaviour
{
	// Token: 0x060001E9 RID: 489 RVA: 0x0000A1F9 File Offset: 0x000083F9
	public void SetStatus(int i)
	{
		this.currentStatus = i;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000A202 File Offset: 0x00008402
	public void AddToStatus(int i)
	{
		this.currentStatus += i;
	}

	// Token: 0x04000209 RID: 521
	public int currentStatus;
}
