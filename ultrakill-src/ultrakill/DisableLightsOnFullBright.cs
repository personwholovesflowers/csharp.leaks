using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class DisableLightsOnFullBright : MonoBehaviour
{
	// Token: 0x06000521 RID: 1313 RVA: 0x0002252A File Offset: 0x0002072A
	private void OnEnable()
	{
		this.fullBrightActive = FullBright.Enabled;
		if (!this.fullBrightActive)
		{
			return;
		}
		this.SetLightsEnabled(false);
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00022547 File Offset: 0x00020747
	private void Update()
	{
		if (this.fullBrightActive && !FullBright.Enabled)
		{
			this.fullBrightActive = false;
			this.SetLightsEnabled(true);
			return;
		}
		if (!this.fullBrightActive && FullBright.Enabled)
		{
			this.fullBrightActive = true;
			this.SetLightsEnabled(false);
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00022584 File Offset: 0x00020784
	private void SetLightsEnabled(bool isEnabled)
	{
		if (this.lights == null)
		{
			this.lights = base.GetComponentsInChildren<Light>();
		}
		Light[] array = this.lights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = isEnabled;
		}
	}

	// Token: 0x0400070D RID: 1805
	private Light[] lights;

	// Token: 0x0400070E RID: 1806
	private bool fullBrightActive;
}
