using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000484 RID: 1156
[Serializable]
public struct TimeSince
{
	// Token: 0x06001A82 RID: 6786 RVA: 0x000DA60A File Offset: 0x000D880A
	public static implicit operator float(TimeSince ts)
	{
		return Time.time - ts.time;
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000DA618 File Offset: 0x000D8818
	public static implicit operator TimeSince(float ts)
	{
		return new TimeSince
		{
			time = Time.time - ts
		};
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000DA63C File Offset: 0x000D883C
	public new string ToString()
	{
		return this.ToString(CultureInfo.InvariantCulture);
	}

	// Token: 0x04002532 RID: 9522
	private float time;

	// Token: 0x04002533 RID: 9523
	public const int Now = 0;
}
