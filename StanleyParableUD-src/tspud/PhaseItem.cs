using System;
using UnityEngine.Events;

// Token: 0x0200007B RID: 123
[Serializable]
public class PhaseItem
{
	// Token: 0x04000309 RID: 777
	public PhaseItem.PhaseMode Mode;

	// Token: 0x0400030A RID: 778
	public string Description;

	// Token: 0x0400030B RID: 779
	public float Duration;

	// Token: 0x0400030C RID: 780
	public UnityEvent PhaseEvent;

	// Token: 0x0200037F RID: 895
	public enum PhaseMode
	{
		// Token: 0x040012A8 RID: 4776
		Default,
		// Token: 0x040012A9 RID: 4777
		Disabled
	}
}
