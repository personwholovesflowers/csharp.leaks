using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class Clock : MonoBehaviour
{
	// Token: 0x060003E5 RID: 997 RVA: 0x00019149 File Offset: 0x00017349
	private void Start()
	{
		this.tracker = MonoSingleton<TimeTracker>.Instance;
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00019158 File Offset: 0x00017358
	private void Update()
	{
		this.hour.localRotation = Quaternion.Euler(0f, (this.tracker.hours % 12f / 12f + this.tracker.minutes / 1440f) * 360f, 0f);
		this.minute.localRotation = Quaternion.Euler(0f, (this.tracker.minutes / 60f + this.tracker.seconds / 3600f) * 360f, 0f);
	}

	// Token: 0x040004CD RID: 1229
	public Transform hour;

	// Token: 0x040004CE RID: 1230
	public Transform minute;

	// Token: 0x040004CF RID: 1231
	private TimeTracker tracker;
}
