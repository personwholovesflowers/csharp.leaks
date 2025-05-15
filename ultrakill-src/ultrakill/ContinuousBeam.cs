using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class ContinuousBeam : MonoBehaviour
{
	// Token: 0x06000445 RID: 1093 RVA: 0x0001D458 File Offset: 0x0001B658
	private void Start()
	{
		this.lr = base.GetComponent<LineRenderer>();
		this.environmentMask = LayerMaskDefaults.Get(LMD.Environment);
		this.hitMask |= this.environmentMask;
		this.hitMask |= 1024;
		this.hitMask |= 4;
		if (this.ignoreInvincibility)
		{
			this.hitMask |= 32768;
		}
		this.hasHadEndPoint = this.endPoint != null;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001D509 File Offset: 0x0001B709
	public void SetPlayerCooldown(float cooldown)
	{
		this.playerCooldown = cooldown;
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0001D514 File Offset: 0x0001B714
	private void Update()
	{
		if (this.hasHadEndPoint && this.destroyIfEndPointDestroyed && this.endPoint == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.playerCooldown > 0f)
		{
			this.playerCooldown = Mathf.MoveTowards(this.playerCooldown, 0f, Time.deltaTime);
		}
		if (this.enemyCooldowns.Count > 0)
		{
			for (int i = 0; i < this.enemyCooldowns.Count; i++)
			{
				this.enemyCooldowns[i] = Mathf.MoveTowards(this.enemyCooldowns[i], 0f, Time.deltaTime);
			}
		}
		Vector3 vector = Vector3.zero;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, this.endPoint ? (this.endPoint.position - base.transform.position) : base.transform.forward, out raycastHit, this.endPoint ? Vector3.Distance(base.transform.position, this.endPoint.position) : float.PositiveInfinity, this.environmentMask))
		{
			if (this.endPoint && this.cancelIfEndPointBlocked)
			{
				this.lr.enabled = false;
				if (this.impactEffect)
				{
					this.impactEffect.SetActive(false);
				}
				return;
			}
			vector = raycastHit.point;
			if (this.impactEffect)
			{
				this.impactEffect.SetActive(true);
			}
		}
		else if (this.endPoint)
		{
			vector = this.endPoint.position;
			if (this.impactEffect)
			{
				this.impactEffect.SetActive(false);
			}
		}
		else
		{
			vector = base.transform.position + base.transform.forward * 999f;
			if (this.impactEffect)
			{
				this.impactEffect.SetActive(false);
			}
		}
		this.lr.enabled = true;
		this.lr.SetPosition(0, base.transform.position);
		this.lr.SetPosition(1, vector);
		if (this.impactEffect)
		{
			this.impactEffect.transform.position = vector;
		}
		if (this.trackOnBeamToPlayer)
		{
			float num = Mathf.Clamp(Vector3.Dot(MonoSingleton<CameraController>.Instance.transform.position - base.transform.position, (vector - base.transform.position).normalized), 0f, (vector - base.transform.position).magnitude);
			this.trackOnBeamToPlayer.transform.position = base.transform.position + (vector - base.transform.position).normalized * num;
		}
		RaycastHit[] array = Physics.SphereCastAll(base.transform.position + base.transform.forward * this.beamWidth, this.beamWidth, vector - base.transform.position, Vector3.Distance(base.transform.position, vector) - this.beamWidth, this.hitMask);
		if (array != null && array.Length != 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (this.canHitPlayer && this.playerCooldown <= 0f && array[j].collider.gameObject.CompareTag("Player"))
				{
					this.playerCooldown = 0.5f;
					if (!Physics.Raycast(base.transform.position, array[j].point - base.transform.position, array[j].distance, this.environmentMask))
					{
						MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(this.damage), true, 1f, false, false, 0.35f, true);
					}
				}
				else if ((array[j].transform.gameObject.layer == 10 || array[j].transform.gameObject.layer == 11) && this.canHitEnemy)
				{
					EnemyIdentifierIdentifier component = array[j].transform.GetComponent<EnemyIdentifierIdentifier>();
					if (component && component.eid && (!this.enemy || (component.eid.enemyType != this.safeEnemyType && !component.eid.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.safeEnemyType, component.eid.enemyType, this.target))))
					{
						EnemyIdentifier eid = component.eid;
						bool flag = false;
						if (this.hitEnemies.Contains(eid))
						{
							flag = true;
						}
						if (!flag || this.enemyCooldowns[this.hitEnemies.IndexOf(eid)] <= 0f)
						{
							if (!flag)
							{
								this.hitEnemies.Add(eid);
								this.enemyCooldowns.Add(0.5f);
							}
							else
							{
								this.enemyCooldowns[this.hitEnemies.IndexOf(eid)] = 0.5f;
							}
							if (this.enemy)
							{
								eid.hitter = "enemy";
							}
							eid.DeliverDamage(array[j].transform.gameObject, (vector - base.transform.position).normalized * 1000f, array[j].point, this.damage * this.parryMultiplier / 10f, true, 0f, null, false, false);
						}
					}
				}
				else if (LayerMaskDefaults.IsMatchingLayer(array[j].transform.gameObject.layer, LMD.Environment))
				{
					Breakable component2 = array[j].transform.GetComponent<Breakable>();
					if (component2 && !component2.playerOnly && !component2.precisionOnly && !component2.specialCaseOnly)
					{
						component2.Break();
					}
					Bleeder bleeder;
					if (array[j].transform.gameObject.TryGetComponent<Bleeder>(out bleeder))
					{
						bleeder.GetHit(array[j].point, GoreType.Small, false);
					}
				}
			}
		}
	}

	// Token: 0x040005CF RID: 1487
	public EnemyTarget target;

	// Token: 0x040005D0 RID: 1488
	private LineRenderer lr;

	// Token: 0x040005D1 RID: 1489
	private LayerMask environmentMask;

	// Token: 0x040005D2 RID: 1490
	private LayerMask hitMask;

	// Token: 0x040005D3 RID: 1491
	public bool canHitPlayer = true;

	// Token: 0x040005D4 RID: 1492
	public bool canHitEnemy = true;

	// Token: 0x040005D5 RID: 1493
	public bool ignoreInvincibility;

	// Token: 0x040005D6 RID: 1494
	public float beamWidth = 0.35f;

	// Token: 0x040005D7 RID: 1495
	public bool enemy;

	// Token: 0x040005D8 RID: 1496
	public EnemyType safeEnemyType;

	// Token: 0x040005D9 RID: 1497
	public float damage;

	// Token: 0x040005DA RID: 1498
	public float parryMultiplier = 1f;

	// Token: 0x040005DB RID: 1499
	private float playerCooldown;

	// Token: 0x040005DC RID: 1500
	private List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();

	// Token: 0x040005DD RID: 1501
	private List<float> enemyCooldowns = new List<float>();

	// Token: 0x040005DE RID: 1502
	public GameObject impactEffect;

	// Token: 0x040005DF RID: 1503
	public GameObject trackOnBeamToPlayer;

	// Token: 0x040005E0 RID: 1504
	[Header("End Point")]
	public Transform endPoint;

	// Token: 0x040005E1 RID: 1505
	private bool hasHadEndPoint;

	// Token: 0x040005E2 RID: 1506
	public bool cancelIfEndPointBlocked;

	// Token: 0x040005E3 RID: 1507
	public bool destroyIfEndPointDestroyed;
}
