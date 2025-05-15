using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class FarZVolume : MonoBehaviour
{
	// Token: 0x06000956 RID: 2390 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
	private void Awake()
	{
		this.position = base.transform.position;
		this.fullExtents = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
		this.volumeBounds = new Bounds(this.position, this.fullExtents);
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0002C210 File Offset: 0x0002A410
	private void Start()
	{
		this.followTransform = StanleyController.Instance.transform;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0002C224 File Offset: 0x0002A424
	private void FixedUpdate()
	{
		if (this.volumeBounds.Contains(StanleyController.StanleyPosition))
		{
			if (!this.touchingLastFrame)
			{
				StanleyController.Instance.SetFarZ(this.farZ, this.CameraMode);
			}
			this.touchingLastFrame = true;
			return;
		}
		this.touchingLastFrame = false;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0002C270 File Offset: 0x0002A470
	public void ToggleDepthOnlySkybox(bool status)
	{
		if (status)
		{
			this.CameraMode = FarZVolume.CameraModes.DepthOnly;
			return;
		}
		this.CameraMode = FarZVolume.CameraModes.RenderSkybox;
	}

	// Token: 0x04000934 RID: 2356
	public float farZ;

	// Token: 0x04000935 RID: 2357
	public FarZVolume.CameraModes CameraMode;

	// Token: 0x04000936 RID: 2358
	private Vector3 fullExtents;

	// Token: 0x04000937 RID: 2359
	private Vector3 position;

	// Token: 0x04000938 RID: 2360
	private bool touchingLastFrame;

	// Token: 0x04000939 RID: 2361
	private Transform followTransform;

	// Token: 0x0400093A RID: 2362
	private Bounds volumeBounds;

	// Token: 0x020003ED RID: 1005
	public enum CameraModes
	{
		// Token: 0x04001474 RID: 5236
		RenderSkybox,
		// Token: 0x04001475 RID: 5237
		DepthOnly
	}
}
