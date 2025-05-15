using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class TSPArea : MonoBehaviour
{
	// Token: 0x04000A0F RID: 2575
	public GameObject StaticObjects;

	// Token: 0x04000A10 RID: 2576
	public GameObject DynamicObjects;

	// Token: 0x04000A11 RID: 2577
	public GameObject LightsBaked;

	// Token: 0x04000A12 RID: 2578
	public GameObject LightsRealtime;

	// Token: 0x04000A13 RID: 2579
	public GameObject Triggers;

	// Token: 0x04000A14 RID: 2580
	public GameObject Audio;

	// Token: 0x04000A15 RID: 2581
	public GameObject Misc;

	// Token: 0x04000A16 RID: 2582
	public GameObject FX;

	// Token: 0x04000A17 RID: 2583
	public GameObject Unsorted;

	// Token: 0x04000A18 RID: 2584
	public GameObject StaticRoot;

	// Token: 0x04000A19 RID: 2585
	public GameObject DynamicRoot;

	// Token: 0x04000A1A RID: 2586
	[SerializeField]
	[HideInInspector]
	private bool setup;

	// Token: 0x020003FD RID: 1021
	public enum AreaRootType
	{
		// Token: 0x040014D2 RID: 5330
		Static,
		// Token: 0x040014D3 RID: 5331
		Dynamic,
		// Token: 0x040014D4 RID: 5332
		LightBaked,
		// Token: 0x040014D5 RID: 5333
		LightRealtime,
		// Token: 0x040014D6 RID: 5334
		Trigger,
		// Token: 0x040014D7 RID: 5335
		Audio,
		// Token: 0x040014D8 RID: 5336
		Misc,
		// Token: 0x040014D9 RID: 5337
		FX,
		// Token: 0x040014DA RID: 5338
		Unsorted
	}
}
