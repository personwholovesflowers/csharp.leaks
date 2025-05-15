using System;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class SecretMissionChecker : MonoBehaviour
{
	// Token: 0x0600166B RID: 5739 RVA: 0x000B4C20 File Offset: 0x000B2E20
	private void Start()
	{
		if (this.primeMission && GameProgressSaver.GetPrime(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0), this.secretMission) == 2)
		{
			this.onMissionGet.Invoke("");
			return;
		}
		if (!this.primeMission)
		{
			int num = GameProgressSaver.GetSecretMission(this.secretMission);
			if (this.requireCompletion && num == 2)
			{
				this.onMissionGet.Invoke("");
				return;
			}
			if (!this.requireCompletion && num >= 1)
			{
				this.onMissionGet.Invoke("");
			}
		}
	}

	// Token: 0x04001EF9 RID: 7929
	public bool requireCompletion = true;

	// Token: 0x04001EFA RID: 7930
	public bool primeMission;

	// Token: 0x04001EFB RID: 7931
	public int secretMission;

	// Token: 0x04001EFC RID: 7932
	public UltrakillEvent onMissionGet;
}
