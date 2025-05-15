using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200043B RID: 1083
public class SpritePoses : MonoBehaviour
{
	// Token: 0x0600185D RID: 6237 RVA: 0x000C6D38 File Offset: 0x000C4F38
	public void ChangePose(int target)
	{
		if (!this.img)
		{
			this.img = base.GetComponent<Image>();
		}
		if (target < this.poses.Length)
		{
			this.img.sprite = this.poses[target];
		}
		else if (this.poses.Length != 0)
		{
			this.img.sprite = this.poses[0];
		}
		SpritePoses[] array = this.copyChangeTo;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ChangePose(target);
		}
	}

	// Token: 0x04002232 RID: 8754
	public Sprite[] poses;

	// Token: 0x04002233 RID: 8755
	private Image img;

	// Token: 0x04002234 RID: 8756
	public SpritePoses[] copyChangeTo;
}
