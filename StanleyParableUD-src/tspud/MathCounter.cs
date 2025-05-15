using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class MathCounter : HammerEntity
{
	// Token: 0x06000676 RID: 1654 RVA: 0x00023072 File Offset: 0x00021272
	private void Awake()
	{
		this.currentValue = this.startValue;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00023080 File Offset: 0x00021280
	private void CheckValue(float newValue)
	{
		float num = this.currentValue;
		this.currentValue = Mathf.Clamp(newValue, this.min, this.max);
		base.FireOutput(Outputs.OutValue, this.currentValue);
		if (num != this.currentValue)
		{
			if (this.currentValue == this.max)
			{
				base.FireOutput(Outputs.OnHitMax);
			}
			if (this.currentValue == this.min)
			{
				base.FireOutput(Outputs.OnHitMin);
			}
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000230ED File Offset: 0x000212ED
	public void Input_Add(float value)
	{
		if (!this.isEnabled)
		{
			return;
		}
		Mathf.Round(value);
		this.CheckValue(this.currentValue + value);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0002310D File Offset: 0x0002130D
	public void Input_Subtract(float value)
	{
		if (!this.isEnabled)
		{
			return;
		}
		Mathf.Round(value);
		this.CheckValue(this.currentValue - value);
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002312D File Offset: 0x0002132D
	public void Input_Divide(float value)
	{
		if (!this.isEnabled)
		{
			return;
		}
		Mathf.Round(value);
		if (value == 0f)
		{
			return;
		}
		this.CheckValue(this.currentValue / value);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00023156 File Offset: 0x00021356
	public void Input_Multiply(float value)
	{
		if (!this.isEnabled)
		{
			return;
		}
		Mathf.Round(value);
		this.CheckValue(this.currentValue * value);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00023176 File Offset: 0x00021376
	public void Input_GetValue()
	{
		base.FireOutput(Outputs.OnGetValue, this.currentValue);
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00023186 File Offset: 0x00021386
	public void Input_SetValue(float value)
	{
		Mathf.Round(value);
		this.CheckValue(value);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00023196 File Offset: 0x00021396
	public void Input_SetValueNoFire(float value)
	{
		Mathf.Round(value);
		this.currentValue = Mathf.Clamp(value, this.min, this.max);
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x000231B7 File Offset: 0x000213B7
	public void Input_SetHitMin(float value)
	{
		Mathf.Round(value);
		this.min = value;
		this.CheckValue(this.currentValue);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000231D3 File Offset: 0x000213D3
	public void Input_SetHitMax(float value)
	{
		Mathf.Round(value);
		this.max = value;
		this.CheckValue(this.currentValue);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x000231EF File Offset: 0x000213EF
	public void Input_SetMinValueNoFire(float value)
	{
		Mathf.Round(value);
		this.min = value;
		this.currentValue = Mathf.Clamp(value, this.min, this.max);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00023217 File Offset: 0x00021417
	public void Input_SetMaxValueNoFire(float value)
	{
		Mathf.Round(value);
		this.max = value;
		this.currentValue = Mathf.Clamp(value, this.min, this.max);
	}

	// Token: 0x040006D4 RID: 1748
	public float min;

	// Token: 0x040006D5 RID: 1749
	public float max;

	// Token: 0x040006D6 RID: 1750
	public float startValue;

	// Token: 0x040006D7 RID: 1751
	private float currentValue;
}
