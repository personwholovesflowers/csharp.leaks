using System;
using System.Collections.Generic;
using plog;
using UnityEngine;
using UnityEngine.AI;

namespace Sandbox
{
	// Token: 0x02000562 RID: 1378
	public class SandboxEnemy : SandboxSpawnableInstance
	{
		// Token: 0x06001F03 RID: 7939 RVA: 0x000FE3A9 File Offset: 0x000FC5A9
		public override void Awake()
		{
			base.Awake();
			this.enemyId = base.GetComponent<EnemyIdentifier>();
			if (this.enemyId == null)
			{
				this.enemyId = base.GetComponentInChildren<EnemyIdentifier>();
			}
			this.radiance = new EnemyRadianceConfig(this.enemyId);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x000FE3E8 File Offset: 0x000FC5E8
		public void RestoreRadiance(EnemyRadianceConfig config)
		{
			this.radiance = config;
			if (config == null)
			{
				return;
			}
			this.UpdateRadiance();
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000FE3FC File Offset: 0x000FC5FC
		public void UpdateRadiance()
		{
			this.enemyId.radianceTier = this.radiance.tier;
			if (!this.lastSpeedBuffState && this.radiance.speedEnabled)
			{
				this.enemyId.SpeedBuff(this.radiance.speedBuff);
			}
			else if (this.lastSpeedBuffState && !this.radiance.speedEnabled)
			{
				this.enemyId.SpeedUnbuff();
			}
			this.enemyId.speedBuffModifier = this.radiance.speedBuff;
			if (!this.lastDamageBuffState && this.radiance.damageEnabled)
			{
				this.enemyId.DamageBuff(this.radiance.damageBuff);
			}
			else if (this.lastDamageBuffState && !this.radiance.damageEnabled)
			{
				this.enemyId.DamageUnbuff();
			}
			this.enemyId.damageBuffModifier = this.radiance.damageBuff;
			if (!this.lastHealthBuffState && this.radiance.healthEnabled)
			{
				this.enemyId.HealthBuff(this.radiance.healthBuff);
			}
			else if (this.lastHealthBuffState && !this.radiance.healthEnabled)
			{
				this.enemyId.HealthUnbuff();
			}
			this.enemyId.healthBuffModifier = this.radiance.healthBuff;
			this.lastSpeedBuffState = this.radiance.speedEnabled;
			this.lastDamageBuffState = this.radiance.damageEnabled;
			this.lastHealthBuffState = this.radiance.healthEnabled;
			this.enemyId.UpdateBuffs(false, true);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000FE588 File Offset: 0x000FC788
		private void OnEnable()
		{
			this.enemyId = base.GetComponent<EnemyIdentifier>();
			if (this.enemyId)
			{
				return;
			}
			this.enemyId = base.GetComponentInChildren<EnemyIdentifier>();
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x000FE5B0 File Offset: 0x000FC7B0
		public SavedEnemy SaveEnemy()
		{
			if (!this.enemyId || this.enemyId.health < 0f || this.enemyId.dead)
			{
				return null;
			}
			SavedEnemy savedEnemy = new SavedEnemy
			{
				Radiance = this.radiance
			};
			SavedGeneric savedGeneric = savedEnemy;
			base.BaseSave(ref savedGeneric);
			if (this.enemyId.originalScale != Vector3.zero)
			{
				savedEnemy.Scale = SavedVector3.One;
			}
			return savedEnemy;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x000FE62C File Offset: 0x000FC82C
		public override void Pause(bool freeze = true)
		{
			base.Pause(freeze);
			GameObject gameObject = this.collider.gameObject;
			EnemyIdentifier enemyIdentifier;
			if (this.collider.gameObject.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
			{
				EnemyIdentifier enemyIdentifier2 = enemyIdentifier;
				enemyIdentifier2.enabled = false;
			}
			else
			{
				EnemyIdentifier enemyIdentifier2 = this.collider.gameObject.GetComponentInChildren<EnemyIdentifier>();
				if (enemyIdentifier2 != null)
				{
					enemyIdentifier2.enabled = false;
					gameObject = enemyIdentifier2.gameObject;
				}
			}
			foreach (Component component in this.GetEnemyComponents(gameObject))
			{
				((Behaviour)component).enabled = false;
			}
			NavMeshAgent navMeshAgent;
			if (gameObject.TryGetComponent<NavMeshAgent>(out navMeshAgent))
			{
				navMeshAgent.enabled = false;
			}
			Animator animator;
			if (gameObject.TryGetComponent<Animator>(out animator))
			{
				animator.enabled = false;
			}
			Rigidbody rigidbody;
			if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
			{
				this.lastKinematicState = rigidbody.isKinematic;
				if (rigidbody.collisionDetectionMode == CollisionDetectionMode.ContinuousDynamic)
				{
					rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
				}
				rigidbody.isKinematic = true;
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x000FE734 File Offset: 0x000FC934
		public override void Resume()
		{
			base.Resume();
			if (this.collider == null)
			{
				return;
			}
			foreach (Component component in this.GetEnemyComponents(this.collider.gameObject))
			{
				((Behaviour)component).enabled = true;
			}
			NavMeshAgent navMeshAgent;
			if (this.collider.gameObject.TryGetComponent<NavMeshAgent>(out navMeshAgent))
			{
				navMeshAgent.enabled = true;
			}
			EnemyIdentifier enemyIdentifier;
			if (this.collider.gameObject.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
			{
				enemyIdentifier.enabled = true;
			}
			Animator animator;
			if (this.collider.gameObject.TryGetComponent<Animator>(out animator))
			{
				animator.enabled = true;
			}
			Rigidbody rigidbody;
			if (this.collider.gameObject.TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody.isKinematic = this.lastKinematicState;
			}
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x000FE81C File Offset: 0x000FCA1C
		private IEnumerable<Component> GetEnemyComponents(GameObject obj)
		{
			foreach (Type type in EnemyTypes.types)
			{
				Component component2;
				if (this.sourceObject.fullEnemyComponent)
				{
					Component[] componentsInChildren = obj.GetComponentsInChildren(type);
					foreach (Component component in componentsInChildren)
					{
						yield return component;
					}
					Component[] array = null;
				}
				else if (obj.TryGetComponent(type, out component2))
				{
					yield return component2;
				}
			}
			HashSet<Type>.Enumerator enumerator = default(HashSet<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000FE833 File Offset: 0x000FCA33
		private void Update()
		{
			if (this.enemyId != null)
			{
				return;
			}
			SandboxEnemy.Log.Fine("Destroying sandbox enemy due to missing EnemyIdentifier component.", null, null, null);
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04002B71 RID: 11121
		private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxEnemy");

		// Token: 0x04002B72 RID: 11122
		public EnemyIdentifier enemyId;

		// Token: 0x04002B73 RID: 11123
		public EnemyRadianceConfig radiance;

		// Token: 0x04002B74 RID: 11124
		private bool lastSpeedBuffState;

		// Token: 0x04002B75 RID: 11125
		private bool lastDamageBuffState;

		// Token: 0x04002B76 RID: 11126
		private bool lastHealthBuffState;

		// Token: 0x04002B77 RID: 11127
		private bool lastKinematicState;
	}
}
