using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class FuncBrush : HammerEntity
{
	// Token: 0x060004F5 RID: 1269 RVA: 0x0001CB70 File Offset: 0x0001AD70
	private void Awake()
	{
		this._renderer = base.GetComponent<MeshRenderer>();
		this._collider = base.GetComponent<MeshCollider>();
		if (!this.isEnabled)
		{
			this._renderer.enabled = false;
			if (this._collider)
			{
				this._collider.enabled = false;
			}
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0001CBC2 File Offset: 0x0001ADC2
	public override void Input_Enable()
	{
		base.Input_Enable();
		if (this._renderer != null)
		{
			this._renderer.enabled = true;
		}
		if (this._collider != null)
		{
			this._collider.enabled = true;
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0001CBFE File Offset: 0x0001ADFE
	public override void Input_Disable()
	{
		base.Input_Disable();
		if (this._renderer != null)
		{
			this._renderer.enabled = false;
		}
		if (this._collider != null)
		{
			this._collider.enabled = false;
		}
	}

	// Token: 0x040004CB RID: 1227
	private MeshRenderer _renderer;

	// Token: 0x040004CC RID: 1228
	private MeshCollider _collider;
}
