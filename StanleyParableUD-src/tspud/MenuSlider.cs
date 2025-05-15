using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class MenuSlider : MenuButton
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060006BF RID: 1727 RVA: 0x000241A8 File Offset: 0x000223A8
	// (set) Token: 0x060006C0 RID: 1728 RVA: 0x000241B0 File Offset: 0x000223B0
	public float position
	{
		get
		{
			return this._position;
		}
		protected set
		{
			if (this._position != Mathf.Clamp01(value))
			{
				this._position = Mathf.Clamp01(value);
				this.Changed();
			}
		}
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x000241D2 File Offset: 0x000223D2
	public override void OnClick(Vector3 point)
	{
		base.OnClick(point);
		this.ClickOrHold(point);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000241E2 File Offset: 0x000223E2
	public override void OnHold(Vector3 point)
	{
		base.OnHold(point);
		this.ClickOrHold(point);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000241F4 File Offset: 0x000223F4
	private void ClickOrHold(Vector3 point)
	{
		float x = this.foregroundRT.InverseTransformPoint(point).x;
		this.position = x / this.maxPixels;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00024224 File Offset: 0x00022424
	public override void OnInput(DPadDir direction)
	{
		base.OnInput(direction);
		if (direction == DPadDir.Up || direction == DPadDir.Down)
		{
			return;
		}
		int num = 1;
		if (direction == DPadDir.Left)
		{
			num = -1;
		}
		if (direction == DPadDir.Right)
		{
			num = 1;
		}
		int num2 = Mathf.RoundToInt(this.position * (float)this.gradations);
		num2 += num;
		this.position = (float)num2 / (float)this.gradations;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00024278 File Offset: 0x00022478
	protected virtual void Changed()
	{
		this.foregroundRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.maxPixels * this.position);
		this.pipRT.anchoredPosition3D = new Vector3(this.maxPixels * this.position, this.pipHeight, 0f);
	}

	// Token: 0x0400070F RID: 1807
	public RectTransform foregroundRT;

	// Token: 0x04000710 RID: 1808
	public RectTransform pipRT;

	// Token: 0x04000711 RID: 1809
	private float maxPixels = 400f;

	// Token: 0x04000712 RID: 1810
	private float pipHeight = 40f;

	// Token: 0x04000713 RID: 1811
	private int gradations = 20;

	// Token: 0x04000714 RID: 1812
	private float _position = 1f;
}
