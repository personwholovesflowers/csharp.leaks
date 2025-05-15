using System;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class ItemlessChecker : MonoBehaviour
{
	// Token: 0x06000F0F RID: 3855 RVA: 0x0006FB02 File Offset: 0x0006DD02
	private void Start()
	{
		MonoSingleton<ChallengeDoneByDefault>.Instance.Prepare();
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0006FB94 File Offset: 0x0006DD94
	private void Update()
	{
		if (!this.failed)
		{
			if (this.itid == null)
			{
				this.itid = base.GetComponent<ItemIdentifier>();
			}
			if (this.itid != null && this.itid.pickedUp)
			{
				MonoSingleton<ChallengeManager>.Instance.challengeFailed = true;
				MonoSingleton<ChallengeManager>.Instance.challengeFailedPermanently = true;
				this.failed = true;
			}
		}
	}

	// Token: 0x0400143A RID: 5178
	private ItemIdentifier itid;

	// Token: 0x0400143B RID: 5179
	private bool failed;
}
