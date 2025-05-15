using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class BeatInfo : MonoBehaviour
{
	// Token: 0x06000221 RID: 545 RVA: 0x0000B47C File Offset: 0x0000967C
	public void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.timeSignatureChanges.Length != 0)
		{
			float num = 0f;
			float num2 = this.timeSignature;
			float num3 = 0f;
			for (int i = 0; i < this.timeSignatureChanges.Length; i++)
			{
				this.timeSignatureChanges[i].time = num + 60f / this.bpm * 4f * (this.timeSignatureChanges[i].onMeasure - num3 - 1f) * num2;
				num = this.timeSignatureChanges[i].time;
				num2 = this.timeSignatureChanges[i].timeSignature;
				num3 = this.timeSignatureChanges[i].onMeasure - 1f;
			}
		}
	}

	// Token: 0x04000260 RID: 608
	[HideInInspector]
	public bool valuesSet;

	// Token: 0x04000261 RID: 609
	public float bpm;

	// Token: 0x04000262 RID: 610
	public float timeSignature;

	// Token: 0x04000263 RID: 611
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x04000264 RID: 612
	public TimeSignatureChange[] timeSignatureChanges;
}
