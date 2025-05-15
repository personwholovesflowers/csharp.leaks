using System;
using UnityEngine;
using UnityEngine.Events;

namespace StanleyUI
{
	// Token: 0x020001F4 RID: 500
	public class ControllerMenuButton : MonoBehaviour
	{
		// Token: 0x06000BA5 RID: 2981 RVA: 0x0003552D File Offset: 0x0003372D
		private void Awake()
		{
			this.parentUIScreen = base.GetComponentInParent<UIScreen>();
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0003553B File Offset: 0x0003373B
		public void InvokeOnButtonPress()
		{
			UnityEvent onWasPressed = this.OnWasPressed;
			if (onWasPressed == null)
			{
				return;
			}
			onWasPressed.Invoke();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00035550 File Offset: 0x00033750
		private void Update()
		{
			if (this.parentUIScreen.active)
			{
				ControllerMenuButton.ControllerButton controllerButton = this.controllerButton;
				if (controllerButton != ControllerMenuButton.ControllerButton.Back_ActionRight)
				{
					if (controllerButton != ControllerMenuButton.ControllerButton.Confirm_ActionDown)
					{
						return;
					}
					if (Singleton<GameMaster>.Instance.stanleyActions.MenuConfirm.WasPressed)
					{
						this.InvokeOnButtonPress();
					}
				}
				else if (Singleton<GameMaster>.Instance.stanleyActions.MenuBack.WasPressed)
				{
					this.InvokeOnButtonPress();
					return;
				}
			}
		}

		// Token: 0x04000B3F RID: 2879
		public ControllerMenuButton.ControllerButton controllerButton;

		// Token: 0x04000B40 RID: 2880
		public UnityEvent OnWasPressed;

		// Token: 0x04000B41 RID: 2881
		private UIScreen parentUIScreen;

		// Token: 0x02000419 RID: 1049
		public enum ControllerButton
		{
			// Token: 0x04001553 RID: 5459
			Back_ActionRight,
			// Token: 0x04001554 RID: 5460
			Confirm_ActionDown
		}
	}
}
