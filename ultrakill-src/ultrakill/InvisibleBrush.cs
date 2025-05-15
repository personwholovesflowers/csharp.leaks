using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class InvisibleBrush : MonoBehaviour
{
	// Token: 0x06001555 RID: 5461 RVA: 0x000AE0F0 File Offset: 0x000AC2F0
	private void Update()
	{
		bool anyArmActive = SummonSandboxArm.AnyArmActive;
		if (this.renderer.enabled != anyArmActive)
		{
			this.renderer.enabled = anyArmActive;
		}
	}

	// Token: 0x04001DA1 RID: 7585
	[SerializeField]
	private Renderer renderer;
}
