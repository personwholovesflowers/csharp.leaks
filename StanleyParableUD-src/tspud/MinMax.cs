using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
[Serializable]
public struct MinMax
{
	// Token: 0x0600045C RID: 1116 RVA: 0x0001A070 File Offset: 0x00018270
	public MinMax(float low, float high)
	{
		this.Min = low;
		this.Max = high;
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001A080 File Offset: 0x00018280
	public MinMax(float i)
	{
		this.Min = i;
		this.Max = i;
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001A090 File Offset: 0x00018290
	public float Range()
	{
		return Mathf.Abs(this.Max - this.Min);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001A0A4 File Offset: 0x000182A4
	public float Random()
	{
		return global::UnityEngine.Random.Range(this.Min, this.Max);
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001A0B7 File Offset: 0x000182B7
	public float MinOrMax()
	{
		if (global::UnityEngine.Random.value > 0.5f)
		{
			return this.Min;
		}
		return this.Max;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001A0D2 File Offset: 0x000182D2
	public float Lerp(float t)
	{
		t = Mathf.Clamp01(t);
		return this.LerpUnclamped(t);
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001A0E3 File Offset: 0x000182E3
	public float LerpUnclamped(float t)
	{
		return (1f - t) * this.Min + t * this.Max;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001A0FC File Offset: 0x000182FC
	public float Average()
	{
		return this.Lerp(0.5f);
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001A109 File Offset: 0x00018309
	public float ILerp(float t)
	{
		return (t - this.Min) / this.Range();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001A11A File Offset: 0x0001831A
	public static MinMax operator *(MinMax mm, float f)
	{
		return new MinMax(mm.Min * f, mm.Max * f);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001A131 File Offset: 0x00018331
	public static MinMax operator *(MinMax mm, MinMax mm2)
	{
		return new MinMax(mm.Min * mm2.Min, mm.Max * mm2.Max);
	}

	// Token: 0x04000441 RID: 1089
	public float Min;

	// Token: 0x04000442 RID: 1090
	public float Max;
}
