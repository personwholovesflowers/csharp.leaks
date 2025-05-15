using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class DistantSprite : MonoBehaviour
{
	// Token: 0x0600052D RID: 1325 RVA: 0x00022679 File Offset: 0x00020879
	private void Start()
	{
		this.sr = base.GetComponent<SpriteRenderer>();
		this.originalAlpha = this.sr.color.a;
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000226A0 File Offset: 0x000208A0
	private void Update()
	{
		this.clr = this.sr.color;
		this.distance = Vector3.Distance(base.transform.position, MonoSingleton<CameraController>.Instance.transform.position);
		if (this.distance > this.mininumDistance)
		{
			if (this.distance > this.maximumDistance)
			{
				this.clr.a = this.originalAlpha;
			}
			else
			{
				this.clr.a = Mathf.Lerp(0f, this.originalAlpha, (this.distance - this.mininumDistance) / (this.maximumDistance - this.mininumDistance));
			}
		}
		else
		{
			this.clr.a = 0f;
		}
		this.sr.color = this.clr;
	}

	// Token: 0x04000713 RID: 1811
	private SpriteRenderer sr;

	// Token: 0x04000714 RID: 1812
	private Color clr;

	// Token: 0x04000715 RID: 1813
	private float distance;

	// Token: 0x04000716 RID: 1814
	private float originalAlpha;

	// Token: 0x04000717 RID: 1815
	public float mininumDistance;

	// Token: 0x04000718 RID: 1816
	public float maximumDistance;
}
