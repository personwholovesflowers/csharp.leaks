using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class Harpoon : MonoBehaviour
{
	// Token: 0x06000CBD RID: 3261 RVA: 0x0005E390 File Offset: 0x0005C590
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.tr = base.GetComponent<TrailRenderer>();
		this.damageLeft = this.damage;
		if (this.drill)
		{
			this.drillHitsLeft = this.drillHits;
		}
		base.Invoke("DestroyIfNotHit", 5f);
		base.Invoke("MasterDestroy", 30f);
		base.Invoke("SlowUpdate", 2f);
		this.startPosition = base.transform.position;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0005E416 File Offset: 0x0005C616
	private void SlowUpdate()
	{
		if (Vector3.Distance(this.startPosition, base.transform.position) > 999f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.Invoke("SlowUpdate", 2f);
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0005E454 File Offset: 0x0005C654
	private void Update()
	{
		if (!this.stopped && !this.punched && this.rb.velocity.magnitude > 1f)
		{
			base.transform.LookAt(base.transform.position + this.rb.velocity);
			return;
		}
		if (this.drilling)
		{
			base.transform.Rotate(Vector3.forward, 14400f * Time.deltaTime);
		}
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0005E4D8 File Offset: 0x0005C6D8
	private void FixedUpdate()
	{
		if (this.stopped && this.drilling && this.target)
		{
			if (this.drillCooldown != 0f)
			{
				this.drillCooldown = Mathf.MoveTowards(this.drillCooldown, 0f, Time.deltaTime);
				return;
			}
			this.drillCooldown = 0.05f;
			if (this.target.eid)
			{
				this.target.eid.hitter = "drill";
				this.target.eid.DeliverDamage(this.target.gameObject, Vector3.zero, base.transform.position, 0.0625f, false, 0f, this.sourceWeapon, false, false);
			}
			if (this.currentDrillSound)
			{
				this.currentDrillSound.pitch = 1.5f - (float)this.drillHitsLeft / (float)this.drillHits / 2f;
			}
			if (this.drillHitsLeft > 0)
			{
				this.drillHitsLeft--;
				return;
			}
			if (!PauseTimedBombs.Paused)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}
		else if (this.drilling && this.target == null)
		{
			this.drilling = false;
			this.DelayedDestroyIfOnCorpse(1f);
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0005E62C File Offset: 0x0005C82C
	private void OnDestroy()
	{
		if (this.target && this.target.eid && this.magnet && this.target.eid.stuckMagnets.Contains(this.magnet))
		{
			this.target.eid.stuckMagnets.Remove(this.magnet);
		}
		if (this.drill)
		{
			Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0005E6C8 File Offset: 0x0005C8C8
	private void OnEnable()
	{
		if (this.stopped && this.target && this.target.eid && this.drill)
		{
			this.target.eid.drillers.Add(this);
		}
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0005E71C File Offset: 0x0005C91C
	private void OnDisable()
	{
		if (this.stopped && this.target && this.target.eid && this.drill && this.target.eid.drillers.Contains(this))
		{
			this.target.eid.drillers.Remove(this);
		}
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0005E788 File Offset: 0x0005C988
	private void OnTriggerEnter(Collider other)
	{
		GoreZone componentInParent = other.GetComponentInParent<GoreZone>();
		if (this.hit || (other.gameObject.layer != 10 && other.gameObject.layer != 11) || (!other.gameObject.CompareTag("Armor") && !other.gameObject.CompareTag("Head") && !other.gameObject.CompareTag("Body") && !other.gameObject.CompareTag("Limb") && !other.gameObject.CompareTag("EndLimb")))
		{
			if (!this.stopped && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
			{
				if (this.drill && !this.hit)
				{
					Object.Destroy(base.gameObject);
					return;
				}
				this.stopped = true;
				this.hit = true;
				this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
				this.rb.isKinematic = true;
				if (other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Moving") || (MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Contains(other.transform)))
				{
					Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
					if (component)
					{
						base.gameObject.AddComponent<FixedJoint>().connectedBody = component;
						this.rb.isKinematic = false;
					}
					else
					{
						GameObject gameObject = new GameObject("ScaleFixer");
						gameObject.transform.position = base.transform.position;
						gameObject.transform.rotation = other.transform.rotation;
						gameObject.transform.SetParent(other.transform, true);
						base.transform.SetParent(gameObject.transform, true);
					}
					this.hit = true;
					ScrollingTexture scrollingTexture;
					if (MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Contains(other.transform) && other.transform.TryGetComponent<ScrollingTexture>(out scrollingTexture))
					{
						scrollingTexture.attachedObjects.Add(base.transform);
						BoxCollider boxCollider;
						if (base.TryGetComponent<BoxCollider>(out boxCollider))
						{
							scrollingTexture.specialScrollers.Add(new WaterDryTracker(base.transform, boxCollider.ClosestPoint(other.ClosestPoint(base.transform.position + base.transform.forward * boxCollider.size.z * base.transform.lossyScale.z)) - base.transform.position));
						}
					}
				}
				else if (componentInParent)
				{
					base.transform.SetParent(componentInParent.transform, true);
				}
				else
				{
					GoreZone[] array = Object.FindObjectsOfType<GoreZone>();
					if (array != null && array.Length != 0)
					{
						GoreZone goreZone = array[0];
						if (array.Length > 1)
						{
							for (int i = 1; i < array.Length; i++)
							{
								if (array[i].gameObject.activeInHierarchy && Vector3.Distance(goreZone.transform.position, base.transform.position) > Vector3.Distance(array[i].transform.position, base.transform.position))
								{
									goreZone = array[i];
								}
							}
						}
						base.transform.SetParent(goreZone.transform, true);
					}
				}
				if (this.aud == null)
				{
					this.aud = base.GetComponent<AudioSource>();
				}
				this.aud.clip = this.environmentHitSound;
				this.aud.pitch = Random.Range(0.9f, 1.1f);
				this.aud.volume = 0.4f;
				this.aud.Play();
				this.tr.emitting = false;
				TimeBomb component2 = base.GetComponent<TimeBomb>();
				if (component2 != null)
				{
					component2.StartCountdown();
				}
			}
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (!other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid || (this.target && this.target.eid && enemyIdentifierIdentifier.eid == this.target.eid) || (this.drill && enemyIdentifierIdentifier.eid.harpooned) || (this.magnet && enemyIdentifierIdentifier.eid.dead && enemyIdentifierIdentifier.eid.enemyType != EnemyType.MaliciousFace && enemyIdentifierIdentifier.eid.enemyType != EnemyType.Gutterman) || (this.target && enemyIdentifierIdentifier.eid == this.target.eid))
		{
			return;
		}
		this.target = enemyIdentifierIdentifier;
		this.hit = true;
		EnemyIdentifier eid = this.target.eid;
		eid.hitter = "harpoon";
		float health = eid.health;
		eid.DeliverDamage(other.gameObject, Vector3.zero, base.transform.position, this.damageLeft, false, 0f, this.sourceWeapon, false, false);
		if (this.drill)
		{
			eid.drillers.Add(this);
		}
		if (health < this.damageLeft)
		{
			this.damageLeft -= health;
		}
		if (other.gameObject.layer == 10)
		{
			this.fj = base.gameObject.AddComponent<FixedJoint>();
			this.fj.connectedBody = other.gameObject.GetComponentInParent<Rigidbody>();
			if (componentInParent != null)
			{
				base.transform.SetParent(componentInParent.transform, true);
			}
		}
		else
		{
			this.rb.velocity = Vector3.zero;
			this.rb.useGravity = false;
			this.rb.constraints = RigidbodyConstraints.FreezeAll;
			base.transform.SetParent(other.transform, true);
		}
		if (!this.magnet && eid.dead && !eid.harpooned && other.gameObject.layer == 10 && (!eid.machine || !eid.machine.specialDeath))
		{
			eid.harpooned = true;
			other.gameObject.transform.position = base.transform.position;
			Rigidbody rigidbody = this.rb;
			if (rigidbody != null)
			{
				rigidbody.AddForce(base.transform.forward, ForceMode.VelocityChange);
			}
			if (this.drill)
			{
				this.hit = false;
			}
		}
		else
		{
			this.stopped = true;
			if (this.drill)
			{
				this.drilling = true;
				this.currentDrillSound = Object.Instantiate<AudioSource>(this.drillSound, base.transform.position, base.transform.rotation);
				this.currentDrillSound.transform.SetParent(base.transform, true);
			}
			this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
			this.tr.emitting = false;
			TimeBomb component3 = base.GetComponent<TimeBomb>();
			if (component3 != null)
			{
				component3.StartCountdown();
			}
			if (this.magnet != null)
			{
				this.magnet.onEnemy = eid;
				this.magnet.ignoredEids.Add(eid);
				this.magnet.ExitEnemy(eid);
				if (eid.enemyType != EnemyType.FleshPrison && eid.enemyType != EnemyType.FleshPanopticon)
				{
					this.magnet.transform.position = other.bounds.center;
				}
				if (!eid.stuckMagnets.Contains(this.magnet))
				{
					eid.stuckMagnets.Add(this.magnet);
				}
				if (!enemyIdentifierIdentifier.eid.dead)
				{
					Breakable[] componentsInChildren = base.GetComponentsInChildren<Breakable>();
					if (componentsInChildren.Length != 0)
					{
						Breakable[] array2 = componentsInChildren;
						for (int j = 0; j < array2.Length; j++)
						{
							Object.Destroy(array2[j].gameObject);
						}
					}
				}
			}
		}
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		this.aud.clip = this.enemyHitSound;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.volume = 0.4f;
		this.aud.Play();
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0005EFBC File Offset: 0x0005D1BC
	public void Punched()
	{
		this.hit = false;
		this.stopped = false;
		this.drilling = false;
		this.punched = true;
		this.damageLeft = this.damage;
		base.CancelInvoke("DestroyIfNotHit");
		base.Invoke("DestroyIfNotHit", 5f);
		base.CancelInvoke("MasterDestroy");
		base.Invoke("MasterDestroy", 30f);
		base.CancelInvoke("DestroyIfOnCorpse");
		this.rb.isKinematic = false;
		this.rb.useGravity = false;
		this.rb.AddForce(base.transform.forward * 150f, ForceMode.VelocityChange);
		this.aud.Stop();
		this.rb.constraints = RigidbodyConstraints.None;
		base.transform.SetParent(null, true);
		if (this.tr)
		{
			this.tr.emitting = true;
		}
		if (this.target && this.target.eid)
		{
			this.target.eid.drillers.Remove(this);
			this.target.eid.hitter = "drillpunch";
			this.target.eid.DeliverDamage(this.target.gameObject, base.transform.forward * 150f, base.transform.position, 4f + (float)this.drillHitsLeft * 0.0625f, true, 0f, null, false, false);
			if (this.fj)
			{
				Object.Destroy(this.fj);
			}
			if (this.currentDrillSound)
			{
				Object.Destroy(this.currentDrillSound);
			}
		}
		this.drillHitsLeft = this.drillHits;
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0005F18C File Offset: 0x0005D38C
	private void DestroyIfNotHit()
	{
		if (!this.hit && !PauseTimedBombs.Paused)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0005F1A8 File Offset: 0x0005D3A8
	private void MasterDestroy()
	{
		if (!PauseTimedBombs.Paused && !NoWeaponCooldown.NoCooldown)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0005F1C3 File Offset: 0x0005D3C3
	public void DelayedDestroyIfOnCorpse(float delay = 1f)
	{
		base.Invoke("DestroyIfOnCorpse", delay);
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0005F1D1 File Offset: 0x0005D3D1
	private void DestroyIfOnCorpse()
	{
		if (this.target && (!this.target.eid || this.target.eid.dead))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040010E2 RID: 4322
	[SerializeField]
	private Magnet magnet;

	// Token: 0x040010E3 RID: 4323
	public bool drill;

	// Token: 0x040010E4 RID: 4324
	private bool drilling;

	// Token: 0x040010E5 RID: 4325
	private float drillCooldown;

	// Token: 0x040010E6 RID: 4326
	private bool hit;

	// Token: 0x040010E7 RID: 4327
	private bool stopped;

	// Token: 0x040010E8 RID: 4328
	private bool punched;

	// Token: 0x040010E9 RID: 4329
	public float damage;

	// Token: 0x040010EA RID: 4330
	private float damageLeft;

	// Token: 0x040010EB RID: 4331
	private AudioSource aud;

	// Token: 0x040010EC RID: 4332
	public AudioClip environmentHitSound;

	// Token: 0x040010ED RID: 4333
	public AudioClip enemyHitSound;

	// Token: 0x040010EE RID: 4334
	private Rigidbody rb;

	// Token: 0x040010EF RID: 4335
	private EnemyIdentifierIdentifier target;

	// Token: 0x040010F0 RID: 4336
	public AudioSource drillSound;

	// Token: 0x040010F1 RID: 4337
	private AudioSource currentDrillSound;

	// Token: 0x040010F2 RID: 4338
	public int drillHits;

	// Token: 0x040010F3 RID: 4339
	private int drillHitsLeft;

	// Token: 0x040010F4 RID: 4340
	private Vector3 startPosition;

	// Token: 0x040010F5 RID: 4341
	[SerializeField]
	private GameObject breakEffect;

	// Token: 0x040010F6 RID: 4342
	private FixedJoint fj;

	// Token: 0x040010F7 RID: 4343
	private TrailRenderer tr;

	// Token: 0x040010F8 RID: 4344
	[HideInInspector]
	public GameObject sourceWeapon;
}
