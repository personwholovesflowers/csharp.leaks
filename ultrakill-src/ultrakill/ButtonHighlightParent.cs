using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009C RID: 156
public class ButtonHighlightParent : MonoBehaviour
{
	// Token: 0x060002FB RID: 763 RVA: 0x00011848 File Offset: 0x0000FA48
	private void Start()
	{
		this.buttons = base.GetComponentsInChildren<Image>();
		this.buttonTexts = new TMP_Text[this.buttons.Length];
		this.buttonSprites = new Sprite[this.buttons.Length];
		for (int i = 0; i < this.buttons.Length; i++)
		{
			this.buttonTexts[i] = this.buttons[i].GetComponentInChildren<TMP_Text>();
			this.buttonSprites[i] = this.buttons[i].sprite;
		}
		if (this.targetOnStart)
		{
			this.ChangeButton(this.targetOnStart);
		}
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000118E0 File Offset: 0x0000FAE0
	public void ChangeButton(Image target)
	{
		for (int i = 0; i < this.buttons.Length; i++)
		{
			if (!(this.buttons[i] == null))
			{
				if (this.pressedVersion)
				{
					this.buttons[i].sprite = ((this.buttons[i] == target) ? this.pressedVersion : this.buttonSprites[i]);
				}
				else
				{
					this.buttons[i].fillCenter = this.buttons[i] == target;
				}
				if (this.buttonTexts[i] != null)
				{
					this.buttonTexts[i].color = ((this.buttons[i] == target) ? Color.black : Color.white);
				}
			}
		}
	}

	// Token: 0x0400039E RID: 926
	private Image[] buttons;

	// Token: 0x0400039F RID: 927
	private TMP_Text[] buttonTexts;

	// Token: 0x040003A0 RID: 928
	private Sprite[] buttonSprites;

	// Token: 0x040003A1 RID: 929
	[SerializeField]
	private Sprite pressedVersion;

	// Token: 0x040003A2 RID: 930
	[SerializeField]
	private Image targetOnStart;
}
