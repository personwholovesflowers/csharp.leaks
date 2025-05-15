using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200017F RID: 383
public class SequelConfirmButton : MonoBehaviour
{
	// Token: 0x06000900 RID: 2304 RVA: 0x0002AE85 File Offset: 0x00029085
	public void InvokeCheck()
	{
		if (this.prefixIndexConfigurable.GetIntValue() != -1 && this.postfixIndexConfigurable.GetIntValue() != -1)
		{
			this.confirmButton.interactable = true;
			return;
		}
		this.confirmButton.interactable = false;
	}

	// Token: 0x040008DC RID: 2268
	public Button confirmButton;

	// Token: 0x040008DD RID: 2269
	public IntConfigurable prefixIndexConfigurable;

	// Token: 0x040008DE RID: 2270
	public IntConfigurable postfixIndexConfigurable;
}
