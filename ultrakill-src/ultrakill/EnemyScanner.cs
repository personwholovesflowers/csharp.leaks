using System;
using System.Collections.Generic;
using System.Linq;
using plog;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class EnemyScanner
{
	// Token: 0x0600081F RID: 2079 RVA: 0x000381AE File Offset: 0x000363AE
	public EnemyScanner(EnemyIdentifier owner)
	{
		this.owner = owner;
		this.ownerRaycastOrigin = owner.GetCenter();
		this.tickInterval = 0.5f + Random.Range(0f, 0.3f);
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000381E4 File Offset: 0x000363E4
	public void Update()
	{
		if (this.owner == null || this.owner.dead)
		{
			this.Reset();
			return;
		}
		if (!this.owner.AttackEnemies)
		{
			this.Reset();
			return;
		}
		if (this.owner.target != null && !this.owner.IsCurrentTargetFallback && this.owner.target.isValid)
		{
			this.Reset();
			return;
		}
		if (this.pendingLineOfSightChecks != null)
		{
			if (this.pendingLineOfSightChecks.Count <= 0)
			{
				this.Reset();
				return;
			}
			EnemyIdentifier enemyIdentifier = this.pendingLineOfSightChecks.Dequeue();
			if (enemyIdentifier == null || enemyIdentifier.dead || enemyIdentifier.ignoredByEnemies)
			{
				return;
			}
			Vector3 position = this.ownerRaycastOrigin.position;
			Vector3 position2 = enemyIdentifier.GetCenter().position;
			Ray ray = new Ray(position, position2 - position);
			float num = Vector3.Distance(position, position2);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, num, LayerMaskDefaults.Get(LMD.Environment)))
			{
				return;
			}
			this.SetTarget(enemyIdentifier);
			this.pendingLineOfSightChecks.Clear();
			return;
		}
		else
		{
			if (this.timeSinceLastTick == null)
			{
				this.timeSinceLastTick = new TimeSince?(0f);
				return;
			}
			TimeSince? timeSince = this.timeSinceLastTick;
			float? num2 = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
			float num3 = this.tickInterval;
			if ((num2.GetValueOrDefault() > num3) & (num2 != null))
			{
				this.Tick();
			}
			return;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0003836C File Offset: 0x0003656C
	public void Reset()
	{
		this.pendingLineOfSightChecks = null;
		this.timeSinceLastTick = new TimeSince?(0f);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0003838C File Offset: 0x0003658C
	private void Tick()
	{
		this.timeSinceLastTick = new TimeSince?(0f);
		IEnumerable<EnemyIdentifier> enumerable = MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies();
		if (enumerable == null)
		{
			return;
		}
		enumerable = enumerable.Where(new Func<EnemyIdentifier, bool>(this.CanBeTargeted));
		enumerable = enumerable.OrderBy((EnemyIdentifier e) => Vector3.Distance(this.owner.GetCenter().position, e.GetCenter().position)).ToList<EnemyIdentifier>();
		this.pendingLineOfSightChecks = new Queue<EnemyIdentifier>(enumerable);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x000383F4 File Offset: 0x000365F4
	private bool CanBeTargeted(EnemyIdentifier enemy)
	{
		return !(enemy == null) && !enemy.dead && !enemy.ignoredByEnemies && !(enemy == this.owner) && !this.owner.IsTypeFriendly(enemy);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00038434 File Offset: 0x00036634
	private void SetTarget(EnemyIdentifier enemy)
	{
		EnemyTarget enemyTarget = new EnemyTarget(enemy);
		if (enemyTarget.isValid)
		{
			this.owner.target = enemyTarget;
		}
	}

	// Token: 0x04000ACE RID: 2766
	private static readonly global::plog.Logger Log = new global::plog.Logger("EnemyScanner");

	// Token: 0x04000ACF RID: 2767
	private const bool DebugMode = false;

	// Token: 0x04000AD0 RID: 2768
	private readonly EnemyIdentifier owner;

	// Token: 0x04000AD1 RID: 2769
	private readonly Transform ownerRaycastOrigin;

	// Token: 0x04000AD2 RID: 2770
	private readonly float tickInterval;

	// Token: 0x04000AD3 RID: 2771
	private TimeSince? timeSinceLastTick;

	// Token: 0x04000AD4 RID: 2772
	private Queue<EnemyIdentifier> pendingLineOfSightChecks;
}
