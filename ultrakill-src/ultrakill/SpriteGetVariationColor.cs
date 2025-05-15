using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class SpriteGetVariationColor : MonoBehaviour
{
	// Token: 0x0600185B RID: 6235 RVA: 0x000C6CAC File Offset: 0x000C4EAC
	private void Update()
	{
		foreach (SpriteRenderer spriteRenderer in this.sprites)
		{
			spriteRenderer.color = new Color(MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation].r, MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation].g, MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variation].b, spriteRenderer.color.a);
		}
	}

	// Token: 0x04002230 RID: 8752
	[SerializeField]
	private SpriteRenderer[] sprites;

	// Token: 0x04002231 RID: 8753
	[SerializeField]
	private int variation;
}
