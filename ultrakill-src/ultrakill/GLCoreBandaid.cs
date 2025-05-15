using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020004A9 RID: 1193
public class GLCoreBandaid : MonoBehaviour
{
	// Token: 0x06001B77 RID: 7031 RVA: 0x000E3E84 File Offset: 0x000E2084
	private void OnEnable()
	{
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore)
		{
			this.optionsToHide.SetActive(false);
			this.dialogToShow.SetActive(true);
			return;
		}
		this.optionsToHide.SetActive(true);
		this.dialogToShow.SetActive(false);
	}

	// Token: 0x040026AF RID: 9903
	public GameObject optionsToHide;

	// Token: 0x040026B0 RID: 9904
	public GameObject dialogToShow;
}
