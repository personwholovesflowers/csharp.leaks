using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003CA RID: 970
public class ScreenBlood : MonoBehaviour
{
	// Token: 0x0600160A RID: 5642 RVA: 0x000B262C File Offset: 0x000B082C
	private void Start()
	{
		this.rct = base.GetComponent<RectTransform>();
		this.rct.anchoredPosition = new Vector2((float)Random.Range(-400, 400), (float)Random.Range(-250, 250));
		this.img = base.GetComponent<Image>();
		this.img.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
		this.clr = this.img.color;
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000B26B4 File Offset: 0x000B08B4
	private void Update()
	{
		if (this.clr.a > 0f)
		{
			this.clr.a = this.clr.a - Time.deltaTime;
			this.img.color = this.clr;
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001E59 RID: 7769
	private Image img;

	// Token: 0x04001E5A RID: 7770
	private Color clr;

	// Token: 0x04001E5B RID: 7771
	private RectTransform rct;

	// Token: 0x04001E5C RID: 7772
	public Sprite[] sprites;
}
