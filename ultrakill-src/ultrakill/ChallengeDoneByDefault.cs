using System;

// Token: 0x020002AE RID: 686
[ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
public class ChallengeDoneByDefault : MonoSingleton<ChallengeDoneByDefault>
{
	// Token: 0x06000EFA RID: 3834 RVA: 0x0006F750 File Offset: 0x0006D950
	private void Start()
	{
		if (!this.prepared)
		{
			this.Prepare();
		}
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0006F760 File Offset: 0x0006D960
	public void Prepare()
	{
		if (!this.prepared)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
			this.prepared = true;
		}
	}

	// Token: 0x04001423 RID: 5155
	private bool prepared;
}
