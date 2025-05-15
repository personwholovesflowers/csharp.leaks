using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class LeviathanController : MonoBehaviour
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0007197A File Offset: 0x0006FB7A
	// (set) Token: 0x06000F5D RID: 3933 RVA: 0x00071982 File Offset: 0x0006FB82
	[HideInInspector]
	public int difficulty
	{
		get
		{
			return this.GetDifficulty();
		}
		set
		{
			this.setDifficulty = value;
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0007198B File Offset: 0x0006FB8B
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.stat = base.GetComponent<Statue>();
		this.tail.lcon = this;
		this.head.lcon = this;
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x000719BD File Offset: 0x0006FBBD
	private void UpdateBuff()
	{
		this.head.SetSpeed();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x000719CC File Offset: 0x0006FBCC
	private int GetDifficulty()
	{
		if (this.setDifficulty < 0)
		{
			this.difficulty = ((this.eid.difficultyOverride >= 0) ? this.eid.difficultyOverride : MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 2));
		}
		return this.setDifficulty;
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00071A19 File Offset: 0x0006FC19
	private void OnDestroy()
	{
		if (this.eid.dead)
		{
			this.DeathEnd();
		}
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x00071A30 File Offset: 0x0006FC30
	private void Update()
	{
		if (this.shaking)
		{
			base.transform.localPosition = this.defaultPosition + Random.onUnitSphere * 0.5f;
		}
		if (!this.active)
		{
			return;
		}
		if (!this.tailAddPhase && this.stat.health <= this.tailAddHealth)
		{
			this.tailAddPhase = true;
			if (!this.inSubPhase)
			{
				this.BeginSubPhase();
			}
			else
			{
				this.BeginMainPhase();
			}
		}
		else if (!this.secondPhase && this.stat.health <= this.phaseChangeHealth)
		{
			this.readyForSecondPhase = true;
			this.stopTail = true;
			this.eid.totalDamageTakenMultiplier = 0.5f;
		}
		if (this.tailAddPhase && !this.tailAttacking)
		{
			this.tailTimer = Mathf.MoveTowards(this.tailTimer, 0f, Time.deltaTime);
			if (this.tailTimer <= 0f && !this.stopTail)
			{
				this.SubAttack();
			}
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00071B2C File Offset: 0x0006FD2C
	private void BeginMainPhase()
	{
		if (!this.active)
		{
			return;
		}
		if (!this.tailAddPhase)
		{
			this.inSubPhase = false;
		}
		if (this.readyForSecondPhase || this.secondPhase)
		{
			if (this.readyForSecondPhase)
			{
				this.eid.totalDamageTakenMultiplier = 1f;
				this.readyForSecondPhase = false;
				this.secondPhase = true;
				UltrakillEvent ultrakillEvent = this.onEnterSecondPhase;
				if (ultrakillEvent != null)
				{
					ultrakillEvent.Invoke("");
				}
			}
			this.head.CenterPosition();
		}
		else
		{
			this.head.ChangePosition();
		}
		this.eid.weakPoint = this.headWeakPoint.gameObject;
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00071BCB File Offset: 0x0006FDCB
	public void MainPhaseOver()
	{
		if (!this.active)
		{
			return;
		}
		if (!this.tailAddPhase)
		{
			this.BeginSubPhase();
			return;
		}
		this.BeginMainPhase();
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00071BEC File Offset: 0x0006FDEC
	public void BeginSubPhase()
	{
		if (this.inSubPhase || !this.active)
		{
			return;
		}
		if (!this.tailAddPhase)
		{
			this.eid.weakPoint = this.tailWeakPoint.gameObject;
		}
		this.inSubPhase = true;
		this.currentAttacks = 2;
		this.SubAttack();
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00071C3C File Offset: 0x0006FE3C
	private void SubAttack()
	{
		if (!this.active)
		{
			return;
		}
		this.tailAttacking = true;
		if (!BlindEnemies.Blind || this.tailAddPhase)
		{
			this.tail.ChangePosition();
			return;
		}
		this.BeginMainPhase();
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00071C70 File Offset: 0x0006FE70
	public void SubAttackOver()
	{
		if (!this.active)
		{
			return;
		}
		if (BlindEnemies.Blind)
		{
			this.currentAttacks = 0;
		}
		this.tailAttacking = false;
		if (this.tailAddPhase)
		{
			if (this.difficulty <= 2)
			{
				this.tailTimer = (10f - (float)this.difficulty * 2.5f) / this.eid.totalSpeedModifier;
			}
			return;
		}
		this.currentAttacks--;
		if (this.currentAttacks <= 0)
		{
			this.BeginMainPhase();
			return;
		}
		this.SubAttack();
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00071CF8 File Offset: 0x0006FEF8
	private void SpecialDeath()
	{
		this.headParts = this.headPartsParent.GetComponentsInChildren<Transform>();
		this.tailParts = this.tailPartsParent.GetComponentsInChildren<Transform>();
		foreach (Animator animator in base.GetComponentsInChildren<Animator>())
		{
			if (animator.gameObject != this.head.gameObject && animator.gameObject != this.tail.gameObject)
			{
				Object.Destroy(animator);
			}
		}
		this.tail.Death();
		this.head.Death();
		this.active = false;
		this.head.active = false;
		this.defaultPosition = base.transform.localPosition;
		this.shaking = true;
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00071DB8 File Offset: 0x0006FFB8
	private void ExplodeTail()
	{
		if (this.tailParts[this.currentPart] == null)
		{
			this.currentPart--;
			this.ExplodeTail();
			return;
		}
		bool flag = true;
		if (this.currentPart >= 0)
		{
			flag = this.tailParts[this.currentPart].position.y > base.transform.position.y - 5f;
			this.tailParts[this.currentPart].localScale = Vector3.zero;
			this.tailParts[this.currentPart].localPosition = Vector3.zero;
			if (flag)
			{
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, false, this.eid.sandified, this.eid.blessed, this.eid, false);
				gore.transform.position = this.tailParts[this.currentPart].position;
				gore.transform.localScale *= 2f;
				if (!this.gz)
				{
					this.gz = GoreZone.ResolveGoreZone(base.transform);
				}
				gore.transform.SetParent(this.gz.goreZone, true);
				AudioSource audioSource;
				if (gore.TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.maxDistance = 500f;
				}
				gore.SetActive(true);
			}
		}
		if (this.currentPart <= 0)
		{
			this.tail.gameObject.SetActive(false);
			this.currentPart = this.headParts.Length - 1;
			base.Invoke("ExplodeHead", 0.125f / this.eid.totalSpeedModifier);
			return;
		}
		this.currentPart = Mathf.Max(0, this.currentPart - 2);
		if (flag)
		{
			base.Invoke("ExplodeTail", 0.125f / this.eid.totalSpeedModifier);
			return;
		}
		base.Invoke("ExplodeTail", 0.025f / this.eid.totalSpeedModifier);
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x00071FAC File Offset: 0x000701AC
	private void ExplodeHead()
	{
		if (this.headParts[this.currentPart] == null)
		{
			if (this.currentPart > 0)
			{
				this.currentPart--;
				this.ExplodeHead();
				return;
			}
			base.Invoke("FinalExplosion", 0.5f / this.eid.totalSpeedModifier);
			return;
		}
		else
		{
			bool flag = true;
			if (this.currentPart >= 0)
			{
				flag = this.headParts[this.currentPart].position.y > base.transform.position.y - 5f;
				this.headParts[this.currentPart].localScale = Vector3.zero;
				this.headParts[this.currentPart].localPosition = Vector3.zero;
				if (flag)
				{
					GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, false, this.eid.sandified, this.eid.blessed, this.eid, false);
					gore.transform.position = this.headParts[this.currentPart].position;
					gore.transform.localScale *= 2f;
					if (!this.gz)
					{
						this.gz = GoreZone.ResolveGoreZone(base.transform);
					}
					gore.transform.SetParent(this.gz.goreZone, true);
					AudioSource audioSource;
					if (gore.TryGetComponent<AudioSource>(out audioSource))
					{
						audioSource.maxDistance = 500f;
					}
					gore.SetActive(true);
				}
			}
			if (this.currentPart <= 0)
			{
				base.Invoke("FinalExplosion", 0.5f / this.eid.totalSpeedModifier);
				return;
			}
			this.currentPart = Mathf.Max(0, this.currentPart - 2);
			if (flag)
			{
				base.Invoke("ExplodeHead", 0.125f / this.eid.totalSpeedModifier);
				return;
			}
			base.Invoke("ExplodeHead", 0.025f / this.eid.totalSpeedModifier);
			return;
		}
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x000721A4 File Offset: 0x000703A4
	public void FinalExplosion()
	{
		MeshRenderer[] componentsInChildren = this.head.tracker.GetComponentsInChildren<MeshRenderer>();
		GameObject gameObject = null;
		int num = 0;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] == null))
			{
				for (int j = 0; j < 3; j++)
				{
					switch (j)
					{
					case 0:
						gameObject = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, false, this.eid.sandified, this.eid.blessed, this.eid, false);
						break;
					case 1:
						gameObject = MonoSingleton<BloodsplatterManager>.Instance.GetGib(BSType.gib);
						break;
					case 2:
						gameObject = MonoSingleton<BloodsplatterManager>.Instance.GetGib((BSType)Random.Range(0, 5));
						break;
					}
					if (!(gameObject == null))
					{
						gameObject.transform.position = componentsInChildren[i].transform.position;
						gameObject.transform.localScale *= (float)((j == 0) ? 3 : 15);
						if (!this.gz)
						{
							this.gz = GoreZone.ResolveGoreZone(base.transform);
						}
						gameObject.transform.SetParent(this.gz.goreZone, true);
						gameObject.SetActive(true);
						num++;
					}
				}
			}
		}
		this.shaking = false;
		MonoSingleton<TimeController>.Instance.SlowDown(0.0001f);
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00072306 File Offset: 0x00070506
	public void DeathEnd()
	{
		UltrakillEvent ultrakillEvent = this.onDeathEnd;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.head.gameObject.SetActive(false);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00072330 File Offset: 0x00070530
	private void GotParried()
	{
		this.stat.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, 20f, 0f, this.head.tracker.position, null, false);
		this.head.GotParried();
	}

	// Token: 0x040014A3 RID: 5283
	[HideInInspector]
	public bool active = true;

	// Token: 0x040014A4 RID: 5284
	public LeviathanHead head;

	// Token: 0x040014A5 RID: 5285
	[SerializeField]
	private Transform headWeakPoint;

	// Token: 0x040014A6 RID: 5286
	public LeviathanTail tail;

	// Token: 0x040014A7 RID: 5287
	[SerializeField]
	private Transform tailWeakPoint;

	// Token: 0x040014A8 RID: 5288
	[HideInInspector]
	public EnemyIdentifier eid;

	// Token: 0x040014A9 RID: 5289
	[HideInInspector]
	public Statue stat;

	// Token: 0x040014AA RID: 5290
	public float tailAddHealth;

	// Token: 0x040014AB RID: 5291
	public float phaseChangeHealth;

	// Token: 0x040014AC RID: 5292
	public bool stopTail;

	// Token: 0x040014AD RID: 5293
	private float tailTimer;

	// Token: 0x040014AE RID: 5294
	private bool tailAttacking;

	// Token: 0x040014AF RID: 5295
	private bool inSubPhase;

	// Token: 0x040014B0 RID: 5296
	private bool tailAddPhase;

	// Token: 0x040014B1 RID: 5297
	public bool readyForSecondPhase;

	// Token: 0x040014B2 RID: 5298
	[HideInInspector]
	public bool secondPhase;

	// Token: 0x040014B3 RID: 5299
	private int currentAttacks;

	// Token: 0x040014B4 RID: 5300
	private int setDifficulty = -1;

	// Token: 0x040014B5 RID: 5301
	public UltrakillEvent onEnterSecondPhase;

	// Token: 0x040014B6 RID: 5302
	[SerializeField]
	private Transform tailPartsParent;

	// Token: 0x040014B7 RID: 5303
	[SerializeField]
	private Transform headPartsParent;

	// Token: 0x040014B8 RID: 5304
	private Transform[] tailParts;

	// Token: 0x040014B9 RID: 5305
	private Transform[] headParts;

	// Token: 0x040014BA RID: 5306
	private int currentPart;

	// Token: 0x040014BB RID: 5307
	private GoreZone gz;

	// Token: 0x040014BC RID: 5308
	private bool shaking;

	// Token: 0x040014BD RID: 5309
	private Vector3 defaultPosition;

	// Token: 0x040014BE RID: 5310
	public UltrakillEvent onDeathEnd;

	// Token: 0x040014BF RID: 5311
	public GameObject bigSplash;
}
