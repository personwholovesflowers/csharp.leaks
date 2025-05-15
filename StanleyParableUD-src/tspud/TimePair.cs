using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
internal struct TimePair
{
	// Token: 0x06000454 RID: 1108 RVA: 0x00019F9D File Offset: 0x0001819D
	public TimePair(float duration)
	{
		this.startTime = Singleton<GameMaster>.Instance.GameTime;
		this.endTime = this.startTime + duration;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00019FC1 File Offset: 0x000181C1
	public TimePair(float start, float duration)
	{
		this.startTime = start;
		this.endTime = this.startTime + duration;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00019FE1 File Offset: 0x000181E1
	public bool IsFinished()
	{
		return Singleton<GameMaster>.Instance.GameTime > this.endTime;
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000457 RID: 1111 RVA: 0x00019FF8 File Offset: 0x000181F8
	public bool keepWaiting
	{
		get
		{
			return Singleton<GameMaster>.Instance.GameTime < this.endTime;
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001A00F File Offset: 0x0001820F
	public float InverseLerp()
	{
		return Mathf.InverseLerp(this.startTime, this.endTime, Singleton<GameMaster>.Instance.GameTime);
	}

	// Token: 0x0400043E RID: 1086
	private fint32 startTime;

	// Token: 0x0400043F RID: 1087
	private fint32 endTime;
}
