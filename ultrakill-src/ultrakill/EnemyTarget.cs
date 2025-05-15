using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200019B RID: 411
public class EnemyTarget
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x00039833 File Offset: 0x00037A33
	public bool isEnemy
	{
		get
		{
			return !this.isPlayer && this.enemyIdentifier != null;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000849 RID: 2121 RVA: 0x0003984C File Offset: 0x00037A4C
	public Vector3 position
	{
		get
		{
			if (this.enemyIdentifier != null && this.enemyIdentifier.overrideCenter)
			{
				return this.enemyIdentifier.overrideCenter.position;
			}
			if (!(this.targetTransform != null))
			{
				return Vector3.zero;
			}
			return this.targetTransform.position;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x000398A9 File Offset: 0x00037AA9
	public Vector3 headPosition
	{
		get
		{
			return this.headTransform.position;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600084B RID: 2123 RVA: 0x000398B6 File Offset: 0x00037AB6
	public Transform headTransform
	{
		get
		{
			if (!(this.enemyIdentifier == null) || !this.isPlayer || MonoSingleton<PlayerTracker>.Instance.playerType != PlayerType.FPS)
			{
				return this.trackedTransform;
			}
			return MonoSingleton<CameraController>.Instance.cam.transform;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x000398F0 File Offset: 0x00037AF0
	public Transform trackedTransform
	{
		get
		{
			if (!(this.enemyIdentifier != null) || !this.enemyIdentifier.overrideCenter)
			{
				return this.targetTransform;
			}
			return this.enemyIdentifier.overrideCenter;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x0600084D RID: 2125 RVA: 0x00039924 File Offset: 0x00037B24
	public Vector3 forward
	{
		get
		{
			return this.targetTransform.forward;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x00039931 File Offset: 0x00037B31
	public Vector3 right
	{
		get
		{
			return this.targetTransform.right;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x0003993E File Offset: 0x00037B3E
	public bool isOnGround
	{
		get
		{
			return !this.isPlayer || MonoSingleton<PlayerTracker>.Instance.GetOnGround();
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00039954 File Offset: 0x00037B54
	public bool IsTargetTransform(Transform other)
	{
		if (this.isPlayer)
		{
			return other == MonoSingleton<PlayerTracker>.Instance.GetPlayer().parent;
		}
		return other == this.targetTransform;
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x00039980 File Offset: 0x00037B80
	public bool isValid
	{
		get
		{
			return this.targetTransform != null && this.targetTransform.gameObject.activeInHierarchy && (this.enemyIdentifier == null || !this.enemyIdentifier.dead);
		}
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000399CD File Offset: 0x00037BCD
	public EnemyTarget(Transform targetTransform)
	{
		this.isPlayer = false;
		this.targetTransform = targetTransform;
		this.enemyIdentifier = this.targetTransform.GetComponent<EnemyIdentifier>();
		this.rigidbody = this.targetTransform.GetComponent<Rigidbody>();
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00039A08 File Offset: 0x00037C08
	public EnemyTarget(EnemyIdentifier otherEnemy)
	{
		this.isPlayer = false;
		this.targetTransform = otherEnemy.transform;
		this.enemyIdentifier = otherEnemy;
		this.enemyIdentifier = this.targetTransform.GetComponent<EnemyIdentifier>();
		this.rigidbody = this.targetTransform.GetComponent<Rigidbody>();
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00039A58 File Offset: 0x00037C58
	public Vector3 GetVelocity()
	{
		if (this.isPlayer)
		{
			return MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false);
		}
		if (this.targetTransform == null)
		{
			return Vector3.zero;
		}
		NavMeshAgent navMeshAgent;
		if (this.targetTransform.TryGetComponent<NavMeshAgent>(out navMeshAgent) && navMeshAgent.enabled)
		{
			return navMeshAgent.velocity;
		}
		if (this.rigidbody != null)
		{
			return this.rigidbody.velocity;
		}
		return Vector3.zero;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00039ACC File Offset: 0x00037CCC
	public Vector3 PredictTargetPosition(float time, bool includeGravity = false)
	{
		Vector3 vector = this.GetVelocity() * time;
		if (this.rigidbody != null)
		{
			if (includeGravity && ((this.isEnemy && !this.rigidbody.isKinematic) || (this.isPlayer && !MonoSingleton<NewMovement>.Instance.gc.onGround)))
			{
				vector += 0.5f * Physics.gravity * (time * time);
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(this.rigidbody.position, vector, out raycastHit, vector.magnitude, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				vector = raycastHit.point;
			}
			else
			{
				vector += this.rigidbody.position;
			}
		}
		else if (this.targetTransform)
		{
			vector += this.targetTransform.position;
		}
		return vector;
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00039BAC File Offset: 0x00037DAC
	private EnemyTarget()
	{
		this.isPlayer = false;
		this.targetTransform = null;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00039BC4 File Offset: 0x00037DC4
	public static EnemyTarget TrackPlayer()
	{
		PlayerTracker instance = MonoSingleton<PlayerTracker>.Instance;
		return new EnemyTarget
		{
			isPlayer = true,
			targetTransform = instance.GetPlayer().transform,
			rigidbody = instance.GetRigidbody()
		};
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00039C00 File Offset: 0x00037E00
	public static EnemyTarget TrackPlayerIfAllowed()
	{
		if (EnemyIgnorePlayer.Active || BlindEnemies.Blind)
		{
			return null;
		}
		return EnemyTarget.TrackPlayer();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00039C18 File Offset: 0x00037E18
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			this.isPlayer ? "Player: " : (this.isEnemy ? "Enemy: " : "Custom Target: "),
			this.targetTransform.name,
			" (",
			this.targetTransform.position.ToString(),
			")"
		});
	}

	// Token: 0x04000B25 RID: 2853
	public bool isPlayer;

	// Token: 0x04000B26 RID: 2854
	public Transform targetTransform;

	// Token: 0x04000B27 RID: 2855
	public EnemyIdentifier enemyIdentifier;

	// Token: 0x04000B28 RID: 2856
	public Rigidbody rigidbody;
}
