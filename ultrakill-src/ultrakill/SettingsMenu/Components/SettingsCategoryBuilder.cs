using System;
using System.Text;
using SettingsMenu.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x02000541 RID: 1345
	public class SettingsCategoryBuilder : MonoBehaviour, ISettingsGroupUser
	{
		// Token: 0x06001E58 RID: 7768 RVA: 0x000FAE98 File Offset: 0x000F9098
		public void ConfigureFrom(SettingsCategory category, SettingsPageBuilder pageBuilder)
		{
			if (category == null)
			{
				return;
			}
			this.pageBuilder = pageBuilder;
			if (this.title != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(category.GetLabel(true));
				if (!string.IsNullOrEmpty(category.description))
				{
					stringBuilder.Append("\n<size=16>");
					stringBuilder.Append(category.description);
					stringBuilder.Append("</size>");
				}
				this.title.text = stringBuilder.ToString();
			}
			if (category.group == null)
			{
				this.button.gameObject.SetActive(false);
				return;
			}
			bool enabled = category.group.GetEnabled();
			this.toggle.SetIsOnWithoutNotify(enabled);
			if (this.restoreDefaultButton != null)
			{
				this.restoreDefaultButton.settingKey = category.group.preferenceKey.key;
			}
			this.button.gameObject.SetActive(true);
			this.group = category.group;
			pageBuilder.AddToGroup(this.group, this);
			pageBuilder.AddSelectableRow(this.button);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000FAFB4 File Offset: 0x000F91B4
		public void ToggleGroup()
		{
			if (this.group == null)
			{
				return;
			}
			bool flag = !this.group.GetEnabled();
			this.SetGroupEnabled(flag);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000FAFE6 File Offset: 0x000F91E6
		public void SetGroupEnabled(bool groupEnabled)
		{
			this.pageBuilder.SetGroupEnabled(this.group, groupEnabled, false);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000FAFFB File Offset: 0x000F91FB
		public void UpdateGroupStatus(bool groupEnabled, SettingsGroupTogglingMode togglingMode)
		{
			this.toggle.isOn = groupEnabled;
		}

		// Token: 0x04002AE2 RID: 10978
		[SerializeField]
		private TMP_Text title;

		// Token: 0x04002AE3 RID: 10979
		[SerializeField]
		private Button button;

		// Token: 0x04002AE4 RID: 10980
		[SerializeField]
		private Toggle toggle;

		// Token: 0x04002AE5 RID: 10981
		[SerializeField]
		private SettingsRestoreDefaultButton restoreDefaultButton;

		// Token: 0x04002AE6 RID: 10982
		private SettingsPageBuilder pageBuilder;

		// Token: 0x04002AE7 RID: 10983
		private SettingsGroup group;
	}
}
