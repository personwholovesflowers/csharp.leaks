using System;
using UnityEngine;

// Token: 0x02000482 RID: 1154
public static class TimeHelper
{
	// Token: 0x06001A7A RID: 6778 RVA: 0x000D9D9C File Offset: 0x000D7F9C
	public static string ConvertSecondsToString(float seconds)
	{
		int num = Mathf.FloorToInt(seconds / 60f);
		string text = (seconds % 60f).ToString("00.000");
		return num.ToString() + ":" + text;
	}
}
