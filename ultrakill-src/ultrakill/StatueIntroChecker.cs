using System;

// Token: 0x02000453 RID: 1107
public class StatueIntroChecker : MonoSingleton<StatueIntroChecker>
{
	// Token: 0x06001940 RID: 6464 RVA: 0x000CF37E File Offset: 0x000CD57E
	public void BeenSeen()
	{
		this.beenSeen = true;
	}

	// Token: 0x0400236D RID: 9069
	public bool beenSeen;
}
