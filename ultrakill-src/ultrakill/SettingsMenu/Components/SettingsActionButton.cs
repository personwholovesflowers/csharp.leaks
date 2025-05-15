using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x0200053A RID: 1338
	public class SettingsActionButton : SettingsBuilderBase
	{
		// Token: 0x06001E3A RID: 7738 RVA: 0x000FA98C File Offset: 0x000F8B8C
		public override void ConfigureFrom(SettingsItemBuilder itemBuilder, SettingsPageBuilder pageBuilder)
		{
			this.label.text = itemBuilder.asset.buttonLabel.ToUpper();
			if (pageBuilder.buttonEvents == null)
			{
				return;
			}
			SettingsButtonEvent settingsButtonEvent = pageBuilder.buttonEvents.Find((SettingsButtonEvent x) => x.buttonItem == itemBuilder.asset);
			this.button.onClick.AddListener(new UnityAction(settingsButtonEvent.onClickEvent.Invoke));
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000FAA08 File Offset: 0x000F8C08
		public override void SetSelected()
		{
			SettingsMenu.SetSelected(this.button);
		}

		// Token: 0x04002AD9 RID: 10969
		[SerializeField]
		private Button button;

		// Token: 0x04002ADA RID: 10970
		[SerializeField]
		private TMP_Text label;
	}
}
