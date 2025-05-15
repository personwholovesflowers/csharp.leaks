using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B0 RID: 176
public class DoIfMac : MonoBehaviour
{
	// Token: 0x0600041A RID: 1050 RVA: 0x00019291 File Offset: 0x00017491
	public void CheckIfMacOSAndInvoke()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEvent onIsMacOS = this.OnIsMacOS;
			if (onIsMacOS == null)
			{
				return;
			}
			onIsMacOS.Invoke();
		}
	}

	// Token: 0x04000407 RID: 1031
	public UnityEvent OnIsMacOS;
}
