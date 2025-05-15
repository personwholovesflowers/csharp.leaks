using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x06000084 RID: 132 RVA: 0x0000576E File Offset: 0x0000396E
	public void Start()
	{
		if (this.lookAtCamera == null)
		{
			this.lookAtCamera = Camera.main;
		}
		if (this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00005797 File Offset: 0x00003997
	public void Update()
	{
		if (!this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000057A7 File Offset: 0x000039A7
	public void LookCam()
	{
		base.transform.LookAt(this.lookAtCamera.transform);
	}

	// Token: 0x040000A7 RID: 167
	public Camera lookAtCamera;

	// Token: 0x040000A8 RID: 168
	public bool lookOnlyOnAwake;
}
