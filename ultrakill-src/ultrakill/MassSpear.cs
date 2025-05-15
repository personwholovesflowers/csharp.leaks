using System;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class MassSpear : MonoBehaviour
{
	// Token: 0x06001078 RID: 4216 RVA: 0x0007E234 File Offset: 0x0007C434
	private void Start()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.lr = base.GetComponentInChildren<LineRenderer>();
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.mass = this.originPoint.GetComponentInParent<Mass>();
		base.Invoke("CheckForDistance", 3f / this.speedMultiplier);
		if (this.difficulty == 1)
		{
			this.rb.AddForce(base.transform.forward * 75f * this.speedMultiplier, ForceMode.VelocityChange);
		}
		if (this.difficulty == 2)
		{
			this.rb.AddForce(base.transform.forward * 200f * this.speedMultiplier, ForceMode.VelocityChange);
			return;
		}
		if (this.difficulty >= 3)
		{
			this.rb.AddForce(base.transform.forward * 250f * this.speedMultiplier, ForceMode.VelocityChange);
		}
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x0007E343 File Offset: 0x0007C543
	private void OnDisable()
	{
		if (!this.returning)
		{
			this.Return();
		}
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x0007E354 File Offset: 0x0007C554
	private void Update()
	{
		if (this.originPoint != null && !this.originPoint.gameObject.activeInHierarchy)
		{
			this.lr.SetPosition(0, this.originPoint.position);
			this.lr.SetPosition(1, this.lr.transform.position);
			if (this.returning)
			{
				if (!this.originPoint || !this.originPoint.parent || !this.originPoint.parent.gameObject.activeInHierarchy)
				{
					Object.Destroy(base.gameObject);
					return;
				}
				base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.originPoint.position, Vector3.up);
				this.rb.velocity = base.transform.forward * -100f * this.speedMultiplier;
				if (Vector3.Distance(base.transform.position, this.originPoint.position) < 1f)
				{
					if (this.mass != null)
					{
						this.mass.SpearReturned();
					}
					Object.Destroy(base.gameObject);
					return;
				}
			}
			else if (this.deflected)
			{
				base.transform.LookAt(this.originPoint.position);
				this.rb.velocity = base.transform.forward * 100f * this.speedMultiplier;
				if (Vector3.Distance(base.transform.position, this.originPoint.position) < 1f && this.mass != null)
				{
					this.mass.SpearReturned();
					BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
					EnemyIdentifier component = this.mass.GetComponent<EnemyIdentifier>();
					Transform child = this.mass.tailEnd.GetChild(0);
					this.HurtEnemy(child.gameObject, component);
					for (int i = 0; i < 3; i++)
					{
						GameObject gore = instance.GetGore(GoreType.Head, component, false);
						gore.transform.position = child.position;
						GoreZone goreZone = GoreZone.ResolveGoreZone(base.transform);
						if (goreZone)
						{
							gore.transform.SetParent(goreZone.goreZone);
						}
					}
					this.mass.SpearParried();
					Object.Destroy(base.gameObject);
					return;
				}
			}
			else if (this.hitPlayer && !this.returning)
			{
				if (this.nmov.hp <= 0)
				{
					this.Return();
					Object.Destroy(base.gameObject);
				}
				if (this.spearHealth > 0f)
				{
					this.spearHealth = Mathf.MoveTowards(this.spearHealth, 0f, Time.deltaTime);
					return;
				}
				if (this.spearHealth <= 0f)
				{
					this.Return();
				}
			}
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0007E648 File Offset: 0x0007C848
	private void HurtEnemy(GameObject target, EnemyIdentifier eid = null)
	{
		if (eid == null)
		{
			eid = target.GetComponent<EnemyIdentifier>();
			if (!eid)
			{
				EnemyIdentifierIdentifier component = target.GetComponent<EnemyIdentifierIdentifier>();
				if (component)
				{
					eid = component.eid;
				}
			}
		}
		if (eid != null && target == null)
		{
			target = eid.gameObject;
		}
		if (!eid)
		{
			return;
		}
		eid.DeliverDamage(target, Vector3.zero, this.originPoint.position, 30f * this.damageMultiplier, false, 0f, null, false, false);
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0007E6D4 File Offset: 0x0007C8D4
	private void OnTriggerEnter(Collider other)
	{
		if (this.beenStopped)
		{
			return;
		}
		if (!this.hitPlayer && !this.hittingPlayer && other.gameObject.CompareTag("Player"))
		{
			this.hittingPlayer = true;
			this.beenStopped = true;
			this.rb.isKinematic = true;
			this.rb.useGravity = false;
			this.rb.velocity = Vector3.zero;
			base.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos();
			base.Invoke("DelayedPlayerCheck", 0.05f);
			return;
		}
		if (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
		{
			this.beenStopped = true;
			this.rb.velocity = Vector3.zero;
			this.rb.useGravity = false;
			base.transform.position += base.transform.forward * 2f;
			base.Invoke("Return", 2f / this.speedMultiplier);
			this.aud.pitch = 1f;
			this.aud.clip = this.stop;
			this.aud.Play();
			return;
		}
		if (this.target != null && this.target.isEnemy && (other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Body") || other.gameObject.CompareTag("Limb") || other.gameObject.CompareTag("EndLimb")) && !other.gameObject.CompareTag("Armor"))
		{
			EnemyIdentifierIdentifier componentInParent = other.gameObject.GetComponentInParent<EnemyIdentifierIdentifier>();
			EnemyIdentifier enemyIdentifier = null;
			if (componentInParent != null && componentInParent.eid != null)
			{
				enemyIdentifier = componentInParent.eid;
			}
			if (enemyIdentifier == null || enemyIdentifier != this.target.enemyIdentifier)
			{
				return;
			}
			if (enemyIdentifier != null)
			{
				this.HurtEnemy(other.gameObject, enemyIdentifier);
				this.Return();
			}
		}
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x0007E8EC File Offset: 0x0007CAEC
	private void DelayedPlayerCheck()
	{
		if (this.deflected)
		{
			return;
		}
		this.hittingPlayer = false;
		this.hitPlayer = true;
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.nmov.GetHurt(Mathf.RoundToInt(25f * this.damageMultiplier), true, 1f, false, false, 0.35f, false);
		this.nmov.slowMode = true;
		base.transform.position = this.nmov.transform.position;
		base.transform.SetParent(this.nmov.transform, true);
		this.rb.velocity = Vector3.zero;
		this.rb.useGravity = false;
		this.rb.isKinematic = true;
		this.beenStopped = true;
		base.GetComponent<CapsuleCollider>().radius *= 0.1f;
		this.aud.pitch = 1f;
		this.aud.clip = this.hit;
		this.aud.Play();
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0007E9F5 File Offset: 0x0007CBF5
	public void GetHurt(float damage)
	{
		Object.Instantiate<GameObject>(this.breakMetalSmall, base.transform.position, Quaternion.identity);
		this.spearHealth -= ((this.difficulty >= 4) ? (damage / 1.5f) : damage);
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x0007EA33 File Offset: 0x0007CC33
	public void Deflected()
	{
		this.deflected = true;
		this.rb.isKinematic = false;
		base.GetComponent<Collider>().enabled = false;
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0007EA54 File Offset: 0x0007CC54
	private void Return()
	{
		if (this.hitPlayer)
		{
			this.nmov.slowMode = false;
			base.transform.SetParent(null, true);
			this.rb.isKinematic = false;
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.aud.clip = this.stop;
			this.aud.pitch = 1f;
			this.aud.Play();
		}
		this.returning = true;
		this.beenStopped = true;
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x0007EAD8 File Offset: 0x0007CCD8
	private void CheckForDistance()
	{
		if (!this.returning && !this.beenStopped && !this.hitPlayer && !this.deflected)
		{
			this.returning = true;
			this.beenStopped = true;
			base.transform.position = this.originPoint.position;
		}
	}

	// Token: 0x0400165F RID: 5727
	public EnemyTarget target;

	// Token: 0x04001660 RID: 5728
	private LineRenderer lr;

	// Token: 0x04001661 RID: 5729
	private Rigidbody rb;

	// Token: 0x04001662 RID: 5730
	public bool hittingPlayer;

	// Token: 0x04001663 RID: 5731
	public bool hitPlayer;

	// Token: 0x04001664 RID: 5732
	public bool beenStopped;

	// Token: 0x04001665 RID: 5733
	private bool returning;

	// Token: 0x04001666 RID: 5734
	private bool deflected;

	// Token: 0x04001667 RID: 5735
	public Transform originPoint;

	// Token: 0x04001668 RID: 5736
	private NewMovement nmov;

	// Token: 0x04001669 RID: 5737
	public float spearHealth;

	// Token: 0x0400166A RID: 5738
	private int difficulty;

	// Token: 0x0400166B RID: 5739
	public GameObject breakMetalSmall;

	// Token: 0x0400166C RID: 5740
	private AudioSource aud;

	// Token: 0x0400166D RID: 5741
	public AudioClip hit;

	// Token: 0x0400166E RID: 5742
	public AudioClip stop;

	// Token: 0x0400166F RID: 5743
	private Mass mass;

	// Token: 0x04001670 RID: 5744
	public float speedMultiplier = 1f;

	// Token: 0x04001671 RID: 5745
	public float damageMultiplier = 1f;
}
