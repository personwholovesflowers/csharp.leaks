using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000066 RID: 102
public class AspectRatioChanger : MonoBehaviour
{
	// Token: 0x060001EC RID: 492 RVA: 0x0000A212 File Offset: 0x00008412
	private void Start()
	{
		this.arf = base.GetComponent<AspectRatioFitter>();
		if (this.targetRatio == 0f)
		{
			this.targetRatio = this.arf.aspectRatio;
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000A240 File Offset: 0x00008440
	private void Update()
	{
		if (this.arf && this.arf.aspectRatio != this.targetRatio)
		{
			this.arf.aspectRatio = Mathf.MoveTowards(this.arf.aspectRatio, this.targetRatio, Time.deltaTime * this.speed);
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000A29A File Offset: 0x0000849A
	public void ChangeRatio(float ratio)
	{
		this.targetRatio = ratio;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000A2A3 File Offset: 0x000084A3
	public void ChangeSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}

	// Token: 0x0400020A RID: 522
	private AspectRatioFitter arf;

	// Token: 0x0400020B RID: 523
	public float targetRatio = 1.7778f;

	// Token: 0x0400020C RID: 524
	public float speed = 1f;
}
