using System;
using System.Collections;
using System.Collections.Generic;
using SettingsMenu.Components.Pages;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class Magnet : MonoBehaviour
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x00078E8A File Offset: 0x0007708A
	private float maxWeightFinal
	{
		get
		{
			return this.maxWeight;
		}
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x00078E94 File Offset: 0x00077094
	private void Start()
	{
		this.col = base.GetComponent<SphereCollider>();
		this.lmask |= 1024;
		this.lmask |= 2048;
		this.tb = base.GetComponentInParent<TimeBomb>();
		this.col.enabled = false;
		this.col.enabled = true;
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x00078F09 File Offset: 0x00077109
	private void OnEnable()
	{
		MonoSingleton<ObjectTracker>.Instance.AddMagnet(this);
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x00078F16 File Offset: 0x00077116
	private void OnDisable()
	{
		if (MonoSingleton<ObjectTracker>.Instance)
		{
			MonoSingleton<ObjectTracker>.Instance.RemoveMagnet(this);
		}
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x00078F30 File Offset: 0x00077130
	private void OnDestroy()
	{
		this.Launch();
		if (this.connectedMagnets.Count > 0)
		{
			for (int i = this.connectedMagnets.Count - 1; i >= 0; i--)
			{
				if (this.connectedMagnets[i] != null)
				{
					this.DisconnectMagnets(this.connectedMagnets[i]);
				}
			}
		}
		if (this.tb && this.tb.gameObject.activeInHierarchy)
		{
			Object.Destroy(this.tb.gameObject);
		}
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00078FC0 File Offset: 0x000771C0
	public void Launch()
	{
		if (this.eids.Count > 0)
		{
			for (int i = this.eids.Count - 1; i >= 0; i--)
			{
				if (this.eids[i])
				{
					this.ExitEnemy(this.eids[i]);
				}
			}
		}
		if (this.affectedRbs.Count == 0 && this.sawblades.Count == 0)
		{
			return;
		}
		List<Nail> list = new List<Nail>();
		foreach (Rigidbody rigidbody in this.sawblades)
		{
			if (rigidbody != null)
			{
				rigidbody.velocity = (base.transform.position - rigidbody.transform.position).normalized * rigidbody.velocity.magnitude;
				Nail nail;
				if (rigidbody.TryGetComponent<Nail>(out nail))
				{
					nail.MagnetRelease(this);
					if (nail.magnets.Count == 0)
					{
						list.Add(nail);
					}
				}
			}
		}
		foreach (Rigidbody rigidbody2 in this.affectedRbs)
		{
			if (rigidbody2 != null)
			{
				rigidbody2.velocity = Vector3.zero;
				if (Physics.SphereCast(new Ray(rigidbody2.transform.position, rigidbody2.transform.position - base.transform.position), 5f, out this.rhit, 100f, this.lmask))
				{
					rigidbody2.AddForce((this.rhit.point - rigidbody2.transform.position).normalized * this.strength * 10f);
				}
				else
				{
					rigidbody2.AddForce((base.transform.position - rigidbody2.transform.position).normalized * this.strength * -10f);
				}
				Nail nail2;
				if (rigidbody2.TryGetComponent<Nail>(out nail2))
				{
					nail2.MagnetRelease(this);
					if (nail2.magnets.Count == 0)
					{
						rigidbody2.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
						list.Add(nail2);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			GameObject gameObject = new GameObject("NailBurstController");
			NailBurstController nailBurstController = gameObject.AddComponent<NailBurstController>();
			nailBurstController.nails = new List<Nail>(list);
			gameObject.AddComponent<RemoveOnTime>().time = 5f;
			foreach (Nail nail3 in list)
			{
				nail3.nbc = nailBurstController;
			}
		}
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x000792E0 File Offset: 0x000774E0
	private void OnTriggerEnter(Collider other)
	{
		Magnet magnet;
		if (other.gameObject.layer == 14 && other.gameObject.CompareTag("Metal"))
		{
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			if (attachedRigidbody != null && !this.affectedRbs.Contains(attachedRigidbody))
			{
				Nail nail;
				Grenade grenade;
				if (attachedRigidbody.TryGetComponent<Nail>(out nail))
				{
					nail.MagnetCaught(this);
					if (!nail.sawblade)
					{
						this.affectedRbs.Add(attachedRigidbody);
						if (GraphicsSettings.simpleNailPhysics)
						{
							attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
						}
					}
					else if (!this.sawblades.Contains(attachedRigidbody))
					{
						this.sawblades.Add(attachedRigidbody);
					}
					if (nail.chainsaw && Vector3.Distance(base.transform.position, nail.transform.position) > 20f)
					{
						nail.transform.position = Vector3.MoveTowards(nail.transform.position, base.transform.position, Vector3.Distance(base.transform.position, nail.transform.position) - 20f);
						return;
					}
				}
				else if (attachedRigidbody.TryGetComponent<Grenade>(out grenade))
				{
					if ((this.onEnemy != null && !this.onEnemy.dead) || grenade.enemy)
					{
						if (!grenade.magnets.Contains(this))
						{
							grenade.latestEnemyMagnet = this;
							grenade.magnets.Add(this);
						}
						if (!this.rockets.Contains(attachedRigidbody))
						{
							this.rockets.Add(attachedRigidbody);
							return;
						}
					}
				}
				else
				{
					Chainsaw chainsaw;
					if (!attachedRigidbody.TryGetComponent<Chainsaw>(out chainsaw))
					{
						this.affectedRbs.Add(attachedRigidbody);
						return;
					}
					if (!this.chainsaws.Contains(attachedRigidbody))
					{
						this.chainsaws.Add(attachedRigidbody);
						return;
					}
				}
			}
		}
		else if (other.gameObject.layer == 12 || other.gameObject.layer == 11)
		{
			EnemyIdentifier component = other.gameObject.GetComponent<EnemyIdentifier>();
			if (component != null && !component.bigEnemy && !this.eids.Contains(component) && !this.ignoredEids.Contains(component))
			{
				Rigidbody component2 = component.GetComponent<Rigidbody>();
				if (component2 != null)
				{
					component2.mass /= 2f;
					this.eids.Add(component);
					this.eidRbs.Add(component2);
					return;
				}
			}
		}
		else if (other.TryGetComponent<Magnet>(out magnet) && magnet != this && !this.connectedMagnets.Contains(magnet))
		{
			this.ConnectMagnets(magnet);
		}
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x00079578 File Offset: 0x00077778
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 14 && other.gameObject.CompareTag("Metal"))
		{
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			if (attachedRigidbody != null)
			{
				if (this.affectedRbs.Contains(attachedRigidbody))
				{
					this.affectedRbs.Remove(attachedRigidbody);
					Nail nail;
					if (other.TryGetComponent<Nail>(out nail))
					{
						nail.MagnetRelease(this);
						if (nail.magnets.Count == 0)
						{
							attachedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
							return;
						}
					}
				}
				else
				{
					if (this.sawblades.Contains(attachedRigidbody))
					{
						Nail nail2;
						if (other.TryGetComponent<Nail>(out nail2))
						{
							nail2.MagnetRelease(this);
						}
						this.sawblades.Remove(attachedRigidbody);
						return;
					}
					if (this.rockets.Contains(attachedRigidbody))
					{
						Grenade grenade;
						if (other.TryGetComponent<Grenade>(out grenade) && grenade.magnets.Contains(this))
						{
							grenade.magnets.Remove(this);
						}
						this.rockets.Remove(attachedRigidbody);
						return;
					}
					if (this.chainsaws.Contains(attachedRigidbody))
					{
						this.chainsaws.Remove(attachedRigidbody);
						return;
					}
				}
			}
		}
		else if (other.gameObject.layer == 12)
		{
			EnemyIdentifier component = other.gameObject.GetComponent<EnemyIdentifier>();
			this.ExitEnemy(component);
		}
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x000796B4 File Offset: 0x000778B4
	public void ConnectMagnets(Magnet target)
	{
		if (!target.connectedMagnets.Contains(this))
		{
			target.connectedMagnets.Add(this);
		}
		if (!this.connectedMagnets.Contains(target))
		{
			this.connectedMagnets.Add(target);
		}
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x000796EA File Offset: 0x000778EA
	public void DisconnectMagnets(Magnet target)
	{
		if (target.connectedMagnets.Contains(this))
		{
			target.connectedMagnets.Remove(this);
		}
		if (this.connectedMagnets.Contains(target))
		{
			this.connectedMagnets.Remove(target);
		}
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x00079724 File Offset: 0x00077924
	public void ExitEnemy(EnemyIdentifier eid)
	{
		if (eid != null && this.eids.Contains(eid))
		{
			int num = this.eids.IndexOf(eid);
			this.eids.RemoveAt(num);
			if (this.eidRbs[num] != null)
			{
				this.eidRbs[num].mass *= 2f;
			}
			this.eidRbs.RemoveAt(num);
		}
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x000797A0 File Offset: 0x000779A0
	private void Update()
	{
		float num = 0f;
		float num2 = this.strength * Time.deltaTime;
		Vector3 position = base.transform.position;
		foreach (Rigidbody rigidbody in this.affectedRbs)
		{
			if (rigidbody != null)
			{
				Vector3 position2 = rigidbody.transform.position;
				if (Mathf.Abs(Vector3.Dot(rigidbody.velocity, position - position2)) < 1000f)
				{
					rigidbody.AddForce((position - position2) * ((this.col.radius - Vector3.Distance(position2, position)) / this.col.radius * 50f * num2));
					num += rigidbody.mass;
				}
			}
			else
			{
				this.removeRbs.Add(rigidbody);
			}
		}
		if (this.chainsaws.Count > 0)
		{
			for (int i = this.chainsaws.Count - 1; i >= 0; i--)
			{
				if (this.chainsaws[i] == null)
				{
					this.chainsaws.RemoveAt(i);
				}
				else if (Vector3.Distance(base.transform.position, this.chainsaws[i].position) < 15f && Vector3.Dot(this.chainsaws[i].position - base.transform.position, this.chainsaws[i].velocity.normalized) < 0f)
				{
					Vector3 position3 = this.chainsaws[i].transform.position;
					if (Mathf.Abs(Vector3.Dot(this.chainsaws[i].velocity, position - position3)) < 1000f)
					{
						this.chainsaws[i].AddForce((position - position3) * ((this.col.radius - Vector3.Distance(position3, position)) / this.col.radius * 50f * num2));
						num += this.chainsaws[i].mass;
					}
				}
			}
		}
		foreach (Rigidbody rigidbody2 in this.sawblades)
		{
			if (rigidbody2 != null)
			{
				num += rigidbody2.mass;
			}
			else
			{
				this.removeRbs.Add(rigidbody2);
			}
		}
		if (this.removeRbs.Count > 0)
		{
			foreach (Rigidbody rigidbody3 in this.removeRbs)
			{
				this.affectedRbs.Remove(rigidbody3);
			}
			this.removeRbs.Clear();
		}
		for (int j = this.eids.Count - 1; j >= 0; j--)
		{
			EnemyIdentifier enemyIdentifier = this.eids[j];
			Rigidbody rigidbody4 = this.eidRbs[j];
			if (enemyIdentifier != null && rigidbody4 != null && !this.ignoredEids.Contains(enemyIdentifier))
			{
				Vector3 position4 = rigidbody4.transform.position;
				if (enemyIdentifier.nailsAmount > 0 && !this.eidRbs[j].isKinematic)
				{
					enemyIdentifier.useBrakes = false;
					enemyIdentifier.pulledByMagnet = true;
					rigidbody4.AddForce((position - position4).normalized * ((this.col.radius - Vector3.Distance(position4, position)) / this.col.radius * (float)enemyIdentifier.nailsAmount * 5f * num2));
					num += rigidbody4.mass;
				}
			}
			else
			{
				this.eids.RemoveAt(j);
				this.eidRbs.RemoveAt(j);
			}
		}
		float num3 = this.maxWeightFinal * (float)(this.connectedMagnets.Count + 1);
		if (num > num3 && !PauseTimedBombs.Paused)
		{
			Object.Destroy(this.tb.gameObject);
			return;
		}
		this.tb.beeperColor = Color.Lerp(Color.green, Color.red, num / num3);
		this.tb.beeperPitch = num / num3 / 2f + 0.25f;
		this.tb.beeperSizeMultiplier = num / num3 + 1f;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x00079C78 File Offset: 0x00077E78
	public IEnumerator Zap(List<GameObject> alreadyHitObjects, float damage = 1f, GameObject sourceWeapon = null)
	{
		if (this.beenZapped)
		{
			yield break;
		}
		this.beenZapped = true;
		alreadyHitObjects.Add(base.gameObject);
		yield return new WaitForSeconds(0.25f);
		EnemyIdentifier.Zap(base.transform.position, damage / 2f, alreadyHitObjects, sourceWeapon, null, null, false);
		this.DamageMagnet(1f);
		yield return new WaitForSeconds(1f);
		this.beenZapped = false;
		yield break;
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x00079C9C File Offset: 0x00077E9C
	public void DamageMagnet(float damage)
	{
		this.health -= damage;
		if (this.health <= 0f)
		{
			Harpoon harpoon;
			if (base.transform.parent && base.transform.parent.TryGetComponent<Harpoon>(out harpoon))
			{
				Object.Destroy(harpoon.gameObject);
				return;
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040015B5 RID: 5557
	private List<Rigidbody> affectedRbs = new List<Rigidbody>();

	// Token: 0x040015B6 RID: 5558
	private List<Rigidbody> removeRbs = new List<Rigidbody>();

	// Token: 0x040015B7 RID: 5559
	private List<EnemyIdentifier> eids = new List<EnemyIdentifier>();

	// Token: 0x040015B8 RID: 5560
	private List<Rigidbody> eidRbs = new List<Rigidbody>();

	// Token: 0x040015B9 RID: 5561
	public List<EnemyIdentifier> ignoredEids = new List<EnemyIdentifier>();

	// Token: 0x040015BA RID: 5562
	public EnemyIdentifier onEnemy;

	// Token: 0x040015BB RID: 5563
	public List<Magnet> connectedMagnets = new List<Magnet>();

	// Token: 0x040015BC RID: 5564
	public List<Rigidbody> sawblades = new List<Rigidbody>();

	// Token: 0x040015BD RID: 5565
	public List<Rigidbody> rockets = new List<Rigidbody>();

	// Token: 0x040015BE RID: 5566
	public List<Rigidbody> chainsaws = new List<Rigidbody>();

	// Token: 0x040015BF RID: 5567
	private SphereCollider col;

	// Token: 0x040015C0 RID: 5568
	public float strength;

	// Token: 0x040015C1 RID: 5569
	private LayerMask lmask;

	// Token: 0x040015C2 RID: 5570
	private RaycastHit rhit;

	// Token: 0x040015C3 RID: 5571
	private bool beenZapped;

	// Token: 0x040015C4 RID: 5572
	[SerializeField]
	private float maxWeight = 10f;

	// Token: 0x040015C5 RID: 5573
	private TimeBomb tb;

	// Token: 0x040015C6 RID: 5574
	[HideInInspector]
	public float health = 3f;
}
