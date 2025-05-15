using System;
using System.Collections;
using Train;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class Zapper : MonoBehaviour
{
	// Token: 0x06001C2C RID: 7212 RVA: 0x000E9A1C File Offset: 0x000E7C1C
	private void Awake()
	{
		this.lr = base.GetComponent<LineRenderer>();
		this.joint = base.GetComponent<ConfigurableJoint>();
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.pulseLine = this.lightningPulseOrb.GetComponent<LineRenderer>();
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x000E9A6C File Offset: 0x000E7C6C
	private void Start()
	{
		if (this.joint)
		{
			this.joint.connectedBody = this.connectedRB;
			SoftJointLimit linearLimit = this.joint.linearLimit;
			linearLimit.limit = this.maxDistance - 5f;
			this.joint.linearLimit = linearLimit;
		}
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x000E9AC2 File Offset: 0x000E7CC2
	private void OnDisable()
	{
		if (this.attachedEnemy && !this.broken)
		{
			this.attachedEnemy.StartCoroutine(this.ZapNextFrame());
		}
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000E9AEB File Offset: 0x000E7CEB
	private IEnumerator ZapNextFrame()
	{
		yield return null;
		this.Zap();
		yield break;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000E9AFC File Offset: 0x000E7CFC
	private void Update()
	{
		this.lr.SetPosition(0, this.lineStartTransform.position);
		this.lr.SetPosition(1, base.transform.position);
		this.distance = Vector3.Distance(base.transform.position, this.connectedRB.position);
		Color color = new Color(0.5f, 0.5f, 0.5f);
		if (this.breakTimer > 0f)
		{
			color = ((this.breakTimer % 0.1f > 0.05f) ? Color.black : Color.white);
		}
		else if (this.distance > this.maxDistance - 10f)
		{
			color = Color.Lerp(Color.red, color, (this.maxDistance - this.distance) / 10f);
		}
		this.lr.startColor = color;
		this.lr.endColor = color;
		if (this.attached)
		{
			this.charge = Mathf.MoveTowards(this.charge, 5f, Time.deltaTime);
			this.aud.pitch = 1f + this.charge / 5f;
			this.lightningPulseOrb.position = Vector3.Lerp(this.lineStartTransform.position, base.transform.position, this.charge % (0.25f / (this.charge / 4f)) * this.charge);
			this.pulseLine.SetPosition(0, this.lightningPulseOrb.position);
			this.pulseLine.SetPosition(1, this.lineStartTransform.position);
			if (this.charge >= 5f || this.attachedEnemy.dead)
			{
				this.Zap();
			}
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000E9CC0 File Offset: 0x000E7EC0
	private void FixedUpdate()
	{
		if (this.attached)
		{
			this.raycastBlocked = Physics.Raycast(base.transform.position, this.connectedRB.position - base.transform.position, this.distance, LayerMaskDefaults.Get(LMD.Environment));
			if (this.distance > this.maxDistance || this.raycastBlocked)
			{
				foreach (AudioSource audioSource in this.distanceWarningSounds)
				{
					if (this.breakTimer == 0f)
					{
						audioSource.Play();
					}
					audioSource.pitch = (float)(this.raycastBlocked ? 2 : 1);
				}
				this.breakTimer = Mathf.MoveTowards(this.breakTimer, 1f, Time.fixedDeltaTime * (float)(this.raycastBlocked ? 2 : 1));
				if (this.breakTimer >= 1f)
				{
					this.Break(false);
					return;
				}
			}
			else
			{
				if (this.breakTimer != 0f)
				{
					AudioSource[] array = this.distanceWarningSounds;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Stop();
					}
				}
				this.breakTimer = 0f;
			}
		}
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000E9DE2 File Offset: 0x000E7FE2
	private void OnTriggerEnter(Collider other)
	{
		if (this.attached || this.broken)
		{
			return;
		}
		this.CheckAttach(other, Vector3.zero);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000E9E04 File Offset: 0x000E8004
	private void OnCollisionEnter(Collision other)
	{
		if (this.attached || this.broken)
		{
			return;
		}
		if (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
		{
			Tram tram;
			if (other.gameObject.CompareTag("Moving") && other.gameObject.TryGetComponent<Tram>(out tram) && tram.controller)
			{
				tram.controller.Zap();
			}
			this.Break(false);
			return;
		}
		this.CheckAttach(other.collider, other.contacts[0].point);
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000E9E94 File Offset: 0x000E8094
	private void CheckAttach(Collider other, Vector3 position)
	{
		if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
		{
			AttributeChecker attributeChecker;
			if (other.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out this.hitLimb) && this.hitLimb.eid && !this.hitLimb.eid.dead)
			{
				this.attached = true;
				this.attachedEnemy = this.hitLimb.eid;
				this.attachedEnemy.zapperer = this;
				base.transform.SetParent(other.attachedRigidbody ? other.attachedRigidbody.transform : other.transform, true);
				if (!this.attachedEnemy.bigEnemy)
				{
					base.transform.position = other.bounds.center;
				}
				else
				{
					if (position == Vector3.zero)
					{
						RaycastHit raycastHit;
						if (Physics.Raycast(base.transform.position - (other.bounds.center - base.transform.position).normalized, other.bounds.center - base.transform.position, out raycastHit, Vector3.Distance(other.bounds.center, base.transform.position) + 1f, LayerMaskDefaults.Get(LMD.Enemies)))
						{
							position = raycastHit.point;
						}
						else
						{
							position = other.bounds.center;
						}
					}
					base.transform.LookAt(position);
					base.transform.position = position;
				}
				this.rb.useGravity = false;
				this.rb.isKinematic = true;
				this.aud.Play();
				Object.Instantiate<GameObject>(this.attachSound, base.transform.position, Quaternion.identity);
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				this.lightningPulseOrb.position = this.lineStartTransform.position;
				this.lightningPulseOrb.gameObject.SetActive(true);
				this.openProngs.SetActive(false);
				this.closedProngs.SetActive(true);
				this.joint.connectedBody = null;
				Object.Destroy(this.joint);
				Ferryman ferryman;
				if (this.attachedEnemy.enemyType == EnemyType.Ferryman && this.attachedEnemy.TryGetComponent<Ferryman>(out ferryman) && ferryman.currentWindup)
				{
					ferryman.GotParried();
					return;
				}
			}
			else if (other.gameObject.TryGetComponent<AttributeChecker>(out attributeChecker) && attributeChecker.targetAttribute == HitterAttribute.Electricity)
			{
				Object.Instantiate<GameObject>(this.zapParticle, attributeChecker.transform.position, Quaternion.identity);
				attributeChecker.Activate();
			}
		}
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000EA178 File Offset: 0x000E8378
	private void Zap()
	{
		if (this.attachedEnemy)
		{
			this.attachedEnemy.hitter = "zapper";
			this.attachedEnemy.hitterAttributes.Add(HitterAttribute.Electricity);
			this.attachedEnemy.DeliverDamage(this.hitLimb.gameObject, Vector3.up * 100000f, this.broken ? this.hitLimb.transform.position : base.transform.position, this.damage, true, 0f, this.sourceWeapon, false, false);
			MonoSingleton<WeaponCharges>.Instance.naiZapperRecharge = 0f;
			foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in this.attachedEnemy.GetComponentsInChildren<EnemyIdentifierIdentifier>())
			{
				if (enemyIdentifierIdentifier != this.hitLimb && enemyIdentifierIdentifier.gameObject != this.attachedEnemy.gameObject)
				{
					this.attachedEnemy.DeliverDamage(enemyIdentifierIdentifier.gameObject, Vector3.zero, enemyIdentifierIdentifier.transform.position, Mathf.Epsilon, false, 0f, null, false, false);
				}
				Object.Instantiate<GameObject>(this.zapParticle, enemyIdentifierIdentifier.transform.position, Quaternion.identity).transform.localScale *= 0.5f;
			}
		}
		this.Break(true);
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000EA2D8 File Offset: 0x000E84D8
	public void Break(bool successful = false)
	{
		if (this.broken)
		{
			return;
		}
		this.broken = true;
		Object.Instantiate<GameObject>(this.breakParticle, base.transform.position, Quaternion.identity);
		if (this.attached && !successful)
		{
			Object.Instantiate<AudioSource>(this.cableSnap, base.transform.position, Quaternion.identity);
		}
		if (this.attachedEnemy)
		{
			this.attachedEnemy.zapperer = this;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000EA35C File Offset: 0x000E855C
	public void ChargeBoost(float amount)
	{
		this.charge += amount;
		LineRenderer lineRenderer = Object.Instantiate<LineRenderer>(MonoSingleton<DefaultReferenceManager>.Instance.electricLine, base.transform.position, Quaternion.identity);
		lineRenderer.SetPosition(0, base.transform.position);
		lineRenderer.SetPosition(1, this.lineStartTransform.position);
		Object.Instantiate<AudioSource>(this.boostSound, base.transform.position, Quaternion.identity);
		ElectricityLine electricityLine;
		if (lineRenderer.TryGetComponent<ElectricityLine>(out electricityLine))
		{
			electricityLine.minWidth = 8f;
			electricityLine.maxWidth = 15f;
		}
	}

	// Token: 0x040027B4 RID: 10164
	private LineRenderer lr;

	// Token: 0x040027B5 RID: 10165
	private Rigidbody rb;

	// Token: 0x040027B6 RID: 10166
	private AudioSource aud;

	// Token: 0x040027B7 RID: 10167
	[HideInInspector]
	public float damage = 10f;

	// Token: 0x040027B8 RID: 10168
	[HideInInspector]
	public GameObject sourceWeapon;

	// Token: 0x040027B9 RID: 10169
	public Transform lineStartTransform;

	// Token: 0x040027BA RID: 10170
	public Rigidbody connectedRB;

	// Token: 0x040027BB RID: 10171
	private ConfigurableJoint joint;

	// Token: 0x040027BC RID: 10172
	[SerializeField]
	private GameObject openProngs;

	// Token: 0x040027BD RID: 10173
	[SerializeField]
	private GameObject closedProngs;

	// Token: 0x040027BE RID: 10174
	public float maxDistance;

	// Token: 0x040027BF RID: 10175
	[HideInInspector]
	public float distance;

	// Token: 0x040027C0 RID: 10176
	[HideInInspector]
	public float charge;

	// Token: 0x040027C1 RID: 10177
	[HideInInspector]
	public float breakTimer;

	// Token: 0x040027C2 RID: 10178
	[HideInInspector]
	public bool raycastBlocked;

	// Token: 0x040027C3 RID: 10179
	private bool broken;

	// Token: 0x040027C4 RID: 10180
	public bool attached;

	// Token: 0x040027C5 RID: 10181
	public EnemyIdentifier attachedEnemy;

	// Token: 0x040027C6 RID: 10182
	public EnemyIdentifierIdentifier hitLimb;

	// Token: 0x040027C7 RID: 10183
	[SerializeField]
	private GameObject attachSound;

	// Token: 0x040027C8 RID: 10184
	[SerializeField]
	private Transform lightningPulseOrb;

	// Token: 0x040027C9 RID: 10185
	private LineRenderer pulseLine;

	// Token: 0x040027CA RID: 10186
	[SerializeField]
	private GameObject zapParticle;

	// Token: 0x040027CB RID: 10187
	[SerializeField]
	private AudioSource[] distanceWarningSounds;

	// Token: 0x040027CC RID: 10188
	[SerializeField]
	private AudioSource cableSnap;

	// Token: 0x040027CD RID: 10189
	[SerializeField]
	private AudioSource boostSound;

	// Token: 0x040027CE RID: 10190
	[SerializeField]
	private GameObject breakParticle;
}
