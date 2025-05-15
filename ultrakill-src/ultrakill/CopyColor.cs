using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E7 RID: 231
public class CopyColor : MonoBehaviour
{
	// Token: 0x0600048D RID: 1165 RVA: 0x0001F9B7 File Offset: 0x0001DBB7
	private void Start()
	{
		this.img = base.GetComponent<Image>();
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001F9C8 File Offset: 0x0001DBC8
	private void Update()
	{
		if (this.img)
		{
			if (this.target)
			{
				this.img.color = this.target.color;
				return;
			}
			if (this.textTarget)
			{
				this.img.color = this.textTarget.color;
			}
		}
	}

	// Token: 0x04000631 RID: 1585
	private Image img;

	// Token: 0x04000632 RID: 1586
	public Image target;

	// Token: 0x04000633 RID: 1587
	public TMP_Text textTarget;
}
