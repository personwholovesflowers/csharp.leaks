using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
[Serializable]
public struct UnscaledTimeSince
{
	// Token: 0x06001A85 RID: 6789 RVA: 0x000DA662 File Offset: 0x000D8862
	public static implicit operator float(UnscaledTimeSince ts)
	{
		return Time.unscaledTime - ts.time;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000DA670 File Offset: 0x000D8870
	public static implicit operator UnscaledTimeSince(float ts)
	{
		return new UnscaledTimeSince
		{
			time = Time.unscaledTime - ts
		};
	}

	// Token: 0x04002534 RID: 9524
	private float time;

	// Token: 0x04002535 RID: 9525
	public const int Now = 0;
}
