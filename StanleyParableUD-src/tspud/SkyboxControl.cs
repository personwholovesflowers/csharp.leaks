using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class SkyboxControl : MonoBehaviour
{
	// Token: 0x0600093E RID: 2366 RVA: 0x0002B7C0 File Offset: 0x000299C0
	public void ToggleSkybox(bool status)
	{
		if (status)
		{
			RenderSettings.skybox = this.skyboxArray[0];
			return;
		}
		RenderSettings.skybox = this.skyboxArray[1];
	}

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private Material[] skyboxArray;
}
