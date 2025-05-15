using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class UnluckDistanceDisabler : MonoBehaviour
{
	// Token: 0x0600007F RID: 127 RVA: 0x000055FC File Offset: 0x000037FC
	public void Start()
	{
		if (this._distanceFromMainCam)
		{
			this._distanceFrom = Camera.main.transform;
		}
		base.InvokeRepeating("CheckDisable", this._disableCheckInterval + Random.value * this._disableCheckInterval, this._disableCheckInterval);
		base.InvokeRepeating("CheckEnable", this._enableCheckInterval + Random.value * this._enableCheckInterval, this._enableCheckInterval);
		base.Invoke("DisableOnStart", 0.01f);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005679 File Offset: 0x00003879
	public void DisableOnStart()
	{
		if (this._disableOnStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00005690 File Offset: 0x00003890
	public void CheckDisable()
	{
		if (base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude > (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000056EC File Offset: 0x000038EC
	public void CheckEnable()
	{
		if (!base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude < (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x040000A1 RID: 161
	public int _distanceDisable = 1000;

	// Token: 0x040000A2 RID: 162
	public Transform _distanceFrom;

	// Token: 0x040000A3 RID: 163
	public bool _distanceFromMainCam;

	// Token: 0x040000A4 RID: 164
	public float _disableCheckInterval = 10f;

	// Token: 0x040000A5 RID: 165
	public float _enableCheckInterval = 1f;

	// Token: 0x040000A6 RID: 166
	public bool _disableOnStart;
}
