using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class control_clock_nut : LogicScriptBase
{
	// Token: 0x06000AC1 RID: 2753 RVA: 0x0003232C File Offset: 0x0003052C
	private void Start()
	{
		base.InvokeRepeating("Think", 0f, 0.1f);
		LogicScript component = base.GetComponent<LogicScript>();
		for (int i = 0; i < component.groups[0].group.Count; i++)
		{
			this.group00.Add(component.groups[0].group[i] as TextureToggle);
		}
		for (int j = 0; j < component.groups[1].group.Count; j++)
		{
			this.group01.Add(component.groups[1].group[j] as TextureToggle);
		}
		for (int k = 0; k < component.groups[2].group.Count; k++)
		{
			this.group02.Add(component.groups[2].group[k] as TextureToggle);
		}
		for (int l = 0; l < component.groups[3].group.Count; l++)
		{
			this.group03.Add(component.groups[3].group[l] as TextureToggle);
		}
		for (int m = 0; m < component.groups[4].group.Count; m++)
		{
			this.group04.Add(component.groups[4].group[m] as TextureToggle);
		}
		for (int n = 0; n < component.groups[5].group.Count; n++)
		{
			this.group05.Add(component.groups[5].group[n] as LogicTimer);
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x00032514 File Offset: 0x00030714
	private void Think()
	{
		if (!this.isEnabled)
		{
			return;
		}
		this.time_counter++;
		if (this.hun_enabled)
		{
			for (int i = 0; i < this.group03.Count; i++)
			{
				this.group03[i].Input_SetTextureIndex((float)this.time_counter);
			}
		}
		if (this.time_counter == 10)
		{
			this.time_counter = 0;
			this.time--;
			if (this.time / 60 < this.min)
			{
				this.min--;
				for (int j = 0; j < this.group00.Count; j++)
				{
					this.group00[j].Input_SetTextureIndex((float)(9 - this.min));
				}
				this.sec_ten = 6;
			}
			if (this.time / 10 - this.min * 6 < this.sec_ten)
			{
				this.sec_ten--;
				if (this.sec_ten < 0)
				{
					this.sec_ten = 5;
				}
				for (int k = 0; k < this.group01.Count; k++)
				{
					this.group01[k].Input_SetTextureIndex((float)(9 - this.sec_ten));
				}
			}
			this.sec_one = this.time - this.min * 60 - this.sec_ten * 10;
			for (int l = 0; l < this.group02.Count; l++)
			{
				this.group02[l].Input_SetTextureIndex((float)(9 - this.sec_one));
			}
			if (this.time < 0)
			{
				this.Disable();
				for (int m = 0; m < this.group00.Count; m++)
				{
					this.group00[m].Input_SetTextureIndex(9f);
				}
				for (int n = 0; n < this.group01.Count; n++)
				{
					this.group01[n].Input_SetTextureIndex(9f);
				}
				for (int num = 0; num < this.group02.Count; num++)
				{
					this.group02[num].Input_SetTextureIndex(9f);
				}
				for (int num2 = 0; num2 < this.group03.Count; num2++)
				{
					this.group03[num2].Input_SetTextureIndex(9f);
				}
				for (int num3 = 0; num3 < this.group04.Count; num3++)
				{
					this.group04[num3].Input_SetTextureIndex(9f);
				}
			}
			if (!this.hun_enabled)
			{
				this.hun_enabled = true;
				for (int num4 = 0; num4 < this.group05.Count; num4++)
				{
					this.group05[num4].Input_Enable();
				}
			}
		}
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x000327E4 File Offset: 0x000309E4
	private void SetTime(int arg)
	{
		if (arg < 0 || arg > 540)
		{
			return;
		}
		this.time = Mathf.RoundToInt((float)arg);
		this.min = Mathf.FloorToInt((float)(this.time / 60));
		this.sec_ten = this.time / 10 - this.min * 6;
		this.sec_one = this.time - this.min * 60 - this.sec_ten * 10;
		this.time_counter = 0;
		this.Refresh();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x00032865 File Offset: 0x00030A65
	private void Enable()
	{
		this.isEnabled = true;
		this.Refresh();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00032874 File Offset: 0x00030A74
	private void Disable()
	{
		this.isEnabled = false;
		for (int i = 0; i < this.group05.Count; i++)
		{
			this.group05[i].Input_Disable();
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000328B0 File Offset: 0x00030AB0
	private void Refresh()
	{
		this.time_counter = 0;
		this.hun_enabled = false;
		for (int i = 0; i < this.group00.Count; i++)
		{
			this.group00[i].Input_SetTextureIndex((float)(9 - this.min));
		}
		for (int j = 0; j < this.group01.Count; j++)
		{
			this.group01[j].Input_SetTextureIndex((float)(9 - this.sec_ten));
		}
		for (int k = 0; k < this.group02.Count; k++)
		{
			this.group02[k].Input_SetTextureIndex((float)(9 - this.sec_one));
		}
		for (int l = 0; l < this.group03.Count; l++)
		{
			this.group03[l].Input_SetTextureIndex(9f);
		}
		for (int m = 0; m < this.group04.Count; m++)
		{
			this.group04[m].Input_SetTextureIndex(9f);
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x000329BC File Offset: 0x00030BBC
	public override void ParseCommand(string command)
	{
		if (command == "Enable()")
		{
			this.Enable();
			return;
		}
		if (!(command == "Disable()"))
		{
			if (command.Substring(0, 7) == "SetTime")
			{
				int num;
				int.TryParse(command.Substring(8, command.Length - 9), out num);
				if (num > 0)
				{
					this.SetTime(num);
				}
			}
			return;
		}
		this.Disable();
	}

	// Token: 0x04000AA9 RID: 2729
	private int time = 120;

	// Token: 0x04000AAA RID: 2730
	private int time_counter;

	// Token: 0x04000AAB RID: 2731
	private int min = 2;

	// Token: 0x04000AAC RID: 2732
	private int sec_ten;

	// Token: 0x04000AAD RID: 2733
	private int sec_one;

	// Token: 0x04000AAE RID: 2734
	private bool isEnabled;

	// Token: 0x04000AAF RID: 2735
	private bool hun_enabled;

	// Token: 0x04000AB0 RID: 2736
	private List<TextureToggle> group00 = new List<TextureToggle>();

	// Token: 0x04000AB1 RID: 2737
	private List<TextureToggle> group01 = new List<TextureToggle>();

	// Token: 0x04000AB2 RID: 2738
	private List<TextureToggle> group02 = new List<TextureToggle>();

	// Token: 0x04000AB3 RID: 2739
	private List<TextureToggle> group03 = new List<TextureToggle>();

	// Token: 0x04000AB4 RID: 2740
	private List<TextureToggle> group04 = new List<TextureToggle>();

	// Token: 0x04000AB5 RID: 2741
	private List<LogicTimer> group05 = new List<LogicTimer>();
}
