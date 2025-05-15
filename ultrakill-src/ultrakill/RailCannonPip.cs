using System;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class RailCannonPip : MonoBehaviour
{
	// Token: 0x06001487 RID: 5255 RVA: 0x000A6704 File Offset: 0x000A4904
	private void Start()
	{
		this.rc = base.GetComponentInParent<Railcannon>();
		this.auds = base.GetComponents<AudioSource>();
		this.origPos = base.transform.localPosition;
		this.tempPos = this.origPos;
		this.targetPos = this.origPos + this.pushAmount * 0.01f;
		this.origRot = base.transform.localRotation;
		this.tempRot = this.origRot;
		if (this.auds != null && this.rc.wid && this.rc.wid.delay != 0f)
		{
			foreach (AudioSource audioSource in this.auds)
			{
				audioSource.volume -= this.rc.wid.delay * 2f;
				if (audioSource.volume < 0f)
				{
					audioSource.volume = 0f;
				}
			}
		}
		this.CheckSounds();
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x000A680F File Offset: 0x000A4A0F
	private void OnEnable()
	{
		this.CheckSounds();
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x000A6818 File Offset: 0x000A4A18
	private void LateUpdate()
	{
		if (MonoSingleton<WeaponCharges>.Instance.raicharge >= this.chargeLevel)
		{
			base.transform.localPosition = this.tempPos;
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.origPos, Vector3.Distance(base.transform.localPosition, this.origPos) * 50f * Time.deltaTime);
			this.tempPos = base.transform.localPosition;
			base.transform.localRotation = this.tempRot;
			base.transform.Rotate(Vector3.up, Time.deltaTime * -2400f, Space.Self);
			this.tempRot = base.transform.localRotation;
			if (this.playClick || this.playIdle)
			{
				if (this.auds != null)
				{
					foreach (AudioSource audioSource in this.auds)
					{
						if ((audioSource.loop && this.playIdle) || (!audioSource.loop && this.playClick))
						{
							audioSource.Play();
						}
					}
				}
				this.playClick = false;
				this.playIdle = false;
				return;
			}
		}
		else
		{
			base.transform.localPosition = this.tempPos;
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetPos, Vector3.Distance(base.transform.localPosition, this.targetPos) * 50f * Time.deltaTime);
			this.tempPos = base.transform.localPosition;
			base.transform.localRotation = this.origRot;
			this.tempRot = this.origRot;
			if (!this.playClick || !this.playIdle)
			{
				this.playClick = true;
				this.playIdle = true;
				if (this.auds != null)
				{
					AudioSource[] array = this.auds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Stop();
					}
				}
			}
		}
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x000A6A0C File Offset: 0x000A4C0C
	private void CheckSounds()
	{
		if (this.rc == null)
		{
			this.rc = base.GetComponentInParent<Railcannon>();
		}
		if (this.auds == null)
		{
			this.auds = base.GetComponents<AudioSource>();
		}
		if (MonoSingleton<WeaponCharges>.Instance.raicharge > this.chargeLevel)
		{
			this.playClick = false;
		}
		else
		{
			this.playClick = true;
		}
		this.playIdle = true;
	}

	// Token: 0x04001C43 RID: 7235
	private Vector3 origPos;

	// Token: 0x04001C44 RID: 7236
	private Vector3 targetPos;

	// Token: 0x04001C45 RID: 7237
	private Vector3 tempPos;

	// Token: 0x04001C46 RID: 7238
	public Vector3 pushAmount;

	// Token: 0x04001C47 RID: 7239
	public float chargeLevel;

	// Token: 0x04001C48 RID: 7240
	private Railcannon rc;

	// Token: 0x04001C49 RID: 7241
	private Quaternion origRot;

	// Token: 0x04001C4A RID: 7242
	private Quaternion tempRot;

	// Token: 0x04001C4B RID: 7243
	private AudioSource[] auds;

	// Token: 0x04001C4C RID: 7244
	private bool playIdle;

	// Token: 0x04001C4D RID: 7245
	private bool playClick;
}
