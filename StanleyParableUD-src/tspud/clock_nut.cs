using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class clock_nut : LogicScriptBase
{
	// Token: 0x06000ABB RID: 2747 RVA: 0x00031D54 File Offset: 0x0002FF54
	private void Start()
	{
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
			this.group05.Add(component.groups[5].group[n] as TextureToggle);
		}
		for (int num = 0; num < component.groups[6].group.Count; num++)
		{
			this.group06.Add(component.groups[6].group[num] as LogicTimer);
		}
		this.SetTime();
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00031F7A File Offset: 0x0003017A
	private void FixedUpdate()
	{
		this.Think();
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00031F84 File Offset: 0x00030184
	private void Think()
	{
		for (int i = 0; i < this.group04.Count; i++)
		{
			this.group04[i].Input_SetTextureIndex(9f - this.t_hun);
		}
		for (int j = 0; j < this.group05.Count; j++)
		{
			this.group05[j].Input_SetTextureIndex(0f);
		}
		this.t_hun += Time.deltaTime * 10f;
		if (this.t_hun >= 10f)
		{
			this.t_hun -= 10f;
			this.t_total++;
			if (Mathf.FloorToInt((float)(this.t_total / 600)) > this.t_min_ten)
			{
				this.t_min_ten++;
				this.t_min_one = 0;
				this.t_sec_ten = 0;
			}
			if (Mathf.FloorToInt((float)(this.t_total / 60 - 10 * this.t_min_ten)) > this.t_min_one)
			{
				this.t_min_one++;
				this.t_sec_ten = 0;
			}
			if (Mathf.FloorToInt((float)(this.t_total / 10 - 60 * this.t_min_ten - 6 * this.t_min_one)) > this.t_sec_ten)
			{
				this.t_sec_ten++;
			}
			this.t_sec_one++;
			if (this.t_sec_one == 10)
			{
				this.t_sec_one = 0;
			}
			this.Refresh();
			if (!this.ten_enabled)
			{
				this.ten_enabled = true;
				for (int k = 0; k < this.group06.Count; k++)
				{
					this.group06[k].Input_Enable();
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00032140 File Offset: 0x00030340
	private void SetTime()
	{
		int num = 912;
		this.t_total = Mathf.RoundToInt((float)num);
		this.t_min_ten = Mathf.FloorToInt((float)(this.t_total / 600));
		this.t_min_one = Mathf.FloorToInt((float)(this.t_total / 60 - 10 * this.t_min_ten));
		this.t_sec_ten = Mathf.FloorToInt((float)(this.t_total / 10 - 60 * this.t_min_ten - 6 * this.t_min_one));
		this.t_sec_one = Mathf.FloorToInt((float)(this.t_total - 600 * this.t_min_ten - 60 * this.t_min_one - 10 * this.t_sec_ten));
		this.Refresh();
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x000321F8 File Offset: 0x000303F8
	private void Refresh()
	{
		for (int i = 0; i < this.group00.Count; i++)
		{
			this.group00[i].Input_SetTextureIndex((float)(9 - this.t_min_ten));
		}
		for (int j = 0; j < this.group01.Count; j++)
		{
			this.group01[j].Input_SetTextureIndex((float)(9 - this.t_min_one));
		}
		for (int k = 0; k < this.group02.Count; k++)
		{
			this.group02[k].Input_SetTextureIndex((float)(9 - this.t_sec_ten));
		}
		for (int l = 0; l < this.group01.Count; l++)
		{
			this.group03[l].Input_SetTextureIndex((float)(9 - this.t_sec_one));
		}
	}

	// Token: 0x04000A9B RID: 2715
	private bool ten_enabled;

	// Token: 0x04000A9C RID: 2716
	private int t_total;

	// Token: 0x04000A9D RID: 2717
	private int t_min_ten;

	// Token: 0x04000A9E RID: 2718
	private int t_min_one;

	// Token: 0x04000A9F RID: 2719
	private int t_sec_ten;

	// Token: 0x04000AA0 RID: 2720
	private int t_sec_one;

	// Token: 0x04000AA1 RID: 2721
	private float t_hun;

	// Token: 0x04000AA2 RID: 2722
	private List<TextureToggle> group00 = new List<TextureToggle>();

	// Token: 0x04000AA3 RID: 2723
	private List<TextureToggle> group01 = new List<TextureToggle>();

	// Token: 0x04000AA4 RID: 2724
	private List<TextureToggle> group02 = new List<TextureToggle>();

	// Token: 0x04000AA5 RID: 2725
	private List<TextureToggle> group03 = new List<TextureToggle>();

	// Token: 0x04000AA6 RID: 2726
	private List<TextureToggle> group04 = new List<TextureToggle>();

	// Token: 0x04000AA7 RID: 2727
	private List<TextureToggle> group05 = new List<TextureToggle>();

	// Token: 0x04000AA8 RID: 2728
	private List<LogicTimer> group06 = new List<LogicTimer>();
}
