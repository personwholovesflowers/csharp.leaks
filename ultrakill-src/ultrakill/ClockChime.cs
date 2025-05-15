using System;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class ClockChime : MonoBehaviour
{
	// Token: 0x06001D5B RID: 7515 RVA: 0x000F629C File Offset: 0x000F449C
	private void Start()
	{
		this.lastHour = DateTime.Now.Hour;
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x000F62BC File Offset: 0x000F44BC
	private void Update()
	{
		int hour = DateTime.Now.Hour;
		if (this.lastHour != hour && hour % 12 == 4)
		{
			base.GetComponent<AudioSource>().Play();
		}
		this.lastHour = hour;
	}

	// Token: 0x0400299B RID: 10651
	private int lastHour;
}
