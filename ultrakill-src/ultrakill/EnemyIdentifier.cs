using System;
using System.Collections.Generic;
using DebugOverlays;
using plog;
using Sandbox;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000190 RID: 400
[DefaultExecutionOrder(-500)]
public class EnemyIdentifier : MonoBehaviour, IAlter, IAlterOptions<bool>, IEnemyHealthDetails
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00032F18 File Offset: 0x00031118
	[HideInInspector]
	public bool isGasolined
	{
		get
		{
			using (List<Flammable>.Enumerator enumerator = this.flammables.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.fuel > 0f)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00032F78 File Offset: 0x00031178
	private void Awake()
	{
		if (this.puppet)
		{
			this.permaPuppet = true;
		}
		this.health = 999f;
		this.InitializeReferences();
		this.ForceGetHealth();
		this.UpdateModifiers();
		if (StockMapInfo.Instance != null && StockMapInfo.Instance.forceUpdateEnemyRenderers)
		{
			SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].updateWhenOffscreen = true;
			}
		}
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00032FE9 File Offset: 0x000311E9
	private void OnEnable()
	{
		UltrakillEvent ultrakillEvent = this.onEnable;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00033000 File Offset: 0x00031200
	private void OnDisable()
	{
		UltrakillEvent ultrakillEvent = this.onEnable;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Revert();
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00033014 File Offset: 0x00031214
	private void InitializeReferences()
	{
		if (this.enemyType == EnemyType.Idol)
		{
			if (!this.idol)
			{
				this.idol = (this.idol ? this.idol : base.GetComponent<Idol>());
			}
			this.ignoredByEnemies = true;
		}
		this.relationshipLogic = base.GetComponents<IEnemyRelationshipLogic>();
		this.rb = base.GetComponent<Rigidbody>();
		this.gce = base.GetComponentInChildren<GroundCheckEnemy>(true);
		foreach (Flammable flammable in base.GetComponentsInChildren<Flammable>())
		{
			if (!this.flammables.Contains(flammable))
			{
				this.flammables.Add(flammable);
			}
			if (!flammable.fuelOnly)
			{
				this.getFireDamageMultiplier = true;
			}
		}
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x000330C8 File Offset: 0x000312C8
	public bool DestroyLimb(Transform limb, LimbDestroyType type = LimbDestroyType.Destroy)
	{
		if (this.puppet)
		{
			type = LimbDestroyType.Destroy;
		}
		CharacterJoint characterJoint;
		if (!limb.TryGetComponent<CharacterJoint>(out characterJoint))
		{
			return false;
		}
		Object.Destroy(characterJoint);
		GoreZone goreZone = this.GetGoreZone();
		if (type == LimbDestroyType.Detach)
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && limb.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				enemyIdentifierIdentifier.SetupForHellBath();
			}
			limb.transform.parent = goreZone.transform;
		}
		if (type == LimbDestroyType.LimbGibs)
		{
			foreach (CharacterJoint characterJoint2 in limb.GetComponentsInChildren<CharacterJoint>())
			{
				EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
				if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint2.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2))
				{
					enemyIdentifierIdentifier2.SetupForHellBath();
				}
				Object.Destroy(characterJoint2);
				characterJoint2.transform.parent = goreZone.transform;
			}
		}
		if (type == LimbDestroyType.Destroy)
		{
			foreach (CharacterJoint characterJoint3 in limb.GetComponentsInChildren<CharacterJoint>())
			{
				Collider collider;
				if (characterJoint3.TryGetComponent<Collider>(out collider))
				{
					Object.Destroy(characterJoint3);
					if (collider.attachedRigidbody)
					{
						Object.Destroy(collider.attachedRigidbody);
					}
					Object.Destroy(collider);
				}
				else
				{
					Object.Destroy(characterJoint3);
				}
				characterJoint3.transform.localScale = Vector3.zero;
				characterJoint3.gameObject.SetActive(false);
			}
		}
		if (type != LimbDestroyType.Detach)
		{
			limb.localScale = Vector3.zero;
			limb.gameObject.SetActive(false);
		}
		return true;
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0003321C File Offset: 0x0003141C
	public bool IsTypeFriendly(EnemyIdentifier owner)
	{
		return this.enemyType == owner.enemyType || (this.enemyClass == EnemyClass.Husk && owner.enemyClass == EnemyClass.Husk);
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00033244 File Offset: 0x00031444
	private GoreZone GetGoreZone()
	{
		if (this.gz)
		{
			return this.gz;
		}
		Transform transform = base.transform;
		if (this.enemyType == EnemyType.Cerberus)
		{
			transform = transform.parent;
		}
		this.gz = GoreZone.ResolveGoreZone(transform);
		return this.gz;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00033290 File Offset: 0x00031490
	public void ForceGetHealth()
	{
		if (this.enemyType == EnemyType.Drone || this.enemyType == EnemyType.Virtue)
		{
			if (!this.drone)
			{
				this.drone = base.GetComponent<Drone>();
			}
			if (this.drone)
			{
				this.health = this.drone.health;
				return;
			}
		}
		else if (this.enemyType == EnemyType.MaliciousFace)
		{
			if (!this.spider)
			{
				this.spider = base.GetComponent<SpiderBody>();
			}
			if (this.spider)
			{
				this.health = this.spider.health;
				return;
			}
		}
		else
		{
			switch (this.enemyClass)
			{
			case EnemyClass.Husk:
				if (!this.zombie)
				{
					this.zombie = base.GetComponent<Zombie>();
				}
				if (this.zombie)
				{
					this.health = this.zombie.health;
					return;
				}
				break;
			case EnemyClass.Machine:
				if (!this.machine)
				{
					this.machine = base.GetComponent<Machine>();
				}
				if (this.machine)
				{
					this.health = this.machine.health;
				}
				break;
			case EnemyClass.Demon:
				if (!this.statue)
				{
					this.statue = base.GetComponent<Statue>();
				}
				if (this.statue)
				{
					this.health = this.statue.health;
					return;
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000333F4 File Offset: 0x000315F4
	private void Start()
	{
		if (OptionsManager.forcePuppet)
		{
			this.puppet = true;
		}
		if (!this.dontUnlockBestiary)
		{
			MonoSingleton<BestiaryData>.Instance.SetEnemy(this.enemyType, 1);
		}
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		if (this.checkingSpawnStatus)
		{
			if (!this.dead)
			{
				if (OptionsManager.forceBossBars)
				{
					this.BossBar(true);
				}
				if (this.puppet)
				{
					this.PuppetSpawn();
				}
				if ((this.sandified || OptionsManager.forceSand) && this.enemyType != EnemyType.Stalker)
				{
					this.Sandify(true);
				}
				if (this.blessed)
				{
					this.Bless(true);
				}
				if (this.speedBuff || this.damageBuff || this.healthBuff || OptionsManager.forceRadiance)
				{
					if (this.speedBuff)
					{
						this.speedBuffRequests++;
					}
					if (this.damageBuff)
					{
						this.damageBuffRequests++;
					}
					if (this.healthBuff)
					{
						this.healthBuffRequests++;
					}
					this.UpdateBuffs(false, this.spawnIn);
				}
			}
			this.checkingSpawnStatus = false;
		}
		if (this.spawnIn && this.spawnEffect && !this.puppet)
		{
			this.spawnEffect.SetActive(true);
			this.spawnIn = false;
			this.timeSinceSpawned = 0f;
		}
		if (!MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies().Contains(this))
		{
			MonoSingleton<EnemyTracker>.Instance.AddEnemy(this);
		}
		if (MonoSingleton<MarkedForDeath>.Instance && MonoSingleton<MarkedForDeath>.Instance.gameObject.activeInHierarchy)
		{
			this.PlayerMarkedForDeath();
		}
		this.isBoss = base.GetComponentInChildren<BossIdentifier>() != null;
		if (this.difficultyOverride >= 0)
		{
			this.difficulty = this.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.UpdateTarget();
		this.SlowUpdate();
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000335D4 File Offset: 0x000317D4
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 1f);
		if (this.drillers.Count > 0)
		{
			for (int i = this.drillers.Count - 1; i >= 0; i--)
			{
				if (this.drillers[i] == null || !this.drillers[i].gameObject.activeInHierarchy)
				{
					this.drillers.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00033650 File Offset: 0x00031850
	private void Update()
	{
		this.UpdateTarget();
		this.UpdateEnemyScanner();
		this.ForceGetHealth();
		this.UpdateModifiers();
		this.UpdateDebugStuff();
		if (this.puppet)
		{
			if (this.puppetSpawnTimer < 1f)
			{
				this.puppetSpawnTimer = Mathf.MoveTowards(this.puppetSpawnTimer, 1f, Time.deltaTime * 2f * Mathf.Max(1f - this.puppetSpawnTimer, 0.001f));
				base.transform.localScale = Vector3.Lerp(this.squishedScale, this.originalScale, this.puppetSpawnTimer);
				this.squishedScale = new Vector3(Mathf.MoveTowards(this.squishedScale.x, this.originalScale.x * this.puppetSpawnTimer, Time.deltaTime * 4f), this.squishedScale.y, Mathf.MoveTowards(this.squishedScale.z, this.originalScale.z * this.puppetSpawnTimer, Time.deltaTime * 4f));
				if (this.puppetSpawnIgnoringPlayer && this.puppetSpawnTimer > 0.75f)
				{
					this.puppetSpawnIgnoringPlayer = false;
					Collider[] array = this.puppetSpawnColliders;
					for (int i = 0; i < array.Length; i++)
					{
						Physics.IgnoreCollision(array[i], MonoSingleton<NewMovement>.Instance.playerCollider, false);
					}
					if (this.rb)
					{
						this.rb.constraints = this.rbc;
					}
				}
				this.UpdateBuffs(false, true);
			}
			foreach (Renderer renderer in this.puppetRenderers)
			{
				if (renderer != null)
				{
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					renderer.GetPropertyBlock(materialPropertyBlock);
					materialPropertyBlock.SetFloat("_VertexNoiseAmplitude", Mathf.Lerp(10f, 1f, Mathf.Max(0f, this.puppetSpawnTimer - 0.5f) * 2f));
					materialPropertyBlock.SetColor("_FlowDirection", new Color(Random.Range(0f, 1f), 0.2f, Random.Range(0f, 1f)));
					renderer.SetPropertyBlock(materialPropertyBlock);
				}
			}
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x000338A0 File Offset: 0x00031AA0
	private void UpdateEnemyScanner()
	{
		if (this.AttackEnemies)
		{
			if (this.enemyScanner == null)
			{
				this.enemyScanner = new EnemyScanner(this);
			}
			this.enemyScanner.Update();
			return;
		}
		EnemyScanner enemyScanner = this.enemyScanner;
		if (enemyScanner == null)
		{
			return;
		}
		enemyScanner.Reset();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000338DC File Offset: 0x00031ADC
	private void UpdateDebugStuff()
	{
		if (EnemyIdentifierDebug.Active)
		{
			if (this.debugOverlay == null)
			{
				this.debugOverlay = base.gameObject.AddComponent<EnemyIdentifierDebugOverlay>();
			}
			this.debugOverlay.ConsumeData(this.enemyType, this.enemyClass, this.dead, this.IgnorePlayer, this.AttackEnemies, this.target);
		}
		else if (this.debugOverlay != null)
		{
			Object.Destroy(this.debugOverlay);
		}
		if (!ForceBossBars.Active)
		{
			if (this.cheatCreatedBossBar != null)
			{
				Object.Destroy(this.cheatCreatedBossBar);
			}
			return;
		}
		if (base.GetComponent<BossHealthBar>())
		{
			return;
		}
		this.cheatCreatedBossBar = base.gameObject.AddComponent<BossHealthBar>();
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00033999 File Offset: 0x00031B99
	private bool HandleTargetCheats()
	{
		if (BlindEnemies.Blind)
		{
			this.target = null;
			return true;
		}
		if (EnemyIgnorePlayer.Active && this.target != null && this.target.isPlayer)
		{
			this.target = null;
		}
		return false;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000339D0 File Offset: 0x00031BD0
	private void UpdateTarget()
	{
		if (this.target != null && !this.target.isValid)
		{
			if (this.target.trackedTransform == this.fallbackTarget)
			{
				this.fallbackTarget = null;
			}
			this.target = null;
		}
		if (this.timeSinceNoTarget != null && this.target != null)
		{
			this.timeSinceNoTarget = null;
		}
		if (this.timeSinceNoTarget == null && this.target == null)
		{
			this.timeSinceNoTarget = new TimeSince?(0f);
		}
		if (this.HandleTargetCheats())
		{
			return;
		}
		if (!this.IgnorePlayer)
		{
			bool flag = this.fallbackTarget == null;
			flag |= this.prioritizePlayerOverFallback && (this.target == null || this.target.trackedTransform == this.fallbackTarget);
			bool flag2;
			if (this.timeSinceNoTarget != null)
			{
				TimeSince? timeSince = this.timeSinceNoTarget;
				float? num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
				float num2 = 1.5f;
				flag2 = (num.GetValueOrDefault() < num2) & (num != null);
			}
			else
			{
				flag2 = true;
			}
			if (flag2)
			{
				if (this.AttackEnemies && this.prioritizeEnemiesUnlessAttacked)
				{
					flag = false;
				}
				if (this.enemyType == EnemyType.Stalker)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.target = EnemyTarget.TrackPlayer();
			}
		}
		else if (this.target != null && this.target.isPlayer)
		{
			this.target = null;
		}
		if (this.target == null && this.fallbackTarget != null)
		{
			EnemyTarget enemyTarget = new EnemyTarget(this.fallbackTarget);
			if (enemyTarget.isValid)
			{
				this.target = enemyTarget;
				return;
			}
			this.fallbackTarget = null;
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00033B91 File Offset: 0x00031D91
	public void SetFallbackTarget(GameObject target)
	{
		this.fallbackTarget = target.transform;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00033B9F File Offset: 0x00031D9F
	public void SetOverrideCenter(Transform center)
	{
		this.overrideCenter = center;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00033BA8 File Offset: 0x00031DA8
	public void ResetTarget()
	{
		this.target = null;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00033BB4 File Offset: 0x00031DB4
	private void UpdateModifiers()
	{
		this.totalSpeedModifier = 1f;
		this.totalHealthModifier = 1f;
		this.totalDamageModifier = 1f;
		float num = Mathf.Max(OptionsManager.radianceTier, this.radianceTier);
		if (this.speedBuff || OptionsManager.forceRadiance)
		{
			this.totalSpeedModifier *= this.speedBuffModifier * ((num > 1f) ? (0.75f + num / 4f) : num);
		}
		if (this.healthBuff || OptionsManager.forceRadiance)
		{
			this.totalHealthModifier *= this.healthBuffModifier * ((num > 1f) ? (0.75f + num / 4f) : num);
		}
		if (this.damageBuff || OptionsManager.forceRadiance)
		{
			this.totalDamageModifier *= this.damageBuffModifier;
		}
		if (this.puppet)
		{
			this.totalHealthModifier /= 2f;
			this.totalSpeedModifier *= Mathf.Lerp(0.01f, Mathf.Max(0.01f, this.puppetSpawnTimer - 0.75f) * 3f, this.puppetSpawnTimer);
		}
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00033CE0 File Offset: 0x00031EE0
	public void StartBurning(float heat)
	{
		foreach (Flammable flammable in this.flammables)
		{
			flammable.Burn(heat, false);
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00033D34 File Offset: 0x00031F34
	public void Burn()
	{
		base.CancelInvoke("Burn");
		if (this.burners.Count == 0)
		{
			return;
		}
		for (int i = this.burners.Count - 1; i >= 0; i--)
		{
			if (this.burners[i] == null)
			{
				this.burners.RemoveAt(i);
			}
			else if (!this.burners[i].burning)
			{
				this.burners.RemoveAt(i);
			}
			else
			{
				this.burners[i].Pulse();
			}
		}
		if (this.burners.Count == 0)
		{
			return;
		}
		base.Invoke("Burn", 0.5f);
		if (!this.dead)
		{
			this.TryIgniteGasoline();
		}
		float num = 0f;
		foreach (Flammable flammable in this.flammables)
		{
			num = Mathf.Max(num, flammable.fuel);
		}
		this.hitter = "fire";
		this.DeliverDamage(base.gameObject, Vector3.zero, base.transform.position, (num > 0f) ? 0.5f : 0.2f, false, 0f, null, false, false);
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			if (!this.buffUnaffectedRenderers.Contains(renderer) && !(renderer is ParticleSystemRenderer))
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat("_OiledAmount", num / 5f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00033EF0 File Offset: 0x000320F0
	private void TryIgniteGasoline()
	{
		MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, 3);
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00033F0C File Offset: 0x0003210C
	public void DeliverDamage(GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
	{
		if (EnemyIdentifierDebug.Active)
		{
			EnemyIdentifier.Log.Fine("Delivering damage to: " + base.gameObject.name + ", Damage:" + multiplier.ToString(), null, null, null);
		}
		if (target == base.gameObject)
		{
			EnemyIdentifierIdentifier componentInChildren = base.GetComponentInChildren<EnemyIdentifierIdentifier>();
			if (componentInChildren != null)
			{
				target = componentInChildren.gameObject;
			}
		}
		if (sourceWeapon != null)
		{
			if (this.prioritizeEnemiesUnlessAttacked)
			{
				this.prioritizeEnemiesUnlessAttacked = false;
			}
			if (!this.IgnorePlayer && ((this.target != null && !this.target.isPlayer) || this.target == null))
			{
				this.target = EnemyTarget.TrackPlayer();
				this.HandleTargetCheats();
			}
		}
		if (!ignoreTotalDamageTakenMultiplier)
		{
			multiplier *= this.totalDamageTakenMultiplier;
		}
		multiplier /= this.totalHealthModifier;
		if (this.isBoss && this.difficulty >= 4)
		{
			if (this.difficulty == 5)
			{
				multiplier /= 2f;
			}
			else
			{
				multiplier /= 1.5f;
			}
		}
		if (this.weaknesses.Length != 0)
		{
			for (int i = 0; i < this.weaknesses.Length; i++)
			{
				if (this.hitter == this.weaknesses[i] || (this.hitterAttributes.Contains(HitterAttribute.Electricity) && this.weaknesses[i] == "electricity"))
				{
					multiplier *= this.weaknessMultipliers[i];
				}
			}
		}
		if (this.getFireDamageMultiplier && this.burners.Count > 0 && this.hitter != "fire" && this.hitter != "explosion" && this.hitter != "ffexplosion")
		{
			multiplier *= 1.5f;
		}
		if (this.nails.Count > 10)
		{
			for (int j = 0; j < this.nails.Count - 10; j++)
			{
				if (this.nails[j] != null)
				{
					Object.Destroy(this.nails[j].gameObject);
				}
				this.nails.RemoveAt(j);
			}
		}
		if (!this.beingZapped && this.hitterAttributes.Contains(HitterAttribute.Electricity) && this.hitter != "aftershock" && (this.nailsAmount > 0 || this.stuckMagnets.Count > 0 || this.touchingWaters.Count > 0))
		{
			this.beingZapped = true;
			foreach (Nail nail in this.nails)
			{
				if (nail != null)
				{
					nail.Zap();
				}
			}
			if (this.hitter == "zapper" && multiplier > this.health)
			{
				multiplier = this.health - 0.001f;
			}
			this.afterShockSourceWeapon = sourceWeapon;
			this.waterOnlyAftershock = this.nailsAmount == 0 && this.stuckMagnets.Count == 0;
			if (this.hitter == "zap")
			{
				this.afterShockFromZap = true;
			}
			base.Invoke("AfterShock", 0.5f);
		}
		if (this.pulledByMagnet && this.hitter != "deathzone")
		{
			this.pulledByMagnet = false;
		}
		bool flag = false;
		EnemyType enemyType = this.enemyType;
		if (enemyType <= EnemyType.MaliciousFace)
		{
			if (enemyType != EnemyType.Drone)
			{
				if (enemyType != EnemyType.MaliciousFace)
				{
					goto IL_0536;
				}
				if (this.spider == null)
				{
					this.spider = base.GetComponent<SpiderBody>();
				}
				if (this.spider == null)
				{
					return;
				}
				if ((this.hitter != "explosion" && this.hitter != "ffexplosion") || this.isGasolined)
				{
					this.spider.GetHurt(target, force, hitPoint, multiplier, sourceWeapon);
				}
				if (this.spider.health <= 0f)
				{
					this.Death();
				}
				this.health = this.spider.health;
				flag = true;
				goto IL_0536;
			}
		}
		else if (enemyType != EnemyType.Virtue)
		{
			if (enemyType != EnemyType.Wicked)
			{
				if (enemyType != EnemyType.Idol)
				{
					goto IL_0536;
				}
				this.idol = (this.idol ? this.idol : base.GetComponent<Idol>());
				if (!(this.hitter == "punch") && !(this.hitter == "heavypunch") && !(this.hitter == "ground slam") && !(this.hitter == "hammer"))
				{
					goto IL_0536;
				}
				Idol idol = this.idol;
				if (idol == null)
				{
					goto IL_0536;
				}
				idol.Death();
				goto IL_0536;
			}
			else
			{
				if (this.wicked == null)
				{
					this.wicked = base.GetComponent<Wicked>();
				}
				if (this.wicked == null)
				{
					return;
				}
				this.wicked.GetHit();
				flag = true;
				goto IL_0536;
			}
		}
		if (this.drone == null)
		{
			this.drone = base.GetComponent<Drone>();
		}
		if (this.drone == null)
		{
			return;
		}
		this.drone.GetHurt(force, multiplier, sourceWeapon, fromExplosion);
		this.health = this.drone.health;
		if (this.health <= 0f)
		{
			this.Death();
		}
		flag = true;
		IL_0536:
		if (!flag)
		{
			switch (this.enemyClass)
			{
			case EnemyClass.Husk:
				if (this.zombie == null)
				{
					this.zombie = base.GetComponent<Zombie>();
				}
				if (this.zombie == null)
				{
					return;
				}
				this.zombie.GetHurt(target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion);
				if (tryForExplode && this.zombie.health <= 0f && !this.exploded)
				{
					this.Explode(fromExplosion);
				}
				if (this.zombie.health <= 0f)
				{
					this.Death();
				}
				this.health = this.zombie.health;
				break;
			case EnemyClass.Machine:
				if (this.machine == null)
				{
					this.machine = base.GetComponent<Machine>();
				}
				if (this.machine == null)
				{
					return;
				}
				this.machine.GetHurt(target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion);
				if (tryForExplode && this.machine.health <= 0f && (this.machine.symbiote == null || this.machine.symbiote.health <= 0f) && !this.machine.dontDie && !this.exploded)
				{
					this.Explode(fromExplosion);
				}
				if (this.machine.health <= 0f && (this.machine.symbiote == null || this.machine.symbiote.health <= 0f))
				{
					this.Death();
				}
				this.health = this.machine.health;
				break;
			case EnemyClass.Demon:
				if (this.statue == null)
				{
					this.statue = base.GetComponent<Statue>();
				}
				if (this.statue == null)
				{
					return;
				}
				this.statue.GetHurt(target, force, multiplier, critMultiplier, hitPoint, sourceWeapon, fromExplosion);
				if (tryForExplode && this.statue.health <= 0f && !this.exploded)
				{
					this.Explode(fromExplosion);
				}
				if (this.statue.health <= 0f)
				{
					this.Death();
				}
				this.health = this.statue.health;
				break;
			}
		}
		this.hitterAttributes.Clear();
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x000346A4 File Offset: 0x000328A4
	private void AfterShock()
	{
		float num = Mathf.Min(6f, (float)this.nailsAmount / 15f);
		this.afterShockFromZap = false;
		if (this.stuckMagnets.Count > 0)
		{
			num += (float)this.stuckMagnets.Count;
			foreach (Magnet magnet in this.stuckMagnets)
			{
				if (magnet != null)
				{
					magnet.health -= 1f;
				}
			}
		}
		if (num < 1f && this.touchingWaters.Count > 0)
		{
			num = 1f;
		}
		GoreZone goreZone = this.GetGoreZone();
		foreach (Nail nail in this.nails)
		{
			if (!(nail == null))
			{
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Small, this, false);
				if (gore && goreZone)
				{
					gore.transform.position = nail.transform.position;
					gore.SetActive(true);
					Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
					gore.transform.SetParent(goreZone.goreZone, true);
					if (component != null && !this.dead)
					{
						component.GetReady();
					}
				}
				Object.Destroy(nail.gameObject);
			}
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(base.gameObject);
		EnemyIdentifier.Zap(this.overrideCenter ? this.overrideCenter.position : base.transform.position, num, list, this.afterShockSourceWeapon, this, null, this.waterOnlyAftershock);
		this.nails.Clear();
		this.nailsAmount = 0;
		EnemyIdentifierIdentifier[] componentsInChildren = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
		if (!this.dead && !this.puppet)
		{
			MonoSingleton<StyleHUD>.Instance.AddPoints(Mathf.Max(1, Mathf.RoundToInt(num * 15f)), "<color=#00ffffff>CONDUCTOR</color>", this.afterShockSourceWeapon, this, -1, "", "");
		}
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == null) && componentsInChildren[i].gameObject != base.gameObject)
				{
					this.hitter = "aftershock";
					this.hitterAttributes.Add(HitterAttribute.Electricity);
					this.DeliverDamage(componentsInChildren[i].gameObject, Vector3.zero, base.transform.position, num, true, 0f, this.afterShockSourceWeapon, false, false);
					break;
				}
			}
		}
		this.beingZapped = false;
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00034980 File Offset: 0x00032B80
	public static void Zap(Vector3 position, float damage = 2f, List<GameObject> alreadyHitObjects = null, GameObject sourceWeapon = null, EnemyIdentifier sourceEid = null, Water sourceWater = null, bool waterOnly = false)
	{
		bool flag = false;
		if (sourceWater && sourceWater.isPlayerTouchingWater)
		{
			flag = true;
		}
		else if (sourceEid && sourceEid.touchingWaters.Count > 0)
		{
			foreach (Water water in sourceEid.touchingWaters)
			{
				if (!(water == null) && water.isPlayerTouchingWater)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			MonoSingleton<NewMovement>.Instance.GetHurt(50, true, 1f, false, false, 1f, false);
			LineRenderer lineRenderer = Object.Instantiate<LineRenderer>(MonoSingleton<DefaultReferenceManager>.Instance.electricLine, Vector3.Lerp(position, MonoSingleton<NewMovement>.Instance.transform.position, 0.5f), Quaternion.identity);
			lineRenderer.SetPosition(0, position);
			lineRenderer.SetPosition(1, MonoSingleton<NewMovement>.Instance.transform.position);
			Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.zapImpactParticle, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity);
		}
		foreach (EnemyIdentifier enemyIdentifier in MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies())
		{
			if (alreadyHitObjects == null || !alreadyHitObjects.Contains(enemyIdentifier.gameObject))
			{
				bool flag2 = false;
				if (!enemyIdentifier.flying || ((sourceWater || sourceEid) && enemyIdentifier.touchingWaters.Count != 0))
				{
					if (enemyIdentifier.touchingWaters.Count > 0)
					{
						if (sourceWater != null)
						{
							flag2 = enemyIdentifier.touchingWaters.Contains(sourceWater);
						}
						if (!flag2 && sourceEid != null && sourceEid.touchingWaters.Count > 0)
						{
							foreach (Water water2 in enemyIdentifier.touchingWaters)
							{
								if (!(water2 == null) && sourceEid.touchingWaters.Contains(water2))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (enemyIdentifier.flying && !flag2)
						{
							continue;
						}
					}
					Vector3 vector = (enemyIdentifier.overrideCenter ? enemyIdentifier.overrideCenter.position : enemyIdentifier.transform.position);
					if ((flag2 && (!waterOnly || enemyIdentifier.lastZapped > 1f)) || (!waterOnly && (Vector3.Distance(position, vector) < 30f || (position.y > vector.y && position.y - vector.y < 60f && Vector3.Distance(position, new Vector3(vector.x, position.y, vector.z)) < 30f)) && !Physics.Raycast(position, vector - position, Vector3.Distance(position, vector), LayerMaskDefaults.Get(LMD.Environment))))
					{
						enemyIdentifier.hitter = "zap";
						enemyIdentifier.hitterAttributes.Add(HitterAttribute.Electricity);
						enemyIdentifier.DeliverDamage(enemyIdentifier.gameObject, Vector3.zero, vector, Mathf.Max((enemyIdentifier.lastZapped < 5f) ? 0.5f : 2f, damage), true, 0f, sourceWeapon, false, false);
						enemyIdentifier.lastZapped = 0f;
						LineRenderer lineRenderer2 = Object.Instantiate<LineRenderer>(MonoSingleton<DefaultReferenceManager>.Instance.electricLine, Vector3.Lerp(position, vector, 0.5f), Quaternion.identity);
						lineRenderer2.SetPosition(0, position);
						lineRenderer2.SetPosition(1, vector);
						Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.zapImpactParticle, vector, Quaternion.identity);
					}
				}
			}
		}
		if (waterOnly)
		{
			return;
		}
		foreach (Magnet magnet in MonoSingleton<ObjectTracker>.Instance.magnetList)
		{
			if (!(magnet == null) && !alreadyHitObjects.Contains(magnet.gameObject) && (!(magnet.onEnemy != null) || !alreadyHitObjects.Contains(magnet.onEnemy.gameObject)) && Vector3.Distance(position, magnet.transform.position) < 30f && !Physics.Raycast(position, magnet.transform.position - position, Vector3.Distance(position, magnet.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				magnet.StartCoroutine(magnet.Zap(alreadyHitObjects, Mathf.Max(0.5f, damage), sourceWeapon));
				LineRenderer lineRenderer3 = Object.Instantiate<LineRenderer>(MonoSingleton<DefaultReferenceManager>.Instance.electricLine, Vector3.Lerp(position, magnet.transform.position, 0.5f), Quaternion.identity);
				lineRenderer3.SetPosition(0, position);
				lineRenderer3.SetPosition(1, magnet.transform.position);
			}
		}
		foreach (Zappable zappable in MonoSingleton<ObjectTracker>.Instance.zappablesList)
		{
			RaycastHit raycastHit;
			if (!(zappable == null) && !alreadyHitObjects.Contains(zappable.gameObject) && Vector3.Distance(position, zappable.transform.position) < 30f && (!Physics.Raycast(position, zappable.transform.position - position, out raycastHit, Vector3.Distance(position, zappable.transform.position), LayerMaskDefaults.Get(LMD.Environment)) || raycastHit.transform.gameObject == zappable.gameObject))
			{
				zappable.StartCoroutine(zappable.Zap(alreadyHitObjects, Mathf.Max(0.5f, damage), sourceWeapon));
				LineRenderer lineRenderer4 = Object.Instantiate<LineRenderer>(MonoSingleton<DefaultReferenceManager>.Instance.electricLine, Vector3.Lerp(position, zappable.transform.position, 0.5f), Quaternion.identity);
				lineRenderer4.SetPosition(0, position);
				lineRenderer4.SetPosition(1, zappable.transform.position);
			}
		}
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00035020 File Offset: 0x00033220
	public void Death()
	{
		this.Death(false);
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0003502C File Offset: 0x0003322C
	public void Death(bool fromExplosion)
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		GameObject[] array = this.activateOnDeath;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		foreach (GameObject gameObject in this.destroyOnDeath)
		{
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
		UnityEvent unityEvent = this.onDeath;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (!this.puppet)
		{
			if (this.hitterWeapons.Count > 1)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.arsenal", null, this, -1, "", "");
			}
			if (!this.dontUnlockBestiary)
			{
				MonoSingleton<BestiaryData>.Instance.SetEnemy(this.enemyType, 2);
				UnlockBestiary unlockBestiary;
				if (base.TryGetComponent<UnlockBestiary>(out unlockBestiary))
				{
					MonoSingleton<BestiaryData>.Instance.SetEnemy(unlockBestiary.enemy, 2);
				}
			}
		}
		if (this.health > 0f)
		{
			this.health = 0f;
		}
		this.DestroyMagnets();
		if (this.drillers.Count > 0 && this.enemyType != EnemyType.MaliciousFace && this.enemyType != EnemyType.Gutterman && this.enemyType != EnemyType.Mindflayer)
		{
			foreach (Harpoon harpoon in this.drillers)
			{
				harpoon.DelayedDestroyIfOnCorpse(1f);
			}
		}
		if (this.usingDoor != null)
		{
			this.usingDoor.Close();
			this.usingDoor = null;
		}
		this.Desandify(true);
		this.Unbless(false);
		if (this.puppet)
		{
			EnemyIdentifierIdentifier[] componentsInChildren = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
			GoreZone goreZone = this.GetGoreZone();
			foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in componentsInChildren)
			{
				GoreType goreType = GoreType.Body;
				if (enemyIdentifierIdentifier.CompareTag("Head") || enemyIdentifierIdentifier.CompareTag("EndLimb"))
				{
					goreType = GoreType.Head;
				}
				else if (enemyIdentifierIdentifier.CompareTag("Limb"))
				{
					goreType = GoreType.Limb;
				}
				GameObject gore = this.bsm.GetGore(goreType, this, fromExplosion);
				gore.transform.position = enemyIdentifierIdentifier.transform.position;
				if (goreZone != null && goreZone.goreZone != null)
				{
					gore.transform.SetParent(goreZone.goreZone, true);
				}
				Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
				if (component != null)
				{
					component.GetReady();
				}
				Object.Destroy(enemyIdentifierIdentifier.gameObject);
			}
			Object.Destroy(base.gameObject);
		}
		if (this.beingZapped)
		{
			base.CancelInvoke("AfterShock");
			base.Invoke("AfterShock", 0.1f);
		}
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000352FC File Offset: 0x000334FC
	public void DestroyMagnets()
	{
		if (this.stuckMagnets.Count > 0)
		{
			for (int i = this.stuckMagnets.Count - 1; i >= 0; i--)
			{
				if (this.stuckMagnets[i] != null)
				{
					Object.Destroy(this.stuckMagnets[i].gameObject);
				}
			}
		}
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x0003535C File Offset: 0x0003355C
	public void InstaKill()
	{
		if (!this.dead)
		{
			this.Death();
			if (this.pulledByMagnet && !this.puppet)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(240, "ultrakill.catapulted", null, this, -1, "", "");
			}
			this.dead = true;
			bool flag = false;
			EnemyType enemyType = this.enemyType;
			if (enemyType <= EnemyType.MaliciousFace)
			{
				if (enemyType != EnemyType.Drone)
				{
					if (enemyType != EnemyType.MaliciousFace)
					{
						goto IL_00E8;
					}
					if (this.spider == null)
					{
						this.spider = base.GetComponent<SpiderBody>();
					}
					this.spider.Die();
					flag = true;
					goto IL_00E8;
				}
			}
			else if (enemyType != EnemyType.Virtue)
			{
				if (enemyType != EnemyType.Idol)
				{
					goto IL_00E8;
				}
				Idol idol;
				if (base.TryGetComponent<Idol>(out idol))
				{
					idol.Death();
					goto IL_00E8;
				}
				goto IL_00E8;
			}
			if (this.drone == null)
			{
				this.drone = base.GetComponent<Drone>();
			}
			this.drone.GetHurt(Vector3.zero, 999f, null, false);
			this.drone.Explode();
			flag = true;
			IL_00E8:
			if (!flag)
			{
				switch (this.enemyClass)
				{
				case EnemyClass.Husk:
					if (this.zombie == null)
					{
						this.zombie = base.GetComponent<Zombie>();
					}
					if (!this.zombie.limp)
					{
						this.zombie.GoLimp();
					}
					break;
				case EnemyClass.Machine:
					if (this.machine == null)
					{
						this.machine = base.GetComponent<Machine>();
					}
					if (!this.machine.limp)
					{
						this.machine.GoLimp();
					}
					break;
				case EnemyClass.Demon:
					if (this.statue == null)
					{
						this.statue = base.GetComponent<Statue>();
					}
					if (!this.statue.limp)
					{
						this.statue.GoLimp();
					}
					break;
				}
			}
			if (this.usingDoor != null)
			{
				this.usingDoor.Close();
				this.usingDoor = null;
			}
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00035530 File Offset: 0x00033730
	public void Explode(bool fromExplosion = false)
	{
		bool flag = this.dead;
		if (!this.dead)
		{
			this.Death();
		}
		if (this.enemyType == EnemyType.MaliciousFace)
		{
			if (this.spider == null)
			{
				this.spider = base.GetComponent<SpiderBody>();
			}
			if (!flag)
			{
				this.hitter = "breaker";
				this.spider.Die();
				return;
			}
			this.spider.BreakCorpse();
			return;
		}
		else
		{
			if (this.enemyType == EnemyType.Drone || this.enemyType == EnemyType.Virtue)
			{
				if (this.drone == null)
				{
					this.drone = base.GetComponent<Drone>();
				}
				this.drone.Explode();
				return;
			}
			if (this.enemyClass == EnemyClass.Husk)
			{
				if (this.zombie == null)
				{
					this.zombie = base.GetComponent<Zombie>();
				}
				if (!this.exploded && this.zombie && !this.zombie.chestExploding)
				{
					this.exploded = true;
					if (this.zombie.chestExploding)
					{
						this.zombie.ChestExplodeEnd();
					}
					if (!flag && this.pulledByMagnet && !this.puppet)
					{
						MonoSingleton<StyleHUD>.Instance.AddPoints(240, "ultrakill.catapulted", null, this, -1, "", "");
					}
					EnemyIdentifierIdentifier[] componentsInChildren = this.zombie.GetComponentsInChildren<EnemyIdentifierIdentifier>(true);
					this.GetGoreZone();
					bool flag2 = false;
					foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in componentsInChildren)
					{
						if (enemyIdentifierIdentifier.gameObject.CompareTag("Limb"))
						{
							this.DestroyLimb(enemyIdentifierIdentifier.transform, LimbDestroyType.Detach);
							if (!flag2)
							{
								this.zombie.GetHurt(enemyIdentifierIdentifier.gameObject, (base.transform.position - enemyIdentifierIdentifier.transform.position).normalized * 1000f, 1E+09f, 1f, null, false);
							}
						}
						else if (enemyIdentifierIdentifier.gameObject.CompareTag("Head") || enemyIdentifierIdentifier.gameObject.CompareTag("EndLimb"))
						{
							flag2 = true;
							this.zombie.GetHurt(enemyIdentifierIdentifier.gameObject, (base.transform.position - enemyIdentifierIdentifier.transform.position).normalized * 1000f, 1E+09f, 1f, null, false);
						}
					}
					if (!flag2)
					{
						this.zombie.GoLimp();
					}
					this.health = this.zombie.health;
					if (this.usingDoor != null)
					{
						this.usingDoor.Close();
						this.usingDoor = null;
						return;
					}
				}
			}
			else if (this.enemyClass == EnemyClass.Machine && this.enemyType != EnemyType.Drone)
			{
				if (this.machine == null)
				{
					this.machine = base.GetComponent<Machine>();
				}
				if (!this.exploded && this.machine)
				{
					this.exploded = true;
					bool flag3 = false;
					if (this.machine.dismemberment)
					{
						Collider[] componentsInChildren2 = this.machine.GetComponentsInChildren<Collider>();
						List<EnemyIdentifierIdentifier> list = new List<EnemyIdentifierIdentifier>();
						Collider[] array2 = componentsInChildren2;
						for (int i = 0; i < array2.Length; i++)
						{
							EnemyIdentifierIdentifier component = array2[i].GetComponent<EnemyIdentifierIdentifier>();
							if (component != null)
							{
								list.Add(component);
							}
						}
						this.GetGoreZone();
						foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier2 in list)
						{
							if (enemyIdentifierIdentifier2.gameObject.CompareTag("Limb"))
							{
								this.DestroyLimb(enemyIdentifierIdentifier2.transform, LimbDestroyType.Detach);
							}
							else if (enemyIdentifierIdentifier2.gameObject.CompareTag("Head") || enemyIdentifierIdentifier2.gameObject.CompareTag("EndLimb"))
							{
								flag3 = true;
								this.machine.GetHurt(enemyIdentifierIdentifier2.gameObject, (base.transform.position - enemyIdentifierIdentifier2.transform.position).normalized * 1000f, 999f, 1f, null, false);
							}
						}
					}
					if (!flag3)
					{
						this.machine.GoLimp(fromExplosion);
					}
					this.health = this.machine.health;
					if (this.usingDoor != null)
					{
						this.usingDoor.Close();
						this.usingDoor = null;
						return;
					}
				}
			}
			else if (this.enemyClass == EnemyClass.Demon)
			{
				if (this.statue == null)
				{
					this.statue = base.GetComponent<Statue>();
				}
				if (!this.exploded)
				{
					this.exploded = true;
					if (!this.statue.limp)
					{
						this.statue.GoLimp();
					}
					this.health = this.statue.health;
				}
			}
			return;
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00035A00 File Offset: 0x00033C00
	public void Splatter(bool styleBonus = true)
	{
		if (InvincibleEnemies.Enabled || this.blessed)
		{
			return;
		}
		if (this.enemyType == EnemyType.MaliciousFace)
		{
			if (this.spider == null)
			{
				this.spider = base.GetComponent<SpiderBody>();
			}
			if (!this.dead)
			{
				this.hitter = "breaker";
				this.spider.Die();
				return;
			}
			this.spider.BreakCorpse();
			return;
		}
		else
		{
			if (this.enemyType == EnemyType.Drone || this.enemyType == EnemyType.Virtue)
			{
				if (this.drone == null)
				{
					this.drone = base.GetComponent<Drone>();
				}
				this.drone.GetHurt(Vector3.zero, 999f, null, false);
				if (this.enemyType == EnemyType.Virtue)
				{
					this.drone.Explode();
				}
				this.Death();
				return;
			}
			switch (this.enemyClass)
			{
			case EnemyClass.Husk:
				if (this.zombie == null)
				{
					this.zombie = base.GetComponent<Zombie>();
				}
				break;
			case EnemyClass.Machine:
				if (this.machine == null)
				{
					this.machine = base.GetComponent<Machine>();
				}
				break;
			case EnemyClass.Demon:
				if (this.statue == null)
				{
					this.statue = base.GetComponent<Statue>();
				}
				break;
			}
			bool flag = this.dead;
			if (this.enemyClass == EnemyClass.Machine && this.machine && !this.machine.dismemberment)
			{
				this.InstaKill();
			}
			else if (this.enemyClass == EnemyClass.Demon && this.statue && (this.statue.massDeath || this.statue.specialDeath))
			{
				this.InstaKill();
			}
			else if (!this.exploded && (this.enemyClass != EnemyClass.Husk || !this.zombie.chestExploding))
			{
				this.exploded = true;
				this.limbs = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
				if (!flag)
				{
					base.SendMessage("GoLimp", SendMessageOptions.DontRequireReceiver);
					StyleHUD instance = MonoSingleton<StyleHUD>.Instance;
					if (!this.puppet)
					{
						if (this.pulledByMagnet)
						{
							instance.AddPoints(120, "ultrakill.catapulted", null, this, -1, "", "");
						}
						if (styleBonus)
						{
							instance.AddPoints(100, "ultrakill.splattered", null, this, -1, "", "");
						}
					}
					base.transform.Rotate(new Vector3(90f, 0f, 0f));
				}
				GameObject gore = this.bsm.GetGore(GoreType.Splatter, this, false);
				gore.transform.position = base.transform.position + Vector3.up;
				GoreZone goreZone = this.GetGoreZone();
				if (goreZone != null && goreZone.goreZone != null)
				{
					gore.transform.SetParent(goreZone.goreZone, true);
				}
				Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
				if (component != null)
				{
					component.GetReady();
				}
				foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in this.limbs)
				{
					if (enemyIdentifierIdentifier.gameObject.CompareTag("Body") || enemyIdentifierIdentifier.gameObject.CompareTag("Limb") || enemyIdentifierIdentifier.gameObject.CompareTag("Head") || enemyIdentifierIdentifier.gameObject.CompareTag("EndLimb"))
					{
						Object.Destroy(enemyIdentifierIdentifier.GetComponent<CharacterJoint>());
						enemyIdentifierIdentifier.transform.SetParent(this.GetGoreZone().gibZone, true);
						Rigidbody component2 = enemyIdentifierIdentifier.GetComponent<Rigidbody>();
						if (component2 != null)
						{
							component2.velocity = Vector3.zero;
							enemyIdentifierIdentifier.transform.position = new Vector3(enemyIdentifierIdentifier.transform.position.x, base.transform.position.y + 0.1f, enemyIdentifierIdentifier.transform.position.z);
							Vector3 vector = new Vector3(base.transform.position.x - enemyIdentifierIdentifier.transform.position.x, 0f, base.transform.position.z - enemyIdentifierIdentifier.transform.position.z);
							component2.AddForce(vector * 15f, ForceMode.VelocityChange);
							component2.constraints = RigidbodyConstraints.FreezePositionY;
						}
					}
				}
				if (this.machine && this.enemyType == EnemyType.Streetcleaner)
				{
					this.machine.CanisterExplosion();
				}
				base.Invoke("StopSplatter", 1f);
				if (this.usingDoor != null)
				{
					this.usingDoor.Close();
					this.usingDoor = null;
				}
			}
			this.Death();
			return;
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00035E98 File Offset: 0x00034098
	public void StopSplatter()
	{
		foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in this.limbs)
		{
			if (enemyIdentifierIdentifier != null)
			{
				Rigidbody component = enemyIdentifierIdentifier.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.constraints = RigidbodyConstraints.None;
				}
			}
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00035EE0 File Offset: 0x000340E0
	public void Sandify(bool ignorePrevious = false)
	{
		if (this.dead)
		{
			return;
		}
		if (!ignorePrevious && this.sandified)
		{
			return;
		}
		this.sandified = true;
		if (this.puppet)
		{
			this.InstaKill();
			return;
		}
		foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
		{
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.sandDrip, enemyIdentifierIdentifier.transform.position, enemyIdentifierIdentifier.transform.rotation);
			Collider component = enemyIdentifierIdentifier.GetComponent<Collider>();
			if (component)
			{
				gameObject.transform.localScale = component.bounds.size;
			}
			gameObject.transform.SetParent(enemyIdentifierIdentifier.transform, true);
			this.sandifiedParticles.Add(gameObject);
		}
		Collider component2 = base.GetComponent<Collider>();
		if (component2)
		{
			Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.sandificationEffect, component2.bounds.center, Quaternion.identity);
		}
		else
		{
			Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.sandificationEffect, base.transform.position, Quaternion.identity);
		}
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			if (!this.buffUnaffectedRenderers.Contains(renderer))
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(EnemyIdentifier.HasSandBuff, 1f);
				materialPropertyBlock.SetFloat(EnemyIdentifier.NewSanded, 1f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00036064 File Offset: 0x00034264
	public void Desandify(bool visualOnly = false)
	{
		if (!visualOnly)
		{
			this.sandified = false;
		}
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!this.buffUnaffectedRenderers.Contains(renderer))
			{
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(EnemyIdentifier.HasSandBuff, 0f);
				materialPropertyBlock.SetFloat(EnemyIdentifier.NewSanded, 0f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
		foreach (GameObject gameObject in this.sandifiedParticles)
		{
			Object.Destroy(gameObject);
		}
		this.sandifiedParticles.Clear();
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00036128 File Offset: 0x00034328
	public void Bless(bool ignorePrevious = false)
	{
		if (!ignorePrevious)
		{
			this.blessings++;
			if (this.blessings > 1)
			{
				return;
			}
		}
		if (!ignorePrevious && this.blessed)
		{
			return;
		}
		this.blessed = true;
		foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
		{
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.blessingGlow, enemyIdentifierIdentifier.transform.position, enemyIdentifierIdentifier.transform.rotation);
			Collider component = enemyIdentifierIdentifier.GetComponent<Collider>();
			if (component)
			{
				gameObject.transform.localScale = component.bounds.size;
			}
			gameObject.transform.SetParent(enemyIdentifierIdentifier.transform, true);
			this.blessingGlows.Add(gameObject);
		}
		if (this.burners != null && this.burners.Count > 0)
		{
			foreach (Flammable flammable in this.burners)
			{
				flammable.PutOut(false);
			}
			this.burners.Clear();
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00036254 File Offset: 0x00034454
	public void Unbless(bool visualOnly = false)
	{
		if (!visualOnly)
		{
			if (this.blessings <= 0)
			{
				return;
			}
			this.blessings--;
			if (this.blessings < 0)
			{
				this.blessings = 0;
			}
			if (this.blessings > 0)
			{
				return;
			}
			this.blessed = false;
		}
		foreach (GameObject gameObject in this.blessingGlows)
		{
			Object.Destroy(gameObject);
		}
		this.blessingGlows.Clear();
		if (!visualOnly)
		{
			MonoSingleton<EnemyTracker>.Instance.UpdateIdolsNow();
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000362F8 File Offset: 0x000344F8
	public void AddFlammable(float amount)
	{
		if (!this.beenGasolined)
		{
			this.beenGasolined = true;
			EnemyIdentifierIdentifier[] componentsInChildren = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i].gameObject == base.gameObject) && !(componentsInChildren[i].eid != this))
				{
					Flammable flammable;
					if (!componentsInChildren[i].TryGetComponent<Flammable>(out flammable))
					{
						flammable = componentsInChildren[i].gameObject.AddComponent<Flammable>();
						flammable.fuelOnly = true;
					}
					if (!this.flammables.Contains(flammable))
					{
						this.flammables.Add(flammable);
					}
				}
			}
		}
		float num = 0f;
		foreach (Flammable flammable2 in this.flammables)
		{
			if (flammable2.fuel < 5f)
			{
				flammable2.fuel += Mathf.Min(amount, 5f - flammable2.fuel);
			}
			num = Mathf.Max(num, flammable2.fuel);
		}
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			if (!this.buffUnaffectedRenderers.Contains(renderer) && !(renderer is ParticleSystemRenderer))
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat("_OiledAmount", num / 5f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00036474 File Offset: 0x00034674
	public void PuppetSpawn()
	{
		if (this.dead)
		{
			return;
		}
		this.dontCountAsKills = true;
		this.puppet = true;
		if (this.sandified && this.enemyType != EnemyType.Stalker)
		{
			this.InstaKill();
			return;
		}
		this.puppetSpawnTimer = 0f;
		SpawnEffect componentInChildren = base.GetComponentInChildren<SpawnEffect>();
		if (componentInChildren)
		{
			componentInChildren.gameObject.SetActive(false);
			Object.Destroy(componentInChildren.gameObject);
		}
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		Material material = new Material(MonoSingleton<DefaultReferenceManager>.Instance.puppetMaterial);
		foreach (EnemySimplifier enemySimplifier in base.GetComponentsInChildren<EnemySimplifier>())
		{
			enemySimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, material);
			enemySimplifier.enragedMaterial = material;
			enemySimplifier.ignoreCustomColor = true;
			if (enemySimplifier.simplifiedMaterial)
			{
				Material material2 = new Material(enemySimplifier.simplifiedMaterial);
				material2.color = Color.red;
				enemySimplifier.simplifiedMaterial = material2;
				if (enemySimplifier.simplifiedMaterial2)
				{
					enemySimplifier.simplifiedMaterial2 = material2;
				}
				if (enemySimplifier.simplifiedMaterial3)
				{
					enemySimplifier.simplifiedMaterial3 = material2;
				}
			}
		}
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!this.buffUnaffectedRenderers.Contains(renderer) && !(renderer is ParticleSystemRenderer))
			{
				Material[] array2 = new Material[renderer.sharedMaterials.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = material;
				}
				renderer.sharedMaterials = array2;
				this.puppetRenderers.Add(renderer);
			}
		}
		if (this.originalScale == Vector3.zero)
		{
			if (this.rb)
			{
				this.rbc = this.rb.constraints;
				this.rb.constraints = RigidbodyConstraints.FreezeAll;
			}
			this.originalScale = base.transform.localScale;
			this.squishedScale = new Vector3(this.originalScale.x * 5f, 0.001f, this.originalScale.z * 5f);
			base.transform.localScale = this.squishedScale;
		}
		this.puppetSpawnColliders = base.GetComponentsInChildren<Collider>();
		Collider[] array3 = this.puppetSpawnColliders;
		for (int i = 0; i < array3.Length; i++)
		{
			Physics.IgnoreCollision(array3[i], MonoSingleton<NewMovement>.Instance.playerCollider, true);
		}
		this.puppetSpawnIgnoringPlayer = true;
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.puppetSpawn, base.transform.position + Vector3.up * 0.25f, Quaternion.identity).transform.SetParent(this.GetGoreZone().transform, true);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00036727 File Offset: 0x00034927
	public void BuffAll()
	{
		this.damageBuffRequests++;
		this.speedBuffRequests++;
		this.healthBuffRequests++;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0003675B File Offset: 0x0003495B
	public void UnbuffAll()
	{
		this.speedBuffRequests--;
		this.healthBuffRequests--;
		this.damageBuffRequests--;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0003678F File Offset: 0x0003498F
	public void DamageBuff()
	{
		this.DamageBuff(this.damageBuffModifier);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0003679D File Offset: 0x0003499D
	public void DamageBuff(float modifier)
	{
		this.damageBuffRequests++;
		this.damageBuffModifier = modifier;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000367BC File Offset: 0x000349BC
	public void DamageUnbuff()
	{
		this.damageBuffRequests--;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x000367D4 File Offset: 0x000349D4
	public void SpeedBuff()
	{
		this.SpeedBuff(this.speedBuffModifier);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x000367E2 File Offset: 0x000349E2
	public void SpeedBuff(float modifier)
	{
		this.speedBuffRequests++;
		this.speedBuffModifier = modifier;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00036801 File Offset: 0x00034A01
	public void SpeedUnbuff()
	{
		this.speedBuffRequests--;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00036819 File Offset: 0x00034A19
	public void HealthBuff()
	{
		this.HealthBuff(this.healthBuffModifier);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00036827 File Offset: 0x00034A27
	public void HealthBuff(float modifier)
	{
		this.healthBuffRequests++;
		this.healthBuffModifier = modifier;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00036846 File Offset: 0x00034A46
	public void HealthUnbuff()
	{
		this.healthBuffRequests--;
		this.UpdateBuffs(false, true);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00036860 File Offset: 0x00034A60
	public void UpdateBuffs(bool visualsOnly = false, bool particle = true)
	{
		this.speedBuff = this.speedBuffRequests > 0;
		this.healthBuff = this.healthBuffRequests > 0;
		this.damageBuff = this.damageBuffRequests > 0;
		if (this.healthBuff || this.speedBuff || this.damageBuff || OptionsManager.forceRadiance)
		{
			if (!this.hasRadianceEffected)
			{
				this.hasRadianceEffected = true;
				if (particle)
				{
					Collider component = base.GetComponent<Collider>();
					if (component)
					{
						Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.radianceEffect, component.bounds.center, Quaternion.identity);
					}
					else
					{
						Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.radianceEffect, base.transform.position, Quaternion.identity);
					}
				}
			}
		}
		else
		{
			this.hasRadianceEffected = false;
			this.speedBuffRequests = 0;
			this.healthBuffRequests = 0;
			this.damageBuffRequests = 0;
		}
		if (!visualsOnly)
		{
			this.UpdateModifiers();
			base.SendMessage("UpdateBuff", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00036954 File Offset: 0x00034B54
	public static bool CheckHurtException(EnemyType attacker, EnemyType receiver, EnemyTarget attackTarget = null)
	{
		if (EnemyIdentifierDebug.Active)
		{
			EnemyIdentifier.Log.Fine(string.Format("Checking hurt exception between <b>{0}</b> and <b>{1}</b> with attack target <b>{2}</b>", attacker, receiver, attackTarget), null, null, null);
		}
		return attacker == receiver || ((attackTarget == null || !attackTarget.isEnemy || attackTarget.enemyIdentifier.enemyType != receiver) && ((attacker == EnemyType.Stalker && receiver != EnemyType.Swordsmachine) || (receiver == EnemyType.Stalker && attacker != EnemyType.Swordsmachine) || ((attacker == EnemyType.Filth || attacker == EnemyType.Stray || attacker == EnemyType.Schism || attacker == EnemyType.Soldier) && (receiver == EnemyType.Filth || receiver == EnemyType.Stray || receiver == EnemyType.Schism || receiver == EnemyType.Soldier)) || receiver == EnemyType.Sisyphus || receiver == EnemyType.Ferryman || (((attacker == EnemyType.Drone || attacker == EnemyType.Virtue) && receiver == EnemyType.FleshPrison) || ((receiver == EnemyType.Drone || receiver == EnemyType.Virtue) && attacker == EnemyType.FleshPrison)) || (((attacker == EnemyType.Drone || attacker == EnemyType.Virtue) && receiver == EnemyType.FleshPanopticon) || ((receiver == EnemyType.Drone || receiver == EnemyType.Virtue) && attacker == EnemyType.FleshPanopticon)) || ((attacker == EnemyType.Gabriel || attacker == EnemyType.GabrielSecond) && (receiver == EnemyType.Gabriel || receiver == EnemyType.GabrielSecond))));
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00036A50 File Offset: 0x00034C50
	public static void FallOnEnemy(EnemyIdentifier eid)
	{
		if (eid.dead)
		{
			eid.Explode(false);
			return;
		}
		EnemyType enemyType = eid.enemyType;
		if (enemyType <= EnemyType.Sisyphus)
		{
			if (enemyType != EnemyType.Mindflayer)
			{
				if (enemyType != EnemyType.Gabriel)
				{
					if (enemyType == EnemyType.Sisyphus)
					{
						Sisyphus sisyphus;
						if (eid.TryGetComponent<Sisyphus>(out sisyphus))
						{
							eid.DeliverDamage(eid.gameObject, Vector3.zero, eid.transform.position, 22f, true, 0f, null, false, false);
							sisyphus.Knockdown(sisyphus.transform.position + sisyphus.transform.forward);
							return;
						}
						return;
					}
				}
				else
				{
					Gabriel gabriel;
					if (eid.TryGetComponent<Gabriel>(out gabriel))
					{
						gabriel.Teleport(false, false, true, false, false);
						return;
					}
					return;
				}
			}
			else
			{
				Mindflayer mindflayer;
				if (eid.TryGetComponent<Mindflayer>(out mindflayer))
				{
					mindflayer.Teleport(false);
					return;
				}
				return;
			}
		}
		else
		{
			if (enemyType == EnemyType.Idol)
			{
				eid.InstaKill();
				return;
			}
			if (enemyType != EnemyType.Ferryman)
			{
				if (enemyType == EnemyType.GabrielSecond)
				{
					GabrielSecond gabrielSecond;
					if (eid.TryGetComponent<GabrielSecond>(out gabrielSecond))
					{
						gabrielSecond.Teleport(false, false, true, false, false);
						return;
					}
					return;
				}
			}
			else
			{
				Ferryman ferryman;
				if (eid.TryGetComponent<Ferryman>(out ferryman))
				{
					ferryman.Roll(false);
					return;
				}
				return;
			}
		}
		eid.Splatter(false);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00036B6C File Offset: 0x00034D6C
	public void PlayerMarkedForDeath()
	{
		this.attackEnemies = false;
		this.prioritizeEnemiesUnlessAttacked = false;
		this.prioritizePlayerOverFallback = true;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00036B84 File Offset: 0x00034D84
	public void BossBar(bool enable)
	{
		BossHealthBar bossHealthBar = base.GetComponent<BossHealthBar>();
		if (!enable)
		{
			if (bossHealthBar != null)
			{
				Object.Destroy(bossHealthBar);
			}
			return;
		}
		if (bossHealthBar == null)
		{
			bossHealthBar = base.gameObject.AddComponent<BossHealthBar>();
		}
		EnemyType enemyType = this.enemyType;
		if (enemyType == EnemyType.FleshPrison)
		{
			bossHealthBar.SetSecondaryBarColor(Color.green);
			bossHealthBar.secondaryBar = true;
			return;
		}
		if (enemyType != EnemyType.FleshPanopticon)
		{
			return;
		}
		bossHealthBar.SetSecondaryBarColor(new Color(1f, 0.7529412f, 0f));
		bossHealthBar.secondaryBar = true;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00036C06 File Offset: 0x00034E06
	public void ChangeDamageTakenMultiplier(float newMultiplier)
	{
		this.totalDamageTakenMultiplier = newMultiplier;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00036C10 File Offset: 0x00034E10
	public void SimpleDamage(float amount)
	{
		this.DeliverDamage(base.gameObject, Vector3.zero, base.transform.position, amount, false, 0f, null, false, false);
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00036C44 File Offset: 0x00034E44
	public void SimpleDamageIgnoreMultiplier(float amount)
	{
		if (this.totalDamageTakenMultiplier != 0f)
		{
			this.DeliverDamage(base.gameObject, Vector3.zero, base.transform.position, amount / this.totalDamageTakenMultiplier, false, 0f, null, false, false);
			return;
		}
		this.DeliverDamage(base.gameObject, Vector3.zero, base.transform.position, amount, false, 0f, null, true, false);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00036CB4 File Offset: 0x00034EB4
	private void TryUnPuppet()
	{
		if (this.permaPuppet)
		{
			return;
		}
		SandboxEnemy sandboxEnemy;
		if (!base.TryGetComponent<SandboxEnemy>(out sandboxEnemy))
		{
			return;
		}
		if (sandboxEnemy.sourceObject.gameObject == null)
		{
			return;
		}
		this.puppet = false;
		SavedEnemy savedEnemy = sandboxEnemy.SaveEnemy();
		savedEnemy.Scale = SavedVector3.One;
		MonoSingleton<SandboxSaver>.Instance.RebuildObjectList();
		MonoSingleton<SandboxSaver>.Instance.RecreateEnemy(savedEnemy, true);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00036D24 File Offset: 0x00034F24
	private bool IsSandboxEnemy
	{
		get
		{
			SandboxEnemy sandboxEnemy;
			return base.TryGetComponent<SandboxEnemy>(out sandboxEnemy) && sandboxEnemy.sourceObject != null;
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00036D4C File Offset: 0x00034F4C
	public float GetReachDistanceMultiplier()
	{
		EnemyType enemyType = this.enemyType;
		if (enemyType <= EnemyType.Sisyphus)
		{
			if (enemyType != EnemyType.HideousMass && enemyType != EnemyType.Sisyphus)
			{
				goto IL_0029;
			}
		}
		else if (enemyType != EnemyType.SisyphusPrime && enemyType - EnemyType.Gutterman > 1)
		{
			goto IL_0029;
		}
		return 0.5f;
		IL_0029:
		return 1f;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00036D87 File Offset: 0x00034F87
	public Transform GetCenter()
	{
		if (!(this.overrideCenter != null))
		{
			return base.transform;
		}
		return this.overrideCenter;
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00036DA4 File Offset: 0x00034FA4
	public bool IsCurrentTargetFallback
	{
		get
		{
			return this.target != null && this.fallbackTarget != null && this.target.trackedTransform == this.fallbackTarget;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00036DD4 File Offset: 0x00034FD4
	public string FullName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrideFullName))
			{
				return this.overrideFullName;
			}
			return EnemyTypes.GetEnemyName(this.enemyType);
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00036DF5 File Offset: 0x00034FF5
	public float Health
	{
		get
		{
			return this.health;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060007EA RID: 2026 RVA: 0x00036DFD File Offset: 0x00034FFD
	public bool Dead
	{
		get
		{
			return this.dead;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00036E05 File Offset: 0x00035005
	public bool Blessed
	{
		get
		{
			return this.blessed;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x00036E10 File Offset: 0x00035010
	public bool AttackEnemies
	{
		get
		{
			if (BlindEnemies.Blind)
			{
				return false;
			}
			if (EnemiesHateEnemies.Active)
			{
				return true;
			}
			if (this.madness)
			{
				return true;
			}
			IEnemyRelationshipLogic[] array = this.relationshipLogic;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ShouldAttackEnemies())
				{
					return true;
				}
			}
			return this.attackEnemies;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x00036E60 File Offset: 0x00035060
	public bool IgnorePlayer
	{
		get
		{
			if (BlindEnemies.Blind)
			{
				return true;
			}
			if (EnemyIgnorePlayer.Active)
			{
				return true;
			}
			if (this.madness)
			{
				return true;
			}
			IEnemyRelationshipLogic[] array = this.relationshipLogic;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ShouldIgnorePlayer())
				{
					return true;
				}
			}
			return this.ignorePlayer;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060007EE RID: 2030 RVA: 0x00036EB0 File Offset: 0x000350B0
	public string alterKey
	{
		get
		{
			return "enemy-identifier";
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x00036EB7 File Offset: 0x000350B7
	public string alterCategoryName
	{
		get
		{
			return "enemy";
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00036EC0 File Offset: 0x000350C0
	public AlterOption<bool>[] options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					name = "Boss Health Bar",
					key = "health-bar",
					callback = delegate(bool value)
					{
						this.BossBar(value);
					},
					value = (base.GetComponent<BossHealthBar>() != null)
				},
				new AlterOption<bool>
				{
					name = "Sandified",
					key = "sandified",
					callback = ((!this.puppet) ? delegate(bool value)
					{
						if (value)
						{
							this.Sandify(false);
							return;
						}
						this.Desandify(false);
					} : null),
					value = this.sandified
				},
				new AlterOption<bool>
				{
					name = "Puppeted",
					key = "puppeted",
					callback = ((!this.puppet || (this.IsSandboxEnemy && !this.permaPuppet)) ? delegate(bool value)
					{
						if (value && !this.puppet)
						{
							this.PuppetSpawn();
						}
						if (!value && this.puppet && this.IsSandboxEnemy)
						{
							this.TryUnPuppet();
						}
					} : null),
					tooltip = (this.permaPuppet ? "This enemy cannot be un-puppeteered " : ((this.puppet && !this.IsSandboxEnemy) ? "Un-puppeteering is not supported for non-sandbox enemies" : null)),
					value = this.puppet
				},
				new AlterOption<bool>
				{
					name = "Ignore Player",
					key = "ignorePlayer",
					callback = delegate(bool value)
					{
						this.ignorePlayer = value;
					},
					value = this.ignorePlayer
				},
				new AlterOption<bool>
				{
					name = "Attack Enemies",
					key = "attackEnemies",
					callback = delegate(bool value)
					{
						this.attackEnemies = value;
					},
					value = this.attackEnemies
				}
			};
		}
	}

	// Token: 0x04000A24 RID: 2596
	private static readonly global::plog.Logger Log = new global::plog.Logger("EnemyIdentifier");

	// Token: 0x04000A25 RID: 2597
	[HideInInspector]
	public Zombie zombie;

	// Token: 0x04000A26 RID: 2598
	[HideInInspector]
	public SpiderBody spider;

	// Token: 0x04000A27 RID: 2599
	[HideInInspector]
	public Machine machine;

	// Token: 0x04000A28 RID: 2600
	[HideInInspector]
	public Statue statue;

	// Token: 0x04000A29 RID: 2601
	[HideInInspector]
	public Wicked wicked;

	// Token: 0x04000A2A RID: 2602
	[HideInInspector]
	public Drone drone;

	// Token: 0x04000A2B RID: 2603
	[HideInInspector]
	public Idol idol;

	// Token: 0x04000A2C RID: 2604
	public EnemyClass enemyClass;

	// Token: 0x04000A2D RID: 2605
	public EnemyType enemyType;

	// Token: 0x04000A2E RID: 2606
	public bool spawnIn;

	// Token: 0x04000A2F RID: 2607
	public GameObject spawnEffect;

	// Token: 0x04000A30 RID: 2608
	public float health;

	// Token: 0x04000A31 RID: 2609
	[HideInInspector]
	public string hitter;

	// Token: 0x04000A32 RID: 2610
	[HideInInspector]
	public List<HitterAttribute> hitterAttributes = new List<HitterAttribute>();

	// Token: 0x04000A33 RID: 2611
	[HideInInspector]
	public List<string> hitterWeapons = new List<string>();

	// Token: 0x04000A34 RID: 2612
	public string[] weaknesses;

	// Token: 0x04000A35 RID: 2613
	public float[] weaknessMultipliers;

	// Token: 0x04000A36 RID: 2614
	public float totalDamageTakenMultiplier = 1f;

	// Token: 0x04000A37 RID: 2615
	public GameObject weakPoint;

	// Token: 0x04000A38 RID: 2616
	public Transform overrideCenter;

	// Token: 0x04000A39 RID: 2617
	[HideInInspector]
	public bool exploded;

	// Token: 0x04000A3A RID: 2618
	public bool dead;

	// Token: 0x04000A3B RID: 2619
	[HideInInspector]
	public DoorController usingDoor;

	// Token: 0x04000A3C RID: 2620
	public bool ignoredByEnemies;

	// Token: 0x04000A3D RID: 2621
	private EnemyIdentifierIdentifier[] limbs;

	// Token: 0x04000A3E RID: 2622
	[HideInInspector]
	public int nailsAmount;

	// Token: 0x04000A3F RID: 2623
	[HideInInspector]
	public List<Nail> nails = new List<Nail>();

	// Token: 0x04000A40 RID: 2624
	public bool useBrakes;

	// Token: 0x04000A41 RID: 2625
	public bool bigEnemy;

	// Token: 0x04000A42 RID: 2626
	public bool unbounceable;

	// Token: 0x04000A43 RID: 2627
	public bool poise;

	// Token: 0x04000A44 RID: 2628
	public bool immuneToFriendlyFire;

	// Token: 0x04000A45 RID: 2629
	[HideInInspector]
	public bool beingZapped;

	// Token: 0x04000A46 RID: 2630
	[HideInInspector]
	public TimeSince lastZapped;

	// Token: 0x04000A47 RID: 2631
	[HideInInspector]
	public bool pulledByMagnet;

	// Token: 0x04000A48 RID: 2632
	[HideInInspector]
	public List<Magnet> stuckMagnets = new List<Magnet>();

	// Token: 0x04000A49 RID: 2633
	[HideInInspector]
	public List<Harpoon> drillers = new List<Harpoon>();

	// Token: 0x04000A4A RID: 2634
	[HideInInspector]
	public bool underwater;

	// Token: 0x04000A4B RID: 2635
	[HideInInspector]
	public HashSet<Water> touchingWaters = new HashSet<Water>();

	// Token: 0x04000A4C RID: 2636
	[HideInInspector]
	public bool checkingSpawnStatus = true;

	// Token: 0x04000A4D RID: 2637
	public bool flying;

	// Token: 0x04000A4E RID: 2638
	public bool dontCountAsKills;

	// Token: 0x04000A4F RID: 2639
	public bool dontUnlockBestiary;

	// Token: 0x04000A50 RID: 2640
	public bool specialOob;

	// Token: 0x04000A51 RID: 2641
	public GameObject[] activateOnDeath;

	// Token: 0x04000A52 RID: 2642
	public UnityEvent onDeath;

	// Token: 0x04000A53 RID: 2643
	public UltrakillEvent onEnable;

	// Token: 0x04000A54 RID: 2644
	private BloodsplatterManager bsm;

	// Token: 0x04000A55 RID: 2645
	[HideInInspector]
	public GroundCheckEnemy gce;

	// Token: 0x04000A56 RID: 2646
	private GoreZone gz;

	// Token: 0x04000A57 RID: 2647
	private Rigidbody rb;

	// Token: 0x04000A58 RID: 2648
	private RigidbodyConstraints rbc;

	// Token: 0x04000A59 RID: 2649
	private List<GameObject> sandifiedParticles = new List<GameObject>();

	// Token: 0x04000A5A RID: 2650
	[HideInInspector]
	public List<GameObject> blessingGlows = new List<GameObject>();

	// Token: 0x04000A5B RID: 2651
	[HideInInspector]
	public EnemyIdentifier buffTargeter;

	// Token: 0x04000A5C RID: 2652
	public int difficultyOverride = -1;

	// Token: 0x04000A5D RID: 2653
	private int difficulty;

	// Token: 0x04000A5E RID: 2654
	[HideInInspector]
	public bool hooked;

	// Token: 0x04000A5F RID: 2655
	public List<Flammable> burners;

	// Token: 0x04000A60 RID: 2656
	[HideInInspector]
	public List<Flammable> flammables;

	// Token: 0x04000A61 RID: 2657
	private bool getFireDamageMultiplier;

	// Token: 0x04000A62 RID: 2658
	[HideInInspector]
	public bool beenGasolined;

	// Token: 0x04000A63 RID: 2659
	[HideInInspector]
	public bool harpooned;

	// Token: 0x04000A64 RID: 2660
	[HideInInspector]
	public Zapper zapperer;

	// Token: 0x04000A65 RID: 2661
	private GameObject afterShockSourceWeapon;

	// Token: 0x04000A66 RID: 2662
	private bool waterOnlyAftershock;

	// Token: 0x04000A67 RID: 2663
	private bool afterShockFromZap;

	// Token: 0x04000A68 RID: 2664
	[Header("Modifiers")]
	public bool hookIgnore;

	// Token: 0x04000A69 RID: 2665
	public bool sandified;

	// Token: 0x04000A6A RID: 2666
	public bool blessed;

	// Token: 0x04000A6B RID: 2667
	public bool puppet;

	// Token: 0x04000A6C RID: 2668
	private bool permaPuppet;

	// Token: 0x04000A6D RID: 2669
	private int blessings;

	// Token: 0x04000A6E RID: 2670
	private float puppetSpawnTimer;

	// Token: 0x04000A6F RID: 2671
	[HideInInspector]
	public Vector3 squishedScale;

	// Token: 0x04000A70 RID: 2672
	[HideInInspector]
	public Vector3 originalScale;

	// Token: 0x04000A71 RID: 2673
	private List<Renderer> puppetRenderers = new List<Renderer>();

	// Token: 0x04000A72 RID: 2674
	private bool puppetSpawnIgnoringPlayer;

	// Token: 0x04000A73 RID: 2675
	private Collider[] puppetSpawnColliders;

	// Token: 0x04000A74 RID: 2676
	public float radianceTier = 1f;

	// Token: 0x04000A75 RID: 2677
	public bool healthBuff;

	// Token: 0x04000A76 RID: 2678
	public float healthBuffModifier = 1.5f;

	// Token: 0x04000A77 RID: 2679
	[HideInInspector]
	public int healthBuffRequests;

	// Token: 0x04000A78 RID: 2680
	public bool speedBuff;

	// Token: 0x04000A79 RID: 2681
	public float speedBuffModifier = 1.5f;

	// Token: 0x04000A7A RID: 2682
	[HideInInspector]
	public int speedBuffRequests;

	// Token: 0x04000A7B RID: 2683
	public bool damageBuff;

	// Token: 0x04000A7C RID: 2684
	public float damageBuffModifier = 1.5f;

	// Token: 0x04000A7D RID: 2685
	[HideInInspector]
	public int damageBuffRequests;

	// Token: 0x04000A7E RID: 2686
	[HideInInspector]
	public bool hasRadianceEffected;

	// Token: 0x04000A7F RID: 2687
	[HideInInspector]
	public float totalSpeedModifier = 1f;

	// Token: 0x04000A80 RID: 2688
	[HideInInspector]
	public float totalDamageModifier = 1f;

	// Token: 0x04000A81 RID: 2689
	[HideInInspector]
	public float totalHealthModifier = 1f;

	// Token: 0x04000A82 RID: 2690
	[HideInInspector]
	public bool isBoss;

	// Token: 0x04000A83 RID: 2691
	[Space(10f)]
	public List<Renderer> buffUnaffectedRenderers = new List<Renderer>();

	// Token: 0x04000A84 RID: 2692
	[SerializeField]
	private string overrideFullName;

	// Token: 0x04000A85 RID: 2693
	[Header("Relationships")]
	public bool ignorePlayer;

	// Token: 0x04000A86 RID: 2694
	public bool attackEnemies;

	// Token: 0x04000A87 RID: 2695
	public EnemyTarget target;

	// Token: 0x04000A88 RID: 2696
	public bool prioritizePlayerOverFallback = true;

	// Token: 0x04000A89 RID: 2697
	public bool prioritizeEnemiesUnlessAttacked;

	// Token: 0x04000A8A RID: 2698
	public Transform fallbackTarget;

	// Token: 0x04000A8B RID: 2699
	[HideInInspector]
	public bool madness;

	// Token: 0x04000A8C RID: 2700
	[HideInInspector]
	public TimeSince timeSinceSpawned;

	// Token: 0x04000A8D RID: 2701
	private TimeSince? timeSinceNoTarget;

	// Token: 0x04000A8E RID: 2702
	[HideInInspector]
	public EnemyScanner enemyScanner;

	// Token: 0x04000A8F RID: 2703
	private IEnemyRelationshipLogic[] relationshipLogic;

	// Token: 0x04000A90 RID: 2704
	private EnemyIdentifierDebugOverlay debugOverlay;

	// Token: 0x04000A91 RID: 2705
	private BossHealthBar cheatCreatedBossBar;

	// Token: 0x04000A92 RID: 2706
	[HideInInspector]
	public List<GameObject> destroyOnDeath = new List<GameObject>();

	// Token: 0x04000A93 RID: 2707
	private static readonly int HasSandBuff = Shader.PropertyToID("_HasSandBuff");

	// Token: 0x04000A94 RID: 2708
	private static readonly int NewSanded = Shader.PropertyToID("_Sanded");
}
