using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Token: 0x02000487 RID: 1159
public class TooltipOnHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06001A89 RID: 6793 RVA: 0x000DA6EA File Offset: 0x000D88EA
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.hovered = true;
		this.sinceHoverStart = 0f;
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000DA704 File Offset: 0x000D8904
	public void OnPointerExit(PointerEventData eventData)
	{
		this.hovered = false;
		if (this.tooltipId != Guid.Empty && this.tooltipManager != null)
		{
			this.tooltipManager.HideTooltip(this.tooltipId);
			this.tooltipId = Guid.Empty;
		}
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000DA754 File Offset: 0x000D8954
	private void Update()
	{
		if (this.hovered && this.tooltipManager != null && this.sinceHoverStart > this.hoverTime && this.tooltipId == Guid.Empty)
		{
			if (!this.multiline)
			{
				this.text = this.text.Replace("\n", " ");
			}
			this.tooltipId = this.tooltipManager.ShowTooltip(Mouse.current.position.ReadValue(), this.text);
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000DA7E5 File Offset: 0x000D89E5
	private void OnDisable()
	{
		if (this.tooltipId != Guid.Empty && this.tooltipManager != null)
		{
			this.tooltipManager.HideTooltip(this.tooltipId);
			this.tooltipId = Guid.Empty;
		}
	}

	// Token: 0x0400253A RID: 9530
	public TooltipManager tooltipManager;

	// Token: 0x0400253B RID: 9531
	public float hoverTime = 0.5f;

	// Token: 0x0400253C RID: 9532
	[HideInInspector]
	public bool multiline;

	// Token: 0x0400253D RID: 9533
	[HideInInspector]
	public string text;

	// Token: 0x0400253E RID: 9534
	private bool hovered;

	// Token: 0x0400253F RID: 9535
	private UnscaledTimeSince sinceHoverStart;

	// Token: 0x04002540 RID: 9536
	private Guid tooltipId = Guid.Empty;
}
