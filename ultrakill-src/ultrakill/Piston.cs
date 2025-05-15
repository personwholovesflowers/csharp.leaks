using System;
using UnityEngine;

// Token: 0x02000338 RID: 824
public class Piston : MonoBehaviour
{
	// Token: 0x060012F4 RID: 4852 RVA: 0x00096CF8 File Offset: 0x00094EF8
	private void Awake()
	{
		if (this.minPos == Vector3.zero)
		{
			this.maxPos = base.transform.localPosition;
			this.minPos = new Vector3(0f, -7f, 0f);
		}
		base.transform.localPosition = this.minPos;
		this.dzone = base.GetComponentInChildren<DeathZone>().GetComponent<Collider>();
		this.dzone.enabled = false;
		this.basedzone = base.transform.parent.Find("Base").GetComponentInChildren<DeathZone>().GetComponent<Collider>();
		this.basedzone.enabled = false;
		this.targetPos = Vector3.one;
		this.part = base.GetComponentInChildren<ParticleSystem>();
		this.partAud = this.part.GetComponent<AudioSource>();
		this.partAudPitch = this.partAud.pitch;
		this.aud = base.GetComponent<AudioSource>();
		this.audPitch = this.aud.pitch;
		this.steamParts = base.transform.parent.GetChild(0).GetComponentsInChildren<ParticleSystem>();
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00096E14 File Offset: 0x00095014
	private void Update()
	{
		if (!this.off)
		{
			this.timer -= Time.deltaTime * 2f;
			if (this.timer <= 0f)
			{
				if (base.transform.localPosition == this.minPos)
				{
					this.targetPos = this.maxPos;
					this.timer = this.returnTime;
					this.dzone.enabled = true;
					this.basedzone.enabled = false;
				}
				else
				{
					this.targetPos = this.minPos;
					this.timer = this.attackTime;
				}
			}
			if (this.timer <= 1f && !this.aud.isPlaying && base.transform.localPosition == this.minPos)
			{
				this.aud.pitch = this.audPitch + Random.Range(-0.1f, 0.1f);
				this.aud.Play();
				ParticleSystem[] array = this.steamParts;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Play();
				}
			}
			if (base.transform.localPosition != this.targetPos && this.targetPos != Vector3.one)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.targetPos, Time.deltaTime * 75f);
			}
			if (base.transform.localPosition == this.maxPos && this.dzone.enabled)
			{
				this.dzone.enabled = false;
				this.part.Play();
				this.partAud.pitch = this.partAudPitch + Random.Range(-0.25f, 0.25f);
				this.partAud.Play();
				if (this.aud.isPlaying)
				{
					this.aud.Stop();
					ParticleSystem[] array = this.steamParts;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Stop();
					}
				}
			}
			if (base.transform.localPosition == this.minPos)
			{
				this.basedzone.enabled = true;
			}
		}
	}

	// Token: 0x04001A03 RID: 6659
	public bool off;

	// Token: 0x04001A04 RID: 6660
	private RaycastHit rhit;

	// Token: 0x04001A05 RID: 6661
	public Vector3 minPos;

	// Token: 0x04001A06 RID: 6662
	public Vector3 maxPos;

	// Token: 0x04001A07 RID: 6663
	public Vector3 targetPos;

	// Token: 0x04001A08 RID: 6664
	private Collider dzone;

	// Token: 0x04001A09 RID: 6665
	private Collider basedzone;

	// Token: 0x04001A0A RID: 6666
	public float timer;

	// Token: 0x04001A0B RID: 6667
	public float returnTime;

	// Token: 0x04001A0C RID: 6668
	public float attackTime;

	// Token: 0x04001A0D RID: 6669
	private ParticleSystem part;

	// Token: 0x04001A0E RID: 6670
	private ParticleSystem[] steamParts;

	// Token: 0x04001A0F RID: 6671
	private AudioSource partAud;

	// Token: 0x04001A10 RID: 6672
	private float partAudPitch;

	// Token: 0x04001A11 RID: 6673
	private AudioSource aud;

	// Token: 0x04001A12 RID: 6674
	private float audPitch;
}
