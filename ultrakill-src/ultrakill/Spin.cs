using System;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class Spin : MonoBehaviour
{
	// Token: 0x0600183D RID: 6205 RVA: 0x000C6044 File Offset: 0x000C4244
	private void Start()
	{
		if (this.eid && this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (this.difficultyVariance)
		{
			if (this.difficulty == 1)
			{
				this.difficultySpeedMultiplier = 0.8f;
			}
			else if (this.difficulty == 0)
			{
				this.difficultySpeedMultiplier = 0.6f;
			}
		}
		if (this.gradual)
		{
			if (this.instantStart && !this.off)
			{
				this.currentSpeed = this.speed * this.difficultySpeedMultiplier * (this.eid ? this.eid.totalSpeedModifier : 1f);
			}
			if (!this.aud)
			{
				this.aud = base.GetComponent<AudioSource>();
				if (this.aud && this.originalPitch == 0f)
				{
					this.originalPitch = this.aud.pitch;
				}
			}
			if (this.aud)
			{
				this.aud.pitch = Mathf.Abs(this.currentSpeed);
			}
		}
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x000C617C File Offset: 0x000C437C
	private void FixedUpdate()
	{
		if (!this.inLateUpdate)
		{
			float num = this.speed * this.difficultySpeedMultiplier;
			if (this.eid)
			{
				num *= this.eid.totalSpeedModifier;
			}
			if (this.gradual)
			{
				if (!this.off && this.currentSpeed != num)
				{
					this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, num, Time.deltaTime * this.gradualSpeed);
				}
				else if (this.off && this.currentSpeed != 0f)
				{
					this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, Time.deltaTime * this.gradualSpeed);
				}
				if (this.currentSpeed != 0f)
				{
					if (!this.notRelative)
					{
						base.transform.Rotate(this.spinDirection, this.currentSpeed * Time.deltaTime, Space.Self);
					}
					else
					{
						base.transform.Rotate(this.spinDirection, this.currentSpeed * Time.deltaTime, Space.World);
					}
				}
				if (this.aud)
				{
					this.aud.pitch = Mathf.Abs(this.currentSpeed / num) * this.originalPitch * this.pitchMultiplier;
					return;
				}
			}
			else
			{
				if (!this.notRelative)
				{
					base.transform.Rotate(this.spinDirection, num * Time.deltaTime, Space.Self);
					return;
				}
				base.transform.Rotate(this.spinDirection, num * Time.deltaTime, Space.World);
			}
		}
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000C62F4 File Offset: 0x000C44F4
	private void LateUpdate()
	{
		if (this.inLateUpdate)
		{
			if (this.totalRotation == Vector3.zero)
			{
				this.totalRotation = base.transform.localRotation.eulerAngles;
			}
			float num = this.speed * this.difficultySpeedMultiplier;
			if (this.eid)
			{
				num *= this.eid.totalSpeedModifier;
			}
			base.transform.localRotation = Quaternion.Euler(this.totalRotation);
			base.transform.Rotate(this.spinDirection, num * Time.deltaTime);
			this.totalRotation = base.transform.localRotation.eulerAngles;
		}
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x000C63A7 File Offset: 0x000C45A7
	public void ChangeState(bool on)
	{
		this.off = !on;
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000C63B3 File Offset: 0x000C45B3
	public void ChangeSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000C63BC File Offset: 0x000C45BC
	public void ChangeGradualSpeed(float newGradualSpeed)
	{
		this.gradualSpeed = newGradualSpeed;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000C63C5 File Offset: 0x000C45C5
	public void ChangePitchMultiplier(float newPitch)
	{
		this.pitchMultiplier = newPitch;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000C63CE File Offset: 0x000C45CE
	public void ChangeSpinDirection(Vector3 newDirection)
	{
		this.spinDirection = newDirection;
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x000C63D7 File Offset: 0x000C45D7
	public void SetReverse(bool reverse)
	{
		if (this.reversed != reverse)
		{
			this.reversed = reverse;
			this.spinDirection *= -1f;
			this.currentSpeed *= -1f;
		}
	}

	// Token: 0x04002203 RID: 8707
	public Vector3 spinDirection;

	// Token: 0x04002204 RID: 8708
	[HideInInspector]
	public bool reversed;

	// Token: 0x04002205 RID: 8709
	public float speed;

	// Token: 0x04002206 RID: 8710
	public bool inLateUpdate;

	// Token: 0x04002207 RID: 8711
	private Vector3 totalRotation;

	// Token: 0x04002208 RID: 8712
	public bool notRelative;

	// Token: 0x04002209 RID: 8713
	public bool gradual;

	// Token: 0x0400220A RID: 8714
	public bool instantStart;

	// Token: 0x0400220B RID: 8715
	public float gradualSpeed = 1f;

	// Token: 0x0400220C RID: 8716
	private float currentSpeed;

	// Token: 0x0400220D RID: 8717
	public bool off;

	// Token: 0x0400220E RID: 8718
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x0400220F RID: 8719
	[HideInInspector]
	public float originalPitch;

	// Token: 0x04002210 RID: 8720
	[HideInInspector]
	public float pitchMultiplier = 1f;

	// Token: 0x04002211 RID: 8721
	[Header("Enemy")]
	public EnemyIdentifier eid;

	// Token: 0x04002212 RID: 8722
	private int difficulty;

	// Token: 0x04002213 RID: 8723
	public bool difficultyVariance;

	// Token: 0x04002214 RID: 8724
	private float difficultySpeedMultiplier = 1f;
}
