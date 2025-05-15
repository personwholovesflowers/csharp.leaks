using System;
using TMPro;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class IntroTimeDisplay : MonoBehaviour
{
	// Token: 0x060005F4 RID: 1524 RVA: 0x00020A68 File Offset: 0x0001EC68
	private void Start()
	{
		if (this.timeOutput != null)
		{
			this.timeOutput.Init();
			this.timeOutput.SaveToDiskAll();
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00020A8E File Offset: 0x0001EC8E
	public void RecordTimeToConfigurable()
	{
		if (this.timeOutput != null)
		{
			this.timeOutput.Init();
			this.timeOutput.SetValue(this.GetFormattedTimeForText());
			this.timeOutput.SaveToDiskAll();
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00020AC8 File Offset: 0x0001ECC8
	public string GetFormattedTimeForText()
	{
		return string.Concat(new string[]
		{
			this.hours.text,
			":",
			this.minutes.text,
			" ",
			this.ampmText.text
		});
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00020B1C File Offset: 0x0001ED1C
	public static string GetFormattedStringForTime(int minutes, int hours)
	{
		string text;
		string hourString = IntroTimeDisplay.GetHourString(hours, out text);
		string minuteString = IntroTimeDisplay.GetMinuteString(hours);
		return string.Concat(new string[] { hourString, ":", minuteString, " ", text });
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00020B60 File Offset: 0x0001ED60
	public static string GetMinuteString(int minute)
	{
		if (minute > 9)
		{
			return minute.ToString();
		}
		return "0" + minute;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00020B80 File Offset: 0x0001ED80
	public static string GetHourString(int hour, out string ampmString)
	{
		int num = hour;
		string text = "AM";
		if (num >= 12)
		{
			num -= 12;
			text = "PM";
		}
		if (num == 0)
		{
			num += 12;
		}
		ampmString = text;
		if (num > 9)
		{
			return num.ToString();
		}
		return "0" + num;
	}

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	private TextMeshProUGUI hours;

	// Token: 0x0400063C RID: 1596
	[SerializeField]
	private TextMeshProUGUI minutes;

	// Token: 0x0400063D RID: 1597
	[SerializeField]
	private TextMeshProUGUI ampmText;

	// Token: 0x0400063E RID: 1598
	[SerializeField]
	private StringConfigurable timeOutput;
}
