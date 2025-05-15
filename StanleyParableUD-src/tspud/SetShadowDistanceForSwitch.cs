using System;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class SetShadowDistanceForSwitch : MonoBehaviour
{
	// Token: 0x06000A12 RID: 2578 RVA: 0x0002F9B6 File Offset: 0x0002DBB6
	private void Start()
	{
		if (Application.isPlaying && CullForSwitchController.IsSwitchEnvironment)
		{
			this.regularShadowDistance = QualitySettings.shadowDistance;
			QualitySettings.shadowDistance = this.shadowDistanceForSwitch;
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002F9DC File Offset: 0x0002DBDC
	private void OnDestroy()
	{
		if (Application.isPlaying && CullForSwitchController.IsSwitchEnvironment)
		{
			QualitySettings.shadowDistance = this.regularShadowDistance;
		}
	}

	// Token: 0x04000A09 RID: 2569
	public float shadowDistanceForSwitch = 500f;

	// Token: 0x04000A0A RID: 2570
	private float regularShadowDistance = 150f;
}
