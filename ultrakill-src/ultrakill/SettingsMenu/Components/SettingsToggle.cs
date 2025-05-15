using System;
using SettingsMenu.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x0200053E RID: 1342
	public class SettingsToggle : SettingsBuilderBase
	{
		// Token: 0x06001E4F RID: 7759 RVA: 0x000FAE24 File Offset: 0x000F9024
		public override void ConfigureFrom(SettingsItemBuilder itemBuilder, SettingsPageBuilder pageBuilder)
		{
			SettingsItem asset = itemBuilder.asset;
			if (asset.preferenceKey.IsValid())
			{
				bool boolValue = asset.preferenceKey.GetBoolValue(false);
				this.toggle.SetIsOnWithoutNotify(boolValue);
			}
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(itemBuilder.ValueChanged<bool>));
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000FAE7A File Offset: 0x000F907A
		public override void SetSelected()
		{
			SettingsMenu.SetSelected(this.toggle);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000FAE87 File Offset: 0x000F9087
		public override void AttachRestoreDefaultButton(SettingsRestoreDefaultButton restoreDefaultButton)
		{
			restoreDefaultButton.toggle = this.toggle;
		}

		// Token: 0x04002AE1 RID: 10977
		[SerializeField]
		private Toggle toggle;
	}
}
