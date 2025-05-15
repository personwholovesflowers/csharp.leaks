using System;
using System.Collections.Generic;
using DebugOverlays;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200046A RID: 1130
public class SwingCheck2 : MonoBehaviour
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x060019CE RID: 6606 RVA: 0x000D3A04 File Offset: 0x000D1C04
	private bool playerOnly
	{
		get
		{
			return this.eid == null || this.eid.target == null || this.eid.target.isPlayer;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060019CF RID: 6607 RVA: 0x000D3A33 File Offset: 0x000D1C33
	private LayerMask relevantLMask
	{
		get
		{
			return this.lmask;
		}
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000D3A3B File Offset: 0x000D1C3B
	private void Awake()
	{
		this.col = base.GetComponent<Collider>();
		this.aud = base.GetComponent<AudioSource>();
		this.lmask = LayerMaskDefaults.Get(LMD.Environment);
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000D3A64 File Offset: 0x000D1C64
	private void Start()
	{
		if (!this.eid)
		{
			this.eid = base.GetComponentInParent<EnemyIdentifier>();
		}
		if (this.eid)
		{
			this.type = this.eid.enemyType;
		}
		if (!this.col.isTrigger)
		{
			this.physicalCollider = true;
		}
		else if (!this.startActive)
		{
			this.col.enabled = false;
		}
		else
		{
			this.DamageStart();
		}
		if (this.interpolateBetweenFrames)
		{
			this.previousPosition = base.transform.position;
		}
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000D3AF3 File Offset: 0x000D1CF3
	private void OnEnable()
	{
		if (this.startActive && this.col)
		{
			this.col.enabled = true;
			this.DamageStart();
		}
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000D3B1C File Offset: 0x000D1D1C
	private void OnTriggerEnter(Collider other)
	{
		if (this.damaging)
		{
			this.CheckCollision(other);
		}
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x000D3B2D File Offset: 0x000D1D2D
	private void OnCollisionEnter(Collision collision)
	{
		if (this.damaging)
		{
			this.CheckCollision(collision.collider);
		}
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000D3B44 File Offset: 0x000D1D44
	private void Update()
	{
		if ((this.interpolateBetweenFrames || this.checkForCollisionsBetween) && this.damaging && this.col.attachedRigidbody)
		{
			if (this.interpolateBetweenFrames)
			{
				if (Input.GetKey(KeyCode.Alpha7) && !this.hasPrinted)
				{
					this.hasPrinted = true;
					Object.Instantiate<GameObject>(new GameObject("previousPosition"), this.previousPosition, Quaternion.identity);
					Object.Instantiate<GameObject>(new GameObject("position"), base.transform.position, Quaternion.identity);
					Object.Instantiate<GameObject>(new GameObject("rigidbodyPosition"), this.col.attachedRigidbody.position, Quaternion.identity);
					Object.Instantiate<GameObject>(new GameObject("checkForCollisionBetween"), this.checkForCollisionsBetween.position, Quaternion.identity);
				}
				foreach (RaycastHit raycastHit in this.col.attachedRigidbody.SweepTestAll(this.previousPosition - base.transform.position, Vector3.Distance(this.previousPosition, base.transform.position), QueryTriggerInteraction.Collide))
				{
					this.CheckCollision(raycastHit.collider);
				}
			}
			if (this.checkForCollisionsBetween)
			{
				foreach (RaycastHit raycastHit2 in this.col.attachedRigidbody.SweepTestAll(this.checkForCollisionsBetween.position - base.transform.position, Vector3.Distance(this.checkForCollisionsBetween.position, base.transform.position), QueryTriggerInteraction.Collide))
				{
					this.CheckCollision(raycastHit2.collider);
				}
			}
			this.previousPosition = base.transform.position;
		}
		this.UpdateDebugOverlay();
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x000D3D24 File Offset: 0x000D1F24
	private void CheckCollision(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!this.hitColliders.Contains(other) && other.gameObject.layer != 15)
			{
				bool flag = false;
				if (this.useRaycastCheck && this.eid)
				{
					Vector3 vector = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
					if (Physics.Raycast(vector, other.bounds.center - vector, Vector3.Distance(vector, other.bounds.center), this.relevantLMask))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
					{
						if (!this.ignoreSlidingPlayer || !MonoSingleton<PlatformerMovement>.Instance.sliding)
						{
							MonoSingleton<PlatformerMovement>.Instance.Explode(false);
						}
						return;
					}
					if (this.nmov == null)
					{
						this.nmov = other.GetComponent<NewMovement>();
					}
					if (!this.ignoreSlidingPlayer || !this.nmov.sliding)
					{
						this.nmov.GetHurt(Mathf.RoundToInt((float)this.damage * this.eid.totalDamageModifier), true, 1f, false, false, 0.35f, false);
						if (!this.canHitPlayerMultipleTimes)
						{
							this.hitColliders.Add(other);
						}
						if (this.knockBackForce > 0f)
						{
							Vector3 forward = base.transform.forward;
							if (this.knockBackDirectionOverride)
							{
								forward = this.knockBackDirection;
							}
							if (this.knockBackDirection == Vector3.down)
							{
								this.nmov.Slamdown(this.knockBackForce);
							}
							else
							{
								this.nmov.LaunchFromPoint(this.nmov.transform.position + forward * -1f, this.knockBackForce, this.knockBackForce);
							}
						}
						this.NotifyTargetBeenHit();
						return;
					}
				}
			}
		}
		else if (other.gameObject.layer == 10 && !this.playerOnly && !this.hitColliders.Contains(other))
		{
			EnemyIdentifierIdentifier component = other.GetComponent<EnemyIdentifierIdentifier>();
			if (component != null && component.eid != null)
			{
				EnemyIdentifier enemyIdentifier = component.eid;
				this.CheckEidCollision(enemyIdentifier, other);
				return;
			}
		}
		else if (other.gameObject.layer == 12 && !this.playerOnly && !this.hitColliders.Contains(other))
		{
			EnemyIdentifier component2 = other.GetComponent<EnemyIdentifier>();
			if (component2 != null)
			{
				this.CheckEidCollision(component2, other);
				return;
			}
		}
		else if (other.gameObject.CompareTag("Breakable"))
		{
			Breakable component3 = other.gameObject.GetComponent<Breakable>();
			if (component3 != null && (this.strong || component3.weak) && !component3.playerOnly && !component3.precisionOnly && !component3.specialCaseOnly)
			{
				component3.Break();
			}
		}
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x000D4034 File Offset: 0x000D2234
	private void CheckEidCollision(EnemyIdentifier enid, Collider other)
	{
		if (this.hitEnemies.Contains(enid))
		{
			return;
		}
		if (enid.enemyType == this.type || this.eid.immuneToFriendlyFire || EnemyIdentifier.CheckHurtException(this.type, enid.enemyType, (this.eid != null) ? this.eid.target : null))
		{
			return;
		}
		if (EnemyIdentifierDebug.Active)
		{
			SwingCheck2.Log.Fine("We're in, no hurt exception", null, null, null);
		}
		if (!this.hitEnemies.Contains(enid) || this.hitEnemies[this.hitEnemies.IndexOf(enid)] != null || (enid.dead && other.gameObject.CompareTag("Head")))
		{
			if (EnemyIdentifierDebug.Active)
			{
				SwingCheck2.Log.Fine("hit enemies doesn't contain " + enid.gameObject.name, null, null, null);
			}
			if (!enid.dead || (enid.dead && !other.gameObject.CompareTag("Body")))
			{
				if (EnemyIdentifierDebug.Active)
				{
					SwingCheck2.Log.Fine("enid not dead or enid dead and not body", null, null, null);
				}
				bool flag = false;
				if (this.useRaycastCheck && this.eid)
				{
					Vector3 vector = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
					RaycastHit raycastHit;
					if (Physics.Raycast(vector, other.transform.position - vector, out raycastHit, Vector3.Distance(vector, other.transform.position), this.relevantLMask))
					{
						flag = true;
						if (EnemyIdentifierDebug.Active)
						{
							SwingCheck2.Log.Fine("block hit by " + raycastHit.collider.gameObject.name, null, null, null);
						}
					}
					else if (EnemyIdentifierDebug.Active)
					{
						SwingCheck2.Log.Fine("no block hit", null, null, null);
					}
				}
				if (!flag)
				{
					enid.hitter = "enemy";
					if (this.enemyDamage == 0)
					{
						this.enemyDamage = this.damage / 10;
					}
					float num = (float)this.enemyDamage * this.eid.totalDamageModifier;
					Gutterman gutterman;
					if (this.type == EnemyType.Guttertank && enid.enemyType == EnemyType.Gutterman && !enid.dead && enid.TryGetComponent<Gutterman>(out gutterman))
					{
						if (gutterman.hasShield)
						{
							gutterman.ShieldBreak(false, false);
						}
						else
						{
							gutterman.GotParried();
						}
						num *= 4f;
					}
					enid.DeliverDamage(other.gameObject, ((base.transform.position - other.transform.position).normalized + Vector3.up) * 10000f, other.transform.position, num, false, 0f, null, false, false);
					this.hitEnemies.Add(enid);
					this.hitColliders.Add(other);
					this.NotifyTargetBeenHit();
				}
			}
		}
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x000D4348 File Offset: 0x000D2548
	private void NotifyTargetBeenHit()
	{
		if (this.eid)
		{
			IHitTargetCallback[] components = this.eid.GetComponents<IHitTargetCallback>();
			if (EnemyIdentifierDebug.Active)
			{
				SwingCheck2.Log.Info(string.Format("We've hit <b>{0}</b>. Broadcasting to <b>{1}</b> receiver{2}.", this.eid.target, components.Length, (components.Length == 1) ? string.Empty : "s"), null, null, null);
			}
			IHitTargetCallback[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TargetBeenHit();
			}
		}
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000D43CC File Offset: 0x000D25CC
	public void DamageStart()
	{
		if (this.damaging)
		{
			this.DamageStop();
		}
		this.previousPosition = base.transform.position;
		this.damaging = true;
		if (!this.physicalCollider)
		{
			if (this.col)
			{
				this.col.enabled = true;
			}
			if (this.additionalColliders != null)
			{
				Collider[] array = this.additionalColliders;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = true;
				}
			}
		}
		if (this.aud != null)
		{
			this.aud.Play();
		}
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000D4460 File Offset: 0x000D2660
	public void DamageStop()
	{
		this.damaging = false;
		if (this.hitColliders.Count > 0)
		{
			this.hitColliders.Clear();
		}
		if (this.hitEnemies.Count > 0)
		{
			this.hitEnemies.Clear();
		}
		if (!this.physicalCollider)
		{
			if (this.col)
			{
				this.col.enabled = false;
			}
			if (this.additionalColliders != null)
			{
				Collider[] array = this.additionalColliders;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
			}
		}
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x000D44ED File Offset: 0x000D26ED
	public void OverrideEnemyIdentifier(EnemyIdentifier newEid)
	{
		this.eid = newEid;
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000D44F8 File Offset: 0x000D26F8
	private void UpdateDebugOverlay()
	{
		if (EnemyIdentifierDebug.Active)
		{
			if (this.debugOverlay == null)
			{
				this.debugOverlay = base.gameObject.AddComponent<SwingCheckDebugOverlay>();
			}
			this.debugOverlay.ConsumeData(this.damaging, this.eid);
			return;
		}
		if (this.debugOverlay != null)
		{
			Object.Destroy(this.debugOverlay);
		}
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x000D455C File Offset: 0x000D275C
	public void CanHitPlayerMultipleTimes(bool yes)
	{
		this.canHitPlayerMultipleTimes = yes;
		if (yes && this.hitColliders.Contains(MonoSingleton<NewMovement>.Instance.playerCollider))
		{
			this.hitColliders.Remove(MonoSingleton<NewMovement>.Instance.playerCollider);
		}
	}

	// Token: 0x0400241F RID: 9247
	private static readonly global::plog.Logger Log = new global::plog.Logger("SwingCheck2");

	// Token: 0x04002420 RID: 9248
	[HideInInspector]
	public EnemyIdentifier eid;

	// Token: 0x04002421 RID: 9249
	public EnemyType type;

	// Token: 0x04002422 RID: 9250
	public List<Collider> hitColliders = new List<Collider>();

	// Token: 0x04002423 RID: 9251
	private NewMovement nmov;

	// Token: 0x04002424 RID: 9252
	public int damage;

	// Token: 0x04002425 RID: 9253
	public int enemyDamage;

	// Token: 0x04002426 RID: 9254
	public float knockBackForce;

	// Token: 0x04002427 RID: 9255
	public bool knockBackDirectionOverride;

	// Token: 0x04002428 RID: 9256
	public Vector3 knockBackDirection;

	// Token: 0x04002429 RID: 9257
	private LayerMask lmask;

	// Token: 0x0400242A RID: 9258
	private List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();

	// Token: 0x0400242B RID: 9259
	public bool strong;

	// Token: 0x0400242C RID: 9260
	[HideInInspector]
	public Collider col;

	// Token: 0x0400242D RID: 9261
	public Collider[] additionalColliders;

	// Token: 0x0400242E RID: 9262
	public bool useRaycastCheck;

	// Token: 0x0400242F RID: 9263
	private AudioSource aud;

	// Token: 0x04002430 RID: 9264
	private bool physicalCollider;

	// Token: 0x04002431 RID: 9265
	[HideInInspector]
	public bool damaging;

	// Token: 0x04002432 RID: 9266
	public bool ignoreSlidingPlayer;

	// Token: 0x04002433 RID: 9267
	public bool canHitPlayerMultipleTimes;

	// Token: 0x04002434 RID: 9268
	public bool startActive;

	// Token: 0x04002435 RID: 9269
	public bool interpolateBetweenFrames;

	// Token: 0x04002436 RID: 9270
	public Transform checkForCollisionsBetween;

	// Token: 0x04002437 RID: 9271
	private Vector3 previousPosition;

	// Token: 0x04002438 RID: 9272
	private SwingCheckDebugOverlay debugOverlay;

	// Token: 0x04002439 RID: 9273
	private bool hasPrinted;
}
