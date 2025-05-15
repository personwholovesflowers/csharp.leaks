using System;

// Token: 0x02000103 RID: 259
public class LogicBranch : HammerEntity
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600062B RID: 1579 RVA: 0x00021FDB File Offset: 0x000201DB
	// (set) Token: 0x0600062C RID: 1580 RVA: 0x00021FE3 File Offset: 0x000201E3
	public int value { get; private set; }

	// Token: 0x0600062D RID: 1581 RVA: 0x00021FEC File Offset: 0x000201EC
	private void Awake()
	{
		this.value = this.initialValue;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00021FFA File Offset: 0x000201FA
	public void Input_SetValue(float val)
	{
		this.value = (int)val;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00022004 File Offset: 0x00020204
	public void Input_SetValueTest(float val)
	{
		this.Input_SetValue(val);
		this.Input_Test();
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00022013 File Offset: 0x00020213
	public void Input_Toggle()
	{
		if (this.value == 1)
		{
			this.value = 0;
			return;
		}
		this.value = 1;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002202D File Offset: 0x0002022D
	public void Input_ToggleTest()
	{
		this.Input_Toggle();
		this.Input_Test();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002203B File Offset: 0x0002023B
	public void Input_Test()
	{
		if (this.value == 1)
		{
			base.FireOutput(Outputs.OnTrue);
			return;
		}
		base.FireOutput(Outputs.OnFalse);
	}

	// Token: 0x0400068A RID: 1674
	public int initialValue;
}
