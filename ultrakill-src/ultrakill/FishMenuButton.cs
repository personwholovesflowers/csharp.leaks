using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EE RID: 494
public class FishMenuButton : MonoBehaviour
{
	// Token: 0x06000A08 RID: 2568 RVA: 0x0004588E File Offset: 0x00043A8E
	public void Populate(FishObject fish, bool locked)
	{
		this.fishIcon.sprite = (locked ? fish.blockedIcon : fish.icon);
		this.fishIcon.color = (locked ? Color.black : Color.white);
	}

	// Token: 0x04000D17 RID: 3351
	[SerializeField]
	private Image fishIcon;
}
