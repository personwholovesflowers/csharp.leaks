using System;
using UnityEngine;

// Token: 0x020003B4 RID: 948
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SandboxSaveConfirmation : MonoSingleton<SandboxSaveConfirmation>
{
	// Token: 0x06001592 RID: 5522 RVA: 0x000AEFA9 File Offset: 0x000AD1A9
	public void ConfirmSave()
	{
		MonoSingleton<SandboxSaver>.Instance.QuickSave();
		this.saveConfirmationDialog.SetActive(false);
		this.blocker.SetActive(false);
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x000AEFCD File Offset: 0x000AD1CD
	public void DisplayDialog()
	{
		this.saveConfirmationDialog.SetActive(true);
		this.blocker.SetActive(true);
	}

	// Token: 0x04001DE7 RID: 7655
	[SerializeField]
	private GameObject saveConfirmationDialog;

	// Token: 0x04001DE8 RID: 7656
	[SerializeField]
	private GameObject blocker;
}
