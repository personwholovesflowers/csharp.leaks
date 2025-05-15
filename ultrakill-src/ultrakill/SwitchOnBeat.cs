using System;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class SwitchOnBeat : MonoBehaviour
{
	// Token: 0x060019E3 RID: 6627 RVA: 0x000D45E7 File Offset: 0x000D27E7
	private void Awake()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x000D45F8 File Offset: 0x000D27F8
	private void Initialize()
	{
		this.initialized = true;
		if (!this.currentBeatInfo)
		{
			this.currentBeatInfo = this.switches[this.target];
		}
		BeatInfo[] array = this.switches;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetValues();
		}
		this.timer = this.currentBeatInfo.aud.time;
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000D465F File Offset: 0x000D285F
	private void Update()
	{
		this.timer = this.currentBeatInfo.aud.time;
		if (this.switching && this.timer >= this.nextMeasure)
		{
			this.Switch();
		}
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000D4694 File Offset: 0x000D2894
	private void Switch()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.switching = false;
		for (int i = 0; i < this.switches.Length; i++)
		{
			if (i == this.target)
			{
				this.switches[i].gameObject.SetActive(true);
				this.currentBeatInfo = this.switches[i];
				this.timer = this.currentBeatInfo.aud.time;
			}
			else
			{
				this.switches[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000D4720 File Offset: 0x000D2920
	public void SetTarget(int newTarget)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.target = newTarget;
		this.switching = true;
		this.nextMeasure = 0f;
		if (this.currentBeatInfo.timeSignatureChanges != null && this.currentBeatInfo.timeSignatureChanges.Length != 0 && this.currentBeatInfo.timeSignatureChanges[0] != null)
		{
			if (this.timer >= this.currentBeatInfo.timeSignatureChanges[0].time)
			{
				for (int i = 0; i < this.currentBeatInfo.timeSignatureChanges.Length; i++)
				{
					if (this.currentBeatInfo.timeSignatureChanges[i].time > this.timer)
					{
						this.nextMeasure = this.currentBeatInfo.timeSignatureChanges[i - 1].time;
						while (this.nextMeasure < this.timer)
						{
							this.nextMeasure += 60f / this.currentBeatInfo.bpm * 4f * this.currentBeatInfo.timeSignatureChanges[i - 1].timeSignature;
						}
						break;
					}
					if (i == this.currentBeatInfo.timeSignatureChanges.Length - 1)
					{
						this.nextMeasure = this.currentBeatInfo.timeSignatureChanges[i].time;
						while (this.nextMeasure < this.timer)
						{
							this.nextMeasure += 60f / this.currentBeatInfo.bpm * 4f * this.currentBeatInfo.timeSignatureChanges[i].timeSignature;
						}
					}
				}
				goto IL_01C1;
			}
		}
		while (this.nextMeasure < this.timer)
		{
			this.nextMeasure += 60f / this.currentBeatInfo.bpm * 4f * this.currentBeatInfo.timeSignature;
		}
		IL_01C1:
		if (this.nextMeasure >= this.currentBeatInfo.aud.clip.length)
		{
			this.nextMeasure = 0f;
		}
	}

	// Token: 0x0400243C RID: 9276
	[HideInInspector]
	public BeatInfo currentBeatInfo;

	// Token: 0x0400243D RID: 9277
	private float timer;

	// Token: 0x0400243E RID: 9278
	private float nextMeasure;

	// Token: 0x0400243F RID: 9279
	private bool switching;

	// Token: 0x04002440 RID: 9280
	private int target;

	// Token: 0x04002441 RID: 9281
	public BeatInfo[] switches;

	// Token: 0x04002442 RID: 9282
	private bool initialized;
}
