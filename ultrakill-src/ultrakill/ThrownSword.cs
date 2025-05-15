using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class ThrownSword : MonoBehaviour
{
	// Token: 0x06001A4E RID: 6734 RVA: 0x000D8BE0 File Offset: 0x000D6DE0
	private void Start()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (this.type == 1 && this.difficulty < 2)
		{
			Spin component = base.transform.parent.GetComponent<Spin>();
			if (component)
			{
				component.speed /= 2f;
			}
		}
		this.timeSince = 0f;
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x000D8C50 File Offset: 0x000D6E50
	private void Update()
	{
		if (this.hittingPlayer)
		{
			base.transform.position = MonoSingleton<NewMovement>.Instance.transform.position;
			return;
		}
		if (this.active)
		{
			if (this.type == 0)
			{
				if (!this.returning && base.transform.position != this.targetPos)
				{
					if (this.difficulty == 1)
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, Time.deltaTime * this.speed / 1.5f);
					}
					else if (this.difficulty == 0)
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, Time.deltaTime * this.speed / 2f);
					}
					else
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, Time.deltaTime * this.speed);
					}
					if (base.transform.position == this.targetPos && !this.calledReturn)
					{
						this.calledReturn = true;
						base.CancelInvoke("Return");
						base.Invoke("Return", 1f);
					}
					if (this.thrownAtVoid && this.timeSince > 1f && Vector3.Distance(base.transform.position, this.thrownBy.transform.position) - 15f > Vector3.Distance(this.thrownBy.transform.position, this.thrownBy.target.headTransform.position))
					{
						this.calledReturn = true;
						this.Return();
						return;
					}
				}
				else
				{
					if (!(this.returnTransform != null))
					{
						Object.Destroy(base.gameObject);
						return;
					}
					if (this.difficulty == 1)
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.returnTransform.position, Time.deltaTime * this.speed / 1.5f);
					}
					else if (this.difficulty == 0)
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.returnTransform.position, Time.deltaTime * this.speed / 2f);
					}
					else
					{
						base.transform.position = Vector3.MoveTowards(base.transform.position, this.returnTransform.position, Time.deltaTime * this.speed);
					}
					if (base.transform.position == this.returnTransform.position)
					{
						SwordsMachine componentInParent = this.returnTransform.GetComponentInParent<SwordsMachine>();
						if (componentInParent != null)
						{
							if (!this.friendly)
							{
								componentInParent.SwordCatch();
							}
							else if (this.friendly)
							{
								componentInParent.Knockdown(false, false);
							}
						}
						Object.Destroy(base.gameObject);
						return;
					}
				}
			}
			else if (this.type == 1)
			{
				if (!this.returning)
				{
					if (this.difficulty < 2)
					{
						base.transform.position += base.transform.parent.forward * (float)(15 + this.difficulty * 5) * Time.deltaTime;
						return;
					}
					if (this.difficulty < 4)
					{
						base.transform.position += base.transform.parent.forward * 25f * Time.deltaTime;
						return;
					}
					base.transform.position += base.transform.parent.forward * 35f * Time.deltaTime;
					return;
				}
				else
				{
					if (base.transform.parent != null)
					{
						base.transform.parent = null;
					}
					if (this.returnTransform == null)
					{
						Object.Destroy(base.gameObject);
						return;
					}
					base.transform.position = Vector3.MoveTowards(base.transform.position, this.returnTransform.position, Time.deltaTime * this.speed * 3f);
					if (base.transform.position == this.returnTransform.position)
					{
						SwordsMachine componentInParent2 = this.returnTransform.GetComponentInParent<SwordsMachine>();
						if (componentInParent2 != null)
						{
							if (!this.friendly)
							{
								componentInParent2.SwordCatch();
							}
							else if (this.friendly)
							{
								componentInParent2.Knockdown(false, false);
							}
						}
						Object.Destroy(base.gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000D9110 File Offset: 0x000D7310
	public void SetPoints(Vector3 target, Transform origin)
	{
		this.targetPos = target;
		this.returnTransform = origin;
		this.active = true;
		if (this.type == 1)
		{
			base.Invoke("Return", 1f);
			return;
		}
		if (!this.thrownAtVoid)
		{
			base.Invoke("Return", 2f);
		}
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000D9164 File Offset: 0x000D7364
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			base.Invoke("RecheckPlayerHit", 0.05f);
			this.hittingPlayer = true;
		}
		else
		{
			EnemyIdentifier enemyIdentifier = other.gameObject.GetComponent<EnemyIdentifier>();
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (enemyIdentifier == null && other.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				enemyIdentifier = enemyIdentifierIdentifier.eid;
			}
			if (enemyIdentifier != null && !this.hitEnemies.Contains(enemyIdentifier) && (this.thrownBy == null || (!enemyIdentifier.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.thrownBy.enemyType, enemyIdentifier.enemyType, null))))
			{
				if (!enemyIdentifier.dead)
				{
					this.hitEnemies.Add(enemyIdentifier);
				}
				enemyIdentifier.hitter = "enemy";
				enemyIdentifier.DeliverDamage(other.gameObject, base.GetComponent<Rigidbody>().velocity, base.transform.position, 5f, false, 0f, null, true, false);
			}
		}
		if (this.deflected && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) && !this.calledReturn)
		{
			this.targetPos = other.ClosestPoint(base.transform.position);
			if (this.targetPos.magnitude < 9999f)
			{
				base.transform.position = this.targetPos;
			}
			else
			{
				this.targetPos = base.transform.position;
			}
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.calledReturn = true;
			base.CancelInvoke("Return");
			base.Invoke("Return", 1f);
		}
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x000D930A File Offset: 0x000D750A
	private void RecheckPlayerHit()
	{
		if (this.hittingPlayer)
		{
			this.hittingPlayer = false;
			base.GetComponent<Collider>().enabled = false;
			MonoSingleton<NewMovement>.Instance.GetHurt(30, true, 1f, false, false, 0.35f, false);
		}
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000D9341 File Offset: 0x000D7541
	private void Return()
	{
		if (!this.returning)
		{
			this.returning = true;
			if (this.type == 1)
			{
				base.GetComponent<Collider>().enabled = false;
				return;
			}
			base.transform.LookAt(this.returnTransform);
		}
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x000D9379 File Offset: 0x000D7579
	public void GetParried()
	{
		base.CancelInvoke("RecheckPlayerHit");
		this.hittingPlayer = false;
		this.friendly = true;
		base.GetComponent<Collider>().enabled = false;
		this.Return();
	}

	// Token: 0x040024D9 RID: 9433
	public EnemyIdentifier thrownBy;

	// Token: 0x040024DA RID: 9434
	public Vector3 targetPos;

	// Token: 0x040024DB RID: 9435
	public Transform returnTransform;

	// Token: 0x040024DC RID: 9436
	public bool active;

	// Token: 0x040024DD RID: 9437
	public float speed;

	// Token: 0x040024DE RID: 9438
	private bool returning;

	// Token: 0x040024DF RID: 9439
	private bool calledReturn;

	// Token: 0x040024E0 RID: 9440
	public int type;

	// Token: 0x040024E1 RID: 9441
	public bool friendly;

	// Token: 0x040024E2 RID: 9442
	public bool deflected;

	// Token: 0x040024E3 RID: 9443
	private bool hittingPlayer;

	// Token: 0x040024E4 RID: 9444
	[HideInInspector]
	public bool thrownAtVoid;

	// Token: 0x040024E5 RID: 9445
	private TimeSince timeSince;

	// Token: 0x040024E6 RID: 9446
	private List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();

	// Token: 0x040024E7 RID: 9447
	private int difficulty;
}
