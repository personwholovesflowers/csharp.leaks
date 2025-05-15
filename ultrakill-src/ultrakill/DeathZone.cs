using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class DeathZone : MonoBehaviour
{
	// Token: 0x060004F1 RID: 1265 RVA: 0x000216A8 File Offset: 0x0001F8A8
	private void Start()
	{
		if (this.unaffectedEnemyTypes == null)
		{
			this.unaffectedEnemyTypes = Array.Empty<EnemyType>();
		}
		this.player = MonoSingleton<NewMovement>.Instance.transform;
		switch (this.affected)
		{
		case AffectedSubjects.All:
			this.enemyAffected = true;
			this.playerAffected = true;
			break;
		case AffectedSubjects.PlayerOnly:
			this.enemyAffected = false;
			this.playerAffected = true;
			break;
		case AffectedSubjects.EnemiesOnly:
			this.enemyAffected = true;
			this.playerAffected = false;
			break;
		}
		base.Invoke("SlowUpdate", 1f);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00021734 File Offset: 0x0001F934
	private void SlowUpdate()
	{
		if (base.gameObject.activeInHierarchy && this.checkForPlayerOutsideTrigger && this.player.transform.position.y < base.transform.position.y)
		{
			this.GotHit(this.player.GetComponent<Collider>());
		}
		base.Invoke("SlowUpdate", 1f);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0002179E File Offset: 0x0001F99E
	private void OnTriggerEnter(Collider other)
	{
		this.GotHit(other);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x000217A7 File Offset: 0x0001F9A7
	private void OnCollisionEnter(Collision collision)
	{
		this.GotHit(collision.collider);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000217B8 File Offset: 0x0001F9B8
	private void GotHit(Collider other)
	{
		Collider collider;
		if (other.gameObject.layer == 20 && other.transform.parent && other.transform.parent != MonoSingleton<NewMovement>.Instance.transform && other.transform.parent.TryGetComponent<Collider>(out collider))
		{
			other = collider;
		}
		IgnoreDeathZones ignoreDeathZones;
		if (other.gameObject.CompareTag("Player") && this.playerAffected)
		{
			if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
			{
				if (this.dontExplode || this.deathType.ToLower() == "fall")
				{
					MonoSingleton<PlatformerMovement>.Instance.Fall();
					return;
				}
				MonoSingleton<PlatformerMovement>.Instance.Explode(true);
				return;
			}
			else
			{
				if (!this.notInstakill)
				{
					if (this.pm == null)
					{
						this.pm = other.GetComponent<NewMovement>();
					}
					this.pm.GetHurt(999999, false, 1f, false, true, 0.35f, false);
					if (this.sawSound != null)
					{
						Object.Instantiate<GameObject>(this.sawSound, other.transform.position, Quaternion.identity);
					}
					base.enabled = false;
					return;
				}
				if (this.pm == null)
				{
					this.pm = other.GetComponent<NewMovement>();
				}
				if (this.pm.hp > 0)
				{
					if (this.damage == 0 || this.pm.hp == 1)
					{
						this.pm.FakeHurt(false);
					}
					else if (this.pm.hp > this.damage)
					{
						this.pm.GetHurt(this.damage, true, 1f, false, false, 0.35f, false);
					}
					else if (this.pm.hp > 1)
					{
						this.pm.GetHurt(this.pm.hp - 1, true, 1f, false, false, 0.35f, false);
					}
					if (this.sawSound != null)
					{
						Object.Instantiate<GameObject>(this.sawSound, other.transform.position, Quaternion.identity);
					}
					other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
					StatsManager instance = MonoSingleton<StatsManager>.Instance;
					Vector3 zero = Vector3.zero;
					if (this.respawnTarget != Vector3.zero)
					{
						other.transform.position = this.respawnTarget + Vector3.up * 1.25f;
						return;
					}
					if (instance.currentCheckPoint != null)
					{
						other.transform.position = instance.currentCheckPoint.transform.position + Vector3.up * 1.25f;
						return;
					}
					other.transform.position = instance.spawnPos;
					return;
				}
			}
		}
		else if ((other.gameObject.CompareTag("Enemy") || other.gameObject.layer == 10 || other.gameObject.layer == 11) && this.enemyAffected && !other.TryGetComponent<IgnoreDeathZones>(out ignoreDeathZones))
		{
			EnemyIdentifier enemyIdentifier = other.gameObject.GetComponentInParent<EnemyIdentifier>();
			if (enemyIdentifier == null)
			{
				EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
				if (component != null)
				{
					if (component.eid != null)
					{
						enemyIdentifier = component.eid;
					}
					else
					{
						Object.Destroy(component.gameObject);
					}
				}
			}
			if (enemyIdentifier != null)
			{
				if (this.unaffectedEnemyTypes.Length != 0)
				{
					for (int i = 0; i < this.unaffectedEnemyTypes.Length; i++)
					{
						if (this.unaffectedEnemyTypes[i] == enemyIdentifier.enemyType)
						{
							return;
						}
					}
				}
				if ((!this.dontExplode && !this.aliveOnly && !enemyIdentifier.exploded) || !enemyIdentifier.dead)
				{
					if (this.sawSound != null)
					{
						Object.Instantiate<GameObject>(this.sawSound, other.transform.position, Quaternion.identity);
					}
					enemyIdentifier.hitter = "deathzone";
					StyleHUD instance2 = MonoSingleton<StyleHUD>.Instance;
					if (instance2 && !enemyIdentifier.puppet)
					{
						if (!enemyIdentifier.dead)
						{
							instance2.AddPoints(this.styleAmount, this.deathType, null, enemyIdentifier, -1, "", "");
						}
						else
						{
							instance2.AddPoints(this.styleAmount / 4, (this.deathType == "") ? "" : ("<color=grey>" + this.deathType + "</color>"), null, enemyIdentifier, -1, "", "");
						}
					}
					if (this.enemiesCanDodge)
					{
						EnemyIdentifier.FallOnEnemy(enemyIdentifier);
					}
					if (this.splatter)
					{
						enemyIdentifier.Splatter(false);
					}
					else if (!this.dontExplode)
					{
						enemyIdentifier.Explode(false);
						Gutterman gutterman;
						if (enemyIdentifier.enemyType == EnemyType.Gutterman && enemyIdentifier.TryGetComponent<Gutterman>(out gutterman))
						{
							gutterman.Explode();
						}
					}
					else
					{
						enemyIdentifier.InstaKill();
					}
				}
				if (this.deleteLimbs && enemyIdentifier.dead && !enemyIdentifier.DestroyLimb(other.transform, LimbDestroyType.Destroy))
				{
					other.gameObject.SetActive(false);
					other.transform.position = new Vector3(-100f, -100f, -100f);
					other.transform.localScale = Vector3.zero;
					return;
				}
			}
		}
		else if (other.gameObject.CompareTag("Coin"))
		{
			Coin component2 = other.GetComponent<Coin>();
			if (component2 == null)
			{
				return;
			}
			component2.GetDeleted();
		}
	}

	// Token: 0x040006BA RID: 1722
	private NewMovement pm;

	// Token: 0x040006BB RID: 1723
	public GameObject sawSound;

	// Token: 0x040006BC RID: 1724
	public string deathType;

	// Token: 0x040006BD RID: 1725
	public bool dontExplode;

	// Token: 0x040006BE RID: 1726
	public bool splatter;

	// Token: 0x040006BF RID: 1727
	public bool enemiesCanDodge;

	// Token: 0x040006C0 RID: 1728
	public bool aliveOnly;

	// Token: 0x040006C1 RID: 1729
	public bool deleteLimbs;

	// Token: 0x040006C2 RID: 1730
	public AffectedSubjects affected;

	// Token: 0x040006C3 RID: 1731
	private bool playerAffected;

	// Token: 0x040006C4 RID: 1732
	private bool enemyAffected;

	// Token: 0x040006C5 RID: 1733
	public bool checkForPlayerOutsideTrigger;

	// Token: 0x040006C6 RID: 1734
	[Space(10f)]
	public bool notInstakill;

	// Token: 0x040006C7 RID: 1735
	public Vector3 respawnTarget;

	// Token: 0x040006C8 RID: 1736
	public bool dontChangeRespawnTarget;

	// Token: 0x040006C9 RID: 1737
	public int damage = 50;

	// Token: 0x040006CA RID: 1738
	public int styleAmount = 80;

	// Token: 0x040006CB RID: 1739
	private Transform player;

	// Token: 0x040006CC RID: 1740
	public EnemyType[] unaffectedEnemyTypes;
}
