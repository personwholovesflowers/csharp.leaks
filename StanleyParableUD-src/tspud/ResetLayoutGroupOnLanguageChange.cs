using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011F RID: 287
public class ResetLayoutGroupOnLanguageChange : MonoBehaviour
{
	// Token: 0x060006D9 RID: 1753 RVA: 0x00024A91 File Offset: 0x00022C91
	private void Awake()
	{
		LocalizationManager.OnLocalizeEvent += this.LocalizationManager_OnLocalizeEvent;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00024AA4 File Offset: 0x00022CA4
	private void OnDestroy()
	{
		LocalizationManager.OnLocalizeEvent -= this.LocalizationManager_OnLocalizeEvent;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00024AB7 File Offset: 0x00022CB7
	private void OnEnable()
	{
		if (this.queueUpdate)
		{
			this.queueUpdate = false;
			this.UpdateLayoutGroup();
		}
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00024ACE File Offset: 0x00022CCE
	private void LocalizationManager_OnLocalizeEvent()
	{
		this.queueUpdate = true;
		this.UpdateLayoutGroup();
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00024ADD File Offset: 0x00022CDD
	private void UpdateLayoutGroup()
	{
		Canvas.ForceUpdateCanvases();
		HorizontalOrVerticalLayoutGroup componentInChildren = base.GetComponentInChildren<HorizontalOrVerticalLayoutGroup>();
		componentInChildren.enabled = false;
		componentInChildren.enabled = true;
		Canvas.ForceUpdateCanvases();
	}

	// Token: 0x04000729 RID: 1833
	private bool queueUpdate;
}
