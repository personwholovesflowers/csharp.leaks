using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class Chainsaw : MonoBehaviour
{
	// Token: 0x0600033B RID: 827 RVA: 0x00013CBC File Offset: 0x00011EBC
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.lr = base.GetComponent<LineRenderer>();
		if (this.lineStartTransform == null)
		{
			this.lineStartTransform = this.attachedTransform;
		}
		this.ignorePlayerTimer = 0f;
		base.Invoke("SlowUpdate", 2f);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00013D27 File Offset: 0x00011F27
	private void OnEnable()
	{
		MonoSingleton<WeaponCharges>.Instance.shoSawAmount++;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00013D3B File Offset: 0x00011F3B
	private void OnDisable()
	{
		if (MonoSingleton<WeaponCharges>.Instance)
		{
			MonoSingleton<WeaponCharges>.Instance.shoSawAmount--;
		}
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00013D5B File Offset: 0x00011F5B
	private void SlowUpdate()
	{
		if (Vector3.Distance(base.transform.position, this.attachedTransform.position) > 1000f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.Invoke("SlowUpdate", 2f);
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00013D9C File Offset: 0x00011F9C
	private void Update()
	{
		this.lr.SetPosition(0, this.lineStartTransform.position);
		this.lr.SetPosition(1, base.transform.position);
		if (this.rb)
		{
			if (this.inPlayer)
			{
				base.transform.forward = MonoSingleton<CameraController>.Instance.transform.forward * -1f;
			}
			else
			{
				base.transform.LookAt(base.transform.position + (base.transform.position - this.attachedTransform.position));
			}
		}
		if (this.sameEnemyHitCooldown > 0f && !this.stopped)
		{
			this.sameEnemyHitCooldown = Mathf.MoveTowards(this.sameEnemyHitCooldown, 0f, Time.deltaTime);
			if (this.sameEnemyHitCooldown <= 0f)
			{
				this.currentHitEnemy = null;
			}
		}
		if (this.inPlayer)
		{
			base.transform.position = this.attachedTransform.position;
			this.playerHitTimer = Mathf.MoveTowards(this.playerHitTimer, 0.25f, Time.deltaTime);
			this.stoppedAud.pitch = 0.5f;
			this.stoppedAud.volume = 0.75f;
			if (this.playerHitTimer >= 0.25f && !this.beingPunched)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		if (this.hitAmount <= 1)
		{
			return;
		}
		if (this.multiHitCooldown > 0f)
		{
			this.multiHitCooldown = Mathf.MoveTowards(this.multiHitCooldown, 0f, Time.deltaTime);
		}
		else if (this.stopped)
		{
			if (!this.currentHitEnemy.dead && this.currentHitAmount > 0)
			{
				this.currentHitAmount--;
				this.DamageEnemy(this.hitTarget, this.currentHitEnemy);
			}
			if (this.currentHitEnemy.dead || this.currentHitAmount <= 0)
			{
				this.stopped = false;
				this.rb.velocity = this.originalVelocity.normalized * Mathf.Max(this.originalVelocity.magnitude, 35f);
				return;
			}
			this.multiHitCooldown = 0.05f;
		}
		if (this.stoppedAud)
		{
			if (this.stopped)
			{
				this.stoppedAud.pitch = 1.1f;
				this.stoppedAud.volume = 0.75f;
				return;
			}
			this.stoppedAud.pitch = 0.85f;
			this.stoppedAud.volume = 0.5f;
		}
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00014030 File Offset: 0x00012230
	private void FixedUpdate()
	{
		if (this.stopped)
		{
			this.rb.velocity = Vector3.zero;
			return;
		}
		if (Vector3.Dot(this.rb.velocity.normalized, (this.attachedTransform.position - base.transform.position).normalized) < 0.5f)
		{
			this.rb.velocity = Vector3.MoveTowards(this.rb.velocity, (this.attachedTransform.position - base.transform.position).normalized * 100f, Time.fixedDeltaTime * Vector3.Distance(this.attachedTransform.position, base.transform.position) * 10f);
		}
		else
		{
			this.rb.velocity = (this.attachedTransform.position - base.transform.position).normalized * Mathf.Min(100f, Mathf.MoveTowards(this.rb.velocity.magnitude, 100f, Time.fixedDeltaTime * Mathf.Max(10f, Vector3.Distance(this.attachedTransform.position, base.transform.position)) * 10f));
		}
		if (this.ignorePlayerTimer > 0.66f && !this.inPlayer && Vector3.Distance(base.transform.position, this.attachedTransform.position) < 1f)
		{
			this.TouchPlayer();
			return;
		}
		if (Physics.Raycast(base.transform.position, this.attachedTransform.position - base.transform.position, Vector3.Distance(base.transform.position, this.attachedTransform.position), LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.raycastBlockedTimer += Time.fixedDeltaTime;
		}
		else
		{
			this.raycastBlockedTimer = 0f;
		}
		if (this.raycastBlockedTimer >= 0.25f)
		{
			this.TurnIntoSawblade();
			return;
		}
		RaycastHit[] array = this.rb.SweepTestAll(this.rb.velocity.normalized, this.rb.velocity.magnitude * 5f * Time.fixedDeltaTime, QueryTriggerInteraction.Ignore);
		if (array == null || array.Length == 0)
		{
			return;
		}
		Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i].transform.gameObject;
			if (gameObject.gameObject == MonoSingleton<NewMovement>.Instance.gameObject && this.ignorePlayerTimer > 0.66f)
			{
				this.TouchPlayer();
			}
			else if ((gameObject.layer == 10 || gameObject.layer == 11) && (gameObject.gameObject.CompareTag("Head") || gameObject.gameObject.CompareTag("Body") || gameObject.gameObject.CompareTag("Limb") || gameObject.gameObject.CompareTag("EndLimb") || gameObject.gameObject.CompareTag("Enemy")))
			{
				this.TouchEnemy(gameObject.transform);
			}
			else if (LayerMaskDefaults.IsMatchingLayer(gameObject.layer, LMD.Environment) || gameObject.layer == 26 || gameObject.CompareTag("Armor"))
			{
				Breakable breakable;
				if (gameObject.TryGetComponent<Breakable>(out breakable) && breakable.weak && !breakable.specialCaseOnly)
				{
					breakable.Break();
					return;
				}
				if (SceneHelper.IsStaticEnvironment(array[i]))
				{
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(array[i], 3, 1f);
				}
				base.transform.position = array[i].point;
				this.rb.velocity = Vector3.Reflect(this.rb.velocity.normalized, array[i].normal) * (this.rb.velocity.magnitude / 2f);
				flag = true;
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.ricochetEffect, array[i].point, Quaternion.LookRotation(array[i].normal));
				AudioSource audioSource;
				if (flag2 && gameObject2.TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.enabled = false;
				}
				this.ignorePlayerTimer = 1f;
				break;
			}
		}
		if (flag)
		{
			this.CheckMultipleRicochets(false);
		}
	}

	// Token: 0x06000341 RID: 833 RVA: 0x000144FC File Offset: 0x000126FC
	private void TouchPlayer()
	{
		this.inPlayer = true;
		this.stopped = true;
		this.originalVelocity = this.rb.velocity;
		base.transform.position = MonoSingleton<NewMovement>.Instance.transform.position;
		this.model.gameObject.SetActive(false);
		this.sprite.localScale = Vector3.one * 20f;
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00014570 File Offset: 0x00012770
	private void TouchEnemy(Transform other)
	{
		if (this.hitAmount <= 1)
		{
			this.HitEnemy(other, null);
			return;
		}
		if (this.stopped)
		{
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (!other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid)
		{
			return;
		}
		if (enemyIdentifierIdentifier.eid.dead)
		{
			this.HitEnemy(other, enemyIdentifierIdentifier);
			return;
		}
		if (this.sameEnemyHitCooldown > 0f && this.currentHitEnemy != null && this.currentHitEnemy == enemyIdentifierIdentifier.eid)
		{
			return;
		}
		this.stopped = true;
		this.currentHitAmount = this.hitAmount;
		this.hitTarget = other;
		this.currentHitEnemy = enemyIdentifierIdentifier.eid;
		this.originalVelocity = this.rb.velocity;
		this.sameEnemyHitCooldown = 0.25f;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0001463C File Offset: 0x0001283C
	private void HitEnemy(Transform other, EnemyIdentifierIdentifier eidid = null)
	{
		if (!eidid && !other.TryGetComponent<EnemyIdentifierIdentifier>(out eidid))
		{
			return;
		}
		if (!eidid.eid)
		{
			return;
		}
		if (this.sameEnemyHitCooldown > 0f && this.currentHitEnemy != null && this.currentHitEnemy == eidid.eid)
		{
			return;
		}
		if (this.hitLimbs.Contains(other))
		{
			return;
		}
		if (!eidid.eid.dead)
		{
			this.sameEnemyHitCooldown = 0.25f;
			this.currentHitEnemy = eidid.eid;
			this.currentHitAmount--;
		}
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		this.aud.clip = this.enemyHitSound;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.volume = 0.2f;
		this.aud.Play();
		if (eidid && eidid.eid)
		{
			this.DamageEnemy(other, eidid.eid);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0001475C File Offset: 0x0001295C
	private void DamageEnemy(Transform other, EnemyIdentifier eid)
	{
		eid.hitter = (this.beenPunched ? "chainsawbounce" : "chainsaw");
		if (!eid.hitterWeapons.Contains(this.weaponType))
		{
			eid.hitterWeapons.Add(this.weaponType);
		}
		if (this.enemyHitParticle != null)
		{
			Object.Instantiate<GameObject>(this.enemyHitParticle, other.transform.position, Quaternion.identity).transform.localScale *= 3f;
		}
		bool dead = eid.dead;
		eid.DeliverDamage(other.gameObject, (other.transform.position - base.transform.position).normalized * 3000f, base.transform.position, this.damage, false, 0f, this.sourceWeapon, false, false);
		if (dead)
		{
			this.hitLimbs.Add(other);
		}
		if (this.heated)
		{
			Flammable componentInChildren = eid.GetComponentInChildren<Flammable>();
			if (componentInChildren != null)
			{
				componentInChildren.Burn(2f, componentInChildren.burning);
			}
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00014880 File Offset: 0x00012A80
	public void CheckMultipleRicochets(bool onStart = false)
	{
		if (!this.rb)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		bool flag = false;
		int num = 0;
		RaycastHit raycastHit;
		while (num < 3 && Physics.Raycast(base.transform.position, this.rb.velocity.normalized, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			Breakable breakable;
			if (raycastHit.transform.TryGetComponent<Breakable>(out breakable) && breakable.weak && !breakable.specialCaseOnly)
			{
				breakable.Break();
			}
			else
			{
				base.transform.position = raycastHit.point;
				this.rb.velocity = Vector3.Reflect(this.rb.velocity.normalized, raycastHit.normal) * (this.rb.velocity.magnitude / 2f);
				GameObject gameObject = Object.Instantiate<GameObject>(this.ricochetEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
				AudioSource audioSource;
				if (flag && gameObject.TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.enabled = false;
				}
				else
				{
					flag = true;
				}
				if (SceneHelper.IsStaticEnvironment(raycastHit))
				{
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(raycastHit, 3, 1f);
				}
			}
			num++;
		}
		if (onStart)
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, 1.5f, LayerMaskDefaults.Get(LMD.Enemies));
			if (array.Length != 0)
			{
				this.TouchEnemy(array[0].transform);
			}
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00014A08 File Offset: 0x00012C08
	public void GetPunched()
	{
		this.beenPunched = true;
		this.beingPunched = false;
		this.inPlayer = false;
		this.stopped = false;
		this.playerHitTimer = 0f;
		this.ignorePlayerTimer = 0f;
		this.sameEnemyHitCooldown = 0f;
		this.sprite.localScale = Vector3.one * 100f;
		this.model.gameObject.SetActive(true);
		if (this.hitAmount < 3)
		{
			this.hitAmount++;
			if (this.hitAmount == 3)
			{
				this.heated = true;
			}
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00014AAC File Offset: 0x00012CAC
	public void TurnIntoSawblade()
	{
		Nail nail = Object.Instantiate<Nail>(this.sawbladeVersion, base.transform.position, base.transform.rotation);
		nail.sourceWeapon = this.sourceWeapon;
		nail.weaponType = this.weaponType;
		nail.heated = this.heated;
		nail.rb.velocity = ((this.rb.velocity == Vector3.zero) ? base.transform.forward : (this.stopped ? this.originalVelocity : this.rb.velocity)).normalized * 105f;
		Object.Instantiate<AudioSource>(this.ropeSnapSound, base.transform.position, Quaternion.identity).volume /= 2f;
		base.gameObject.SetActive(false);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000405 RID: 1029
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x04000406 RID: 1030
	public float damage;

	// Token: 0x04000407 RID: 1031
	public Transform attachedTransform;

	// Token: 0x04000408 RID: 1032
	[HideInInspector]
	public Transform lineStartTransform;

	// Token: 0x04000409 RID: 1033
	[SerializeField]
	private AudioSource ropeSnapSound;

	// Token: 0x0400040A RID: 1034
	private AudioSource aud;

	// Token: 0x0400040B RID: 1035
	public AudioSource stoppedAud;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private GameObject ricochetEffect;

	// Token: 0x0400040D RID: 1037
	[SerializeField]
	private AudioClip enemyHitSound;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private GameObject enemyHitParticle;

	// Token: 0x0400040F RID: 1039
	private LineRenderer lr;

	// Token: 0x04000410 RID: 1040
	[HideInInspector]
	public bool stopped;

	// Token: 0x04000411 RID: 1041
	public bool heated;

	// Token: 0x04000412 RID: 1042
	public int hitAmount = 1;

	// Token: 0x04000413 RID: 1043
	private int currentHitAmount;

	// Token: 0x04000414 RID: 1044
	private Transform hitTarget;

	// Token: 0x04000415 RID: 1045
	private List<Transform> hitLimbs = new List<Transform>();

	// Token: 0x04000416 RID: 1046
	private EnemyIdentifier currentHitEnemy;

	// Token: 0x04000417 RID: 1047
	private float multiHitCooldown;

	// Token: 0x04000418 RID: 1048
	private float sameEnemyHitCooldown;

	// Token: 0x04000419 RID: 1049
	[HideInInspector]
	public Vector3 originalVelocity;

	// Token: 0x0400041A RID: 1050
	[HideInInspector]
	public bool beingPunched;

	// Token: 0x0400041B RID: 1051
	private bool beenPunched;

	// Token: 0x0400041C RID: 1052
	private bool inPlayer;

	// Token: 0x0400041D RID: 1053
	private float playerHitTimer;

	// Token: 0x0400041E RID: 1054
	private TimeSince ignorePlayerTimer;

	// Token: 0x0400041F RID: 1055
	private float raycastBlockedTimer;

	// Token: 0x04000420 RID: 1056
	[HideInInspector]
	public string weaponType;

	// Token: 0x04000421 RID: 1057
	[HideInInspector]
	public GameObject sourceWeapon;

	// Token: 0x04000422 RID: 1058
	public Nail sawbladeVersion;

	// Token: 0x04000423 RID: 1059
	[SerializeField]
	private Renderer model;

	// Token: 0x04000424 RID: 1060
	[SerializeField]
	private Transform sprite;
}
