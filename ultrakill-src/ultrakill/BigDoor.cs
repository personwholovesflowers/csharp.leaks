using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000078 RID: 120
public class BigDoor : MonoBehaviour
{
	// Token: 0x06000232 RID: 562 RVA: 0x0000B8A0 File Offset: 0x00009AA0
	private void Awake()
	{
		if (!this.gotPos)
		{
			this.targetRotation.eulerAngles = base.transform.localRotation.eulerAngles + this.openRotation;
			this.origRotation = base.transform.localRotation;
			this.gotPos = true;
		}
		this.cc = MonoSingleton<CameraController>.Instance;
		this.aud = base.GetComponent<AudioSource>();
		if (this.aud)
		{
			this.origPitch = this.aud.pitch;
		}
		this.controller = base.GetComponentInParent<Door>();
		this.tempSpeed = this.speed;
		if (this.open)
		{
			base.transform.localRotation = this.targetRotation;
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000B95C File Offset: 0x00009B5C
	private void Update()
	{
		if (this.gradualSpeedMultiplier != 0f)
		{
			if ((this.open && base.transform.localRotation != this.targetRotation) || (!this.open && base.transform.localRotation != this.origRotation))
			{
				this.tempSpeed += Time.deltaTime * this.tempSpeed * this.gradualSpeedMultiplier;
			}
			else
			{
				this.tempSpeed = this.speed;
			}
		}
		if (this.open && base.transform.localRotation != this.targetRotation)
		{
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, this.targetRotation, Time.deltaTime * (this.playerSpeedMultiplier ? Mathf.Max(this.tempSpeed, this.tempSpeed * (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 15f)) : this.tempSpeed));
			if (this.screenShake)
			{
				this.cc.CameraShake(0.05f);
			}
			if (base.transform.localRotation == this.targetRotation)
			{
				if (this.aud)
				{
					this.aud.clip = this.closeSound;
					this.aud.loop = false;
					this.aud.pitch = Random.Range(this.origPitch - 0.1f, this.origPitch + 0.1f);
					this.aud.Play();
				}
				Door door = this.controller;
				if (door == null)
				{
					return;
				}
				UnityEvent onFullyOpened = door.onFullyOpened;
				if (onFullyOpened == null)
				{
					return;
				}
				onFullyOpened.Invoke();
				return;
			}
		}
		else if (!this.open && base.transform.localRotation != this.origRotation)
		{
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, this.origRotation, Time.deltaTime * (this.playerSpeedMultiplier ? Mathf.Max(this.tempSpeed, this.tempSpeed * (MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 15f)) : this.tempSpeed));
			if (this.screenShake)
			{
				this.cc.CameraShake(0.05f);
			}
			if (base.transform.localRotation == this.origRotation)
			{
				if (this.aud)
				{
					this.aud.clip = this.closeSound;
					this.aud.loop = false;
					this.aud.pitch = Random.Range(this.origPitch - 0.1f, this.origPitch + 0.1f);
					this.aud.Play();
				}
				if (this.controller && this.controller.doorType != DoorType.Normal)
				{
					this.controller.BigDoorClosed();
				}
				if (this.openLight != null)
				{
					this.openLight.enabled = false;
				}
			}
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000BC78 File Offset: 0x00009E78
	public void Open()
	{
		if (base.transform.localRotation != this.targetRotation)
		{
			if (!this.aud)
			{
				this.aud = base.GetComponent<AudioSource>();
				if (this.aud)
				{
					this.origPitch = this.aud.pitch;
				}
			}
			this.open = true;
			if (this.aud)
			{
				this.aud.clip = this.openSound;
				this.aud.loop = true;
				this.aud.pitch = Random.Range(this.origPitch - 0.1f, this.origPitch + 0.1f);
				this.aud.Play();
			}
			if (Quaternion.Angle(base.transform.localRotation, this.origRotation) < 20f)
			{
				if (this.reverseDirection)
				{
					this.targetRotation.eulerAngles = this.origRotation.eulerAngles - this.openRotation;
					return;
				}
				this.targetRotation.eulerAngles = this.origRotation.eulerAngles + this.openRotation;
			}
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000BDA4 File Offset: 0x00009FA4
	public void Close()
	{
		if (base.transform.localRotation != this.origRotation)
		{
			this.open = false;
			if (this.aud)
			{
				this.aud.clip = this.openSound;
				this.aud.loop = true;
				this.aud.pitch = Random.Range(this.origPitch / 2f - 0.1f, this.origPitch / 2f + 0.1f);
				this.aud.Play();
			}
		}
	}

	// Token: 0x04000279 RID: 633
	public bool open;

	// Token: 0x0400027A RID: 634
	[HideInInspector]
	public bool gotPos;

	// Token: 0x0400027B RID: 635
	public Vector3 openRotation;

	// Token: 0x0400027C RID: 636
	[HideInInspector]
	public Quaternion targetRotation;

	// Token: 0x0400027D RID: 637
	[HideInInspector]
	public Quaternion origRotation;

	// Token: 0x0400027E RID: 638
	public float speed;

	// Token: 0x0400027F RID: 639
	private float tempSpeed;

	// Token: 0x04000280 RID: 640
	public float gradualSpeedMultiplier;

	// Token: 0x04000281 RID: 641
	private CameraController cc;

	// Token: 0x04000282 RID: 642
	public bool screenShake;

	// Token: 0x04000283 RID: 643
	private AudioSource aud;

	// Token: 0x04000284 RID: 644
	public AudioClip openSound;

	// Token: 0x04000285 RID: 645
	public AudioClip closeSound;

	// Token: 0x04000286 RID: 646
	private float origPitch;

	// Token: 0x04000287 RID: 647
	public Light openLight;

	// Token: 0x04000288 RID: 648
	public bool reverseDirection;

	// Token: 0x04000289 RID: 649
	private Door controller;

	// Token: 0x0400028A RID: 650
	public bool playerSpeedMultiplier;
}
