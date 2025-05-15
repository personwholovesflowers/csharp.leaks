using System;
using TMPro;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class PauseMenuButton : MonoBehaviour
{
	// Token: 0x0600079F RID: 1951 RVA: 0x00026B66 File Offset: 0x00024D66
	private void Start()
	{
		this.initialText.text = this.buttonTextObject.text;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00026B7E File Offset: 0x00024D7E
	public void BoldText()
	{
		this.buttonTextObject.fontStyle = FontStyles.Bold;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00026B8C File Offset: 0x00024D8C
	public void UnboldText()
	{
		this.buttonTextObject.fontStyle = FontStyles.Normal;
	}

	// Token: 0x040007C1 RID: 1985
	public TextMeshProUGUI buttonTextObject;

	// Token: 0x040007C2 RID: 1986
	private TextMeshProUGUI initialText;
}
