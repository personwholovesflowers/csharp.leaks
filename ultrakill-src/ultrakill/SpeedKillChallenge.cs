using System;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class SpeedKillChallenge : MonoBehaviour
{
	// Token: 0x06000F1E RID: 3870 RVA: 0x0006FDFC File Offset: 0x0006DFFC
	private void Start()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0006FE0C File Offset: 0x0006E00C
	private void Update()
	{
		if (this.timeLeft <= 0f)
		{
			Object.Destroy(this);
			return;
		}
		if (this.eid.dead)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
			return;
		}
		this.timeLeft = Mathf.MoveTowards(this.timeLeft, 0f, Time.deltaTime);
	}

	// Token: 0x04001447 RID: 5191
	public float timeLeft;

	// Token: 0x04001448 RID: 5192
	private EnemyIdentifier eid;
}
