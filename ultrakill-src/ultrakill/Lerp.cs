using System;
using UnityEngine;

// Token: 0x020002AB RID: 683
public class Lerp : MonoBehaviour
{
	// Token: 0x06000EEA RID: 3818 RVA: 0x0006EF24 File Offset: 0x0006D124
	private void Start()
	{
		if (this.onEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0006EF24 File Offset: 0x0006D124
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0006EF34 File Offset: 0x0006D134
	private void Update()
	{
		if (this.moving && !this.inFixedUpdate)
		{
			this.Move(Time.deltaTime);
		}
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0006EF51 File Offset: 0x0006D151
	private void FixedUpdate()
	{
		if (this.moving && this.inFixedUpdate)
		{
			this.Move(Time.fixedDeltaTime);
		}
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0006EF70 File Offset: 0x0006D170
	private void Move(float amount)
	{
		if (this.ease)
		{
			this.currentEaseMultiplier = Mathf.MoveTowards(this.currentEaseMultiplier, 1f, amount * this.easeSpeed * Mathf.Max(this.currentEaseMultiplier, 0.01f));
			amount *= this.currentEaseMultiplier;
		}
		if (!this.inLocalSpace)
		{
			Vector3 vector = Vector3.MoveTowards(base.transform.position, this.position, this.moveSpeed * amount);
			Quaternion quaternion = Quaternion.RotateTowards(base.transform.rotation, this.qRot, this.rotateSpeed * amount);
			base.transform.SetPositionAndRotation(vector, quaternion);
			if (base.transform.position == this.position && base.transform.rotation == this.qRot)
			{
				this.moving = false;
				UltrakillEvent ultrakillEvent = this.onComplete;
				if (ultrakillEvent == null)
				{
					return;
				}
				ultrakillEvent.Invoke("");
				return;
			}
		}
		else
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.position, this.moveSpeed * amount);
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, this.qRot, this.rotateSpeed * amount);
			if (base.transform.localPosition == this.position && base.transform.localRotation == this.qRot)
			{
				this.moving = false;
				UltrakillEvent ultrakillEvent2 = this.onComplete;
				if (ultrakillEvent2 == null)
				{
					return;
				}
				ultrakillEvent2.Invoke("");
			}
		}
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0006F105 File Offset: 0x0006D305
	public void Activate()
	{
		if (this.moving)
		{
			return;
		}
		this.qRot = Quaternion.Euler(this.rotation);
		this.moving = true;
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0006F128 File Offset: 0x0006D328
	public void Skip()
	{
		this.Activate();
		this.Move(99999f);
	}

	// Token: 0x040013FE RID: 5118
	[SerializeField]
	private Vector3 position;

	// Token: 0x040013FF RID: 5119
	[SerializeField]
	private Vector3 rotation;

	// Token: 0x04001400 RID: 5120
	[SerializeField]
	private float moveSpeed;

	// Token: 0x04001401 RID: 5121
	[SerializeField]
	private float rotateSpeed;

	// Token: 0x04001402 RID: 5122
	private Quaternion qRot;

	// Token: 0x04001403 RID: 5123
	[SerializeField]
	private bool ease;

	// Token: 0x04001404 RID: 5124
	[SerializeField]
	private float easeSpeed = 1f;

	// Token: 0x04001405 RID: 5125
	private float currentEaseMultiplier;

	// Token: 0x04001406 RID: 5126
	[SerializeField]
	private bool onEnable = true;

	// Token: 0x04001407 RID: 5127
	[SerializeField]
	private bool inFixedUpdate;

	// Token: 0x04001408 RID: 5128
	[SerializeField]
	private bool inLocalSpace;

	// Token: 0x04001409 RID: 5129
	private bool moving;

	// Token: 0x0400140A RID: 5130
	[SerializeField]
	private UltrakillEvent onComplete;
}
