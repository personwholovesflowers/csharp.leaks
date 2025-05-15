using System;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class DynamicLight : HammerEntity
{
	// Token: 0x06000423 RID: 1059 RVA: 0x00019599 File Offset: 0x00017799
	private void Awake()
	{
		this.light = base.GetComponent<Light>();
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x000195A7 File Offset: 0x000177A7
	public void Input_TurnOn()
	{
		if (this.light)
		{
			this.light.enabled = true;
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000195C2 File Offset: 0x000177C2
	public void Input_TurnOff()
	{
		if (this.light)
		{
			this.light.enabled = false;
		}
	}

	// Token: 0x0400041D RID: 1053
	private Light light;
}
