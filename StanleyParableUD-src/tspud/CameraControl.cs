using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class CameraControl : MonoBehaviour
{
	// Token: 0x060000F0 RID: 240 RVA: 0x0000889D File Offset: 0x00006A9D
	private void Awake()
	{
		this.cachedCamera = base.GetComponent<Camera>();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000088AB File Offset: 0x00006AAB
	public void ToggleSolidColorSkybox(bool status)
	{
		if (status)
		{
			this.cachedCamera.clearFlags = CameraClearFlags.Color;
			return;
		}
		this.cachedCamera.clearFlags = CameraClearFlags.Skybox;
	}

	// Token: 0x04000165 RID: 357
	private Camera cachedCamera;
}
