using System;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class LightningStrikeDecorative : MonoBehaviour
{
	// Token: 0x06000FAA RID: 4010 RVA: 0x00074790 File Offset: 0x00072990
	private void Awake()
	{
		this.originalFlashIntensity = this.flash.intensity;
		this.flash.intensity = 0f;
		this.originalColor = this.lightning.color;
		this.lightning.color = new Color(this.originalColor.r, this.originalColor.g, this.originalColor.b, 0f);
		this.cooldown = Random.Range(5f, 60f);
		if (this.flashOnStart)
		{
			this.FlashStart();
		}
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x00074828 File Offset: 0x00072A28
	private void Update()
	{
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		if (this.cooldown == 0f)
		{
			this.FlashStart();
		}
		if (this.flashing)
		{
			this.flash.intensity = Mathf.MoveTowards(this.flash.intensity, 0f, this.originalFlashIntensity / 1.5f * Time.deltaTime);
			this.lightning.color = new Color(this.originalColor.r, this.originalColor.g, this.originalColor.b, Mathf.MoveTowards(this.lightning.color.a, 0f, Time.deltaTime / 1.5f));
			if (this.flash.intensity == 0f && this.lightning.color.a == 0f)
			{
				this.flashing = false;
			}
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x00074928 File Offset: 0x00072B28
	private void FlashStart()
	{
		this.flashing = true;
		this.cooldown = Random.Range(25f, 60f);
		this.thunder.pitch = Random.Range(0.8f, 1.2f);
		this.thunder.Play();
		this.flash.intensity = this.originalFlashIntensity;
		this.lightning.color = this.originalColor;
	}

	// Token: 0x0400150E RID: 5390
	[SerializeField]
	private SpriteRenderer lightning;

	// Token: 0x0400150F RID: 5391
	[SerializeField]
	private Light flash;

	// Token: 0x04001510 RID: 5392
	[SerializeField]
	private AudioSource thunder;

	// Token: 0x04001511 RID: 5393
	private float originalFlashIntensity;

	// Token: 0x04001512 RID: 5394
	private Color originalColor;

	// Token: 0x04001513 RID: 5395
	private float cooldown = 15f;

	// Token: 0x04001514 RID: 5396
	private bool flashing;

	// Token: 0x04001515 RID: 5397
	public bool flashOnStart;
}
