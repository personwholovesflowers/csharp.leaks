using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
[ExecuteInEditMode]
public class AspectRatioScaler : MonoBehaviour
{
	// Token: 0x06000373 RID: 883 RVA: 0x00016DA0 File Offset: 0x00014FA0
	public void Update()
	{
		float num = this.aspectRatioWidthAtScaleOne / this.aspectRatioHeightAtScaleOne;
		float num2 = (float)Screen.width / (float)Screen.height / num;
		if (this.invertScale)
		{
			num2 = 1f / num2;
		}
		num2 = Mathf.Clamp(num2, 0f, this.maxScale);
		base.transform.localScale = (this.scaleYZ ? new Vector3(1f, num2, num2) : new Vector3(num2, num2, 1f));
	}

	// Token: 0x04000367 RID: 871
	public float aspectRatioWidthAtScaleOne = 16f;

	// Token: 0x04000368 RID: 872
	public float aspectRatioHeightAtScaleOne = 9f;

	// Token: 0x04000369 RID: 873
	public bool invertScale;

	// Token: 0x0400036A RID: 874
	public float maxScale = 1f;

	// Token: 0x0400036B RID: 875
	public bool scaleYZ;
}
