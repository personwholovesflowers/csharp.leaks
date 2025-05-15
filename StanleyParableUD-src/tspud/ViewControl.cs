using System;
using UnityEngine;

// Token: 0x020001CC RID: 460
public class ViewControl : HammerEntity
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x00031474 File Offset: 0x0002F674
	private void Awake()
	{
		this.cam.nearClipPlane = 0.05f;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00031488 File Offset: 0x0002F688
	public override void Input_Enable()
	{
		base.Input_Enable();
		if (this.setFOV)
		{
			this.cam.fieldOfView = StanleyController.Instance.cam.fieldOfView;
		}
		StanleyController.Instance.DisableCamera(this.cam);
		if (this.freezePlayer)
		{
			StanleyController.Instance.FreezeMotion(false);
			StanleyController.Instance.FreezeView(false);
		}
		this.cam.enabled = true;
		if (this.trackTrain)
		{
			this.trackTrain.Input_StartForward();
			if (this.transformFollower)
			{
				this.transformFollower.enabled = true;
			}
		}
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00031528 File Offset: 0x0002F728
	public override void Input_Disable()
	{
		base.Input_Disable();
		this.cam.enabled = false;
		StanleyController.Instance.EnableCamera();
		StanleyController.Instance.UnfreezeMotion(false);
		StanleyController.Instance.UnfreezeView(false);
		if (this.trackTrain)
		{
			this.trackTrain.Input_Stop();
			if (this.transformFollower)
			{
				this.transformFollower.enabled = false;
			}
		}
	}

	// Token: 0x04000A70 RID: 2672
	public float FOV = 90f;

	// Token: 0x04000A71 RID: 2673
	public bool setFOV;

	// Token: 0x04000A72 RID: 2674
	public float holdTime = 10f;

	// Token: 0x04000A73 RID: 2675
	public bool freezePlayer = true;

	// Token: 0x04000A74 RID: 2676
	public Camera cam;

	// Token: 0x04000A75 RID: 2677
	public Tracktrain trackTrain;

	// Token: 0x04000A76 RID: 2678
	public TransformFollow transformFollower;
}
