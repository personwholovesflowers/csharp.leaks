using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class Cannonball : MonoBehaviour
{
	// Token: 0x06000328 RID: 808 RVA: 0x00013273 File Offset: 0x00011473
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.col = base.GetComponent<Collider>();
		this.instaBreakDefence = 1f;
		if (this.physicsCannonball)
		{
			MonoSingleton<ObjectTracker>.Instance.AddCannonball(this);
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x000132B0 File Offset: 0x000114B0
	private void OnDestroy()
	{
		if (this.physicsCannonball && MonoSingleton<ObjectTracker>.Instance)
		{
			MonoSingleton<ObjectTracker>.Instance.RemoveCannonball(this);
		}
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000132D4 File Offset: 0x000114D4
	private void FixedUpdate()
	{
		if (this.launched)
		{
			this.rb.velocity = base.transform.forward * this.speed;
		}
		RaycastHit raycastHit;
		if (this.physicsCannonball && this.groundHitShockwave && this.rb.velocity.magnitude > 0f && this.rb.SweepTest(this.rb.velocity.normalized, out raycastHit, this.rb.velocity.magnitude * Time.fixedDeltaTime) && LayerMaskDefaults.IsMatchingLayer(raycastHit.transform.gameObject.layer, LMD.Environment) && Vector3.Angle(raycastHit.normal, Vector3.up) < 45f)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.groundHitShockwave, raycastHit.point + raycastHit.normal * 0.1f, Quaternion.identity);
			gameObject.transform.up = raycastHit.normal;
			PhysicalShockwave physicalShockwave;
			if (gameObject.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
			{
				physicalShockwave.force = 10000f + this.rb.velocity.magnitude * 80f;
			}
			this.Break();
		}
		if (this.hitEnemies.Count > 0)
		{
			for (int i = this.hitEnemies.Count - 1; i >= 0; i--)
			{
				if (this.hitEnemies[i] == null || Vector3.Distance(base.transform.position, this.hitEnemies[i].transform.position) > 20f)
				{
					this.hitEnemies.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001349C File Offset: 0x0001169C
	public void Launch()
	{
		if (!this.launchable)
		{
			return;
		}
		this.launched = true;
		this.rb.isKinematic = false;
		this.rb.useGravity = false;
		this.col.isTrigger = true;
		this.hitEnemies.Clear();
		this.InstaBreakDefenceCancel();
		if (this.currentBounces == 1 && this.hasBounced)
		{
			this.damage += 2f;
		}
		this.currentBounces++;
		if (this.sisy)
		{
			this.sisy.GotParried();
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00013538 File Offset: 0x00011738
	public void Unlaunch(bool relaunchable = true)
	{
		this.launchable = relaunchable;
		this.launched = false;
		if (this.rb)
		{
			this.rb.isKinematic = !this.physicsCannonball;
			this.rb.useGravity = this.physicsCannonball;
			this.rb.velocity = Vector3.zero;
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00013598 File Offset: 0x00011798
	private void OnTriggerEnter(Collider other)
	{
		if (!this.ghostCollider || (this.launched && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12)))
		{
			this.Collide(other);
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000135EC File Offset: 0x000117EC
	public void Collide(Collider other)
	{
		if ((this.launched || this.canBreakBeforeLaunched) && !other.isTrigger && (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || (this.launched && other.gameObject.layer == 0 && (!other.gameObject.CompareTag("Player") || !this.col.isTrigger))))
		{
			this.Break();
			return;
		}
		if ((this.launched || this.physicsCannonball) && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12) && !this.checkingForBreak)
		{
			this.checkingForBreak = true;
			EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
			EnemyIdentifier enemyIdentifier;
			if (component && component.eid)
			{
				enemyIdentifier = component.eid;
			}
			else
			{
				enemyIdentifier = other.gameObject.GetComponent<EnemyIdentifier>();
			}
			if (enemyIdentifier && !this.hitEnemies.Contains(enemyIdentifier))
			{
				if (this.physicsCannonball && this.instaBreakDefence < 1f)
				{
					this.hitEnemies.Add(enemyIdentifier);
					return;
				}
				bool flag = true;
				if (!enemyIdentifier.dead)
				{
					flag = false;
				}
				enemyIdentifier.hitter = "cannonball";
				if (!this.physicsCannonball)
				{
					enemyIdentifier.DeliverDamage(other.gameObject, (other.transform.position - base.transform.position).normalized * 100f, base.transform.position, this.damage, true, 0f, null, false, false);
				}
				else if (this.forceMaxSpeed)
				{
					enemyIdentifier.DeliverDamage(other.gameObject, base.transform.forward.normalized * 1000f, base.transform.position, this.damage, true, 0f, null, false, false);
				}
				else if (this.rb.velocity.magnitude > 10f)
				{
					enemyIdentifier.DeliverDamage(other.gameObject, this.rb.velocity.normalized * this.rb.velocity.magnitude * 1000f, base.transform.position, Mathf.Min(this.damage, this.rb.velocity.magnitude * 0.15f), true, 0f, null, false, false);
				}
				this.hitEnemies.Add(enemyIdentifier);
				if (this.physicsCannonball && this.launched && !flag)
				{
					if (this.hasBounced)
					{
						MonoSingleton<StyleHUD>.Instance.AddPoints(150, "ultrakill.cannonballedfrombounce", this.sourceWeapon, enemyIdentifier, -1, "", "");
					}
					else
					{
						MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.cannonboost", this.sourceWeapon, enemyIdentifier, -1, "", "");
					}
				}
				if (!enemyIdentifier || enemyIdentifier.dead)
				{
					if (!flag)
					{
						this.durability--;
						if (this.durability <= 0)
						{
							this.Break();
						}
					}
					if (this.physicsCannonball && !this.launched && (!flag || other.gameObject.layer == 11))
					{
						this.Bounce();
					}
					if (enemyIdentifier)
					{
						enemyIdentifier.Explode(false);
					}
					this.checkingForBreak = false;
					return;
				}
				if (!this.physicsCannonball || this.rb.velocity.magnitude < 15f)
				{
					this.Break();
				}
				else
				{
					this.Bounce();
				}
				Sisyphus sisyphus;
				if (enemyIdentifier.enemyType == EnemyType.Sisyphus && enemyIdentifier.TryGetComponent<Sisyphus>(out sisyphus))
				{
					sisyphus.Knockdown(base.transform.position);
					return;
				}
			}
			else
			{
				this.checkingForBreak = false;
			}
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x000139C8 File Offset: 0x00011BC8
	public void Break()
	{
		if (this.sisy)
		{
			this.checkingForBreak = false;
			this.launched = false;
			this.launchable = false;
			this.rb.useGravity = true;
			this.rb.velocity = Vector3.up * 25f;
			MonoSingleton<CameraController>.Instance.CameraShake(1f);
			if (this.breakEffect)
			{
				Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, base.transform.rotation);
			}
			this.sisy.SwingStop();
			return;
		}
		if (this.broken)
		{
			return;
		}
		this.broken = true;
		if (this.breakEffect)
		{
			Object.Instantiate<GameObject>(this.breakEffect, base.transform.position, base.transform.rotation);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00013AB4 File Offset: 0x00011CB4
	private void Bounce()
	{
		if (this.currentBounces >= this.maxBounces)
		{
			this.Break();
			return;
		}
		this.instaBreakDefence = 0f;
		this.currentBounces++;
		this.durability = 99;
		this.hasBounced = true;
		this.launched = false;
		this.launchable = true;
		this.checkingForBreak = false;
		this.rb.useGravity = true;
		this.rb.velocity = Vector3.up * this.rb.velocity.magnitude * 0.15f + this.rb.velocity.normalized * -5f;
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
		if (this.bounceSound)
		{
			Object.Instantiate<AudioSource>(this.bounceSound, base.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00013BB0 File Offset: 0x00011DB0
	public void Explode()
	{
		if (this.interruptionExplosion)
		{
			Object.Instantiate<GameObject>(this.interruptionExplosion, base.transform.position, Quaternion.identity);
		}
		if (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("simpleExplosions", false))
		{
			this.breakEffect = null;
		}
		this.Break();
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00013C05 File Offset: 0x00011E05
	public void InstaBreakDefenceCancel()
	{
		this.instaBreakDefence = 1f;
	}

	// Token: 0x040003EA RID: 1002
	public bool launchable = true;

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	public bool launched;

	// Token: 0x040003EC RID: 1004
	private Rigidbody rb;

	// Token: 0x040003ED RID: 1005
	private Collider col;

	// Token: 0x040003EE RID: 1006
	[SerializeField]
	private GameObject breakEffect;

	// Token: 0x040003EF RID: 1007
	private bool checkingForBreak;

	// Token: 0x040003F0 RID: 1008
	private bool broken;

	// Token: 0x040003F1 RID: 1009
	public float damage;

	// Token: 0x040003F2 RID: 1010
	public float speed;

	// Token: 0x040003F3 RID: 1011
	public bool parry;

	// Token: 0x040003F4 RID: 1012
	[HideInInspector]
	public Sisyphus sisy;

	// Token: 0x040003F5 RID: 1013
	public bool ghostCollider;

	// Token: 0x040003F6 RID: 1014
	public bool canBreakBeforeLaunched;

	// Token: 0x040003F7 RID: 1015
	[Header("Physics Cannonball Settings")]
	public bool physicsCannonball;

	// Token: 0x040003F8 RID: 1016
	public AudioSource bounceSound;

	// Token: 0x040003F9 RID: 1017
	[HideInInspector]
	public List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();

	// Token: 0x040003FA RID: 1018
	public int maxBounces;

	// Token: 0x040003FB RID: 1019
	private int currentBounces;

	// Token: 0x040003FC RID: 1020
	[HideInInspector]
	public bool hasBounced;

	// Token: 0x040003FD RID: 1021
	[HideInInspector]
	public bool forceMaxSpeed;

	// Token: 0x040003FE RID: 1022
	public int durability = 99;

	// Token: 0x040003FF RID: 1023
	[SerializeField]
	private GameObject interruptionExplosion;

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private GameObject groundHitShockwave;

	// Token: 0x04000401 RID: 1025
	[HideInInspector]
	public GameObject sourceWeapon;

	// Token: 0x04000402 RID: 1026
	private TimeSince instaBreakDefence;
}
