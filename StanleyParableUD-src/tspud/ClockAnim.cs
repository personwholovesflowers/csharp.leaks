using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class ClockAnim : MonoBehaviour
{
	// Token: 0x060003D7 RID: 983 RVA: 0x0001891C File Offset: 0x00016B1C
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		float num = ((float)this.startHour + (float)this.startMinute / 60f) / 12f;
		float num2 = ((float)this.startMinute + (float)this.startSecond / 60f) / 60f;
		float num3 = (float)this.startSecond / 60f;
		this.anim.Play("HourHand", 0, num);
		this.anim.Play("MinuteHand", 1, num2);
		this.anim.Play("SecondHand", 2, num3);
		GameMaster.OnPause += this.Pause;
		GameMaster.OnResume += this.Resume;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x000189D3 File Offset: 0x00016BD3
	private void OnDisable()
	{
		GameMaster.OnPause -= this.Pause;
		GameMaster.OnResume -= this.Resume;
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x000189F7 File Offset: 0x00016BF7
	private void Pause()
	{
		this.anim.enabled = false;
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00018A05 File Offset: 0x00016C05
	private void Resume()
	{
		this.anim.enabled = true;
	}

	// Token: 0x040003D8 RID: 984
	private Animator anim;

	// Token: 0x040003D9 RID: 985
	public int startHour;

	// Token: 0x040003DA RID: 986
	public int startMinute;

	// Token: 0x040003DB RID: 987
	public int startSecond;
}
