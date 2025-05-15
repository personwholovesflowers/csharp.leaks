using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class ProjectedTexLight : HammerEntity
{
	// Token: 0x06000882 RID: 2178 RVA: 0x000284AC File Offset: 0x000266AC
	private void Awake()
	{
		this._light = base.GetComponent<Light>();
		if (!this.startActive)
		{
			if (this._light != null)
			{
				this._light.enabled = false;
			}
			if (this.connected)
			{
				this.connected.SetActive(false);
			}
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00028500 File Offset: 0x00026700
	public void Input_TurnOn()
	{
		if (this._light != null)
		{
			this._light.enabled = true;
		}
		if (this.connected)
		{
			this.connected.SetActive(true);
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00028535 File Offset: 0x00026735
	public void Input_TurnOff()
	{
		if (this._light != null)
		{
			this._light.enabled = false;
		}
		if (this.connected)
		{
			this.connected.SetActive(false);
		}
	}

	// Token: 0x04000851 RID: 2129
	private Light _light;

	// Token: 0x04000852 RID: 2130
	public bool startActive = true;

	// Token: 0x04000853 RID: 2131
	public GameObject connected;
}
