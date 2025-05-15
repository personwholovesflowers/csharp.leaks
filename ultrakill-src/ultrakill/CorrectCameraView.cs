using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class CorrectCameraView : MonoBehaviour
{
	// Token: 0x06001B6F RID: 7023 RVA: 0x000E3D4A File Offset: 0x000E1F4A
	private void OnEnable()
	{
		this.mainCam = MonoSingleton<CameraController>.Instance.cam;
		this.hudCam = MonoSingleton<PostProcessV2_Handler>.Instance.hudCam;
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void OnDisable()
	{
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x000E3D6C File Offset: 0x000E1F6C
	private void LateUpdate()
	{
		if (!this.canModifyTarget)
		{
			return;
		}
		this.mainCam = MonoSingleton<CameraController>.Instance.cam;
		this.hudCam = MonoSingleton<PostProcessV2_Handler>.Instance.hudCam;
		Vector3 vector = this.hudCam.WorldToScreenPoint(base.transform.position);
		vector = this.mainCam.ScreenToWorldPoint(vector);
		Quaternion quaternion = Quaternion.LookRotation(base.transform.position + base.transform.forward * 4f - vector);
		this.targetObject.SetPositionAndRotation(vector, quaternion);
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000E3E04 File Offset: 0x000E2004
	private void OnPostRenderCallback(Camera cam)
	{
		if (!this.canModifyTarget)
		{
			return;
		}
		if (cam != this.mainCam)
		{
			return;
		}
		this.targetObject.SetPositionAndRotation(this.lastpos, this.lastrot);
	}

	// Token: 0x040026A9 RID: 9897
	private Camera mainCam;

	// Token: 0x040026AA RID: 9898
	private Camera hudCam;

	// Token: 0x040026AB RID: 9899
	private Vector3 lastpos;

	// Token: 0x040026AC RID: 9900
	private Quaternion lastrot;

	// Token: 0x040026AD RID: 9901
	public Transform targetObject;

	// Token: 0x040026AE RID: 9902
	public bool canModifyTarget = true;
}
