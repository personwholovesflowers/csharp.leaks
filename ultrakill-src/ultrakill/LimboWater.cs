using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class LimboWater : MonoBehaviour
{
	// Token: 0x06001B81 RID: 7041 RVA: 0x000E41A2 File Offset: 0x000E23A2
	private void OnEnable()
	{
		this.nm = MonoSingleton<NewMovement>.Instance;
		this.et = MonoSingleton<EnemyTracker>.Instance;
		this.cb = new ComputeBuffer(256, LimboWater.stride, ComputeBufferType.Structured);
		this.positions = new Vector3[256];
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000E41E4 File Offset: 0x000E23E4
	private void Update()
	{
		List<EnemyIdentifier> currentEnemies = this.et.GetCurrentEnemies();
		int num = Math.Min(255, currentEnemies.Count);
		Shader.SetGlobalInteger("_FadePositionsCount", num + 1);
		Array.Clear(this.positions, 0, this.positions.Length);
		this.positions[0] = this.nm.transform.position;
		for (int i = 0; i < num; i++)
		{
			EnemyIdentifier enemyIdentifier = currentEnemies[i];
			if (!enemyIdentifier.dead)
			{
				this.positions[i + 1] = enemyIdentifier.transform.position;
			}
		}
		this.cb.SetData(this.positions);
		Shader.SetGlobalBuffer("_FadePositions", this.cb);
	}

	// Token: 0x040026C5 RID: 9925
	private NewMovement nm;

	// Token: 0x040026C6 RID: 9926
	private EnemyTracker et;

	// Token: 0x040026C7 RID: 9927
	private ComputeBuffer cb;

	// Token: 0x040026C8 RID: 9928
	private static int stride = 12;

	// Token: 0x040026C9 RID: 9929
	private Vector3[] positions;
}
