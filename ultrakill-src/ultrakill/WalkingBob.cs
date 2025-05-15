using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class WalkingBob : MonoBehaviour
{
	// Token: 0x06001BB4 RID: 7092 RVA: 0x000E5D1C File Offset: 0x000E3F1C
	private void Awake()
	{
		this.nmov = base.GetComponentInParent<NewMovement>();
		this.originalPos = base.transform.localPosition;
		this.rightPos = new Vector3(this.originalPos.x + 0.08f, this.originalPos.y - 0.025f, this.originalPos.z);
		this.leftPos = new Vector3(this.originalPos.x - 0.08f, this.originalPos.y - 0.025f, this.originalPos.z);
		this.target = this.rightPos;
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x000E5DC4 File Offset: 0x000E3FC4
	private void Update()
	{
		if (this.nmov.walking)
		{
			this.speed = Time.deltaTime * (2f - Vector3.Distance(base.transform.localPosition, this.originalPos) * 3f) * (Mathf.Min(this.nmov.rb.velocity.magnitude, 15f) / 15f);
			if (this.backToStart)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.originalPos, this.speed * 0.25f);
			}
			else
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.target, this.speed * 0.25f);
			}
			if (base.transform.localPosition == this.originalPos)
			{
				this.backToStart = false;
				return;
			}
			if (base.transform.localPosition == this.rightPos)
			{
				this.backToStart = true;
				this.target = this.leftPos;
				return;
			}
			if (base.transform.localPosition == this.leftPos)
			{
				this.backToStart = true;
				this.target = this.rightPos;
				return;
			}
		}
		else if (base.transform.localPosition != this.originalPos)
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.originalPos, Time.deltaTime);
		}
	}

	// Token: 0x040026FB RID: 9979
	private NewMovement nmov;

	// Token: 0x040026FC RID: 9980
	private Vector3 originalPos;

	// Token: 0x040026FD RID: 9981
	private Vector3 rightPos;

	// Token: 0x040026FE RID: 9982
	private Vector3 leftPos;

	// Token: 0x040026FF RID: 9983
	private Vector3 target;

	// Token: 0x04002700 RID: 9984
	private bool backToStart;

	// Token: 0x04002701 RID: 9985
	private float speed;
}
