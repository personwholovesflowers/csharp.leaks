using System;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class HudOpenEffect : MonoBehaviour
{
	// Token: 0x06000D2A RID: 3370 RVA: 0x00064424 File Offset: 0x00062624
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0006442C File Offset: 0x0006262C
	private void Initialize()
	{
		if (this.tran == null)
		{
			this.tran = base.GetComponent<RectTransform>();
		}
		if (this.gotValues)
		{
			return;
		}
		this.originalDimensions = (this.dontUseScale ? this.tran.sizeDelta : new Vector2(this.tran.localScale.x, this.tran.localScale.y));
		this.targetDimensions = this.originalDimensions;
		this.originalSpeed = this.speed;
		this.gotValues = true;
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x000644BB File Offset: 0x000626BB
	private void OnEnable()
	{
		this.ResetValues();
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x000644C4 File Offset: 0x000626C4
	private void Update()
	{
		if (this.animating)
		{
			float num = (this.dontUseScale ? (this.tran.sizeDelta.x / this.originalDimensions.x) : this.tran.localScale.x);
			float num2 = (this.dontUseScale ? (this.tran.sizeDelta.y / this.originalDimensions.y) : this.tran.localScale.y);
			if (!this.skip)
			{
				if (this.YFirst && num2 != this.targetDimensions.y)
				{
					num2 = Mathf.MoveTowards(num2, this.targetDimensions.y, Time.unscaledDeltaTime * ((Mathf.Abs(this.targetDimensions.y - num2) + 0.1f) * this.speed));
				}
				else if (num != this.targetDimensions.x)
				{
					num = Mathf.MoveTowards(num, this.targetDimensions.x, Time.unscaledDeltaTime * ((Mathf.Abs(this.targetDimensions.x - num) + 0.1f) * this.speed));
				}
				else if (num2 != this.targetDimensions.y)
				{
					num2 = Mathf.MoveTowards(num2, this.targetDimensions.y, Time.unscaledDeltaTime * ((Mathf.Abs(this.targetDimensions.y - num2) + 0.1f) * this.speed));
				}
			}
			else
			{
				num = this.targetDimensions.x;
				num2 = this.targetDimensions.y;
			}
			if (this.dontUseScale)
			{
				this.tran.sizeDelta = new Vector2(num * this.originalDimensions.x, num2 * this.originalDimensions.y);
			}
			else
			{
				this.tran.localScale = new Vector3(num, num2, this.tran.localScale.z);
			}
			if (num == this.targetDimensions.x && num2 == this.targetDimensions.y)
			{
				this.animating = false;
				if (num == 0f && num2 == 0f)
				{
					base.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x000646E2 File Offset: 0x000628E2
	public Vector2 GetOriginalDimensions()
	{
		this.Initialize();
		return this.originalDimensions;
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x00064424 File Offset: 0x00062624
	public void Force()
	{
		this.Initialize();
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x000646F0 File Offset: 0x000628F0
	public void ResetValues()
	{
		this.ResetValues(null);
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x0006470C File Offset: 0x0006290C
	public void ResetValues(Vector2? inheritedOriginalDimensions)
	{
		if (inheritedOriginalDimensions != null)
		{
			this.originalDimensions = inheritedOriginalDimensions.Value;
			this.gotValues = true;
		}
		this.Initialize();
		this.speed = this.originalSpeed;
		if (this.skip)
		{
			return;
		}
		if (this.reverse)
		{
			this.Reverse(this.speed);
		}
		if (this.dontUseScale)
		{
			this.tran.sizeDelta = this.originalDimensions;
		}
		else
		{
			this.tran.localScale = new Vector3(this.reverse ? 1f : 0.05f, this.reverse ? 1f : 0.05f, this.tran.localScale.z);
		}
		this.animating = true;
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x000647CF File Offset: 0x000629CF
	public void Reverse(float newSpeed = 10f)
	{
		this.targetDimensions = new Vector2(0.025f, 0f);
		this.speed = newSpeed;
		this.animating = true;
	}

	// Token: 0x040011B2 RID: 4530
	private RectTransform tran;

	// Token: 0x040011B3 RID: 4531
	[HideInInspector]
	public Vector2 originalDimensions;

	// Token: 0x040011B4 RID: 4532
	[HideInInspector]
	public Vector2 targetDimensions;

	// Token: 0x040011B5 RID: 4533
	[HideInInspector]
	public bool gotValues;

	// Token: 0x040011B6 RID: 4534
	public bool animating;

	// Token: 0x040011B7 RID: 4535
	public float speed = 30f;

	// Token: 0x040011B8 RID: 4536
	[HideInInspector]
	public float originalSpeed;

	// Token: 0x040011B9 RID: 4537
	public bool skip;

	// Token: 0x040011BA RID: 4538
	public bool reverse;

	// Token: 0x040011BB RID: 4539
	public bool YFirst;

	// Token: 0x040011BC RID: 4540
	public bool dontUseScale;
}
