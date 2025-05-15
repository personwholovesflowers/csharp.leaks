using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class TriggerMultiple : HammerEntity
{
	// Token: 0x06000A36 RID: 2614 RVA: 0x00030216 File Offset: 0x0002E416
	private void Awake()
	{
		this._body = base.GetComponent<Rigidbody>();
		this._collider = base.GetComponent<Collider>();
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x00030230 File Offset: 0x0002E430
	private void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.touchingColliders.Contains(col))
		{
			return;
		}
		bool isEnabled = this.isEnabled;
		this.touchingColliders.Add(col);
		this.StartTouch();
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x00030268 File Offset: 0x0002E468
	private void OnTriggerExit(Collider col)
	{
		if (this.touchingColliders.Contains(col))
		{
			this.touchingColliders.Remove(col);
			if (this.isEnabled)
			{
				base.FireOutput(Outputs.OnEndTouch);
			}
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00030294 File Offset: 0x0002E494
	public override void Input_Enable()
	{
		base.Input_Enable();
		for (int i = 0; i < this.touchingColliders.Count; i++)
		{
			if (this.touchingColliders[i].CompareTag("Player"))
			{
				this.StartTouch();
			}
		}
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x000302DB File Offset: 0x0002E4DB
	protected virtual void StartTouch()
	{
		if (!this.isEnabled)
		{
			return;
		}
		base.FireOutput(Outputs.OnStartTouch);
		base.FireOutput(Outputs.OnTrigger);
		if (this.onceOnly)
		{
			Object.Destroy(this._body);
			Object.Destroy(this._collider);
		}
	}

	// Token: 0x04000A2B RID: 2603
	public bool onceOnly;

	// Token: 0x04000A2C RID: 2604
	private List<Collider> touchingColliders = new List<Collider>();

	// Token: 0x04000A2D RID: 2605
	protected Rigidbody _body;

	// Token: 0x04000A2E RID: 2606
	protected Collider _collider;
}
