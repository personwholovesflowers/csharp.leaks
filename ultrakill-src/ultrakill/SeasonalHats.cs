using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
public class SeasonalHats : MonoBehaviour
{
	// Token: 0x06001660 RID: 5728 RVA: 0x000B45C4 File Offset: 0x000B27C4
	private void Start()
	{
		if (!MonoSingleton<PrefsManager>.Instance.GetBool("seasonalEvents", false) || MonoSingleton<StatsManager>.Instance.firstPlayThrough)
		{
			return;
		}
		this.time = DateTime.Now;
		int month = this.time.Month;
		if (month == 10)
		{
			if (this.time.Day >= 25 && this.time.Day <= 31)
			{
				this.halloween.SetActive(true);
			}
			return;
		}
		if (month == 12)
		{
			if (this.time.Day >= 22 && this.time.Day <= 28)
			{
				this.christmas.SetActive(true);
			}
			return;
		}
		DateTime dateTime = this.GetEaster(this.time.Year);
		if (this.time.DayOfYear >= dateTime.DayOfYear - 2 && this.time.DayOfYear <= dateTime.DayOfYear)
		{
			this.easter.SetActive(true);
		}
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x000B46B0 File Offset: 0x000B28B0
	private DateTime GetEaster(int year)
	{
		int num = year % 19;
		int num2 = year / 100;
		int num3 = (num2 - num2 / 4 - (8 * num2 + 13) / 25 + 19 * num + 15) % 30;
		int num4 = num3 - num3 / 28 * (1 - num3 / 28 * (29 / (num3 + 1)) * ((21 - num) / 11));
		int num5 = num4 - (year + year / 4 + num4 + 2 - num2 + num2 / 4) % 7;
		int num6 = 3 + (num5 + 40) / 44;
		int num7 = num5 + 28 - 31 * (num6 / 4);
		return new DateTime(year, num6, num7);
	}

	// Token: 0x04001EE3 RID: 7907
	private DateTime time;

	// Token: 0x04001EE4 RID: 7908
	[SerializeField]
	private GameObject christmas;

	// Token: 0x04001EE5 RID: 7909
	[SerializeField]
	private GameObject halloween;

	// Token: 0x04001EE6 RID: 7910
	[SerializeField]
	private GameObject easter;
}
