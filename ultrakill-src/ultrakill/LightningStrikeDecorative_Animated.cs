using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class LightningStrikeDecorative_Animated : MonoBehaviour
{
	// Token: 0x06000FAE RID: 4014 RVA: 0x000749AC File Offset: 0x00072BAC
	private void Start()
	{
		this.originalFlashIntensity = this.flash.intensity;
		this.flash.intensity = 0f;
		this.time = 0f;
		this.lightningAnim.AddTime(this.time);
		this.cooldown = Random.Range(this.minInitialCooldown, this.maxInitialCooldown);
		this.originalScale = this.lightningAnim.transform.localScale.x;
		if (this.origPitch == 0f)
		{
			this.origPitch = this.thunder.pitch;
		}
		if (this.flashOnStart)
		{
			this.FlashStart();
		}
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x00074A54 File Offset: 0x00072C54
	private void Update()
	{
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		if (this.cooldown == 0f && !this.flashing)
		{
			this.FlashStart();
		}
		if (this.flashing)
		{
			if (this.inBuildup)
			{
				this.time = Mathf.MoveTowards(this.time, 1f, Time.deltaTime / this.buildupTime);
				this.lightningAnim.AddTime(this.time);
				this.flash.intensity = 0f;
				if (this.time >= 1f)
				{
					this.inBuildup = false;
					this.inStrike = true;
					this.time = 0f;
					this.lightningAnim.arrayTex = this.strikeAnim[this.atlasIndex];
					this.thunder.pitch = Random.Range(this.origPitch - 0.2f, this.origPitch + 0.2f);
					this.thunder.Play();
					this.flash.intensity = this.originalFlashIntensity;
					return;
				}
			}
			else if (this.inStrike)
			{
				this.time = Mathf.MoveTowards(this.time, 1f, Time.deltaTime / this.strikeTime);
				this.lightningAnim.AddTime(this.time);
				float num = Mathf.InverseLerp(0f, 1f, this.time);
				this.flash.intensity = Mathf.Lerp(this.originalFlashIntensity, 0f, num);
				if (this.time >= 1f && this.flash.intensity == 0f)
				{
					this.flashing = false;
					this.inStrike = false;
					this.time = 1f;
					this.lightningAnim.AddTime(this.time);
					this.cooldown = Random.Range(this.minInitialCooldown, this.maxInitialCooldown);
				}
			}
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x00074C4C File Offset: 0x00072E4C
	private void FlashStart()
	{
		this.flashing = true;
		this.inBuildup = true;
		this.inStrike = false;
		this.time = 0f;
		bool flag = Random.value > 0.5f;
		Vector3 localScale = this.lightningAnim.transform.localScale;
		localScale.x = (flag ? (-this.originalScale) : this.originalScale);
		this.lightningAnim.transform.localScale = localScale;
		this.atlasIndex = Random.Range(0, this.buildupAnim.Count);
		this.lightningAnim.arrayTex = this.buildupAnim[this.atlasIndex];
	}

	// Token: 0x04001516 RID: 5398
	[Header("Animations")]
	[SerializeField]
	private List<Texture2DArray> buildupAnim;

	// Token: 0x04001517 RID: 5399
	[SerializeField]
	private float buildupTime = 0.75f;

	// Token: 0x04001518 RID: 5400
	[SerializeField]
	private List<Texture2DArray> strikeAnim;

	// Token: 0x04001519 RID: 5401
	[SerializeField]
	private float strikeTime = 1f;

	// Token: 0x0400151A RID: 5402
	[SerializeField]
	private AnimatedTexture lightningAnim;

	// Token: 0x0400151B RID: 5403
	[Header("Lightning Effects")]
	[SerializeField]
	private Light flash;

	// Token: 0x0400151C RID: 5404
	[SerializeField]
	private AudioSource thunder;

	// Token: 0x0400151D RID: 5405
	[HideInInspector]
	public float origPitch;

	// Token: 0x0400151E RID: 5406
	[Header("Cooldown Settings")]
	[SerializeField]
	private float minInitialCooldown = 5f;

	// Token: 0x0400151F RID: 5407
	[SerializeField]
	private float maxInitialCooldown = 60f;

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private float minAfterCooldown = 25f;

	// Token: 0x04001521 RID: 5409
	[SerializeField]
	private float maxAfterCooldown = 60f;

	// Token: 0x04001522 RID: 5410
	[Header("Start Settings")]
	public bool flashOnStart;

	// Token: 0x04001523 RID: 5411
	private float originalFlashIntensity;

	// Token: 0x04001524 RID: 5412
	private float time;

	// Token: 0x04001525 RID: 5413
	private float cooldown;

	// Token: 0x04001526 RID: 5414
	private bool flashing;

	// Token: 0x04001527 RID: 5415
	private bool inBuildup;

	// Token: 0x04001528 RID: 5416
	private bool inStrike;

	// Token: 0x04001529 RID: 5417
	private int atlasIndex;

	// Token: 0x0400152A RID: 5418
	private float originalScale;
}
