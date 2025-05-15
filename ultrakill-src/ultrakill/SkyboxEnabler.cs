using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class SkyboxEnabler : MonoBehaviour
{
	// Token: 0x060017AB RID: 6059 RVA: 0x000C24D5 File Offset: 0x000C06D5
	private void OnEnable()
	{
		if (!this.dontActivateOnEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000C24E8 File Offset: 0x000C06E8
	public void Activate()
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		this.activated = true;
		Camera camera;
		if (MonoSingleton<CameraController>.Instance.TryGetComponent<Camera>(out camera))
		{
			camera.clearFlags = (this.disable ? CameraClearFlags.Color : CameraClearFlags.Skybox);
		}
		if (this.changeSkybox)
		{
			RenderSettings.skybox = new Material(this.changeSkybox);
		}
	}

	// Token: 0x04002102 RID: 8450
	public bool disable;

	// Token: 0x04002103 RID: 8451
	public bool oneTime;

	// Token: 0x04002104 RID: 8452
	public bool dontActivateOnEnable;

	// Token: 0x04002105 RID: 8453
	private bool activated;

	// Token: 0x04002106 RID: 8454
	public Material changeSkybox;
}
