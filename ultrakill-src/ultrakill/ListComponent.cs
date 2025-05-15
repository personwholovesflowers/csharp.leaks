using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D8 RID: 728
public abstract class ListComponent<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x06000FC9 RID: 4041 RVA: 0x00075C1F File Offset: 0x00073E1F
	protected virtual void Awake()
	{
		ListComponent<T>.InstanceList.Add(this as T);
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x00075C36 File Offset: 0x00073E36
	protected virtual void OnDestroy()
	{
		ListComponent<T>.InstanceList.Remove(this as T);
	}

	// Token: 0x0400155E RID: 5470
	public static List<T> InstanceList = new List<T>();
}
