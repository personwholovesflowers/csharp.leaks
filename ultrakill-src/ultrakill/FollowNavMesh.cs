using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020001FD RID: 509
public class FollowNavMesh : MonoBehaviour
{
	// Token: 0x06000A63 RID: 2659 RVA: 0x000492E6 File Offset: 0x000474E6
	private void Start()
	{
		this.nma = base.GetComponent<NavMeshAgent>();
		if (!this.target)
		{
			this.target = MonoSingleton<PlayerTracker>.Instance.GetPlayer();
		}
		base.Invoke("Track", this.trackFrequency);
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00049324 File Offset: 0x00047524
	private void Track()
	{
		base.Invoke("Track", this.trackFrequency);
		Transform transform = this.target;
		if (this.chaseEnemies)
		{
			List<EnemyIdentifier> currentEnemies = MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies();
			float num = this.chaseEnemiesRange;
			foreach (EnemyIdentifier enemyIdentifier in currentEnemies)
			{
				if (!enemyIdentifier.flying && Vector3.Distance(base.transform.position, enemyIdentifier.transform.position) < num)
				{
					transform = enemyIdentifier.transform;
					num = Vector3.Distance(base.transform.position, enemyIdentifier.transform.position);
				}
			}
		}
		if (transform != this.target)
		{
			this.nma.stoppingDistance = 0f;
		}
		else
		{
			this.nma.stoppingDistance = 10f;
		}
		RaycastHit raycastHit;
		if (transform && Physics.Raycast(transform.position, Vector3.down, out raycastHit, 50f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.nma.SetDestination(transform.position);
		}
	}

	// Token: 0x04000DCE RID: 3534
	public Transform target;

	// Token: 0x04000DCF RID: 3535
	private NavMeshAgent nma;

	// Token: 0x04000DD0 RID: 3536
	public float trackFrequency = 0.1f;

	// Token: 0x04000DD1 RID: 3537
	public bool chaseEnemies;

	// Token: 0x04000DD2 RID: 3538
	public float chaseEnemiesRange = 50f;
}
