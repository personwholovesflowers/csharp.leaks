using System;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class SandboxIconSwitcher : MonoBehaviour
{
	// Token: 0x060015CD RID: 5581 RVA: 0x000B1295 File Offset: 0x000AF495
	public void SwitchIconPack()
	{
		MonoSingleton<IconManager>.Instance.SetIconPack(this.iconPack, true);
		MonoSingleton<IconManager>.Instance.Reload();
	}

	// Token: 0x04001E08 RID: 7688
	public int iconPack;
}
