using System;

// Token: 0x0200035D RID: 861
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class PreviousMissionSaver : MonoSingleton<PreviousMissionSaver>
{
	// Token: 0x04001B5A RID: 7002
	public int previousMission;
}
