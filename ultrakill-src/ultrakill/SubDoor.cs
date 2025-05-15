using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200045E RID: 1118
public class SubDoor : MonoBehaviour
{
	// Token: 0x06001997 RID: 6551 RVA: 0x000D2828 File Offset: 0x000D0A28
	private void Awake()
	{
		this.SetValues();
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000D2830 File Offset: 0x000D0A30
	private void Update()
	{
		if (this.type == SubDoorType.Animation)
		{
			if (!this.anim)
			{
				return;
			}
			float normalizedTime = this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
			if (normalizedTime > 1f)
			{
				this.anim.Play(0, -1, 1f);
				this.anim.SetFloat("Speed", 0f);
				if (this.aud)
				{
					this.PlayStopSound();
					return;
				}
			}
			else if (normalizedTime < 0f)
			{
				this.anim.Play(0, -1, 0f);
				this.anim.SetFloat("Speed", 0f);
				if (this.aud)
				{
					this.PlayStopSound();
					return;
				}
			}
		}
		else if (base.transform.localPosition != this.targetPos)
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetPos, Time.deltaTime * (this.playerSpeedMultiplier ? Mathf.Max(this.speed, this.speed * (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 15f)) : this.speed));
			if (base.transform.localPosition == this.targetPos)
			{
				if (this.targetPos == this.origPos)
				{
					Door door = this.dr;
					if (door != null)
					{
						door.BigDoorClosed();
					}
				}
				else
				{
					Door door2 = this.dr;
					if (door2 != null)
					{
						UnityEvent onFullyOpened = door2.onFullyOpened;
						if (onFullyOpened != null)
						{
							onFullyOpened.Invoke();
						}
					}
				}
				if (this.aud)
				{
					if (this.stopSound)
					{
						this.aud.clip = this.stopSound;
						this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
						this.aud.Play();
						return;
					}
					this.aud.Stop();
				}
			}
		}
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000D2A3C File Offset: 0x000D0C3C
	public void Open()
	{
		this.SetValues();
		this.isOpen = true;
		if (this.type == SubDoorType.Animation)
		{
			if (this.aud && this.anim.GetFloat("Speed") != this.speed)
			{
				if (this.openSound)
				{
					this.aud.clip = this.openSound;
				}
				this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
				this.aud.Play();
			}
			this.anim.SetFloat("Speed", this.playerSpeedMultiplier ? Mathf.Max(this.speed, this.speed * (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 15f)) : this.speed);
			return;
		}
		this.targetPos = this.origPos + this.openPos;
		if (this.aud && base.transform.localPosition != this.targetPos)
		{
			if (this.openSound)
			{
				this.aud.clip = this.openSound;
			}
			this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
			this.aud.Play();
		}
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000D2BA8 File Offset: 0x000D0DA8
	public void Close()
	{
		this.SetValues();
		this.isOpen = false;
		if (this.type == SubDoorType.Animation)
		{
			if (this.aud && this.anim.GetFloat("Speed") != -this.speed)
			{
				if (this.openSound)
				{
					this.aud.clip = this.openSound;
				}
				this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
				this.aud.Play();
			}
			this.anim.SetFloat("Speed", -(this.playerSpeedMultiplier ? Mathf.Max(this.speed, this.speed * (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 15f)) : this.speed));
			return;
		}
		this.targetPos = this.origPos;
		if (this.aud && base.transform.localPosition != this.targetPos)
		{
			if (this.openSound)
			{
				this.aud.clip = this.openSound;
			}
			this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
			this.aud.Play();
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000D2D0C File Offset: 0x000D0F0C
	public void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.origPos = base.transform.localPosition;
		this.targetPos = this.origPos;
		this.aud = base.GetComponent<AudioSource>();
		if (this.aud)
		{
			this.origPitch = this.aud.pitch;
		}
		if (this.type == SubDoorType.Animation)
		{
			this.anim = base.GetComponent<Animator>();
		}
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000D2D85 File Offset: 0x000D0F85
	public void AnimationEvent(int i)
	{
		this.animationEvents[i].Invoke("");
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000D2D9C File Offset: 0x000D0F9C
	public void PlaySound(int targetSound)
	{
		if (this.aud.clip == this.sounds[targetSound] && this.aud.isPlaying)
		{
			return;
		}
		this.aud.clip = this.sounds[targetSound];
		this.aud.loop = true;
		this.aud.Play();
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x000D2DFC File Offset: 0x000D0FFC
	public void PlayStopSound()
	{
		if (!this.aud)
		{
			return;
		}
		if (this.stopSound)
		{
			this.aud.loop = false;
			this.aud.clip = this.stopSound;
			this.aud.Play();
			return;
		}
		if (this.aud.isPlaying)
		{
			this.aud.Stop();
		}
	}

	// Token: 0x040023DD RID: 9181
	public SubDoorType type;

	// Token: 0x040023DE RID: 9182
	public Vector3 openPos;

	// Token: 0x040023DF RID: 9183
	public Vector3 origPos;

	// Token: 0x040023E0 RID: 9184
	public Vector3 targetPos;

	// Token: 0x040023E1 RID: 9185
	public float speed = 1f;

	// Token: 0x040023E2 RID: 9186
	public bool playerSpeedMultiplier;

	// Token: 0x040023E3 RID: 9187
	[HideInInspector]
	public bool valuesSet;

	// Token: 0x040023E4 RID: 9188
	[HideInInspector]
	public bool isOpen;

	// Token: 0x040023E5 RID: 9189
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x040023E6 RID: 9190
	private float origPitch;

	// Token: 0x040023E7 RID: 9191
	public Door dr;

	// Token: 0x040023E8 RID: 9192
	[HideInInspector]
	public Animator anim;

	// Token: 0x040023E9 RID: 9193
	public AudioClip[] sounds;

	// Token: 0x040023EA RID: 9194
	public AudioClip openSound;

	// Token: 0x040023EB RID: 9195
	public AudioClip stopSound;

	// Token: 0x040023EC RID: 9196
	public UltrakillEvent[] animationEvents;
}
