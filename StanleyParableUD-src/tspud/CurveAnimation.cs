using System;
using AmazingAssets.CurvedWorld;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class CurveAnimation : MonoBehaviour
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000404 RID: 1028 RVA: 0x00019056 File Offset: 0x00017256
	public float RealtimeSinceStartup
	{
		get
		{
			return this.startTime + Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00019064 File Offset: 0x00017264
	private void Awake()
	{
		this.controller = base.GetComponent<CurvedWorldController>();
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00019074 File Offset: 0x00017274
	private void Update()
	{
		this.controller.SetBendCurvatureSize(this.GetMappedNoise(this.RealtimeSinceStartup * this.speed + this.curveXOffset, Time.realtimeSinceStartup * this.speed + this.horizontalXOffset) * this.range);
		this.controller.SetBendHorizontalSize(this.GetMappedNoise(this.RealtimeSinceStartup * this.speed + this.horizontalXOffset, Time.realtimeSinceStartup * this.speed + this.horizontalYOffset) * this.range);
		this.controller.SetBendVerticalSize(this.GetMappedNoise(this.RealtimeSinceStartup * this.speed + this.verticalXOffset, Time.realtimeSinceStartup * this.speed + this.verticalYOffset) * this.range);
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001913E File Offset: 0x0001733E
	private float GetMappedNoise(float x, float y)
	{
		return Mathf.Lerp(-1f, 1f, Mathf.PerlinNoise(x, y));
	}

	// Token: 0x040003F9 RID: 1017
	private CurvedWorldController controller;

	// Token: 0x040003FA RID: 1018
	[SerializeField]
	private float speed = 10f;

	// Token: 0x040003FB RID: 1019
	[SerializeField]
	private float range = 10f;

	// Token: 0x040003FC RID: 1020
	[SerializeField]
	private float curveXOffset;

	// Token: 0x040003FD RID: 1021
	[SerializeField]
	private float curveYOffset;

	// Token: 0x040003FE RID: 1022
	[SerializeField]
	private float horizontalXOffset;

	// Token: 0x040003FF RID: 1023
	[SerializeField]
	private float horizontalYOffset;

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private float verticalXOffset;

	// Token: 0x04000401 RID: 1025
	[SerializeField]
	private float verticalYOffset;

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private float startTime = 12f;
}
