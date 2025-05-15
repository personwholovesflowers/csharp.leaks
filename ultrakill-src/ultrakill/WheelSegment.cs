using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x020004CA RID: 1226
public class WheelSegment
{
	// Token: 0x06001C0E RID: 7182 RVA: 0x000E91A8 File Offset: 0x000E73A8
	public void SetActive(bool active)
	{
		this.segment.color = (active ? Color.red : Color.black);
		this.icon.color = (active ? Color.red : Color.white);
		Color color = (active ? Color.red : Color.black);
		color.a = 0.7f;
		this.iconGlow.color = color;
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000E9211 File Offset: 0x000E7411
	public void DestroySegment()
	{
		Object.Destroy(this.segment.gameObject);
		Object.Destroy(this.divider.gameObject);
	}

	// Token: 0x04002795 RID: 10133
	public WeaponDescriptor descriptor;

	// Token: 0x04002796 RID: 10134
	public int slotIndex;

	// Token: 0x04002797 RID: 10135
	public UICircle segment;

	// Token: 0x04002798 RID: 10136
	public UICircle divider;

	// Token: 0x04002799 RID: 10137
	public Image icon;

	// Token: 0x0400279A RID: 10138
	public Image iconGlow;
}
