using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001A2 RID: 418
public class EventOnDestroy : MonoBehaviour
{
	// Token: 0x06000871 RID: 2161 RVA: 0x0003A43A File Offset: 0x0003863A
	private void OnDestroy()
	{
		if (base.transform.parent && base.transform.parent.gameObject.activeInHierarchy)
		{
			UnityEvent unityEvent = this.stuff;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x04000B3A RID: 2874
	public UnityEvent stuff;
}
