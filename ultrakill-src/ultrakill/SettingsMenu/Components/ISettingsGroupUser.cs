using System;
using SettingsMenu.Models;

namespace SettingsMenu.Components
{
	// Token: 0x0200053F RID: 1343
	public interface ISettingsGroupUser
	{
		// Token: 0x06001E53 RID: 7763
		void UpdateGroupStatus(bool enabled, SettingsGroupTogglingMode togglingMode);
	}
}
