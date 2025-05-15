using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class DisableAtDistance : MonoBehaviour
{
	// Token: 0x0600051A RID: 1306 RVA: 0x000223B4 File Offset: 0x000205B4
	private void Update()
	{
		if (!MonoSingleton<CameraController>.Instance)
		{
			return;
		}
		if (this.toDisable.activeSelf && Vector3.Distance(base.transform.position, MonoSingleton<CameraController>.Instance.transform.position) > this.distance)
		{
			this.toDisable.SetActive(false);
			return;
		}
		if (!this.toDisable.activeSelf && Vector3.Distance(base.transform.position, MonoSingleton<CameraController>.Instance.transform.position) < this.distance)
		{
			this.toDisable.SetActive(true);
		}
	}

	// Token: 0x04000705 RID: 1797
	public float distance;

	// Token: 0x04000706 RID: 1798
	public GameObject toDisable;
}
