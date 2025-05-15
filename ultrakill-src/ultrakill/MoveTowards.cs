using System;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class MoveTowards : MonoBehaviour
{
	// Token: 0x060011A1 RID: 4513 RVA: 0x000896A2 File Offset: 0x000878A2
	private void Start()
	{
		if (this.useRigidBody)
		{
			this.UseRigidbody(true);
		}
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x000896B4 File Offset: 0x000878B4
	private void FixedUpdate()
	{
		if (base.transform.position != this.targetPosition)
		{
			float num = this.speed;
			if (this.easeAtEnd != 0f)
			{
				num = Mathf.Min(num, Vector3.Distance(base.transform.position, this.targetPosition) * 2f / this.easeAtEnd);
			}
			Vector3 vector = Vector3.MoveTowards(base.transform.position, this.targetPosition, num * Time.fixedDeltaTime);
			if (Vector3.Distance(vector, this.targetPosition) < 0.1f)
			{
				if (this.useRigidBody)
				{
					this.rb.MovePosition(this.targetPosition);
				}
				else
				{
					base.transform.position = this.targetPosition;
				}
				UltrakillEvent ultrakillEvent = this.onReachTarget;
				if (ultrakillEvent != null)
				{
					ultrakillEvent.Invoke("");
				}
			}
			else if (this.useRigidBody)
			{
				this.rb.MovePosition(vector);
			}
			else
			{
				base.transform.position = vector;
			}
			if (this.pitchAudioWithSpeed && this.aud)
			{
				this.aud.pitch = num / this.speed * this.originalPitch;
				return;
			}
		}
		else if (this.pitchAudioWithSpeed && this.aud)
		{
			this.aud.pitch = 0f;
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00089806 File Offset: 0x00087A06
	public void SnapToTarget()
	{
		base.transform.position = this.targetPosition;
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x00089819 File Offset: 0x00087A19
	public void ChangeTarget(Vector3 target)
	{
		this.targetPosition = target;
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x00089822 File Offset: 0x00087A22
	public void ChangeX(float number)
	{
		this.ChangeTarget(new Vector3(number, this.targetPosition.y, this.targetPosition.z));
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00089846 File Offset: 0x00087A46
	public void ChangeY(float number)
	{
		this.ChangeTarget(new Vector3(this.targetPosition.x, number, this.targetPosition.z));
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x0008986A File Offset: 0x00087A6A
	public void ChangeZ(float number)
	{
		this.ChangeTarget(new Vector3(this.targetPosition.x, this.targetPosition.y, number));
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x0008988E File Offset: 0x00087A8E
	public void UseRigidbody(bool use)
	{
		if (!this.rb && use)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		this.useRigidBody = use;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x000898B5 File Offset: 0x00087AB5
	public void PitchAudioWithSpeed(bool use)
	{
		if (!this.aud && use)
		{
			this.aud = base.GetComponent<AudioSource>();
			this.originalPitch = this.aud.pitch;
		}
		this.pitchAudioWithSpeed = use;
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x000898ED File Offset: 0x00087AED
	public void UpdateAudioOriginalPitch()
	{
		if (this.aud)
		{
			this.originalPitch = this.aud.pitch;
		}
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x0008990D File Offset: 0x00087B0D
	public void EaseAtEnd(float newEase)
	{
		this.easeAtEnd = newEase;
	}

	// Token: 0x040017FE RID: 6142
	public Vector3 targetPosition;

	// Token: 0x040017FF RID: 6143
	public float speed;

	// Token: 0x04001800 RID: 6144
	public float easeAtEnd;

	// Token: 0x04001801 RID: 6145
	public bool useRigidBody;

	// Token: 0x04001802 RID: 6146
	private Rigidbody rb;

	// Token: 0x04001803 RID: 6147
	public bool pitchAudioWithSpeed;

	// Token: 0x04001804 RID: 6148
	private AudioSource aud;

	// Token: 0x04001805 RID: 6149
	private float originalPitch;

	// Token: 0x04001806 RID: 6150
	public UltrakillEvent onReachTarget;
}
