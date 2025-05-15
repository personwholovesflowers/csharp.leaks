using System;

// Token: 0x020001B7 RID: 439
public class TriggerChangelevel : TriggerMultiple
{
	// Token: 0x06000A2F RID: 2607 RVA: 0x0003015F File Offset: 0x0002E35F
	public void Input_ChangeLevel()
	{
		if (this.isEnabled)
		{
			Singleton<GameMaster>.Instance.ChangeLevel(this.destination, true);
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0003017B File Offset: 0x0002E37B
	public void Input_ChangeDestination(string newDestination)
	{
		this.destination = newDestination;
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00030184 File Offset: 0x0002E384
	protected override void StartTouch()
	{
		if (!this.isEnabled)
		{
			return;
		}
		base.StartTouch();
		Singleton<GameMaster>.Instance.ChangeLevel(this.destination, true);
	}

	// Token: 0x04000A29 RID: 2601
	public string destination = "";
}
