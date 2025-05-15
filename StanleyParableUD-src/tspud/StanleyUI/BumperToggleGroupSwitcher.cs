using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x020001FD RID: 509
	[RequireComponent(typeof(ToggleGroup))]
	public class BumperToggleGroupSwitcher : MonoBehaviour, IScreenRegisterNotificationReciever
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x00035A78 File Offset: 0x00033C78
		private void Update()
		{
			if (Singleton<GameMaster>.Instance == null)
			{
				return;
			}
			if (Singleton<GameMaster>.Instance.MenuManager && (this.onlyTabSwitchWhenScreenVisible == null || this.onlyTabSwitchWhenScreenVisible.active))
			{
				if (Singleton<GameMaster>.Instance.stanleyActions.MenuTabLeft.WasPressed)
				{
					this.GotoNextTab(-1);
				}
				if (Singleton<GameMaster>.Instance.stanleyActions.MenuTabRight.WasPressed)
				{
					this.GotoNextTab(1);
				}
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00035AFC File Offset: 0x00033CFC
		private Toggle GetActiveToggle()
		{
			using (IEnumerator<Toggle> enumerator = base.GetComponent<ToggleGroup>().ActiveToggles().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00035B4C File Offset: 0x00033D4C
		public void RegisteredScreenVisible()
		{
			this.GotoNextTab(0);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00035B58 File Offset: 0x00033D58
		private void GotoNextTab(int dir)
		{
			UIBehaviour siblingThatIsActive = this.GetActiveToggle().GetSiblingThatIsActive(dir);
			Toggle toggle = ((siblingThatIsActive != null) ? siblingThatIsActive.GetComponent<Toggle>() : null);
			if (toggle != null)
			{
				toggle.isOn = true;
				if (toggle.GetComponent<ISelectableHolderScreen>() != null)
				{
					StanleyInputModuleAssistant.RegisterScreenAsNewlyVisible(toggle.gameObject, false);
				}
			}
		}

		// Token: 0x04000B5C RID: 2908
		public UIScreen onlyTabSwitchWhenScreenVisible;
	}
}
