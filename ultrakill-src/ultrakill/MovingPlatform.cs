using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000309 RID: 777
public class MovingPlatform : MonoBehaviour
{
	// Token: 0x060011AD RID: 4525 RVA: 0x00089918 File Offset: 0x00087B18
	private void Start()
	{
		if (this.useRigidbody && !this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		if (!this.rb)
		{
			this.useRigidbody = false;
		}
		if (this.relativePoints.Length >= 1)
		{
			bool flag = false;
			for (int i = 0; i < this.relativePoints.Length; i++)
			{
				if (this.relativePoints[i] == Vector3.zero)
				{
					flag = true;
					break;
				}
			}
			if (!flag && !this.stopAtEnd)
			{
				Vector3[] array = new Vector3[this.relativePoints.Length + 1];
				for (int j = 0; j < this.relativePoints.Length; j++)
				{
					array[j] = this.relativePoints[j];
				}
				array[array.Length - 1] = Vector3.zero;
				this.relativePoints = array;
			}
		}
		if (!this.reverseAtEnd)
		{
			this.forward = true;
		}
		if (this.relativePoints.Length > 1 && !this.infoSet)
		{
			this.infoSet = true;
			this.aud = base.GetComponentInChildren<AudioSource>();
			if (this.aud)
			{
				this.origPitch = this.aud.pitch;
				this.origDoppler = this.aud.dopplerLevel;
				this.aud.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
			}
			this.originalPosition = base.transform.position;
			this.currentPosition = this.originalPosition;
			this.targetPosition = this.originalPosition;
			if (this.randomStartPoint)
			{
				this.startPoint = Random.Range(0, this.relativePoints.Length);
			}
			if (this.startPoint != 0)
			{
				this.currentPoint = this.startPoint;
				base.transform.position = base.transform.position + this.relativePoints[this.currentPoint];
			}
			base.Invoke("NextPoint", this.startOffset);
			return;
		}
		if (this.infoSet && !this.moving)
		{
			base.Invoke("NextPoint", this.startOffset);
		}
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x00089B20 File Offset: 0x00087D20
	private void FixedUpdate()
	{
		if (this.moving)
		{
			float num = this.speed;
			if (this.easeOut && Vector3.Distance(base.transform.position, this.targetPosition) < this.speed / 2f)
			{
				num *= (Vector3.Distance(base.transform.position, this.targetPosition) / (this.speed / 2f) + 0.1f) * this.easeSpeedMultiplier;
			}
			if (this.easeIn && Vector3.Distance(base.transform.position, this.currentPosition) < this.speed / 2f)
			{
				num *= (Vector3.Distance(base.transform.position, this.currentPosition) / (this.speed / 2f) + 0.1f) * this.easeSpeedMultiplier;
			}
			Vector3 vector = Vector3.MoveTowards(base.transform.position, this.targetPosition, Time.deltaTime * num);
			if (this.useRigidbody && this.rb)
			{
				this.rb.MovePosition(vector);
			}
			else
			{
				base.transform.position = vector;
			}
			if (Vector3.Distance(base.transform.position, this.targetPosition) < 0.01f)
			{
				base.transform.position = this.targetPosition;
				this.moving = false;
				if (this.onReachPoint.Length > this.currentPoint)
				{
					this.onReachPoint[this.currentPoint].Invoke("");
				}
				base.Invoke("NextPoint", this.moveDelay);
				if (this.aud && this.stopSound)
				{
					this.aud.clip = this.stopSound;
					this.aud.loop = false;
					this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
					if (this.aud.clip != null)
					{
						this.aud.Play();
						return;
					}
				}
				else if (this.aud && this.moveSound)
				{
					this.aud.Stop();
					return;
				}
			}
			else if (this.aud && !this.aud.isPlaying && this.moveSound)
			{
				this.aud.clip = this.moveSound;
				this.aud.loop = true;
				this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
				if (this.aud.clip != null)
				{
					this.aud.Play();
				}
			}
		}
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x00089DE4 File Offset: 0x00087FE4
	private void NextPoint()
	{
		int num = 1;
		if (!this.forward)
		{
			num = -1;
		}
		if ((this.forward && this.currentPoint < this.relativePoints.Length - 1) || (!this.forward && this.currentPoint > 0))
		{
			this.currentPoint += num;
		}
		else
		{
			if (this.teleportBackToStart)
			{
				this.TeleportToPoint(0);
				return;
			}
			if (this.stopAtEnd)
			{
				return;
			}
			if (!this.reverseAtEnd)
			{
				this.currentPoint = 0;
			}
			else if (this.forward)
			{
				this.forward = false;
			}
			else
			{
				this.forward = true;
			}
		}
		this.currentPosition = this.targetPosition;
		this.targetPosition = this.originalPosition + this.relativePoints[this.currentPoint];
		this.moving = true;
		if (this.aud && this.moveSound)
		{
			this.aud.clip = this.moveSound;
			this.aud.loop = true;
			this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
			if (this.aud.clip != null)
			{
				this.aud.Play();
			}
		}
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x00089F28 File Offset: 0x00088128
	public void TeleportToPoint(int newPoint)
	{
		base.Invoke("ReEnableDoppler", 0.1f);
		if (this.aud)
		{
			this.aud.dopplerLevel = 0f;
		}
		base.transform.position = this.originalPosition + this.relativePoints[newPoint];
		this.currentPoint = newPoint - 1;
		this.NextPoint();
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x00089F93 File Offset: 0x00088193
	private void ReEnableDoppler()
	{
		if (this.aud)
		{
			this.aud.dopplerLevel = this.origDoppler;
		}
	}

	// Token: 0x04001807 RID: 6151
	[HideInInspector]
	public bool infoSet;

	// Token: 0x04001808 RID: 6152
	public Vector3[] relativePoints;

	// Token: 0x04001809 RID: 6153
	[HideInInspector]
	public Vector3 originalPosition;

	// Token: 0x0400180A RID: 6154
	[HideInInspector]
	public Vector3 currentPosition;

	// Token: 0x0400180B RID: 6155
	[HideInInspector]
	public Vector3 targetPosition;

	// Token: 0x0400180C RID: 6156
	[HideInInspector]
	public int currentPoint;

	// Token: 0x0400180D RID: 6157
	public bool useRigidbody;

	// Token: 0x0400180E RID: 6158
	private Rigidbody rb;

	// Token: 0x0400180F RID: 6159
	public float speed;

	// Token: 0x04001810 RID: 6160
	[FormerlySerializedAs("ease")]
	public bool easeIn;

	// Token: 0x04001811 RID: 6161
	[FormerlySerializedAs("ease")]
	public bool easeOut;

	// Token: 0x04001812 RID: 6162
	public float easeSpeedMultiplier = 1f;

	// Token: 0x04001813 RID: 6163
	public bool reverseAtEnd;

	// Token: 0x04001814 RID: 6164
	public bool teleportBackToStart;

	// Token: 0x04001815 RID: 6165
	public bool stopAtEnd;

	// Token: 0x04001816 RID: 6166
	[HideInInspector]
	public bool forward = true;

	// Token: 0x04001817 RID: 6167
	public bool moving;

	// Token: 0x04001818 RID: 6168
	public bool randomStartPoint;

	// Token: 0x04001819 RID: 6169
	public int startPoint;

	// Token: 0x0400181A RID: 6170
	public float startOffset;

	// Token: 0x0400181B RID: 6171
	public float moveDelay;

	// Token: 0x0400181C RID: 6172
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x0400181D RID: 6173
	[HideInInspector]
	public float origPitch;

	// Token: 0x0400181E RID: 6174
	[HideInInspector]
	public float origDoppler;

	// Token: 0x0400181F RID: 6175
	public AudioClip moveSound;

	// Token: 0x04001820 RID: 6176
	public AudioClip stopSound;

	// Token: 0x04001821 RID: 6177
	public UltrakillEvent[] onReachPoint;
}
