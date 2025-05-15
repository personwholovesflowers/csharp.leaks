using System;
using Nest.Components;

// Token: 0x02000107 RID: 263
public class LogicRelay : HammerEntity
{
	// Token: 0x0600063E RID: 1598 RVA: 0x0002255A File Offset: 0x0002075A
	private void Start()
	{
		if (this.auto)
		{
			base.FireOutput(Outputs.OnMapSpawn);
		}
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002256B File Offset: 0x0002076B
	public override void Input_Kill()
	{
		this.killed = true;
		base.Input_Kill();
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0002257C File Offset: 0x0002077C
	public void Input_Trigger()
	{
		if (this.isEnabled && !this.killed)
		{
			base.FireOutput(Outputs.OnTrigger);
			for (int i = 0; i < this.manualOutputs.Length; i++)
			{
				if (this.manualOutputs[i] != null)
				{
					this.manualOutputs[i].Invoke();
				}
			}
		}
	}

	// Token: 0x04000696 RID: 1686
	public bool auto;

	// Token: 0x04000697 RID: 1687
	public bool killed;

	// Token: 0x04000698 RID: 1688
	public NestInput[] manualOutputs;
}
