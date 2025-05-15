using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class BossIdentifier : MonoBehaviour
{
	// Token: 0x060002D2 RID: 722 RVA: 0x00010992 File Offset: 0x0000EB92
	private void Awake()
	{
		this.CheckDifficultyOverride();
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00010992 File Offset: 0x0000EB92
	private void OnEnable()
	{
		this.CheckDifficultyOverride();
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0001099C File Offset: 0x0000EB9C
	public void CheckDifficultyOverride()
	{
		if (!this.eid && !base.TryGetComponent<EnemyIdentifier>(out this.eid))
		{
			if (MonoSingleton<AssistController>.Instance.majorEnabled && (this.alac || base.TryGetComponent<AlwaysLookAtCamera>(out this.alac)))
			{
				this.alac.ChangeDifficulty(MonoSingleton<AssistController>.Instance.difficultyOverride);
				return;
			}
			Object.Destroy(this);
			return;
		}
		else
		{
			if (MonoSingleton<AssistController>.Instance.majorEnabled)
			{
				this.eid.difficultyOverride = MonoSingleton<AssistController>.Instance.difficultyOverride;
				return;
			}
			this.eid.difficultyOverride = -1;
			return;
		}
	}

	// Token: 0x04000362 RID: 866
	private EnemyIdentifier eid;

	// Token: 0x04000363 RID: 867
	private AlwaysLookAtCamera alac;
}
