using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200024A RID: 586
[RequireComponent(typeof(CanvasGroup))]
public class HideUICheatGroup : MonoBehaviour
{
	// Token: 0x06000CDB RID: 3291 RVA: 0x0005FB54 File Offset: 0x0005DD54
	private void Awake()
	{
		if (base.TryGetComponent<CanvasGroup>(out this.canvasGroup))
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			this.canvasGroup.blocksRaycasts = false;
			this.canvasGroup.enabled = false;
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0005FBA3 File Offset: 0x0005DDA3
	private void Update()
	{
		if (this.canvasGroup == null)
		{
			return;
		}
		this.canvasGroup.enabled = HideUI.Active;
	}

	// Token: 0x0400111E RID: 4382
	private CanvasGroup canvasGroup;
}
