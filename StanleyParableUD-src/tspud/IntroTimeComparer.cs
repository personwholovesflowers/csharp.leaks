using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020000F5 RID: 245
public class IntroTimeComparer : MonoBehaviour
{
	// Token: 0x060005EF RID: 1519 RVA: 0x00020751 File Offset: 0x0001E951
	private void Awake()
	{
		this.ZEROTIME = IntroTimeDisplay.GetFormattedStringForTime(0, 0);
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00020760 File Offset: 0x0001E960
	private UnityEvent GetTime12ComparisonEvent(string time1, string time2)
	{
		if (time1 == this.ZEROTIME)
		{
			if (time2 == this.ZEROTIME)
			{
				return this.on00_aka_4;
			}
			return this.on0X_aka_2;
		}
		else
		{
			if (time2 == this.ZEROTIME)
			{
				return this.onX0_aka_3;
			}
			if (time1 == time2)
			{
				return this.onXX_aka_5;
			}
			return this.onXY_aka_1;
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000207C4 File Offset: 0x0001E9C4
	public void DoTime12Comparison()
	{
		string text = this.time1Configurable.GetStringValue();
		string text2 = this.time2Configurable.GetStringValue();
		if (text == "")
		{
			text = this.ZEROTIME;
		}
		if (text2 == "")
		{
			text2 = this.ZEROTIME;
		}
		UnityEvent time12ComparisonEvent = this.GetTime12ComparisonEvent(text, text2);
		if (time12ComparisonEvent == null)
		{
			Debug.LogError("no event found for time 1&2 comparison: time1" + text + " time2" + text2);
		}
		if (time12ComparisonEvent != null)
		{
			time12ComparisonEvent.Invoke();
		}
		UnityEvent unityEvent = this.onTime12ComparisonDone;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00020850 File Offset: 0x0001EA50
	public void DoTime123Comparison()
	{
		string text = this.time1Configurable.GetStringValue();
		string text2 = this.time2Configurable.GetStringValue();
		string text3 = this.time3Configurable.GetStringValue();
		if (text == "")
		{
			text = this.ZEROTIME;
		}
		if (text2 == "")
		{
			text2 = this.ZEROTIME;
		}
		if (text3 == "")
		{
			text3 = this.ZEROTIME;
		}
		UnityEvent time12ComparisonEvent = this.GetTime12ComparisonEvent(text, text2);
		if (time12ComparisonEvent == null)
		{
			Debug.LogError(string.Concat(new string[] { "no event found for time 1&2 comparison: time1", text, " time2", text2, " time3", text3 }));
		}
		if (time12ComparisonEvent == this.onXX_aka_5)
		{
			if (text3 == text2)
			{
				UnityEvent unityEvent = this.onXXX_aka_5a;
				if (unityEvent != null)
				{
					unityEvent.Invoke();
				}
			}
			else
			{
				UnityEvent unityEvent2 = this.onXXV_aka_5b;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke();
				}
			}
		}
		else if (time12ComparisonEvent == this.onXY_aka_1)
		{
			if (text3 == this.ZEROTIME)
			{
				UnityEvent unityEvent3 = this.onXY0_aka_1b;
				if (unityEvent3 != null)
				{
					unityEvent3.Invoke();
				}
			}
			else
			{
				UnityEvent unityEvent4 = this.onXYA_aka_1a;
				if (unityEvent4 != null)
				{
					unityEvent4.Invoke();
				}
			}
		}
		else if (time12ComparisonEvent == this.on0X_aka_2)
		{
			if (text3 == this.ZEROTIME)
			{
				UnityEvent unityEvent5 = this.on0X0_aka_2b;
				if (unityEvent5 != null)
				{
					unityEvent5.Invoke();
				}
			}
			else
			{
				UnityEvent unityEvent6 = this.on0XA_aka_2a;
				if (unityEvent6 != null)
				{
					unityEvent6.Invoke();
				}
			}
		}
		else if (time12ComparisonEvent == this.onX0_aka_3)
		{
			if (text3 == this.ZEROTIME)
			{
				UnityEvent unityEvent7 = this.onX00_aka_3b;
				if (unityEvent7 != null)
				{
					unityEvent7.Invoke();
				}
			}
			else
			{
				UnityEvent unityEvent8 = this.onX0A_aka_3a;
				if (unityEvent8 != null)
				{
					unityEvent8.Invoke();
				}
			}
		}
		else if (time12ComparisonEvent == this.on00_aka_4)
		{
			if (text3 == this.ZEROTIME)
			{
				UnityEvent unityEvent9 = this.on000_aka_4b;
				if (unityEvent9 != null)
				{
					unityEvent9.Invoke();
				}
			}
			else
			{
				UnityEvent unityEvent10 = this.on00X_aka_4a;
				if (unityEvent10 != null)
				{
					unityEvent10.Invoke();
				}
			}
		}
		UnityEvent unityEvent11 = this.onTime123ComparisonDone;
		if (unityEvent11 == null)
		{
			return;
		}
		unityEvent11.Invoke();
	}

	// Token: 0x04000626 RID: 1574
	[Header("X|Y|Z=nonzero, 0=zero, A=any nonzero, U|V|W=X|Y|Z or zero")]
	[SerializeField]
	private StringConfigurable time1Configurable;

	// Token: 0x04000627 RID: 1575
	[SerializeField]
	private StringConfigurable time2Configurable;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	private StringConfigurable time3Configurable;

	// Token: 0x04000629 RID: 1577
	[Header("Time1 and Time2")]
	[SerializeField]
	[FormerlySerializedAs("onDifferent")]
	private UnityEvent onXY_aka_1;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	[FormerlySerializedAs("onSame")]
	private UnityEvent onXX_aka_5;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	[FormerlySerializedAs("onBothZero")]
	private UnityEvent on00_aka_4;

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	private UnityEvent on0X_aka_2;

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	private UnityEvent onX0_aka_3;

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	[FormerlySerializedAs("onTime12ComparisonDone")]
	private UnityEvent onTime12ComparisonDone;

	// Token: 0x0400062F RID: 1583
	[Header("Time1, 2 ,3")]
	[SerializeField]
	private UnityEvent onXYA_aka_1a;

	// Token: 0x04000630 RID: 1584
	[SerializeField]
	private UnityEvent onXY0_aka_1b;

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private UnityEvent on0XA_aka_2a;

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	private UnityEvent on0X0_aka_2b;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private UnityEvent onX0A_aka_3a;

	// Token: 0x04000634 RID: 1588
	[SerializeField]
	private UnityEvent onX00_aka_3b;

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	private UnityEvent on00X_aka_4a;

	// Token: 0x04000636 RID: 1590
	[SerializeField]
	private UnityEvent on000_aka_4b;

	// Token: 0x04000637 RID: 1591
	[SerializeField]
	private UnityEvent onXXX_aka_5a;

	// Token: 0x04000638 RID: 1592
	[SerializeField]
	private UnityEvent onXXV_aka_5b;

	// Token: 0x04000639 RID: 1593
	[SerializeField]
	private UnityEvent onTime123ComparisonDone;

	// Token: 0x0400063A RID: 1594
	private string ZEROTIME = "12:00 AM";
}
