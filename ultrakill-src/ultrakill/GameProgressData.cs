using System;

// Token: 0x02000216 RID: 534
[Serializable]
public class GameProgressData
{
	// Token: 0x06000B41 RID: 2881 RVA: 0x00050975 File Offset: 0x0004EB75
	public GameProgressData()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x04000EE0 RID: 3808
	public int levelNum;

	// Token: 0x04000EE1 RID: 3809
	public int difficulty;

	// Token: 0x04000EE2 RID: 3810
	public int[] primeLevels;

	// Token: 0x04000EE3 RID: 3811
	public int encores;
}
