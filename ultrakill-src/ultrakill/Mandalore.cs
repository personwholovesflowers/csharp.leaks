using System;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class Mandalore : MonoBehaviour
{
	// Token: 0x0600100A RID: 4106 RVA: 0x0007A065 File Offset: 0x00078265
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.maxHp = base.GetComponent<Drone>().health;
		this.voices = base.GetComponentsInChildren<MandaloreVoice>();
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0007A09C File Offset: 0x0007829C
	private void Start()
	{
		if (this.taunt)
		{
			int num = Random.Range(0, this.voices[0].taunts.Length);
			MandaloreVoice[] array = this.voices;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Taunt(num);
			}
			switch (num)
			{
			case 0:
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>You cannot imagine what you'll face here</color>", null, false);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>I'm gonna shoot em with a gun</color>", 2.5f, base.gameObject);
				return;
			case 1:
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>Why are we in the past</color>", null, false);
				return;
			case 2:
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>I'm going to fucking poison you</color>", null, false);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>What</color>", 2f, base.gameObject);
				return;
			case 3:
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>Hold still</color>", 0.6f, base.gameObject);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0007A184 File Offset: 0x00078384
	private void Update()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (this.eid.dead)
		{
			if (!this.voices[0].dying)
			{
				MandaloreVoice[] array = this.voices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Death();
				}
				base.GetComponent<Rigidbody>().mass = 5f;
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>Oh great, now we lost the fight, fantastic</color>", null, false);
			}
			return;
		}
		if (this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		else if (!this.voices[0].talking && !this.voices[1].talking && this.eid.target != null)
		{
			if (Random.Range(0f, 1f) > this.fullerChance)
			{
				if (this.fullerChance < 0.5f)
				{
					this.fullerChance = 0.5f;
				}
				this.fullerChance += 0.2f;
				if (this.phase == 1)
				{
					this.cooldown = 4f;
				}
				else if (this.phase == 2)
				{
					this.cooldown = 3.25f;
				}
				else
				{
					this.cooldown = 2.5f;
				}
				this.aud.PlayOneShot(this.voiceFull);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Full auto", null, false);
				base.Invoke("FullBurst", 1f);
				this.shotsLeft = 4;
			}
			else
			{
				if (this.fullerChance > 0.5f)
				{
					this.fullerChance = 0.5f;
				}
				this.fullerChance -= 0.2f;
				if (this.eid.health > this.maxHp / 3f * 2f)
				{
					this.cooldown = 4f;
				}
				else if (this.eid.health > this.maxHp / 3f)
				{
					this.cooldown = 3f;
				}
				else
				{
					this.cooldown = 2f;
				}
				this.aud.PlayOneShot(this.voiceFuller);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Fuller auto", null, false);
				base.Invoke("FullerBurst", 1f);
				this.shotsLeft = 40;
			}
		}
		if (!this.aud.isPlaying && !this.voices[0].talking && !this.voices[1].talking)
		{
			if (this.phase < 4 && this.eid.health < this.maxHp / 4f)
			{
				this.phase = 4;
				MandaloreVoice[] array = this.voices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].FinalPhase();
				}
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>Use the salt!</color>", null, false);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>I'm reaching!</color>", 1.5f, base.gameObject);
				base.Invoke("Sandify", 2.5f / this.eid.totalSpeedModifier);
				return;
			}
			if (this.phase < 3 && this.eid.health < this.maxHp / 2f)
			{
				this.phase = 3;
				MandaloreVoice[] array = this.voices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].ThirdPhase();
				}
				Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.enrageEffect, base.transform);
				EnemySimplifier[] componentsInChildren = base.GetComponentsInChildren<EnemySimplifier>();
				if (componentsInChildren.Length != 0)
				{
					EnemySimplifier[] array2 = componentsInChildren;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].enraged = true;
					}
				}
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>Feel my maximum speed!</color>", null, false);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>Slow down</color>", 3.25f, base.gameObject);
				return;
			}
			if (this.phase < 2 && this.eid.health < this.maxHp / 4f * 3f)
			{
				this.phase = 2;
				MandaloreVoice[] array = this.voices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SecondPhase();
				}
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#FFC49E>Through the magic of the Druids, I increase my speed!</color>", null, false);
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("<color=#9EE6FF>Just fucking hit em</color>", 2.5f, base.gameObject);
			}
		}
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0007A5D0 File Offset: 0x000787D0
	public void FullBurst()
	{
		if (this.eid.dead)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.fullAutoProjectile, base.transform.position, base.transform.rotation);
		Projectile componentInChildren = gameObject.GetComponentInChildren<Projectile>();
		componentInChildren.speed = 250f;
		componentInChildren.safeEnemyType = EnemyType.Drone;
		componentInChildren.precheckForCollisions = true;
		componentInChildren.damage *= this.eid.totalDamageModifier;
		gameObject.GetComponent<ProjectileSpread>().projectileAmount = 6;
		this.shotsLeft--;
		if (this.shotsLeft > 0)
		{
			base.Invoke("FullBurst", 0.2f / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0007A680 File Offset: 0x00078880
	public void FullerBurst()
	{
		if (this.eid.dead)
		{
			return;
		}
		Projectile componentInChildren = Object.Instantiate<GameObject>(this.fullerAutoProjectile, base.transform.position, Random.rotation).GetComponentInChildren<Projectile>();
		componentInChildren.speed = 2.5f;
		componentInChildren.turnSpeed = 150f;
		componentInChildren.target = this.eid.target;
		componentInChildren.safeEnemyType = EnemyType.Drone;
		componentInChildren.damage *= this.eid.totalDamageModifier;
		this.shotsLeft--;
		if (this.shotsLeft > 0)
		{
			base.Invoke("FullerBurst", 0.02f / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0007A733 File Offset: 0x00078933
	public void Sandify()
	{
		this.eid.Sandify(false);
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0007A741 File Offset: 0x00078941
	public void DontTaunt()
	{
		this.taunt = false;
	}

	// Token: 0x040015D3 RID: 5587
	private AudioSource aud;

	// Token: 0x040015D4 RID: 5588
	private EnemyIdentifier eid;

	// Token: 0x040015D5 RID: 5589
	public AudioClip voiceFull;

	// Token: 0x040015D6 RID: 5590
	public AudioClip voiceFuller;

	// Token: 0x040015D7 RID: 5591
	private float cooldown = 2f;

	// Token: 0x040015D8 RID: 5592
	private float fullerChance;

	// Token: 0x040015D9 RID: 5593
	private int shotsLeft;

	// Token: 0x040015DA RID: 5594
	private float maxHp;

	// Token: 0x040015DB RID: 5595
	private int phase = 1;

	// Token: 0x040015DC RID: 5596
	public GameObject fullAutoProjectile;

	// Token: 0x040015DD RID: 5597
	public GameObject fullerAutoProjectile;

	// Token: 0x040015DE RID: 5598
	public MandaloreVoice[] voices;

	// Token: 0x040015DF RID: 5599
	private bool taunt = true;
}
