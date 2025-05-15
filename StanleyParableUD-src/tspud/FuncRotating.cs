using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class FuncRotating : HammerEntity
{
	// Token: 0x060004FF RID: 1279 RVA: 0x0001CCD8 File Offset: 0x0001AED8
	private void Awake()
	{
		if (this.XAxis)
		{
			this.axis = -Vector3.right;
		}
		else if (this.YAxis)
		{
			this.axis = -Vector3.forward;
		}
		if (this.reverseDir)
		{
			this.axis = -this.axis;
		}
		this.currentlyOn = this.startOn;
		this.startRotation = base.transform.rotation;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0001CD50 File Offset: 0x0001AF50
	private void Update()
	{
		if (!this.currentlyOn)
		{
			return;
		}
		this.currentAngle += this.maxSpeed * Singleton<GameMaster>.Instance.GameDeltaTime;
		base.transform.rotation = Quaternion.AngleAxis(this.currentAngle, this.axis) * this.startRotation;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0001CDAB File Offset: 0x0001AFAB
	public void Input_StartForward()
	{
		this.currentlyOn = true;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001CDAB File Offset: 0x0001AFAB
	public void Input_Start()
	{
		this.currentlyOn = true;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
	public void Input_Stop()
	{
		this.currentlyOn = false;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001CDBD File Offset: 0x0001AFBD
	public void Input_SetSpeed(float newSpeed)
	{
		this.maxSpeed = newSpeed;
	}

	// Token: 0x040004D1 RID: 1233
	public float maxSpeed;

	// Token: 0x040004D2 RID: 1234
	public bool startOn = true;

	// Token: 0x040004D3 RID: 1235
	public bool reverseDir;

	// Token: 0x040004D4 RID: 1236
	public bool XAxis;

	// Token: 0x040004D5 RID: 1237
	public bool YAxis;

	// Token: 0x040004D6 RID: 1238
	private float currentAngle;

	// Token: 0x040004D7 RID: 1239
	private bool currentlyOn = true;

	// Token: 0x040004D8 RID: 1240
	private Vector3 axis = -Vector3.up;

	// Token: 0x040004D9 RID: 1241
	private Quaternion startRotation;
}
