using System;
using UnityEngine;

// Token: 0x020002B2 RID: 690
public class DoorlessChecker : MonoBehaviour
{
	// Token: 0x06000F0A RID: 3850 RVA: 0x0006FB02 File Offset: 0x0006DD02
	private void Start()
	{
		MonoSingleton<ChallengeDoneByDefault>.Instance.Prepare();
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0006FB10 File Offset: 0x0006DD10
	private void Update()
	{
		if (!this.failed)
		{
			if (this.dr == null)
			{
				this.dr = base.GetComponent<Door>();
			}
			if (this.dr != null && this.dr.open)
			{
				MonoSingleton<ChallengeManager>.Instance.challengeFailed = true;
				this.failed = true;
			}
		}
	}

	// Token: 0x04001433 RID: 5171
	private Door dr;

	// Token: 0x04001434 RID: 5172
	private bool failed;
}
