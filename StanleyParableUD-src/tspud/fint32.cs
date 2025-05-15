using System;
using UnityEngine;

// Token: 0x020000BA RID: 186
public struct fint32
{
	// Token: 0x0600044A RID: 1098 RVA: 0x00019DAE File Offset: 0x00017FAE
	public fint32(uint i)
	{
		this.integer = i;
		this.fraction = 0U;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00019DBE File Offset: 0x00017FBE
	public fint32(uint i, uint f)
	{
		this.integer = i;
		this.fraction = f;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00019DD0 File Offset: 0x00017FD0
	public static implicit operator fint32(float f)
	{
		if (f < 0f)
		{
			return new fint32(0U);
		}
		fint32 fint;
		fint.integer = (uint)Mathf.FloorToInt(f);
		fint.fraction = (uint)(100000000f * (f - fint.integer));
		return fint;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00019E12 File Offset: 0x00018012
	public static implicit operator float(fint32 fi)
	{
		return fi.integer + fi.fraction / fint32.FINT32_MAX_FRACTION;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00019E30 File Offset: 0x00018030
	public static fint32 operator +(fint32 a, fint32 b)
	{
		fint32 fint;
		fint.integer = a.integer + b.integer;
		uint num = a.fraction + b.fraction;
		if (num > fint32.FINT32_MAX_FRACTION)
		{
			num -= fint32.FINT32_MAX_FRACTION;
			fint.integer += 1U;
		}
		fint.fraction = num;
		return fint;
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00019E84 File Offset: 0x00018084
	public static fint32 operator +(fint32 a, float b)
	{
		fint32 fint = b;
		fint32 fint2;
		fint2.integer = a.integer + fint.integer;
		uint num = a.fraction + fint.fraction;
		if (num > fint32.FINT32_MAX_FRACTION)
		{
			num -= fint32.FINT32_MAX_FRACTION;
			fint2.integer += 1U;
		}
		fint2.fraction = num;
		return fint2;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00019EDF File Offset: 0x000180DF
	public static bool operator >(fint32 a, fint32 b)
	{
		return a.integer > b.integer || (a.integer >= b.integer && a.fraction > b.fraction);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00019F0F File Offset: 0x0001810F
	public static bool operator <(fint32 a, fint32 b)
	{
		return a.integer < b.integer || (a.integer <= b.integer && a.fraction < b.fraction);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00019F40 File Offset: 0x00018140
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"( ",
			this.integer,
			".",
			this.fraction.ToString("D8"),
			" )"
		});
	}

	// Token: 0x0400043B RID: 1083
	private uint integer;

	// Token: 0x0400043C RID: 1084
	private uint fraction;

	// Token: 0x0400043D RID: 1085
	public static readonly uint FINT32_MAX_FRACTION = 100000000U;
}
