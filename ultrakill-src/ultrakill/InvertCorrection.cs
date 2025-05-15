using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class InvertCorrection : MonoBehaviour
{
	// Token: 0x06000E5C RID: 3676 RVA: 0x0006AF1C File Offset: 0x0006911C
	private void Update()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		Vector3 localScale = base.transform.localScale;
		if (this.invert)
		{
			if (this.checkX && lossyScale.x > 0f)
			{
				localScale.x *= -1f;
			}
			if (this.checkY && lossyScale.y > 0f)
			{
				localScale.y *= -1f;
			}
			if (this.checkZ && lossyScale.z > 0f)
			{
				localScale.z *= -1f;
			}
		}
		else
		{
			if (this.checkX && lossyScale.x < 0f)
			{
				localScale.x *= -1f;
			}
			if (this.checkY && lossyScale.y < 0f)
			{
				localScale.y *= -1f;
			}
			if (this.checkZ && lossyScale.z < 0f)
			{
				localScale.z *= -1f;
			}
		}
		base.transform.localScale = localScale;
	}

	// Token: 0x040012F6 RID: 4854
	public bool invert;

	// Token: 0x040012F7 RID: 4855
	public bool checkX = true;

	// Token: 0x040012F8 RID: 4856
	public bool checkY = true;

	// Token: 0x040012F9 RID: 4857
	public bool checkZ = true;
}
