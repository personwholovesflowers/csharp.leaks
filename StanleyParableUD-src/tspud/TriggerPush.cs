using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class TriggerPush : HammerEntity
{
	// Token: 0x06000A3C RID: 2620 RVA: 0x00030325 File Offset: 0x0002E525
	private void Awake()
	{
		this._body = base.GetComponent<Rigidbody>();
		this._collider = base.GetComponent<Collider>();
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00030340 File Offset: 0x0002E540
	private void FixedUpdate()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (this.touchingBodies.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<Collider, Rigidbody> keyValuePair in this.touchingBodies)
		{
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.AddForce(this.pushDirection * this.pushAmount * Time.fixedDeltaTime * 2000f, ForceMode.Force);
			}
		}
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x000303E4 File Offset: 0x0002E5E4
	private void OnTriggerEnter(Collider col)
	{
		if (this.touchingBodies.ContainsKey(col))
		{
			return;
		}
		Rigidbody component = col.GetComponent<Rigidbody>();
		if (component)
		{
			this.touchingBodies.Add(col, component);
			return;
		}
		this.touchingBodies.Add(col, null);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0003042C File Offset: 0x0002E62C
	private void OnTriggerStay(Collider col)
	{
		if (this.touchingBodies.ContainsKey(col))
		{
			return;
		}
		Rigidbody component = col.GetComponent<Rigidbody>();
		if (component)
		{
			this.touchingBodies.Add(col, component);
			return;
		}
		this.touchingBodies.Add(col, null);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00030472 File Offset: 0x0002E672
	private void OnTriggerExit(Collider col)
	{
		if (this.touchingBodies.ContainsKey(col))
		{
			this.touchingBodies.Remove(col);
		}
	}

	// Token: 0x04000A2F RID: 2607
	private Dictionary<Collider, Rigidbody> touchingBodies = new Dictionary<Collider, Rigidbody>();

	// Token: 0x04000A30 RID: 2608
	private Rigidbody _body;

	// Token: 0x04000A31 RID: 2609
	private Collider _collider;

	// Token: 0x04000A32 RID: 2610
	public Vector3 pushDirection = Vector3.forward;

	// Token: 0x04000A33 RID: 2611
	public float pushAmount = 1f;
}
