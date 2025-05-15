using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020004D5 RID: 1237
public class ZombieMelee : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x06001C56 RID: 7254 RVA: 0x000ED00F File Offset: 0x000EB20F
	private void Awake()
	{
		this.zmb = base.GetComponent<Zombie>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x000ED038 File Offset: 0x000EB238
	private void Start()
	{
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (this.difficulty != 2)
		{
			if (this.difficulty >= 3)
			{
				this.defaultCoolDown = 0.25f;
			}
			else if (this.difficulty == 1)
			{
				this.defaultCoolDown = 0.75f;
			}
			else if (this.difficulty == 0)
			{
				this.defaultCoolDown = 1f;
			}
		}
		if (!this.musicRequested && !this.eid.IgnorePlayer)
		{
			this.musicRequested = true;
			this.zmb.musicRequested = true;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance)
			{
				instance.PlayBattleMusic();
			}
		}
		this.ensim = base.GetComponentInChildren<EnemySimplifier>();
		this.nma = this.zmb.nma;
		this.anim = this.zmb.anim;
		this.TrackTick();
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000ED134 File Offset: 0x000EB334
	private void Update()
	{
		if (this.diving)
		{
			this.modelTransform.LookAt(base.transform.position + base.transform.forward + Vector3.up * this.rb.velocity.normalized.y * 5f);
			this.modelTransform.Rotate(Vector3.right * 90f, Space.Self);
		}
		else
		{
			this.modelTransform.localRotation = Quaternion.identity;
		}
		if (!this.diving && this.damaging)
		{
			this.rb.isKinematic = false;
			float num = 1f;
			if (this.difficulty >= 4)
			{
				num = 1.25f;
			}
			this.rb.velocity = base.transform.forward * 40f * num * this.anim.speed;
		}
		if (this.track && this.eid.target != null)
		{
			if (this.difficulty > 1)
			{
				base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
			}
			else
			{
				float num2 = 720f;
				if (this.difficulty == 0)
				{
					num2 = 360f;
				}
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z) - base.transform.position), Time.deltaTime * num2 * this.eid.totalSpeedModifier);
			}
		}
		float num3 = 3f;
		if (this.eid.target != null && !this.eid.target.isPlayer)
		{
			num3 = 4f;
		}
		if (this.coolDown == 0f)
		{
			if (this.eid.target != null && this.zmb.grounded && !this.nma.isOnOffMeshLink && !this.aboutToDive && !this.inAction)
			{
				if (this.difficulty >= 4)
				{
					float num4 = Vector3.Distance(this.eid.target.position, base.transform.position);
					if (this.eid.target.position.y > base.transform.position.y + 5f && num4 < 20f && !Physics.Raycast(base.transform.position + Vector3.up, this.eid.target.position - (base.transform.position + Vector3.up), Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.aboutToDive = true;
						base.Invoke("JumpAttack", Random.Range(0f, 0.5f));
						return;
					}
					if (num4 < num3 && !this.damaging)
					{
						this.Swing();
						return;
					}
					if (num4 < 20f && num4 > 10f && this.randomJumpChanceCooldown > 1f)
					{
						if (Random.Range(0f, 1f) > 0.8f && !Physics.Raycast(base.transform.position + Vector3.up, this.eid.target.position - (base.transform.position + Vector3.up), Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment)))
						{
							this.JumpAttack();
						}
						this.randomJumpChanceCooldown = 0f;
						return;
					}
				}
				else if (Vector3.Distance(this.eid.target.position, base.transform.position) < num3 && !this.damaging)
				{
					this.Swing();
				}
			}
			return;
		}
		if (this.coolDown - Time.deltaTime > 0f)
		{
			this.coolDown -= Time.deltaTime / 2.5f * this.eid.totalSpeedModifier;
			return;
		}
		this.coolDown = 0f;
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000ED624 File Offset: 0x000EB824
	private void OnEnable()
	{
		if (this.zmb == null)
		{
			this.zmb = base.GetComponent<Zombie>();
		}
		if (!this.musicRequested && !this.eid.IgnorePlayer)
		{
			this.musicRequested = true;
			this.zmb.musicRequested = true;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance)
			{
				instance.PlayBattleMusic();
			}
		}
		this.CancelAttack();
		if (this.zmb.grounded && this.rb)
		{
			this.rb.velocity = Vector3.zero;
			this.rb.isKinematic = true;
		}
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000ED6C4 File Offset: 0x000EB8C4
	private void OnDisable()
	{
		if (this.musicRequested && !this.eid.IgnorePlayer && !this.zmb.limp)
		{
			this.musicRequested = false;
			this.zmb.musicRequested = false;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance)
			{
				instance.PlayCleanMusic();
			}
		}
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x000ED71C File Offset: 0x000EB91C
	private void FixedUpdate()
	{
		if (this.zmb.grounded && this.nma != null && this.nma.enabled && this.nma.isOnNavMesh)
		{
			if (this.nma.isStopped || this.nma.velocity == Vector3.zero)
			{
				this.anim.SetBool("Running", false);
				return;
			}
			this.anim.SetBool("Running", true);
		}
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x000ED7A8 File Offset: 0x000EB9A8
	public void JumpAttack()
	{
		this.aboutToDive = false;
		if (this.nma.isOnOffMeshLink)
		{
			return;
		}
		this.anim.Play("JumpStart");
		this.coolDown = this.defaultCoolDown;
		this.inAction = true;
		this.zmb.stopped = true;
		this.nma.enabled = false;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000ED808 File Offset: 0x000EBA08
	public void JumpStart()
	{
		Vector3 vector = this.eid.target.position;
		if (this.eid.target.isPlayer)
		{
			vector = MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(0.5f, false, false);
		}
		base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
		this.zmb.Jump(Vector3.up * 25f + Vector3.ClampMagnitude(new Vector3((vector.x - base.transform.position.x) * 2f, 0f, (vector.z - base.transform.position.z) * 2f), 25f));
		Object.Instantiate<GameObject>(this.swingSound, base.transform);
		this.diving = true;
		this.DamageStart();
		this.zmb.ParryableCheck();
		base.Invoke("CheckThatJumpStarted", 1f);
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000ED91D File Offset: 0x000EBB1D
	private void CheckThatJumpStarted()
	{
		if (this.diving && !this.zmb.falling)
		{
			this.JumpEnd();
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x000ED93C File Offset: 0x000EBB3C
	public void JumpEnd()
	{
		base.CancelInvoke("CheckThatJumpStarted");
		this.anim.Play("JumpEnd");
		this.DamageEnd();
		this.diving = false;
		this.zmb.attacking = false;
		Object.Instantiate<GameObject>(this.hitGroundParticle, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x000ED999 File Offset: 0x000EBB99
	public void PullOut()
	{
		Object.Instantiate<GameObject>(this.pullOutParticle, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x000ED9B7 File Offset: 0x000EBBB7
	public void JumpEndEnd()
	{
		this.inAction = false;
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x000ED9C0 File Offset: 0x000EBBC0
	public void Swing()
	{
		if (this.damaging)
		{
			return;
		}
		if (this.harmless)
		{
			return;
		}
		if (this.eid.target == null)
		{
			return;
		}
		base.GetComponentInChildren<SwingCheck2>().OverrideEnemyIdentifier(this.eid);
		this.zmb.stopped = true;
		this.track = true;
		this.coolDown = this.defaultCoolDown;
		this.nma.enabled = false;
		this.anim.SetTrigger("Swing");
		Object.Instantiate<GameObject>(this.swingSound, base.transform);
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x000EDA4B File Offset: 0x000EBC4B
	public void SwingEnd()
	{
		if (this.zmb.grounded)
		{
			this.nma.enabled = true;
		}
		this.zmb.stopped = false;
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x000EDA74 File Offset: 0x000EBC74
	public void DamageStart()
	{
		if (this.harmless)
		{
			return;
		}
		this.damaging = true;
		if (this.diving)
		{
			this.diveTrail.emitting = true;
			this.diveSwingCheck.DamageStart();
			return;
		}
		this.biteTrail.enabled = true;
		this.swingCheck.DamageStart();
		this.MouthClose();
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x000EDACE File Offset: 0x000EBCCE
	public void TargetBeenHit()
	{
		this.MouthClose();
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000EDAD8 File Offset: 0x000EBCD8
	public void DamageEnd()
	{
		if (this.rb == null)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		this.damaging = false;
		this.zmb.attacking = false;
		this.rb.velocity = Vector3.zero;
		this.rb.isKinematic = true;
		this.biteTrail.enabled = false;
		this.diveTrail.emitting = false;
		this.diving = false;
		this.swingCheck.DamageStop();
		this.diveSwingCheck.DamageStop();
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x000EDB64 File Offset: 0x000EBD64
	public void StopTracking()
	{
		this.track = false;
		if (this.difficulty >= 4 && this.eid.target.isPlayer)
		{
			Vector3 vector = MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(0.2f, false, false);
			base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
		}
		this.zmb.ParryableCheck();
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x000EDBDC File Offset: 0x000EBDDC
	public void CancelAttack()
	{
		this.damaging = false;
		this.zmb.attacking = false;
		this.inAction = false;
		this.biteTrail.enabled = false;
		this.diveTrail.emitting = false;
		this.diving = false;
		this.zmb.stopped = false;
		this.track = false;
		this.coolDown = this.defaultCoolDown;
		this.swingCheck.DamageStop();
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x000EDC4C File Offset: 0x000EBE4C
	public void TrackTick()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.nma == null)
			{
				this.nma = this.zmb.nma;
			}
			if (this.zmb.grounded && !this.inAction && this.nma != null && this.nma.enabled && this.nma.isOnNavMesh && this.eid.target != null)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(this.eid.target.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.nma.SetDestination(raycastHit.point);
				}
				else
				{
					this.nma.SetDestination(this.eid.target.position);
				}
			}
		}
		base.Invoke("TrackTick", 0.1f);
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x000EDD64 File Offset: 0x000EBF64
	public void MouthClose()
	{
		if (this.eid.puppet)
		{
			return;
		}
		if (this.ensim)
		{
			this.ensim.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.biteMaterial);
		}
		base.CancelInvoke("MouthOpen");
		base.Invoke("MouthOpen", 0.75f);
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000EDDB9 File Offset: 0x000EBFB9
	private void MouthOpen()
	{
		if (this.eid.puppet)
		{
			return;
		}
		if (this.ensim)
		{
			this.ensim.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.originalMaterial);
		}
	}

	// Token: 0x04002803 RID: 10243
	public bool harmless;

	// Token: 0x04002804 RID: 10244
	public bool damaging;

	// Token: 0x04002805 RID: 10245
	public TrailRenderer biteTrail;

	// Token: 0x04002806 RID: 10246
	public TrailRenderer diveTrail;

	// Token: 0x04002807 RID: 10247
	public bool track;

	// Token: 0x04002808 RID: 10248
	public float coolDown;

	// Token: 0x04002809 RID: 10249
	public Zombie zmb;

	// Token: 0x0400280A RID: 10250
	private NavMeshAgent nma;

	// Token: 0x0400280B RID: 10251
	private Animator anim;

	// Token: 0x0400280C RID: 10252
	private EnemyIdentifier eid;

	// Token: 0x0400280D RID: 10253
	private bool customStart;

	// Token: 0x0400280E RID: 10254
	private bool musicRequested;

	// Token: 0x0400280F RID: 10255
	private int difficulty = -1;

	// Token: 0x04002810 RID: 10256
	private float defaultCoolDown = 0.5f;

	// Token: 0x04002811 RID: 10257
	public GameObject swingSound;

	// Token: 0x04002812 RID: 10258
	private Rigidbody rb;

	// Token: 0x04002813 RID: 10259
	[HideInInspector]
	public SwingCheck2 swingCheck;

	// Token: 0x04002814 RID: 10260
	[HideInInspector]
	public SwingCheck2 diveSwingCheck;

	// Token: 0x04002815 RID: 10261
	[HideInInspector]
	public bool diving;

	// Token: 0x04002816 RID: 10262
	private bool inAction;

	// Token: 0x04002817 RID: 10263
	[SerializeField]
	private Transform modelTransform;

	// Token: 0x04002818 RID: 10264
	private TimeSince randomJumpChanceCooldown;

	// Token: 0x04002819 RID: 10265
	private bool aboutToDive;

	// Token: 0x0400281A RID: 10266
	[SerializeField]
	private GameObject hitGroundParticle;

	// Token: 0x0400281B RID: 10267
	[SerializeField]
	private GameObject pullOutParticle;

	// Token: 0x0400281C RID: 10268
	private EnemySimplifier ensim;

	// Token: 0x0400281D RID: 10269
	public Material originalMaterial;

	// Token: 0x0400281E RID: 10270
	public Material biteMaterial;
}
