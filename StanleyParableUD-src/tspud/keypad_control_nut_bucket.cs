using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020001D9 RID: 473
public class keypad_control_nut_bucket : LogicScriptBase
{
	// Token: 0x06000AD1 RID: 2769 RVA: 0x00033018 File Offset: 0x00031218
	private void Start()
	{
		this.ls = base.GetComponent<LogicScript>();
		for (int i = 0; i < this.ls.groups[0].group.Count; i++)
		{
			this.keypad.Add(this.ls.groups[0].group[i] as PropDynamic);
		}
		for (int j = 0; j < this.ls.groups[11].group.Count; j++)
		{
			this.number1.Add(this.ls.groups[11].group[j] as PropDynamic);
		}
		for (int k = 0; k < this.ls.groups[12].group.Count; k++)
		{
			this.number2.Add(this.ls.groups[12].group[k] as PropDynamic);
		}
		for (int l = 0; l < this.ls.groups[13].group.Count; l++)
		{
			this.number3.Add(this.ls.groups[13].group[l] as PropDynamic);
		}
		for (int m = 0; m < this.ls.groups[14].group.Count; m++)
		{
			this.number4.Add(this.ls.groups[14].group[m] as PropDynamic);
		}
		for (int n = 0; n < this.ls.groups[15].group.Count; n++)
		{
			this.counter8.Add(this.ls.groups[15].group[n] as MathCounter);
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x00033231 File Offset: 0x00031431
	private IEnumerator TurnOn(float delay)
	{
		yield return new WaitForGameSeconds(delay);
		this.activated = true;
		this.num1 = 0;
		this.num2 = 0;
		this.num3 = 0;
		this.num4 = 0;
		this.SetSkin(this.number1, 10);
		this.SetSkin(this.number2, 10);
		this.SetSkin(this.number3, 10);
		this.SetSkin(this.number4, 10);
		this.SetSkin(this.keypad, 1);
		yield break;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x00033248 File Offset: 0x00031448
	private void SetSkin(List<PropDynamic> prop, int index)
	{
		for (int i = 0; i < prop.Count; i++)
		{
			prop[i].Input_Skin((float)index);
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x00033274 File Offset: 0x00031474
	private void Input(int i)
	{
		if (!this.activated)
		{
			return;
		}
		this.ls.Input_FireUser1();
		switch (this.currentNum)
		{
		case 1:
			this.SetSkin(this.number1, i);
			this.num1 = i;
			break;
		case 2:
			this.SetSkin(this.number2, i);
			this.num2 = i;
			break;
		case 3:
			this.SetSkin(this.number3, i);
			this.num3 = i;
			break;
		case 4:
			this.SetSkin(this.number4, i);
			this.num4 = i;
			break;
		}
		this.activated = false;
		this.currentNum++;
		if (this.currentNum > 4)
		{
			this.currentNum = 1;
			base.StartCoroutine(this.BeginCompare());
			return;
		}
		base.StartCoroutine(this.Reactivate(0.5f));
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00033351 File Offset: 0x00031551
	private IEnumerator Reactivate(float delay)
	{
		yield return new WaitForGameSeconds(delay);
		this.activated = true;
		yield break;
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x00033367 File Offset: 0x00031567
	private IEnumerator BeginCompare()
	{
		yield return new WaitForGameSeconds(0.2f);
		this.SetSkin(this.keypad, 2);
		yield return new WaitForGameSeconds(0.2f);
		this.SetSkin(this.number1, 10);
		this.SetSkin(this.number2, 10);
		this.SetSkin(this.number3, 10);
		this.SetSkin(this.number4, 10);
		yield return new WaitForGameSeconds(0.2f);
		this.SetSkin(this.number1, this.num1);
		this.SetSkin(this.number2, this.num2);
		this.SetSkin(this.number3, this.num3);
		this.SetSkin(this.number4, this.num4);
		yield return new WaitForGameSeconds(0.2f);
		this.SetSkin(this.number1, 10);
		this.SetSkin(this.number2, 10);
		this.SetSkin(this.number3, 10);
		this.SetSkin(this.number4, 10);
		yield return new WaitForGameSeconds(0.2f);
		this.SetSkin(this.number1, this.num1);
		this.SetSkin(this.number2, this.num2);
		this.SetSkin(this.number3, this.num3);
		this.SetSkin(this.number4, this.num4);
		yield return new WaitForGameSeconds(0.2f);
		int num = this.num1 * 1000 + this.num2 * 100 + this.num3 * 10 + this.num4;
		if (num == this.correctNum)
		{
			this.SetSkin(this.keypad, 3);
			this.ls.Input_FireUser2();
			base.StartCoroutine(this.TurnOn(1f));
		}
		else
		{
			if (num == 8888)
			{
				for (int i = 0; i < this.counter8.Count; i++)
				{
					this.counter8[i].Input_Add(4f);
				}
			}
			this.SetSkin(this.keypad, 3);
			this.ls.Input_FireUser2();
			base.StartCoroutine(this.TurnOn(1f));
		}
		yield break;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00033378 File Offset: 0x00031578
	public override void ParseCommand(string command)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(command);
		if (num <= 1843243223U)
		{
			if (num <= 1070768921U)
			{
				if (num != 311168960U)
				{
					if (num != 440298307U)
					{
						if (num != 1070768921U)
						{
							return;
						}
						if (!(command == "Input(2);"))
						{
							return;
						}
						this.Input(2);
						return;
					}
					else
					{
						if (!(command == "Input(4);"))
						{
							return;
						}
						this.Input(4);
						return;
					}
				}
				else
				{
					if (!(command == "Input(9);"))
					{
						return;
					}
					this.Input(9);
					return;
				}
			}
			else
			{
				if (num == 1248840533U)
				{
					command == "TurnOff();";
					return;
				}
				if (num != 1533524986U)
				{
					if (num != 1843243223U)
					{
						return;
					}
					if (!(command == "Input(0);"))
					{
						return;
					}
					this.Input(0);
					return;
				}
				else
				{
					if (!(command == "Input(3);"))
					{
						return;
					}
					this.Input(3);
					return;
				}
			}
		}
		else if (num <= 2265109686U)
		{
			if (num != 2029774648U)
			{
				if (num != 2261727007U)
				{
					if (num != 2265109686U)
					{
						return;
					}
					if (!(command == "Input(7);"))
					{
						return;
					}
					this.Input(7);
					return;
				}
				else
				{
					if (!(command == "Input(8);"))
					{
						return;
					}
					this.Input(8);
					return;
				}
			}
			else
			{
				if (!(command == "Input(1);"))
				{
					return;
				}
				this.Input(1);
				return;
			}
		}
		else if (num != 2536028345U)
		{
			if (num != 3032361108U)
			{
				if (num != 4215667733U)
				{
					return;
				}
				if (!(command == "Input(6);"))
				{
					return;
				}
				this.Input(6);
				return;
			}
			else
			{
				if (!(command == "Input(5);"))
				{
					return;
				}
				this.Input(5);
				return;
			}
		}
		else
		{
			if (!(command == "TurnOn();"))
			{
				return;
			}
			base.StartCoroutine(this.TurnOn(0f));
			return;
		}
	}

	// Token: 0x04000AC4 RID: 2756
	private List<PropDynamic> number1 = new List<PropDynamic>();

	// Token: 0x04000AC5 RID: 2757
	private List<PropDynamic> number2 = new List<PropDynamic>();

	// Token: 0x04000AC6 RID: 2758
	private List<PropDynamic> number3 = new List<PropDynamic>();

	// Token: 0x04000AC7 RID: 2759
	private List<PropDynamic> number4 = new List<PropDynamic>();

	// Token: 0x04000AC8 RID: 2760
	private List<PropDynamic> keypad = new List<PropDynamic>();

	// Token: 0x04000AC9 RID: 2761
	private List<MathCounter> counter8 = new List<MathCounter>();

	// Token: 0x04000ACA RID: 2762
	private int num1;

	// Token: 0x04000ACB RID: 2763
	private int num2;

	// Token: 0x04000ACC RID: 2764
	private int num3;

	// Token: 0x04000ACD RID: 2765
	private int num4;

	// Token: 0x04000ACE RID: 2766
	private int correctNum = 2845;

	// Token: 0x04000ACF RID: 2767
	private int currentNum = 1;

	// Token: 0x04000AD0 RID: 2768
	private bool activated;

	// Token: 0x04000AD1 RID: 2769
	private LogicScript ls;
}
