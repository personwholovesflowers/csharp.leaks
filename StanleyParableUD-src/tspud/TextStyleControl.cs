using System;
using TMPro;
using UnityEngine;

// Token: 0x02000044 RID: 68
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextStyleControl : MonoBehaviour
{
	// Token: 0x0600016B RID: 363 RVA: 0x0000A030 File Offset: 0x00008230
	private void Awake()
	{
		this.label = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000A03E File Offset: 0x0000823E
	public void SetChangedColor()
	{
		if (this.label != null)
		{
			this.label.color = this.changedColor;
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000A05F File Offset: 0x0000825F
	public void SetDefaultColor()
	{
		if (this.label != null)
		{
			this.label.color = this.defaultColor;
		}
	}

	// Token: 0x040001C1 RID: 449
	private TextMeshProUGUI label;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private Color changedColor = Color.red;

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private Color defaultColor = Color.white;
}
