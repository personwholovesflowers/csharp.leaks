using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200025E RID: 606
public class IconPackSprite : MonoBehaviour
{
	// Token: 0x06000D65 RID: 3429 RVA: 0x00065DF4 File Offset: 0x00063FF4
	public void Start()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("iconPack", 0);
		base.GetComponent<Image>().sprite = ((this.sprites.Length > @int) ? this.sprites[@int] : this.sprites[0]);
	}

	// Token: 0x040011FA RID: 4602
	[SerializeField]
	private Sprite[] sprites;
}
