using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class SimpleRotate : MonoBehaviour
{
	// Token: 0x06000935 RID: 2357 RVA: 0x0002B648 File Offset: 0x00029848
	private void Start()
	{
		this.startRotation = base.transform.localEulerAngles.y;
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0002B660 File Offset: 0x00029860
	public void ResetToStartRotation()
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.y = this.startRotation;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0002B692 File Offset: 0x00029892
	private void Update()
	{
		base.transform.localEulerAngles += Vector3.up * this.speed * Time.deltaTime;
	}

	// Token: 0x0400090F RID: 2319
	public float speed = 45f;

	// Token: 0x04000910 RID: 2320
	private float startRotation;
}
