using System;
using System.Collections.Generic;
using plog;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class BlackHoleProjectile : MonoBehaviour
{
	// Token: 0x06000242 RID: 578 RVA: 0x0000C168 File Offset: 0x0000A368
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.bhlight = base.GetComponent<Light>();
		this.targetRange = (float)Random.Range(0, 15);
		this.aura = base.transform.GetChild(0);
		this.aud = base.GetComponent<AudioSource>();
		base.Invoke("ShootRandomLightning", Random.Range(0.5f, 1.5f));
		this.col = base.GetComponent<Collider>();
		if (this.enemy && !this.activated)
		{
			this.col.enabled = false;
		}
		if (this.target == null)
		{
			this.target = EnemyTarget.TrackPlayerIfAllowed();
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000C210 File Offset: 0x0000A410
	private void FixedUpdate()
	{
		if (!this.enemy)
		{
			if (!this.activated)
			{
				this.rb.velocity = base.transform.forward * this.speed;
				return;
			}
		}
		else if (!this.collapsing && this.activated)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(this.target.position - base.transform.position), Time.fixedDeltaTime * 10f * this.speed);
			this.rb.velocity = base.transform.forward * this.speed;
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
	private void OnDisable()
	{
		base.CancelInvoke("ShootRandomLightning");
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000C2DD File Offset: 0x0000A4DD
	private void OnEnable()
	{
		this.ShootRandomLightning();
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
	private void Update()
	{
		if (this.bhlight.range != this.targetRange)
		{
			this.bhlight.range = Mathf.MoveTowards(this.bhlight.range, this.targetRange, 100f * Time.deltaTime);
		}
		else if (this.activated)
		{
			this.targetRange = (float)Random.Range(10, 20);
		}
		else
		{
			this.targetRange = (float)Random.Range(0, 15);
		}
		if (this.activated && !this.enemy)
		{
			this.aura.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
		}
		else
		{
			this.aura.transform.localPosition = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
		}
		if (this.fadingIn)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.origScale, Time.deltaTime * this.origScale.magnitude);
			this.aud.pitch = 1f - Vector3.Distance(base.transform.localScale, this.origScale) / this.origScale.magnitude;
			if (base.transform.localScale == this.origScale)
			{
				this.aud.pitch = 1f;
				this.fadingIn = false;
			}
		}
		if (!this.collapsing)
		{
			if (this.activated)
			{
				if (!this.enemy)
				{
					this.aud.pitch += Time.deltaTime / 2f;
				}
				if (this.power < 3f)
				{
					this.power += Time.deltaTime;
				}
				if (this.caughtList.Count != 0)
				{
					List<Rigidbody> list = new List<Rigidbody>();
					foreach (Rigidbody rigidbody in this.caughtList)
					{
						if (rigidbody == null)
						{
							list.Add(rigidbody);
						}
						else
						{
							if (Vector3.Distance(rigidbody.transform.position, base.transform.position) < 9f)
							{
								rigidbody.transform.position = Vector3.MoveTowards(rigidbody.transform.position, base.transform.position, this.power * Time.deltaTime * (10f - Vector3.Distance(rigidbody.transform.position, base.transform.position)));
							}
							else
							{
								rigidbody.transform.position = Vector3.MoveTowards(rigidbody.transform.position, base.transform.position, this.power * Time.deltaTime);
							}
							if (Vector3.Distance(rigidbody.transform.position, base.transform.position) < 1f)
							{
								CharacterJoint component = rigidbody.GetComponent<CharacterJoint>();
								if (component != null)
								{
									Object.Destroy(component);
								}
								rigidbody.GetComponent<Collider>().enabled = false;
							}
							if (Vector3.Distance(rigidbody.transform.position, base.transform.position) < 0.25f)
							{
								List<Rigidbody> list2 = new List<Rigidbody>();
								list.Add(rigidbody);
								rigidbody.useGravity = false;
								rigidbody.velocity = Vector3.zero;
								rigidbody.isKinematic = true;
								rigidbody.transform.SetParent(base.transform);
								rigidbody.transform.localPosition = Vector3.zero;
								if (list2.Count != 0)
								{
									foreach (Rigidbody rigidbody2 in list2)
									{
										this.caughtList.Remove(rigidbody2);
									}
								}
								list2.Clear();
							}
						}
					}
					if (list.Count != 0)
					{
						foreach (Rigidbody rigidbody3 in list)
						{
							this.caughtList.Remove(rigidbody3);
						}
					}
				}
			}
			return;
		}
		if (this.aud.pitch > 0f)
		{
			this.aud.pitch -= Time.deltaTime;
		}
		else if (this.aud.pitch != 0f)
		{
			this.aud.pitch = 0f;
		}
		foreach (Rigidbody rigidbody4 in this.caughtList)
		{
			if (rigidbody4 != null)
			{
				rigidbody4.transform.position = base.transform.position;
			}
		}
		if (base.transform.localScale.x > 0f)
		{
			base.transform.localScale -= Vector3.one * Time.deltaTime;
			return;
		}
		this.Explode();
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000C87C File Offset: 0x0000AA7C
	private void ShootRandomLightning()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		int num = Random.Range(2, 6);
		for (int i = 0; i < num; i++)
		{
			if (Physics.Raycast(base.transform.position, Random.insideUnitSphere.normalized, out this.rhit, 8f * base.transform.localScale.x, LayerMaskDefaults.Get(LMD.Environment)))
			{
				LineRenderer component = Object.Instantiate<GameObject>(this.lightningBolt, base.transform.position, base.transform.rotation).GetComponent<LineRenderer>();
				component.SetPosition(0, base.transform.position);
				component.SetPosition(1, this.rhit.point);
				component.widthMultiplier = base.transform.localScale.x * 2f;
			}
		}
		if (!this.activated || this.enemy)
		{
			base.Invoke("ShootRandomLightning", Random.Range(0.5f, 3f));
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000C988 File Offset: 0x0000AB88
	private void ShootTargetLightning()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.shootList.Count != 0)
		{
			List<EnemyIdentifier> list = new List<EnemyIdentifier>();
			foreach (EnemyIdentifier enemyIdentifier in this.shootList)
			{
				if (enemyIdentifier == null)
				{
					list.Add(enemyIdentifier);
				}
				else if (!this.enemy || (enemyIdentifier.enemyType != this.safeType && !enemyIdentifier.immuneToFriendlyFire && !EnemyIdentifier.CheckHurtException(this.safeType, enemyIdentifier.enemyType, null)))
				{
					LineRenderer component = Object.Instantiate<GameObject>(this.lightningBolt2, base.transform.position, base.transform.rotation).GetComponent<LineRenderer>();
					component.SetPosition(0, base.transform.position);
					component.SetPosition(1, enemyIdentifier.transform.position);
					if (!this.enemy)
					{
						enemyIdentifier.hitter = "secret";
					}
					else
					{
						enemyIdentifier.hitter = "enemy";
					}
					enemyIdentifier.DeliverDamage(enemyIdentifier.gameObject, Vector3.zero, enemyIdentifier.transform.position, 1f, false, 0f, null, false, false);
					if (enemyIdentifier.dead)
					{
						list.Add(enemyIdentifier);
						foreach (Rigidbody rigidbody in enemyIdentifier.GetComponentsInChildren<Rigidbody>())
						{
							this.caughtList.Add(rigidbody);
						}
					}
				}
			}
			if (list.Count != 0)
			{
				foreach (EnemyIdentifier enemyIdentifier2 in list)
				{
					this.shootList.Remove(enemyIdentifier2);
				}
				list.Clear();
			}
		}
		if (!this.enemy)
		{
			this.ShootRandomLightning();
		}
		base.Invoke("ShootTargetLightning", 0.5f);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000CBA8 File Offset: 0x0000ADA8
	public void Activate()
	{
		if (this.fadingIn)
		{
			base.transform.localScale = this.origScale;
		}
		if (this.spawnEffect)
		{
			Object.Instantiate<GameObject>(this.spawnEffect, base.transform.position, Quaternion.identity).transform.localScale = base.transform.localScale * 5f;
		}
		this.activated = true;
		if (!this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		this.rb.velocity = Vector3.zero;
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().material = this.additive;
		base.GetComponentInChildren<ParticleSystem>().Play();
		this.ShootTargetLightning();
		if (!this.enemy)
		{
			base.Invoke("Collapse", 3f);
			return;
		}
		if (this.col)
		{
			this.col.enabled = true;
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
	private void OnTriggerEnter(Collider other)
	{
		if (!this.enemy || this.target == null)
		{
			return;
		}
		BlackHoleProjectile.Log.Info(string.Format("BlackHole OnTriggerEnter <b>{0}</b> for target: <b>{1}</b>", other.name, this.target), null, null, null);
		if (!this.target.IsTargetTransform(other.gameObject.transform))
		{
			return;
		}
		this.Explode();
		NewMovement instance = MonoSingleton<NewMovement>.Instance;
		if (instance == null)
		{
			return;
		}
		if (instance.hp > 10)
		{
			instance.GetHurt(instance.hp - 1, true, 1f, false, false, 0.35f, false);
			instance.ForceAntiHP(99f, false, false, true, false);
			return;
		}
		instance.GetHurt(10, true, 1f, false, false, 0.35f, false);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000CD60 File Offset: 0x0000AF60
	private void Collapse()
	{
		this.collapsing = true;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000CD69 File Offset: 0x0000AF69
	public void FadeIn()
	{
		if (this.origScale == Vector3.zero)
		{
			this.origScale = base.transform.localScale;
		}
		base.transform.localScale = Vector3.zero;
		this.fadingIn = true;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
	public void Explode()
	{
		BlackHoleProjectile.Log.Info("Explode. FadeIn: " + this.fadingIn.ToString(), null, null, null);
		Object.Instantiate<GameObject>(this.explosionEffect, base.transform.position, Quaternion.identity).transform.localScale = base.transform.localScale * 5f;
		foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
		{
			if ((enemyIdentifierIdentifier & enemyIdentifierIdentifier.eid) && (enemyIdentifierIdentifier.gameObject.CompareTag("EndLimb") || enemyIdentifierIdentifier.gameObject.CompareTag("Head")))
			{
				if (!this.enemy)
				{
					enemyIdentifierIdentifier.eid.hitter = "secret";
				}
				else
				{
					enemyIdentifierIdentifier.eid.hitter = "enemy";
				}
				enemyIdentifierIdentifier.eid.DeliverDamage(enemyIdentifierIdentifier.gameObject, Vector3.zero, enemyIdentifierIdentifier.gameObject.transform.position, 100f, false, 0f, null, false, false);
				if (!enemyIdentifierIdentifier.eid.exploded)
				{
					enemyIdentifierIdentifier.eid.exploded = true;
					if (!this.enemy && !enemyIdentifierIdentifier.eid.blessed && !enemyIdentifierIdentifier.eid.puppet)
					{
						if (this.scalc == null)
						{
							this.scalc = MonoSingleton<StyleCalculator>.Instance;
						}
						this.killAmount++;
						this.scalc.shud.AddPoints(50 - this.killAmount * 10, "ultrakill.compressed", null, null, -1, "", "");
						this.scalc.HitCalculator("", "", "", true, null, null);
					}
				}
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000299 RID: 665
	private static readonly global::plog.Logger Log = new global::plog.Logger("BlackHoleProjectile");

	// Token: 0x0400029A RID: 666
	public EnemyTarget target;

	// Token: 0x0400029B RID: 667
	private Rigidbody rb;

	// Token: 0x0400029C RID: 668
	public float speed;

	// Token: 0x0400029D RID: 669
	private Light bhlight;

	// Token: 0x0400029E RID: 670
	private float targetRange;

	// Token: 0x0400029F RID: 671
	private RaycastHit rhit;

	// Token: 0x040002A0 RID: 672
	private AudioSource aud;

	// Token: 0x040002A1 RID: 673
	public GameObject lightningBolt;

	// Token: 0x040002A2 RID: 674
	public GameObject lightningBolt2;

	// Token: 0x040002A3 RID: 675
	private Transform aura;

	// Token: 0x040002A4 RID: 676
	public Material additive;

	// Token: 0x040002A5 RID: 677
	private bool activated;

	// Token: 0x040002A6 RID: 678
	private bool collapsing;

	// Token: 0x040002A7 RID: 679
	private float power;

	// Token: 0x040002A8 RID: 680
	private StyleCalculator scalc;

	// Token: 0x040002A9 RID: 681
	private int killAmount;

	// Token: 0x040002AA RID: 682
	public List<EnemyIdentifier> shootList = new List<EnemyIdentifier>();

	// Token: 0x040002AB RID: 683
	private List<Rigidbody> caughtList = new List<Rigidbody>();

	// Token: 0x040002AC RID: 684
	public bool enemy;

	// Token: 0x040002AD RID: 685
	public EnemyType safeType;

	// Token: 0x040002AE RID: 686
	private Collider col;

	// Token: 0x040002AF RID: 687
	[HideInInspector]
	public bool fadingIn;

	// Token: 0x040002B0 RID: 688
	private Vector3 origScale;

	// Token: 0x040002B1 RID: 689
	public GameObject spawnEffect;

	// Token: 0x040002B2 RID: 690
	public GameObject explosionEffect;
}
