using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x020001EA RID: 490
	public class ItemsGenerator : MonoBehaviour
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x00033E9C File Offset: 0x0003209C
		public void Generate()
		{
			this.DestroyChildren();
			int num = Random.Range(0, ItemsGenerator.colors.Length - 1);
			for (int i = 0; i < this.count; i++)
			{
				Item item = Object.Instantiate<Item>(this.itemPrefab);
				item.transform.SetParent(this.target, false);
				item.Set(string.Format("{0} {1:D2}", this.baseName, i + 1), this.image, ItemsGenerator.colors[(num + i) % ItemsGenerator.colors.Length], Random.Range(0.4f, 1f), Random.Range(0.4f, 1f));
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00033F43 File Offset: 0x00032143
		private void DestroyChildren()
		{
			while (this.target.childCount > 0)
			{
				Object.DestroyImmediate(this.target.GetChild(0).gameObject);
			}
		}

		// Token: 0x04000B01 RID: 2817
		public RectTransform target;

		// Token: 0x04000B02 RID: 2818
		public Sprite image;

		// Token: 0x04000B03 RID: 2819
		public int count;

		// Token: 0x04000B04 RID: 2820
		public string baseName;

		// Token: 0x04000B05 RID: 2821
		public Item itemPrefab;

		// Token: 0x04000B06 RID: 2822
		private static readonly Color[] colors = new Color[]
		{
			Color.red,
			Color.green,
			Color.blue,
			Color.cyan,
			Color.yellow,
			Color.magenta,
			Color.gray
		};
	}
}
