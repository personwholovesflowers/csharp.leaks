using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x020001E9 RID: 489
	public class Item : MonoBehaviour
	{
		// Token: 0x06000B31 RID: 2865 RVA: 0x00033DE4 File Offset: 0x00031FE4
		public void Set(string name, Sprite sprite, Color color, float health, float damage)
		{
			if (this.image)
			{
				this.image.sprite = sprite;
				this.image.color = color;
			}
			if (this.title)
			{
				this.title.text = name;
			}
			if (this.description)
			{
				this.description.text = "The short description of " + name;
			}
			if (this.healthBar)
			{
				this.healthBar.anchorMax = new Vector2(health, 1f);
			}
			if (this.damageBar)
			{
				this.damageBar.anchorMax = new Vector2(damage, 1f);
			}
		}

		// Token: 0x04000AFC RID: 2812
		public Image image;

		// Token: 0x04000AFD RID: 2813
		public Text title;

		// Token: 0x04000AFE RID: 2814
		public Text description;

		// Token: 0x04000AFF RID: 2815
		public RectTransform healthBar;

		// Token: 0x04000B00 RID: 2816
		public RectTransform damageBar;
	}
}
