using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
[ExecuteInEditMode]
public class SetFOVBasedOnAspectRatio : MonoBehaviour
{
	// Token: 0x06000919 RID: 2329 RVA: 0x0002B229 File Offset: 0x00029429
	private void LateUpdate()
	{
		this.cam.fieldOfView = this.aspectRatioToFOV.Evaluate((float)Screen.width / (float)Screen.height);
	}

	// Token: 0x040008EF RID: 2287
	public AnimationCurve aspectRatioToFOV;

	// Token: 0x040008F0 RID: 2288
	public Camera cam;
}
