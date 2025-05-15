using System;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class SetPlayerPref : MonoBehaviour
{
	// Token: 0x06001687 RID: 5767 RVA: 0x000B5253 File Offset: 0x000B3453
	private void Start()
	{
		if (this.newSystem)
		{
			MonoSingleton<PrefsManager>.Instance.SetInt(this.playerPref, this.intValue);
			return;
		}
		PlayerPrefs.SetInt(this.playerPref, this.intValue);
	}

	// Token: 0x04001F1E RID: 7966
	public string playerPref;

	// Token: 0x04001F1F RID: 7967
	public int intValue;

	// Token: 0x04001F20 RID: 7968
	public bool newSystem;
}
