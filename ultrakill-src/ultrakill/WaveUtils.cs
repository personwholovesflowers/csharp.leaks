using System;
using plog;
using UnityEngine;

// Token: 0x02000187 RID: 391
public static class WaveUtils
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00032A1B File Offset: 0x00030C1B
	public static bool IsWaveSelectable(int waveToCheck, int highestWave)
	{
		return highestWave >= waveToCheck * 2;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00032A26 File Offset: 0x00030C26
	public static bool IsValidStartingWave(int wave)
	{
		WaveUtils.Log.Info("Checking wave validity: " + wave.ToString(), null, null, null);
		return wave >= 0 && wave % 5 == 0;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00032A52 File Offset: 0x00030C52
	public static int GetSafeStartingWave(int requestedWave)
	{
		if (WaveUtils.IsValidStartingWave(requestedWave))
		{
			return requestedWave;
		}
		WaveUtils.Log.Warning("Invalid starting wave format", null, null, null);
		return 0;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00032A74 File Offset: 0x00030C74
	public static int? GetHighestWaveForDifficulty(int difficulty)
	{
		CyberRankData bestCyber = GameProgressSaver.GetBestCyber();
		if (bestCyber != null && bestCyber.preciseWavesByDifficulty.Length > difficulty)
		{
			return new int?(Mathf.FloorToInt(bestCyber.preciseWavesByDifficulty[difficulty]));
		}
		return null;
	}

	// Token: 0x040009D6 RID: 2518
	private static readonly global::plog.Logger Log = new global::plog.Logger("WaveUtils");
}
