using System;
using UnityEngine;

namespace SettingsMenu.Components
{
	// Token: 0x02000540 RID: 1344
	public abstract class SettingsBuilderBase : MonoBehaviour
	{
		// Token: 0x06001E54 RID: 7764
		public abstract void ConfigureFrom(SettingsItemBuilder itemBuilder, SettingsPageBuilder pageBuilder);

		// Token: 0x06001E55 RID: 7765
		public abstract void SetSelected();

		// Token: 0x06001E56 RID: 7766 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void AttachRestoreDefaultButton(SettingsRestoreDefaultButton restoreDefaultButton)
		{
		}
	}
}
