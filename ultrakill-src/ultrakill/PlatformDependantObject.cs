using System;
using SettingsMenu.Models;
using UnityEngine;

// Token: 0x02000339 RID: 825
public class PlatformDependantObject : MonoBehaviour
{
	// Token: 0x060012F7 RID: 4855 RVA: 0x00097048 File Offset: 0x00095248
	private void Awake()
	{
		bool flag = this.requiresSteam;
		bool flag2 = this.requiresDiscord;
		bool flag3 = this.requiresFileSystemAccess;
		if (this.hideInSolsticeRelease && PlatformRequirements.IsCloudManagedRelease())
		{
			this.onDestroy.Invoke("");
			Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x04001A13 RID: 6675
	[SerializeField]
	private bool requiresSteam;

	// Token: 0x04001A14 RID: 6676
	[SerializeField]
	private bool requiresDiscord;

	// Token: 0x04001A15 RID: 6677
	[SerializeField]
	private bool requiresFileSystemAccess;

	// Token: 0x04001A16 RID: 6678
	[SerializeField]
	private bool hideInSolsticeRelease;

	// Token: 0x04001A17 RID: 6679
	[SerializeField]
	private UltrakillEvent onDestroy;
}
