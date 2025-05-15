using System;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class LogicTimer : HammerEntity
{
	// Token: 0x06000642 RID: 1602 RVA: 0x000225D0 File Offset: 0x000207D0
	private void Update()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (!this.toggledOn)
		{
			return;
		}
		this.currentTime += Singleton<GameMaster>.Instance.GameDeltaTime;
		if (this.currentTime > this.nextRefireTime)
		{
			this.currentTime -= this.nextRefireTime;
			if (this.oscillator)
			{
				if (this.high)
				{
					base.FireOutput(Outputs.OnTimerHigh);
				}
				else
				{
					base.FireOutput(Outputs.OnTimerLow);
				}
			}
			else
			{
				base.FireOutput(Outputs.OnTimer);
			}
			this.ChooseNewRefireTime();
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0002265C File Offset: 0x0002085C
	private void ChooseNewRefireTime()
	{
		if (this.oscillator)
		{
			if (this.high)
			{
				this.nextRefireTime = this.lowerRandomBound;
				this.high = false;
				return;
			}
			this.nextRefireTime = this.upperRandomBound;
			this.high = true;
			return;
		}
		else
		{
			if (this.useRandomTime)
			{
				this.nextRefireTime = Random.Range(this.lowerRandomBound, this.upperRandomBound);
				return;
			}
			this.nextRefireTime = this.refireTime;
			return;
		}
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x000226CD File Offset: 0x000208CD
	public void Input_Toggle()
	{
		this.toggledOn = !this.toggledOn;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x000226DE File Offset: 0x000208DE
	public void Input_RefireTime(float newTime)
	{
		this.refireTime = (float)((int)newTime);
		this.ChooseNewRefireTime();
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x000226EF File Offset: 0x000208EF
	public void Input_ResetTimer()
	{
		this.currentTime = 0f;
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x000226FC File Offset: 0x000208FC
	public void Input_FireTimer()
	{
		base.FireOutput(Outputs.OnTimer);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00022706 File Offset: 0x00020906
	public void Input_LowerRandomBound(float newBound)
	{
		this.lowerRandomBound = newBound;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002270F File Offset: 0x0002090F
	public void Input_UpperRandomBound(float newBound)
	{
		this.upperRandomBound = newBound;
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00022718 File Offset: 0x00020918
	public void Input_AddToTimer(float addition)
	{
		this.currentTime += addition;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00022728 File Offset: 0x00020928
	public void Input_SubtractFromTimer(float subtraction)
	{
		this.currentTime -= subtraction;
	}

	// Token: 0x04000699 RID: 1689
	public float refireTime;

	// Token: 0x0400069A RID: 1690
	public bool useRandomTime;

	// Token: 0x0400069B RID: 1691
	public float lowerRandomBound;

	// Token: 0x0400069C RID: 1692
	public float upperRandomBound;

	// Token: 0x0400069D RID: 1693
	public bool oscillator;

	// Token: 0x0400069E RID: 1694
	private bool high;

	// Token: 0x0400069F RID: 1695
	private float currentTime;

	// Token: 0x040006A0 RID: 1696
	private float nextRefireTime;

	// Token: 0x040006A1 RID: 1697
	private bool toggledOn = true;
}
