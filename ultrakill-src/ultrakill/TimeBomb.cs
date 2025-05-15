using System;
using System.Collections;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class TimeBomb : MonoBehaviour
{
	// Token: 0x06001A56 RID: 6742 RVA: 0x000D93B9 File Offset: 0x000D75B9
	private void Start()
	{
		if (!this.dontStartOnAwake)
		{
			this.StartCountdown();
		}
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000D93CC File Offset: 0x000D75CC
	private void OnEnable()
	{
		if (!this.isActive && MonoSingleton<GunControl>.Instance && MonoSingleton<GunControl>.Instance.gameObject.activeInHierarchy && MonoSingleton<GunControl>.Instance.enabled)
		{
			MonoSingleton<GunControl>.Instance.StartCoroutine(this.CheckDisabled());
		}
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000D941B File Offset: 0x000D761B
	private IEnumerator CheckDisabled()
	{
		WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();
		this.isActive = true;
		while (this && base.gameObject.activeInHierarchy)
		{
			yield return waitForEnd;
		}
		this.isActive = false;
		yield break;
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000D942C File Offset: 0x000D762C
	private void OnDestroy()
	{
		if (!this.dontExplode && this.explosion != null && this.isActive)
		{
			Object.Instantiate<GameObject>(this.explosion, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x000D947C File Offset: 0x000D767C
	private void Update()
	{
		if (this.activated)
		{
			if (!PauseTimedBombs.Paused && (!NoWeaponCooldown.NoCooldown || !this.freezeOnNoCooldown))
			{
				this.timer = Mathf.MoveTowards(this.timer, 0f, Time.deltaTime);
				this.beeptimer = Mathf.MoveTowards(this.beeptimer, 0f, Time.deltaTime);
				if (this.beeptimer == 0f)
				{
					this.Beep();
				}
			}
			if (this.beeperSpriteRenderer)
			{
				this.beeperSpriteRenderer.color = this.beeperColor;
			}
			if (this.aud)
			{
				this.aud.pitch = this.beeperPitch;
			}
			if (this.timer != 0f && this.beeper)
			{
				this.beeper.transform.localScale = Vector3.Lerp(this.beeper.transform.localScale, Vector3.zero, Time.deltaTime * 5f);
				return;
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000D958A File Offset: 0x000D778A
	public void StartCountdown()
	{
		if (!this.activated)
		{
			this.activated = true;
		}
		this.Beep();
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000D95A4 File Offset: 0x000D77A4
	private void Beep()
	{
		if (this.beeper == null)
		{
			this.beeper = Object.Instantiate<GameObject>(this.beepLight, base.transform.position, base.transform.rotation);
			this.beeper.transform.SetParent(base.transform, true);
			this.origScale = new Vector3(this.beeperSize, this.beeperSize, 1f);
			this.beeperSpriteRenderer = this.beeper.GetComponent<SpriteRenderer>();
			this.aud = this.beeper.GetComponent<AudioSource>();
			if (this.aud)
			{
				this.aud.pitch = this.beeperPitch;
			}
		}
		if (this.aud)
		{
			this.aud.Play();
		}
		this.beeper.transform.localScale = this.origScale * this.beeperSizeMultiplier;
		this.beeptimer = this.timer / 6f;
	}

	// Token: 0x040024E8 RID: 9448
	public bool dontStartOnAwake;

	// Token: 0x040024E9 RID: 9449
	private bool activated;

	// Token: 0x040024EA RID: 9450
	public float timer;

	// Token: 0x040024EB RID: 9451
	public float beeptimer;

	// Token: 0x040024EC RID: 9452
	public bool freezeOnNoCooldown;

	// Token: 0x040024ED RID: 9453
	private AudioSource aud;

	// Token: 0x040024EE RID: 9454
	public GameObject beepLight;

	// Token: 0x040024EF RID: 9455
	public float beeperSize;

	// Token: 0x040024F0 RID: 9456
	[HideInInspector]
	public float beeperSizeMultiplier = 1f;

	// Token: 0x040024F1 RID: 9457
	private GameObject beeper;

	// Token: 0x040024F2 RID: 9458
	private Vector3 origScale;

	// Token: 0x040024F3 RID: 9459
	public Color beeperColor = Color.white;

	// Token: 0x040024F4 RID: 9460
	private SpriteRenderer beeperSpriteRenderer;

	// Token: 0x040024F5 RID: 9461
	public float beeperPitch = 0.65f;

	// Token: 0x040024F6 RID: 9462
	public GameObject explosion;

	// Token: 0x040024F7 RID: 9463
	public bool dontExplode;

	// Token: 0x040024F8 RID: 9464
	private bool isActive;
}
