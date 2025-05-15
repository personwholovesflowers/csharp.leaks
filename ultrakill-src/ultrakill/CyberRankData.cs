using System;

// Token: 0x020000FE RID: 254
[Serializable]
public class CyberRankData
{
	// Token: 0x060004E9 RID: 1257 RVA: 0x00021487 File Offset: 0x0001F687
	public CyberRankData()
	{
		this.preciseWavesByDifficulty = new float[6];
		this.kills = new int[6];
		this.style = new int[6];
		this.time = new float[6];
	}

	// Token: 0x040006AB RID: 1707
	public int wave;

	// Token: 0x040006AC RID: 1708
	public float[] preciseWavesByDifficulty;

	// Token: 0x040006AD RID: 1709
	public int[] kills;

	// Token: 0x040006AE RID: 1710
	public int[] style;

	// Token: 0x040006AF RID: 1711
	public float[] time;
}
