using System;
using TMPro;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class CopyText : MonoBehaviour
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x0001FCFA File Offset: 0x0001DEFA
	private void Start()
	{
		this.txt = base.GetComponent<TMP_Text>();
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001FD08 File Offset: 0x0001DF08
	private void Update()
	{
		this.txt.text = this.target.text;
	}

	// Token: 0x04000647 RID: 1607
	private TMP_Text txt;

	// Token: 0x04000648 RID: 1608
	public TMP_Text target;
}
