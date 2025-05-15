using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class DestroyOnDisable : MonoBehaviour
{
	// Token: 0x0600050B RID: 1291 RVA: 0x00022071 File Offset: 0x00020271
	private void Start()
	{
		if (!this.beenActivated)
		{
			this.beenActivated = true;
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0002208E File Offset: 0x0002028E
	private void OnDisable()
	{
		if (base.gameObject.activeSelf)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040006F5 RID: 1781
	[HideInInspector]
	public bool beenActivated;
}
