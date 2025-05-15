using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000BC RID: 188
public class CheckForOtherObject : MonoBehaviour
{
	// Token: 0x060003B3 RID: 947 RVA: 0x00016E34 File Offset: 0x00015034
	private void Start()
	{
		if (this.target != null && this.target.activeInHierarchy)
		{
			if (this.disableSelf)
			{
				base.gameObject.SetActive(false);
			}
			UnityEvent unityEvent = this.onOtherObjectFound;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x04000486 RID: 1158
	public GameObject target;

	// Token: 0x04000487 RID: 1159
	public bool disableSelf;

	// Token: 0x04000488 RID: 1160
	public UnityEvent onOtherObjectFound;
}
