using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class Coin : MonoBehaviour
{
	// Token: 0x060003E8 RID: 1000 RVA: 0x000191F4 File Offset: 0x000173F4
	private void Start()
	{
		MonoSingleton<CoinList>.Instance.AddCoin(this);
		this.shud = MonoSingleton<StyleHUD>.Instance;
		this.doubled = false;
		base.Invoke("GetDeleted", 5f);
		base.Invoke("StartCheckingSpeed", 0.1f);
		base.Invoke("TripleTime", 0.35f);
		base.Invoke("TripleTimeEnd", 0.417f);
		base.Invoke("DoubleTime", 1f);
		this.rb = base.GetComponent<Rigidbody>();
		this.cols = base.GetComponents<Collider>();
		this.scol = base.GetComponent<SphereCollider>();
		Collider[] array = this.cols;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000192B0 File Offset: 0x000174B0
	private void Update()
	{
		if (!this.shot)
		{
			if (this.checkingSpeed && this.rb.velocity.magnitude < 1f)
			{
				this.timeToDelete -= Time.deltaTime * 10f;
			}
			else
			{
				this.timeToDelete = 1f;
			}
			if (this.timeToDelete <= 0f)
			{
				this.GetDeleted();
			}
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00019320 File Offset: 0x00017520
	private void OnDestroy()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<CoinList>.Instance.RemoveCoin(this);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00019350 File Offset: 0x00017550
	private void TripleTime()
	{
		if (!this.shot)
		{
			this.hitTimes = 2;
			this.doubled = true;
			if (this.currentCharge)
			{
				Object.Destroy(this.currentCharge);
			}
			if (this.flash)
			{
				this.currentCharge = Object.Instantiate<GameObject>(this.flash, base.transform.position, Quaternion.identity);
				this.currentCharge.transform.SetParent(base.transform, true);
			}
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x000193D0 File Offset: 0x000175D0
	private void TripleTimeEnd()
	{
		if (!this.shot)
		{
			this.hitTimes = 1;
			this.doubled = true;
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x000193E8 File Offset: 0x000175E8
	private void DoubleTime()
	{
		if (!this.shot)
		{
			this.hitTimes = 2;
			this.doubled = true;
			if (this.currentCharge)
			{
				Object.Destroy(this.currentCharge);
			}
			if (this.chargeEffect)
			{
				this.currentCharge = Object.Instantiate<GameObject>(this.chargeEffect, base.transform.position, base.transform.rotation);
				this.currentCharge.transform.SetParent(base.transform, true);
			}
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00019470 File Offset: 0x00017670
	public void DelayedReflectRevolver(Vector3 hitp, GameObject beam = null)
	{
		if (this.checkingSpeed)
		{
			if (this.shotByEnemy)
			{
				base.CancelInvoke("EnemyReflect");
				base.CancelInvoke("ShootAtPlayer");
				this.shotByEnemy = false;
			}
			this.ricochets++;
			base.CancelInvoke("TripleTime");
			base.CancelInvoke("TripleTimeEnd");
			base.CancelInvoke("DoubleTime");
			if (!this.ccc && this.altBeam == null)
			{
				GameObject gameObject = new GameObject();
				this.ccc = gameObject.AddComponent<CoinChainCache>();
				gameObject.AddComponent<RemoveOnTime>().time = 5f;
			}
			this.rb.isKinematic = true;
			this.shot = true;
			this.hitPoint = hitp;
			this.altBeam = beam;
			base.Invoke("ReflectRevolver", 0.1f);
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001954C File Offset: 0x0001774C
	public void ReflectRevolver()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		Vector3 vector = base.transform.position;
		this.scol.enabled = false;
		bool flag = false;
		bool flag2 = false;
		if (MonoSingleton<CoinList>.Instance.revolverCoinsList.Count > 1)
		{
			foreach (Coin coin in MonoSingleton<CoinList>.Instance.revolverCoinsList)
			{
				if (coin != this && (!coin.shot || coin.shotByEnemy))
				{
					float sqrMagnitude = (coin.transform.position - vector).sqrMagnitude;
					RaycastHit raycastHit;
					if (sqrMagnitude < num && !Physics.Raycast(base.transform.position, coin.transform.position - base.transform.position, out raycastHit, Vector3.Distance(base.transform.position, coin.transform.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						gameObject = coin.gameObject;
						num = sqrMagnitude;
					}
				}
			}
			if (gameObject != null)
			{
				flag = true;
				Coin component = gameObject.GetComponent<Coin>();
				component.power = this.power + 1f;
				component.ricochets += this.ricochets;
				if (this.quickDraw)
				{
					component.quickDraw = true;
				}
				if (component.shotByEnemy)
				{
					component.CancelInvoke("EnemyReflect");
					component.CancelInvoke("ShootAtPlayer");
					component.shotByEnemy = false;
				}
				if (this.altBeam == null)
				{
					if (this.ccc)
					{
						component.ccc = this.ccc;
					}
					else
					{
						GameObject gameObject2 = new GameObject();
						this.ccc = gameObject2.AddComponent<CoinChainCache>();
						component.ccc = this.ccc;
						gameObject2.AddComponent<RemoveOnTime>().time = 5f;
					}
					component.DelayedReflectRevolver(gameObject.transform.position, null);
					LineRenderer component2 = this.SpawnBeam().GetComponent<LineRenderer>();
					AudioSource[] components = component2.GetComponents<AudioSource>();
					if (this.hitPoint == Vector3.zero)
					{
						component2.SetPosition(0, base.transform.position);
					}
					else
					{
						component2.SetPosition(0, this.hitPoint);
					}
					component2.SetPosition(1, gameObject.transform.position);
					if (this.power > 2f)
					{
						foreach (AudioSource audioSource in components)
						{
							audioSource.pitch = 1f + (this.power - 2f) / 5f;
							audioSource.Play();
						}
					}
				}
			}
		}
		if (!flag)
		{
			List<Transform> list = new List<Transform>();
			foreach (Grenade grenade in MonoSingleton<ObjectTracker>.Instance.grenadeList)
			{
				if (!grenade.playerRiding && !grenade.enemy)
				{
					list.Add(grenade.transform);
				}
			}
			foreach (Cannonball cannonball in MonoSingleton<ObjectTracker>.Instance.cannonballList)
			{
				list.Add(cannonball.transform);
			}
			Transform transform = null;
			gameObject = null;
			num = float.PositiveInfinity;
			vector = base.transform.position;
			foreach (Transform transform2 in list)
			{
				float magnitude = (transform2.transform.position - vector).magnitude;
				RaycastHit raycastHit2;
				if (magnitude < num && Vector3.Distance(MonoSingleton<PlayerTracker>.Instance.GetPlayer().transform.position, transform2.transform.position) < 100f && !Physics.Raycast(base.transform.position, transform2.transform.position - base.transform.position, out raycastHit2, Vector3.Distance(base.transform.position, transform2.transform.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					gameObject = transform2.gameObject;
					transform = transform2;
					num = magnitude;
				}
			}
			if (gameObject != null && transform != null && !this.altBeam)
			{
				LineRenderer component3 = this.SpawnBeam().GetComponent<LineRenderer>();
				component3.GetComponents<AudioSource>();
				if (this.hitPoint == Vector3.zero)
				{
					component3.SetPosition(0, base.transform.position);
				}
				else
				{
					component3.SetPosition(0, this.hitPoint);
				}
				component3.SetPosition(1, gameObject.transform.position);
				Grenade grenade2;
				Cannonball cannonball2;
				if (transform.TryGetComponent<Grenade>(out grenade2))
				{
					grenade2.Explode(grenade2.rocket, false, !grenade2.rocket, 1f, false, null, false);
				}
				else if (transform.TryGetComponent<Cannonball>(out cannonball2))
				{
					cannonball2.Explode();
				}
			}
			if (gameObject == null)
			{
				foreach (GameObject gameObject3 in GameObject.FindGameObjectsWithTag("Enemy"))
				{
					Vector3 vector2 = gameObject3.transform.position;
					EnemyIdentifier enemyIdentifier;
					if (gameObject3.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
					{
						if (enemyIdentifier.weakPoint)
						{
							vector2 = enemyIdentifier.weakPoint.transform.position;
						}
						else if (enemyIdentifier.overrideCenter)
						{
							vector2 = enemyIdentifier.overrideCenter.position;
						}
					}
					float sqrMagnitude2 = (vector2 - vector).sqrMagnitude;
					Debug.Log(string.Format("Distance for {0}: {1}", gameObject3.name, sqrMagnitude2));
					if (sqrMagnitude2 < num)
					{
						enemyIdentifier = gameObject3.GetComponent<EnemyIdentifier>();
						if (enemyIdentifier != null && !enemyIdentifier.dead && (!this.ccc || !this.ccc.beenHit.Contains(enemyIdentifier.gameObject)) && (!enemyIdentifier.blessed || !this.ignoreBlessedEnemies))
						{
							Transform transform3;
							if (enemyIdentifier.weakPoint != null && enemyIdentifier.weakPoint.activeInHierarchy)
							{
								transform3 = enemyIdentifier.weakPoint.transform;
							}
							else
							{
								EnemyIdentifierIdentifier componentInChildren = enemyIdentifier.GetComponentInChildren<EnemyIdentifierIdentifier>();
								if (componentInChildren)
								{
									transform3 = componentInChildren.transform;
								}
								else
								{
									transform3 = enemyIdentifier.transform;
								}
							}
							RaycastHit raycastHit3;
							if (!Physics.Raycast(base.transform.position, transform3.position - base.transform.position, out raycastHit3, Vector3.Distance(base.transform.position, transform3.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)))
							{
								gameObject = gameObject3;
								num = sqrMagnitude2;
							}
							else
							{
								enemyIdentifier = null;
							}
						}
						else
						{
							enemyIdentifier = null;
						}
					}
				}
				if (gameObject != null)
				{
					if (this.eid == null)
					{
						this.eid = gameObject.GetComponent<EnemyIdentifier>();
					}
					flag2 = true;
					if (this.altBeam == null)
					{
						if (this.ccc)
						{
							this.ccc.beenHit.Add(this.eid.gameObject);
						}
						LineRenderer component4 = this.SpawnBeam().GetComponent<LineRenderer>();
						AudioSource[] components2 = component4.GetComponents<AudioSource>();
						if (this.hitPoint == Vector3.zero)
						{
							component4.SetPosition(0, base.transform.position);
						}
						else
						{
							component4.SetPosition(0, this.hitPoint);
						}
						Vector3 vector3 = Vector3.zero;
						if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy)
						{
							vector3 = this.eid.weakPoint.transform.position;
						}
						else
						{
							vector3 = this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position;
						}
						component4.SetPosition(1, vector3);
						if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy && this.eid.weakPoint.GetComponent<EnemyIdentifierIdentifier>() != null)
						{
							bool flag3 = false;
							RaycastHit raycastHit4;
							if (this.eid.enemyType == EnemyType.Streetcleaner && Physics.Raycast(base.transform.position, this.eid.weakPoint.transform.position - base.transform.position, out raycastHit4, Vector3.Distance(base.transform.position, this.eid.weakPoint.transform.position), LayerMaskDefaults.Get(LMD.Enemies)) && raycastHit4.transform != this.eid.weakPoint.transform)
							{
								EnemyIdentifierIdentifier component5 = raycastHit4.transform.GetComponent<EnemyIdentifierIdentifier>();
								if (component5 && component5.eid && component5.eid == this.eid)
								{
									this.eid.DeliverDamage(raycastHit4.transform.gameObject, (raycastHit4.transform.position - base.transform.position).normalized * 10000f, raycastHit4.transform.position, this.power, false, 1f, this.sourceWeapon, false, false);
								}
								flag3 = true;
							}
							if (!this.eid.blessed && !this.eid.puppet)
							{
								this.RicoshotPointsCheck();
								if (this.quickDraw)
								{
									this.shud.AddPoints(50, "ultrakill.quickdraw", this.sourceWeapon, this.eid, -1, "", "");
								}
							}
							this.eid.hitter = "revolver";
							if (!this.eid.hitterWeapons.Contains("revolver1"))
							{
								this.eid.hitterWeapons.Add("revolver1");
							}
							if (!flag3)
							{
								this.eid.DeliverDamage(this.eid.weakPoint, (this.eid.weakPoint.transform.position - base.transform.position).normalized * 10000f, vector3, this.power, false, 1f, this.sourceWeapon, false, false);
							}
						}
						else if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy && this.eid.weakPoint.GetComponentInChildren<Breakable>() != null)
						{
							Breakable componentInChildren2 = this.eid.weakPoint.GetComponentInChildren<Breakable>();
							this.RicoshotPointsCheck();
							if (componentInChildren2.precisionOnly)
							{
								this.shud.AddPoints(100, "ultrakill.interruption", this.sourceWeapon, this.eid, -1, "", "");
								MonoSingleton<TimeController>.Instance.ParryFlash();
								if (componentInChildren2.interruptEnemy && !componentInChildren2.interruptEnemy.blessed)
								{
									componentInChildren2.interruptEnemy.Explode(true);
								}
							}
							componentInChildren2.Break();
						}
						else
						{
							this.RicoshotPointsCheck();
							this.eid.hitter = "revolver";
							this.eid.DeliverDamage(this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, (this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position - base.transform.position).normalized * 10000f, vector3, this.power, false, 1f, this.sourceWeapon, false, false);
						}
						if (this.power > 2f)
						{
							foreach (AudioSource audioSource2 in components2)
							{
								audioSource2.pitch = 1f + (this.power - 2f) / 5f;
								audioSource2.Play();
							}
						}
						this.eid = null;
					}
				}
				else
				{
					gameObject = null;
					List<GameObject> list2 = new List<GameObject>();
					foreach (GameObject gameObject4 in GameObject.FindGameObjectsWithTag("Glass"))
					{
						list2.Add(gameObject4);
					}
					foreach (GameObject gameObject5 in GameObject.FindGameObjectsWithTag("GlassFloor"))
					{
						list2.Add(gameObject5);
					}
					if (list2.Count > 0)
					{
						gameObject = null;
						num = float.PositiveInfinity;
						vector = base.transform.position;
						foreach (GameObject gameObject6 in list2)
						{
							float sqrMagnitude3 = (gameObject6.transform.position - vector).sqrMagnitude;
							if (sqrMagnitude3 < num)
							{
								Glass componentInChildren3 = gameObject6.GetComponentInChildren<Glass>();
								if (componentInChildren3 != null && !componentInChildren3.broken && (!this.ccc || !this.ccc.beenHit.Contains(componentInChildren3.gameObject)))
								{
									Transform transform4 = gameObject6.transform;
									RaycastHit raycastHit5;
									if (!Physics.Raycast(base.transform.position, transform4.position - base.transform.position, out raycastHit5, Vector3.Distance(base.transform.position, transform4.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)) || raycastHit5.transform.gameObject.CompareTag("Glass") || raycastHit5.transform.gameObject.CompareTag("GlassFloor"))
									{
										gameObject = gameObject6;
										num = sqrMagnitude3;
									}
								}
							}
						}
						if (gameObject != null && this.altBeam == null)
						{
							gameObject.GetComponentInChildren<Glass>().Shatter();
							if (this.ccc)
							{
								this.ccc.beenHit.Add(gameObject);
							}
							LineRenderer component6 = this.SpawnBeam().GetComponent<LineRenderer>();
							if (this.power > 2f)
							{
								foreach (AudioSource audioSource3 in component6.GetComponents<AudioSource>())
								{
									audioSource3.pitch = 1f + (this.power - 2f) / 5f;
									audioSource3.Play();
								}
							}
							if (this.hitPoint == Vector3.zero)
							{
								component6.SetPosition(0, base.transform.position);
							}
							else
							{
								component6.SetPosition(0, this.hitPoint);
							}
							component6.SetPosition(1, gameObject.transform.position);
						}
					}
					if ((list2.Count == 0 || gameObject == null) && this.altBeam == null)
					{
						Vector3 normalized = Random.insideUnitSphere.normalized;
						LineRenderer component7 = this.SpawnBeam().GetComponent<LineRenderer>();
						if (this.power > 2f)
						{
							foreach (AudioSource audioSource4 in component7.GetComponents<AudioSource>())
							{
								audioSource4.pitch = 1f + (this.power - 2f) / 5f;
								audioSource4.Play();
							}
						}
						if (this.hitPoint == Vector3.zero)
						{
							component7.SetPosition(0, base.transform.position);
						}
						else
						{
							component7.SetPosition(0, this.hitPoint);
						}
						RaycastHit raycastHit6;
						if (Physics.Raycast(base.transform.position, normalized, out raycastHit6, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
						{
							component7.SetPosition(1, raycastHit6.point);
							if (SceneHelper.IsStaticEnvironment(raycastHit6))
							{
								MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(raycastHit6, 3, 1f);
							}
						}
						else
						{
							component7.SetPosition(1, base.transform.position + normalized * 1000f);
						}
					}
				}
			}
		}
		if (this.altBeam != null)
		{
			AudioSource[] components3 = Object.Instantiate<GameObject>(this.coinHitSound, base.transform.position, Quaternion.identity).GetComponents<AudioSource>();
			RevolverBeam component8 = this.altBeam.GetComponent<RevolverBeam>();
			this.altBeam.transform.position = base.transform.position;
			if (component8.beamType == BeamType.Revolver && this.hitTimes > 1 && component8.strongAlt && component8.hitAmount < 99)
			{
				component8.hitAmount++;
				component8.maxHitsPerTarget = component8.hitAmount;
			}
			if (flag2)
			{
				if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy)
				{
					this.altBeam.transform.LookAt(this.eid.weakPoint.transform.position);
				}
				else
				{
					this.altBeam.transform.LookAt(this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position);
				}
				if (!this.eid.blessed && !this.eid.puppet)
				{
					this.RicoshotPointsCheck();
					if (this.quickDraw)
					{
						this.shud.AddPoints(50, "ultrakill.quickdraw", this.sourceWeapon, this.eid, -1, "", "");
					}
				}
				if (component8.beamType == BeamType.Revolver)
				{
					this.eid.hitter = "revolver";
					if (!this.eid.hitterWeapons.Contains("revolver" + component8.gunVariation.ToString()))
					{
						this.eid.hitterWeapons.Add("revolver" + component8.gunVariation.ToString());
					}
				}
				else
				{
					this.eid.hitter = "railcannon";
					if (!this.eid.hitterWeapons.Contains("railcannon0"))
					{
						this.eid.hitterWeapons.Add("railcannon0");
					}
				}
			}
			else if (gameObject != null)
			{
				this.altBeam.transform.LookAt(gameObject.transform.position);
			}
			else
			{
				this.altBeam.transform.forward = Random.insideUnitSphere.normalized;
			}
			if (!flag)
			{
				if (component8.beamType == BeamType.Revolver && component8.hasBeenRicocheter)
				{
					if (component8.maxHitsPerTarget < (component8.strongAlt ? 4 : 3))
					{
						component8.maxHitsPerTarget = Mathf.Min(component8.maxHitsPerTarget + 2, component8.strongAlt ? 4 : 3);
					}
				}
				else
				{
					component8.addedDamage += this.power / 4f;
					component8.damage += this.power / 4f;
				}
			}
			if (this.power > 2f)
			{
				foreach (AudioSource audioSource5 in components3)
				{
					audioSource5.pitch = 1f + (this.power - 2f) / 5f;
					audioSource5.Play();
				}
			}
			this.altBeam.SetActive(true);
		}
		this.hitTimes--;
		if (this.hitTimes > 0 && this.altBeam == null)
		{
			base.Invoke("ReflectRevolver", 0.05f);
			return;
		}
		base.gameObject.SetActive(false);
		new GameObject().AddComponent<CoinCollector>().coin = base.gameObject;
		base.CancelInvoke("GetDeleted");
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001A99C File Offset: 0x00018B9C
	public void DelayedPunchflection()
	{
		if (this.checkingSpeed && (!this.shot || this.shotByEnemy))
		{
			if (this.shotByEnemy)
			{
				base.CancelInvoke("EnemyReflect");
				base.CancelInvoke("ShootAtPlayer");
				this.shotByEnemy = false;
			}
			base.CancelInvoke("TripleTime");
			base.CancelInvoke("TripleTimeEnd");
			base.CancelInvoke("DoubleTime");
			this.ricochets++;
			if (this.currentCharge)
			{
				Object.Destroy(this.currentCharge);
			}
			this.rb.isKinematic = true;
			this.shot = true;
			this.Punchflection();
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001AA4C File Offset: 0x00018C4C
	public async void Punchflection()
	{
		bool flag = false;
		bool flag2 = false;
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		GameObject gameObject2 = Object.Instantiate<GameObject>(base.gameObject, base.transform.position, Quaternion.identity);
		gameObject2.SetActive(false);
		Vector3 vector = base.transform.position;
		this.scol.enabled = false;
		foreach (GameObject gameObject3 in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			Vector3 vector2 = gameObject3.transform.position;
			EnemyIdentifier enemyIdentifier;
			if (gameObject3.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
			{
				if (enemyIdentifier.weakPoint)
				{
					vector2 = enemyIdentifier.weakPoint.transform.position;
				}
				else if (enemyIdentifier.overrideCenter)
				{
					vector2 = enemyIdentifier.overrideCenter.position;
				}
			}
			float sqrMagnitude = (vector2 - position).sqrMagnitude;
			Debug.Log(string.Format("Distance for {0}: {1}", gameObject3.name, sqrMagnitude));
			if (sqrMagnitude < num)
			{
				enemyIdentifier = gameObject3.GetComponent<EnemyIdentifier>();
				if (enemyIdentifier != null && !enemyIdentifier.dead)
				{
					Transform transform;
					if (enemyIdentifier.weakPoint != null && enemyIdentifier.weakPoint.activeInHierarchy)
					{
						transform = enemyIdentifier.weakPoint.transform;
					}
					else
					{
						transform = enemyIdentifier.GetComponentInChildren<EnemyIdentifierIdentifier>().transform;
					}
					RaycastHit raycastHit;
					if (!Physics.Raycast(base.transform.position, transform.position - base.transform.position, out raycastHit, Vector3.Distance(base.transform.position, transform.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						gameObject = gameObject3;
						num = sqrMagnitude;
					}
					else
					{
						enemyIdentifier = null;
					}
				}
				else
				{
					enemyIdentifier = null;
				}
			}
		}
		if (gameObject != null)
		{
			if (this.eid == null)
			{
				this.eid = gameObject.GetComponent<EnemyIdentifier>();
			}
			LineRenderer component = this.SpawnBeam().GetComponent<LineRenderer>();
			AudioSource[] components = component.GetComponents<AudioSource>();
			if (this.hitPoint == Vector3.zero)
			{
				component.SetPosition(0, base.transform.position);
			}
			else
			{
				component.SetPosition(0, this.hitPoint);
			}
			Vector3 vector3 = Vector3.zero;
			if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy)
			{
				vector3 = this.eid.weakPoint.transform.position;
			}
			else
			{
				vector3 = this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position;
			}
			if (this.eid.blessed)
			{
				flag2 = true;
			}
			component.SetPosition(1, vector3);
			vector = vector3;
			if (!this.eid.puppet && !this.eid.blessed)
			{
				this.shud.AddPoints(50, "ultrakill.fistfullofdollar", this.sourceWeapon, this.eid, -1, "", "");
			}
			if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy && this.eid.weakPoint.GetComponent<EnemyIdentifierIdentifier>() != null)
			{
				this.eid.hitter = "coin";
				if (!this.eid.hitterWeapons.Contains("coin"))
				{
					this.eid.hitterWeapons.Add("coin");
				}
				this.eid.DeliverDamage(this.eid.weakPoint, (this.eid.weakPoint.transform.position - base.transform.position).normalized * 10000f, vector3, this.power, false, 1f, this.sourceWeapon, false, false);
			}
			else if (this.eid.weakPoint != null && this.eid.weakPoint.activeInHierarchy)
			{
				Breakable componentInChildren = this.eid.weakPoint.GetComponentInChildren<Breakable>();
				if (componentInChildren.precisionOnly)
				{
					this.shud.AddPoints(100, "ultrakill.interruption", this.sourceWeapon, this.eid, -1, "", "");
					MonoSingleton<TimeController>.Instance.ParryFlash();
					if (componentInChildren.interruptEnemy && !componentInChildren.interruptEnemy.blessed)
					{
						componentInChildren.interruptEnemy.Explode(true);
					}
				}
				componentInChildren.Break();
			}
			else
			{
				this.eid.hitter = "coin";
				this.eid.DeliverDamage(this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, (this.eid.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position - base.transform.position).normalized * 10000f, this.hitPoint, this.power, false, 1f, this.sourceWeapon, false, false);
			}
			if (this.power > 2f)
			{
				foreach (AudioSource audioSource in components)
				{
					audioSource.pitch = 1f + (this.power - 2f) / 5f;
					audioSource.Play();
				}
				this.eid = null;
			}
		}
		else
		{
			flag = true;
			Vector3 forward = MonoSingleton<CameraController>.Instance.transform.forward;
			LineRenderer component2 = this.SpawnBeam().GetComponent<LineRenderer>();
			if (this.power > 2f)
			{
				foreach (AudioSource audioSource2 in component2.GetComponents<AudioSource>())
				{
					audioSource2.pitch = 1f + (this.power - 2f) / 5f;
					audioSource2.Play();
				}
			}
			if (this.hitPoint == Vector3.zero)
			{
				component2.SetPosition(0, base.transform.position);
			}
			else
			{
				component2.SetPosition(0, this.hitPoint);
			}
			RaycastHit raycastHit2;
			if (Physics.Raycast(MonoSingleton<CameraController>.Instance.transform.position, forward, out raycastHit2, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
			{
				component2.SetPosition(1, raycastHit2.point);
				vector = raycastHit2.point - forward;
				if (SceneHelper.IsStaticEnvironment(raycastHit2))
				{
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(raycastHit2, 3, 1f);
				}
			}
			else
			{
				component2.SetPosition(1, MonoSingleton<CameraController>.Instance.transform.position + forward * 1000f);
				Object.Destroy(gameObject2);
			}
		}
		if (gameObject2)
		{
			gameObject2.transform.position = vector;
			gameObject2.SetActive(true);
			Coin component3 = gameObject2.GetComponent<Coin>();
			if (component3)
			{
				component3.shot = false;
				if (component3.power < 5f || (!flag && !flag2))
				{
					component3.power += 1f;
				}
				gameObject2.name = "NewCoin+" + (component3.power - 2f).ToString();
			}
			Rigidbody component4 = gameObject2.GetComponent<Rigidbody>();
			if (component4)
			{
				component4.isKinematic = false;
				component4.velocity = Vector3.zero;
				component4.AddForce(Vector3.up * 25f, ForceMode.VelocityChange);
			}
		}
		base.gameObject.SetActive(false);
		new GameObject().AddComponent<CoinCollector>().coin = base.gameObject;
		base.CancelInvoke("GetDeleted");
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001AA84 File Offset: 0x00018C84
	public void Bounce()
	{
		if (this.shot)
		{
			return;
		}
		if (this.currentCharge)
		{
			Object.Destroy(this.currentCharge);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject, base.transform.position, Quaternion.identity);
		gameObject.name = "NewCoin+" + (this.power - 2f).ToString();
		gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		gameObject.transform.position = position;
		gameObject.SetActive(true);
		this.scol.enabled = false;
		this.shot = true;
		Coin component = gameObject.GetComponent<Coin>();
		if (component)
		{
			component.shot = false;
		}
		Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
		if (component2)
		{
			component2.isKinematic = false;
			component2.velocity = Vector3.zero;
			component2.AddForce(Vector3.up * 25f, ForceMode.VelocityChange);
		}
		base.gameObject.SetActive(false);
		new GameObject().AddComponent<CoinCollector>().coin = base.gameObject;
		base.CancelInvoke("GetDeleted");
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001ABA4 File Offset: 0x00018DA4
	public void DelayedEnemyReflect()
	{
		if (this.shot)
		{
			return;
		}
		this.shotByEnemy = true;
		this.wasShotByEnemy = true;
		base.CancelInvoke("TripleTime");
		base.CancelInvoke("TripleTimeEnd");
		base.CancelInvoke("DoubleTime");
		this.ricochets++;
		if (!this.ccc)
		{
			GameObject gameObject = new GameObject();
			this.ccc = gameObject.AddComponent<CoinChainCache>();
			this.ccc.target = this.customTarget;
			gameObject.AddComponent<RemoveOnTime>().time = 5f;
		}
		this.rb.isKinematic = true;
		this.shot = true;
		base.Invoke("EnemyReflect", 0.1f);
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001AC5C File Offset: 0x00018E5C
	public void EnemyReflect()
	{
		bool flag = false;
		if (MonoSingleton<CoinList>.Instance.revolverCoinsList.Count > 1)
		{
			GameObject gameObject = null;
			float num = float.PositiveInfinity;
			Vector3 position = base.transform.position;
			foreach (Coin coin in MonoSingleton<CoinList>.Instance.revolverCoinsList)
			{
				if (coin != this && !coin.shot)
				{
					float sqrMagnitude = (coin.transform.position - position).sqrMagnitude;
					RaycastHit raycastHit;
					if (sqrMagnitude < num && !Physics.Raycast(base.transform.position, coin.transform.position - base.transform.position, out raycastHit, Vector3.Distance(base.transform.position, coin.transform.position) - 0.5f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						gameObject = coin.gameObject;
						num = sqrMagnitude;
					}
				}
			}
			if (gameObject != null)
			{
				flag = true;
				Coin component = gameObject.GetComponent<Coin>();
				component.power = this.power + 1f;
				component.ricochets += this.ricochets;
				if (this.quickDraw)
				{
					component.quickDraw = true;
				}
				if (this.ccc)
				{
					component.ccc = this.ccc;
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					this.ccc = gameObject2.AddComponent<CoinChainCache>();
					component.ccc = this.ccc;
					gameObject2.AddComponent<RemoveOnTime>().time = 5f;
				}
				component.DelayedEnemyReflect();
				LineRenderer component2 = this.SpawnBeam().GetComponent<LineRenderer>();
				AudioSource[] components = component2.GetComponents<AudioSource>();
				if (this.hitPoint == Vector3.zero)
				{
					component2.SetPosition(0, base.transform.position);
				}
				else
				{
					component2.SetPosition(0, this.hitPoint);
				}
				component2.SetPosition(1, gameObject.transform.position);
				Gradient gradient = new Gradient();
				gradient.SetKeys(new GradientColorKey[]
				{
					new GradientColorKey(Color.red, 0f),
					new GradientColorKey(Color.red, 1f)
				}, new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(1f, 1f)
				});
				component2.colorGradient = gradient;
				if (this.power > 2f)
				{
					foreach (AudioSource audioSource in components)
					{
						audioSource.pitch = 1f + (this.power - 2f) / 5f;
						audioSource.Play();
					}
				}
			}
		}
		if (!flag)
		{
			base.Invoke("ShootAtPlayer", 0.5f);
			if (this.scol)
			{
				this.scol.radius = 20f;
			}
			if (this.enemyFlash)
			{
				Object.Instantiate<GameObject>(this.enemyFlash, base.transform.position, Quaternion.identity).transform.SetParent(base.transform, true);
				return;
			}
		}
		else
		{
			this.shotByEnemy = false;
			base.gameObject.SetActive(false);
			new GameObject().AddComponent<CoinCollector>().coin = base.gameObject;
			base.CancelInvoke("GetDeleted");
			this.scol.enabled = false;
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001AFF8 File Offset: 0x000191F8
	private void ShootAtPlayer()
	{
		if (this.customTarget == null && this.ccc != null)
		{
			this.customTarget = this.ccc.target;
		}
		this.scol.enabled = false;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = ((this.customTarget == null) ? MonoSingleton<CameraController>.Instance.GetDefaultPos() : this.customTarget.position);
		if (this.difficulty < 0)
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (this.difficulty <= 2)
		{
			Vector3 vector3 = ((this.customTarget == null) ? MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).normalized : this.customTarget.GetVelocity());
			vector2 -= vector3 * ((float)(3 - this.difficulty) / 1.5f);
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, vector2 - base.transform.position, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
		{
			vector = raycastHit.point;
		}
		else
		{
			vector = base.transform.position + (vector2 - base.transform.position) * 999f;
		}
		RaycastHit raycastHit2;
		if (this.customTarget == null || this.customTarget.isPlayer)
		{
			if (MonoSingleton<NewMovement>.Instance.gameObject.layer != 15 && Physics.Raycast(base.transform.position, vector2 - base.transform.position, raycastHit.distance, 4))
			{
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(7.5f * this.power), true, 1f, false, false, 0.35f, false);
			}
		}
		else if (Physics.Raycast(base.transform.position, vector2 - base.transform.position, out raycastHit2, raycastHit.distance, 1024))
		{
			EnemyIdentifierIdentifier component = raycastHit2.collider.GetComponent<EnemyIdentifierIdentifier>();
			if (component != null && component.eid == this.customTarget.enemyIdentifier)
			{
				this.customTarget.enemyIdentifier.SimpleDamage((float)Mathf.RoundToInt(7.5f * this.power));
			}
		}
		LineRenderer component2 = this.SpawnBeam().GetComponent<LineRenderer>();
		AudioSource[] components = component2.GetComponents<AudioSource>();
		if (this.hitPoint == Vector3.zero)
		{
			component2.SetPosition(0, base.transform.position);
		}
		else
		{
			component2.SetPosition(0, this.hitPoint);
		}
		component2.SetPosition(1, vector);
		Gradient gradient = new Gradient();
		gradient.SetKeys(new GradientColorKey[]
		{
			new GradientColorKey(Color.red, 0f),
			new GradientColorKey(Color.red, 1f)
		}, new GradientAlphaKey[]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		});
		component2.colorGradient = gradient;
		component2.widthMultiplier *= 2f;
		if (this.power > 2f)
		{
			foreach (AudioSource audioSource in components)
			{
				audioSource.pitch = 1f + (this.power - 2f) / 5f;
				audioSource.Play();
			}
		}
		base.gameObject.SetActive(false);
		new GameObject().AddComponent<CoinCollector>().coin = base.gameObject;
		base.CancelInvoke("GetDeleted");
		this.shotByEnemy = false;
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001B3A0 File Offset: 0x000195A0
	private void OnCollisionEnter(Collision collision)
	{
		if (LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment))
		{
			GoreZone componentInParent = collision.transform.GetComponentInParent<GoreZone>();
			if (componentInParent != null)
			{
				base.transform.SetParent(componentInParent.gibZone, true);
			}
			this.GetDeleted();
		}
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001B3F0 File Offset: 0x000195F0
	public void GetDeleted()
	{
		if (base.gameObject.activeInHierarchy)
		{
			Object.Instantiate<GameObject>(this.coinBreak, base.transform.position, Quaternion.identity);
		}
		base.GetComponentInChildren<MeshRenderer>().material = this.uselessMaterial;
		AudioLowPassFilter[] componentsInChildren = base.GetComponentsInChildren<AudioLowPassFilter>();
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			AudioLowPassFilter[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
		}
		Object.Destroy(base.GetComponent<AudioSource>());
		Object.Destroy(base.transform.GetChild(1).GetComponent<AudioSource>());
		Object.Destroy(base.GetComponent<TrailRenderer>());
		Object.Destroy(this.scol);
		Zappable zappable;
		if (base.TryGetComponent<Zappable>(out zappable))
		{
			Object.Destroy(zappable);
		}
		base.gameObject.AddComponent<RemoveOnTime>().time = 5f;
		if (this.currentCharge)
		{
			Object.Destroy(this.currentCharge);
		}
		Object.Destroy(this);
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0001B4D8 File Offset: 0x000196D8
	private void StartCheckingSpeed()
	{
		Collider[] array = this.cols;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		this.checkingSpeed = true;
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001B50A File Offset: 0x0001970A
	private GameObject SpawnBeam()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.refBeam, base.transform.position, Quaternion.identity);
		gameObject.GetComponent<RevolverBeam>().sourceWeapon = this.sourceWeapon;
		return gameObject;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001B538 File Offset: 0x00019738
	public void RicoshotPointsCheck()
	{
		string text = "";
		int num = 50;
		RevolverBeam revolverBeam;
		if (this.altBeam != null && this.altBeam.TryGetComponent<RevolverBeam>(out revolverBeam) && revolverBeam.ultraRicocheter)
		{
			text = "<color=orange>ULTRA</color>";
			num += 50;
		}
		if (this.wasShotByEnemy)
		{
			text += "<color=red>COUNTER</color>";
			num += 50;
		}
		if (this.ricochets > 1)
		{
			num += this.ricochets * 15;
		}
		StyleHUD styleHUD = this.shud;
		int num2 = num;
		string text2 = "ultrakill.ricoshot";
		string text3 = text;
		styleHUD.AddPoints(num2, text2, this.sourceWeapon, this.eid, this.ricochets, text3, "");
	}

	// Token: 0x040004D0 RID: 1232
	public EnemyTarget customTarget;

	// Token: 0x040004D1 RID: 1233
	public GameObject sourceWeapon;

	// Token: 0x040004D2 RID: 1234
	private Rigidbody rb;

	// Token: 0x040004D3 RID: 1235
	private bool checkingSpeed;

	// Token: 0x040004D4 RID: 1236
	private float timeToDelete = 1f;

	// Token: 0x040004D5 RID: 1237
	public GameObject refBeam;

	// Token: 0x040004D6 RID: 1238
	public Vector3 hitPoint = Vector3.zero;

	// Token: 0x040004D7 RID: 1239
	private Collider[] cols;

	// Token: 0x040004D8 RID: 1240
	private SphereCollider scol;

	// Token: 0x040004D9 RID: 1241
	public bool shot;

	// Token: 0x040004DA RID: 1242
	[HideInInspector]
	public bool shotByEnemy;

	// Token: 0x040004DB RID: 1243
	private bool wasShotByEnemy;

	// Token: 0x040004DC RID: 1244
	public GameObject coinBreak;

	// Token: 0x040004DD RID: 1245
	public float power;

	// Token: 0x040004DE RID: 1246
	private EnemyIdentifier eid;

	// Token: 0x040004DF RID: 1247
	public bool quickDraw;

	// Token: 0x040004E0 RID: 1248
	public Material uselessMaterial;

	// Token: 0x040004E1 RID: 1249
	private GameObject altBeam;

	// Token: 0x040004E2 RID: 1250
	public GameObject coinHitSound;

	// Token: 0x040004E3 RID: 1251
	[HideInInspector]
	public int hitTimes = 1;

	// Token: 0x040004E4 RID: 1252
	public bool doubled;

	// Token: 0x040004E5 RID: 1253
	public GameObject flash;

	// Token: 0x040004E6 RID: 1254
	public GameObject enemyFlash;

	// Token: 0x040004E7 RID: 1255
	public GameObject chargeEffect;

	// Token: 0x040004E8 RID: 1256
	private GameObject currentCharge;

	// Token: 0x040004E9 RID: 1257
	private StyleHUD shud;

	// Token: 0x040004EA RID: 1258
	public CoinChainCache ccc;

	// Token: 0x040004EB RID: 1259
	public int ricochets;

	// Token: 0x040004EC RID: 1260
	[HideInInspector]
	public int difficulty = -1;

	// Token: 0x040004ED RID: 1261
	public bool dontDestroyOnPlayerRespawn;

	// Token: 0x040004EE RID: 1262
	public bool ignoreBlessedEnemies;
}
