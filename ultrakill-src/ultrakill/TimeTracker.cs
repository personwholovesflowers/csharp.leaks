using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class TimeTracker : MonoSingleton<TimeTracker>
{
	// Token: 0x06001A87 RID: 6791 RVA: 0x000DA694 File Offset: 0x000D8894
	private void Update()
	{
		this.timeNow = DateTime.Now;
		this.hours = (float)this.timeNow.Hour;
		this.minutes = (float)this.timeNow.Minute;
		this.seconds = (float)this.timeNow.Second;
	}

	// Token: 0x04002536 RID: 9526
	[HideInInspector]
	public DateTime timeNow;

	// Token: 0x04002537 RID: 9527
	[HideInInspector]
	public float hours;

	// Token: 0x04002538 RID: 9528
	[HideInInspector]
	public float minutes;

	// Token: 0x04002539 RID: 9529
	[HideInInspector]
	public float seconds;
}
