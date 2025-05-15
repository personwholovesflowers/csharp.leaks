using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
[ExecuteInEditMode]
public class SetScalingByAspectRatio : MonoBehaviour
{
	// Token: 0x06000921 RID: 2337 RVA: 0x0002B304 File Offset: 0x00029504
	private void LateUpdate()
	{
		this.aspectRatio = (float)Screen.width / (float)Screen.height;
		this.trans.localScale = new Vector3(this.aspectRatioToXScale.Evaluate(this.aspectRatio), this.aspectRatioToYScale.Evaluate(this.aspectRatio), 1f);
	}

	// Token: 0x040008F3 RID: 2291
	public AnimationCurve aspectRatioToXScale;

	// Token: 0x040008F4 RID: 2292
	public AnimationCurve aspectRatioToYScale;

	// Token: 0x040008F5 RID: 2293
	public Transform trans;

	// Token: 0x040008F6 RID: 2294
	private float aspectRatio;
}
