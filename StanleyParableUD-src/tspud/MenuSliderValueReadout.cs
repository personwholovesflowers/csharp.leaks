using System;
using TMPro;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class MenuSliderValueReadout : MonoBehaviour
{
	// Token: 0x06000741 RID: 1857 RVA: 0x00025A5C File Offset: 0x00023C5C
	private void Start()
	{
		this.valueText = base.GetComponent<TextMeshPro>();
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00025A6A File Offset: 0x00023C6A
	public void textUpdate(int value)
	{
		this.valueText.text = value.ToString();
	}

	// Token: 0x04000769 RID: 1897
	private TextMeshPro valueText;
}
