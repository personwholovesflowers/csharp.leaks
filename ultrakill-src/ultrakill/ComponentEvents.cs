using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D4 RID: 212
[DisallowMultipleComponent]
public sealed class ComponentEvents : MonoBehaviour
{
	// Token: 0x0600043E RID: 1086 RVA: 0x0001D3FC File Offset: 0x0001B5FC
	private void OnEnable()
	{
		UnityEvent unityEvent = this.onEnable;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0001D40E File Offset: 0x0001B60E
	private void OnDisable()
	{
		UnityEvent unityEvent = this.onDisable;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0400054D RID: 1357
	[SerializeField]
	private UnityEvent onEnable;

	// Token: 0x0400054E RID: 1358
	[SerializeField]
	private UnityEvent onDisable;
}
