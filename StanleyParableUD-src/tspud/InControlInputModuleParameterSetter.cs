using System;
using InControl;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[RequireComponent(typeof(InControlInputModule))]
public class InControlInputModuleParameterSetter : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001F757 File Offset: 0x0001D957
	private InControlInputModule InputModule
	{
		get
		{
			if (this.inputModule == null)
			{
				this.inputModule = base.GetComponent<InControlInputModule>();
			}
			return this.inputModule;
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0001F779 File Offset: 0x0001D979
	private void Start()
	{
		this.InputModule.submitButton = (InControlInputModule.Button)PlatformGamepad.ConfirmButton;
		this.InputModule.cancelButton = (InControlInputModule.Button)PlatformGamepad.BackButton;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0001F79B File Offset: 0x0001D99B
	public void SetFocusMouseOnHover(bool focusMouseOnHover)
	{
		this.InputModule.focusOnMouseHover = focusMouseOnHover;
	}

	// Token: 0x040005DB RID: 1499
	private InControlInputModule inputModule;
}
