using System;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class SecretMissionPit : MonoBehaviour
{
	// Token: 0x06001672 RID: 5746 RVA: 0x000B4E1C File Offset: 0x000B301C
	private void OnTriggerEnter(Collider other)
	{
		if (this.done)
		{
			return;
		}
		if (other.gameObject.CompareTag("Player"))
		{
			this.done = true;
			if (this.primeMission)
			{
				if (this.halfUnlock)
				{
					GameProgressSaver.SetPrime(this.missionNumber, 1);
					return;
				}
				GameProgressSaver.SetPrime(this.missionNumber, 2);
				return;
			}
			else
			{
				if (this.halfUnlock)
				{
					GameProgressSaver.FoundSecretMission(this.missionNumber);
					return;
				}
				GameProgressSaver.SetSecretMission(this.missionNumber);
			}
		}
	}

	// Token: 0x04001F04 RID: 7940
	public int missionNumber;

	// Token: 0x04001F05 RID: 7941
	public bool halfUnlock;

	// Token: 0x04001F06 RID: 7942
	public bool primeMission;

	// Token: 0x04001F07 RID: 7943
	private bool done;
}
