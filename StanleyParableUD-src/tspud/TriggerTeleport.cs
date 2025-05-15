using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class TriggerTeleport : TriggerMultiple
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x000304B8 File Offset: 0x0002E6B8
	private void OnValidate()
	{
		if (this.targetObj != null && this.targetObj.name != this.target)
		{
			this.target = this.targetObj.name;
		}
		if (this.targetObj == null || this.targetObj.name != this.target)
		{
			GameObject gameObject = GameObject.Find(this.target);
			if (gameObject)
			{
				this.targetObj = gameObject;
			}
			else
			{
				this.targetObj = null;
			}
		}
		if (this.landmarkObj != null && this.landmarkObj.name != this.landmark)
		{
			this.landmark = this.landmarkObj.name;
		}
		if (this.landmarkObj == null || this.landmarkObj.name != this.landmark)
		{
			GameObject gameObject2 = GameObject.Find(this.landmark);
			if (gameObject2)
			{
				this.landmarkObj = gameObject2;
			}
			else
			{
				this.landmarkObj = null;
			}
		}
		if (this.orientationObj != null && this.orientationObj.name != this.orientation)
		{
			this.orientation = this.orientationObj.name;
		}
		if (this.orientationObj == null || this.orientationObj.name != this.orientation)
		{
			GameObject gameObject3 = GameObject.Find(this.orientation);
			if (gameObject3)
			{
				this.orientationObj = gameObject3.transform;
				return;
			}
			this.orientationObj = null;
		}
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0003064C File Offset: 0x0002E84C
	public void ForceTouch()
	{
		this.StartTouch();
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00030654 File Offset: 0x0002E854
	public override void Input_Enable()
	{
		base.Input_Enable();
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0003065C File Offset: 0x0002E85C
	protected override void StartTouch()
	{
		if (!this.isEnabled)
		{
			return;
		}
		base.StartTouch();
		if (this.targetObj)
		{
			Vector3 position = this.targetObj.transform.position;
			if (this.landmarkObj != null)
			{
				StanleyController.Instance.Teleport(StanleyController.TeleportType.TriggerTeleport, this.landmarkObj.transform.position, this.targetObj.transform.position, -this.targetObj.transform.up, this.useLandmarkAngles, true, true, null);
			}
			else
			{
				StanleyController.Instance.Teleport(StanleyController.TeleportType.TriggerTeleport, this.targetObj.transform.position, -this.targetObj.transform.up, this.useLandmarkAngles, true, true, null);
			}
			if (this.useOrientationAngles)
			{
				StanleyController.Instance.transform.rotation = this.orientationObj.rotation;
			}
		}
		if (this.onceOnly)
		{
			Object.Destroy(this._body);
			Object.Destroy(this._collider);
		}
	}

	// Token: 0x04000A34 RID: 2612
	public bool useLandmarkAngles;

	// Token: 0x04000A35 RID: 2613
	public bool useOrientationAngles;

	// Token: 0x04000A36 RID: 2614
	public string target = "";

	// Token: 0x04000A37 RID: 2615
	public string landmark = "";

	// Token: 0x04000A38 RID: 2616
	public string orientation = "";

	// Token: 0x04000A39 RID: 2617
	public GameObject targetObj;

	// Token: 0x04000A3A RID: 2618
	public GameObject landmarkObj;

	// Token: 0x04000A3B RID: 2619
	public Transform orientationObj;
}
