using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E7 RID: 487
public class FishingRodTarget : MonoBehaviour
{
	// Token: 0x060009EA RID: 2538 RVA: 0x0004465C File Offset: 0x0004285C
	private void Awake()
	{
		this.goodBadge.SetActive(false);
		this.badBadge.SetActive(false);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00044676 File Offset: 0x00042876
	public void SetState(bool isGood, float distance)
	{
		this.goodBadge.SetActive(isGood);
		this.badBadge.SetActive(!isGood);
		this.canvas.localScale = Vector3.one * (0.4f + distance * 0.05f);
	}

	// Token: 0x04000CE2 RID: 3298
	[SerializeField]
	private GameObject goodBadge;

	// Token: 0x04000CE3 RID: 3299
	[SerializeField]
	private GameObject badBadge;

	// Token: 0x04000CE4 RID: 3300
	[SerializeField]
	private Transform canvas;

	// Token: 0x04000CE5 RID: 3301
	public Text waterNameText;
}
