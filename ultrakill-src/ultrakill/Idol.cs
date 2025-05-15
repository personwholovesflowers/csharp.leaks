using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class Idol : MonoBehaviour
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x00065E3C File Offset: 0x0006403C
	private void Awake()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (this.overrideTarget && this.overrideTarget.gameObject.activeInHierarchy)
		{
			this.ChangeTarget(this.overrideTarget);
		}
		this.eid = base.GetComponent<EnemyIdentifier>();
		if (this.unradiantBeam == null)
		{
			this.unradiantBeam = this.beam.material;
		}
		if (this.unradiantHalo == null)
		{
			this.unradiantHalo = this.halo.sprite;
			this.haloColor = this.halo.color;
		}
		this.SlowUpdate();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00065EEB File Offset: 0x000640EB
	private void OnDisable()
	{
		base.CancelInvoke("SlowUpdate");
		if (this.target)
		{
			this.ChangeTarget(null);
		}
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x00065F0C File Offset: 0x0006410C
	private void OnEnable()
	{
		base.CancelInvoke("SlowUpdate");
		this.SlowUpdate();
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00065F20 File Offset: 0x00064120
	private void UpdateBuff()
	{
		this.beam.material = ((this.eid.damageBuff || this.eid.healthBuff || this.eid.speedBuff) ? this.radiantBeam : this.unradiantBeam);
		this.halo.sprite = ((this.eid.damageBuff || this.eid.healthBuff || this.eid.speedBuff) ? this.radiantHalo : this.unradiantHalo);
		this.halo.color = ((this.eid.damageBuff || this.eid.healthBuff || this.eid.speedBuff) ? Color.white : this.haloColor);
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00065FEC File Offset: 0x000641EC
	private void Update()
	{
		if (this.overrideTarget && this.target != this.overrideTarget && !this.overrideTarget.dead && this.overrideTarget.gameObject.activeInHierarchy)
		{
			this.ChangeTarget(this.overrideTarget);
		}
		if (this.beam.enabled != this.target)
		{
			this.beam.enabled = this.target;
		}
		if (this.target)
		{
			this.beam.SetPosition(0, this.beam.transform.position);
			this.beam.SetPosition(1, this.target.transform.position + this.beamOffset);
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x000660C4 File Offset: 0x000642C4
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.2f);
		if (BlindEnemies.Blind)
		{
			if (this.target && (!this.overrideTarget || this.target != this.overrideTarget || this.overrideTarget.dead))
			{
				this.ChangeTarget(null);
			}
			return;
		}
		if (this.overrideTarget)
		{
			if (this.overrideTarget && !this.overrideTarget.dead && (this.overrideTarget.gameObject.activeInHierarchy || !this.activeWhileWaitingForOverride))
			{
				if (this.target != this.overrideTarget && this.overrideTarget.gameObject.activeInHierarchy)
				{
					this.ChangeTarget(this.overrideTarget);
				}
				return;
			}
			this.overrideTarget = null;
			this.ChangeTarget(null);
		}
		this.PickNewTarget(false);
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x000661B4 File Offset: 0x000643B4
	public void PickNewTarget(bool ignoreIfAlreadyTargeting = true)
	{
		if (ignoreIfAlreadyTargeting && (this.target != null || (this.overrideTarget != null && !this.overrideTarget.Equals(null))))
		{
			return;
		}
		List<EnemyIdentifier> currentEnemies = MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies();
		if (currentEnemies != null && currentEnemies.Count > 0)
		{
			bool flag = false;
			float num = float.PositiveInfinity;
			EnemyIdentifier enemyIdentifier = null;
			int num2 = 1;
			if (this.target && !this.target.dead)
			{
				num2 = Mathf.Max(MonoSingleton<EnemyTracker>.Instance.GetEnemyRank(this.target), 2);
			}
			for (int i = 7; i > num2; i--)
			{
				for (int j = 0; j < currentEnemies.Count; j++)
				{
					if (((!currentEnemies[j].blessed && currentEnemies[j].enemyType != EnemyType.Idol) || currentEnemies[j] == this.target) && (MonoSingleton<EnemyTracker>.Instance.GetEnemyRank(currentEnemies[j]) == i || (MonoSingleton<EnemyTracker>.Instance.GetEnemyRank(currentEnemies[j]) <= 2 && i == 2)))
					{
						float num3 = Vector3.Distance(MonoSingleton<PlayerTracker>.Instance.GetPlayer().position, currentEnemies[j].transform.position);
						if (num3 < num)
						{
							enemyIdentifier = currentEnemies[j];
							flag = true;
							num = num3;
						}
					}
				}
				if (flag)
				{
					this.ChangeTarget(enemyIdentifier);
					return;
				}
			}
		}
	}

	// Token: 0x06000D6E RID: 3438 RVA: 0x00066328 File Offset: 0x00064528
	public void Death()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		if (this.target && (this.eid.damageBuff || this.eid.speedBuff || this.eid.healthBuff))
		{
			if (this.eid.damageBuff)
			{
				this.target.DamageBuff();
			}
			if (this.eid.speedBuff)
			{
				this.target.SpeedBuff();
			}
			if (this.eid.healthBuff)
			{
				this.target.HealthBuff();
			}
		}
		GoreZone goreZone = GoreZone.ResolveGoreZone(base.transform);
		if (this.eid)
		{
			this.eid.Death();
		}
		if (this.deathParticle)
		{
			Object.Instantiate<GameObject>(this.deathParticle, this.beam.transform.position, Quaternion.identity, goreZone.gibZone);
		}
		for (int i = 0; i < 3; i++)
		{
			GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, this.eid, false);
			if (!gore)
			{
				break;
			}
			gore.transform.position = this.beam.transform.position;
			gore.transform.SetParent(goreZone.goreZone, true);
			gore.SetActive(true);
			Bloodsplatter bloodsplatter;
			if (gore.TryGetComponent<Bloodsplatter>(out bloodsplatter))
			{
				bloodsplatter.GetReady();
			}
		}
		if (!this.eid.dontCountAsKills)
		{
			ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
			if (componentInParent != null)
			{
				componentInParent.AddDeadEnemy();
			}
		}
		MonoSingleton<StyleHUD>.Instance.AddPoints(80, "ultrakill.iconoclasm", null, this.eid, -1, "", "");
		base.gameObject.SetActive(false);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x000664E8 File Offset: 0x000646E8
	private void ChangeTarget(EnemyIdentifier newTarget)
	{
		if (this.target)
		{
			this.target.Unbless(false);
		}
		if (!newTarget)
		{
			this.target = null;
			return;
		}
		this.target = newTarget;
		this.target.Bless(false);
		Collider collider;
		if (this.target.TryGetComponent<Collider>(out collider))
		{
			this.beamOffset = collider.bounds.center - this.target.transform.position;
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00066569 File Offset: 0x00064769
	public void ChangeOverrideTarget(EnemyIdentifier eid)
	{
		this.overrideTarget = eid;
		this.ChangeTarget(eid);
	}

	// Token: 0x040011FB RID: 4603
	public EnemyIdentifier overrideTarget;

	// Token: 0x040011FC RID: 4604
	public bool activeWhileWaitingForOverride;

	// Token: 0x040011FD RID: 4605
	[HideInInspector]
	public EnemyIdentifier target;

	// Token: 0x040011FE RID: 4606
	private int difficulty;

	// Token: 0x040011FF RID: 4607
	[SerializeField]
	private LineRenderer beam;

	// Token: 0x04001200 RID: 4608
	[HideInInspector]
	public Material unradiantBeam;

	// Token: 0x04001201 RID: 4609
	[SerializeField]
	private Material radiantBeam;

	// Token: 0x04001202 RID: 4610
	[SerializeField]
	private SpriteRenderer halo;

	// Token: 0x04001203 RID: 4611
	[HideInInspector]
	public Sprite unradiantHalo;

	// Token: 0x04001204 RID: 4612
	[HideInInspector]
	public Color haloColor;

	// Token: 0x04001205 RID: 4613
	[SerializeField]
	private Sprite radiantHalo;

	// Token: 0x04001206 RID: 4614
	private Vector3 beamOffset;

	// Token: 0x04001207 RID: 4615
	[SerializeField]
	private GameObject deathParticle;

	// Token: 0x04001208 RID: 4616
	private bool dead;

	// Token: 0x04001209 RID: 4617
	private EnemyIdentifier eid;
}
