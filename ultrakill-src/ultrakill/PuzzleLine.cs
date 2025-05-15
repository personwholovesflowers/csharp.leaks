using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036B RID: 875
public class PuzzleLine : MonoBehaviour
{
	// Token: 0x06001464 RID: 5220 RVA: 0x000A55A0 File Offset: 0x000A37A0
	public void DrawLine(Vector3 pointA, Vector3 pointB, TileColor color)
	{
		if (this.imageRectTransform == null)
		{
			this.imageRectTransform = base.GetComponent<RectTransform>();
		}
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		Vector3 vector = pointB - pointA;
		this.imageRectTransform.sizeDelta = new Vector2((float)this.length, 8f);
		this.imageRectTransform.pivot = new Vector2(0f, 0.5f);
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		this.imageRectTransform.localRotation = Quaternion.Euler(0f, 0f, num);
		this.img.color = this.TranslateColor(color);
		this.img.enabled = true;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000A5670 File Offset: 0x000A3870
	public void Hide()
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		this.img.enabled = false;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x000A5698 File Offset: 0x000A3898
	public Color TranslateColor(TileColor color)
	{
		Color color2 = Color.white;
		switch (color)
		{
		case TileColor.None:
			color2 = Color.white;
			break;
		case TileColor.Red:
			color2 = Color.red;
			break;
		case TileColor.Green:
			color2 = Color.green;
			break;
		case TileColor.Blue:
			color2 = new Color(0f, 0.25f, 1f);
			break;
		}
		return color2;
	}

	// Token: 0x04001BF9 RID: 7161
	private RectTransform imageRectTransform;

	// Token: 0x04001BFA RID: 7162
	private Image img;

	// Token: 0x04001BFB RID: 7163
	public int length;
}
