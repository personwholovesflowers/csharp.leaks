using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E9 RID: 233
public class CopyImage : MonoBehaviour
{
	// Token: 0x06000490 RID: 1168 RVA: 0x0001FA2C File Offset: 0x0001DC2C
	private void Update()
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		if (this.imgToCopy == null && this.copyType != CopyType.None && MonoSingleton<WeaponHUD>.Instance != null)
		{
			if (this.copyType == CopyType.WeaponIcon)
			{
				this.imgToCopy = MonoSingleton<WeaponHUD>.Instance.GetComponent<Image>();
			}
			else if (this.copyType == CopyType.WeaponShadow)
			{
				this.imgToCopy = MonoSingleton<WeaponHUD>.Instance.transform.GetChild(0).GetComponent<Image>();
			}
		}
		if (this.imgToCopy != null)
		{
			this.img.sprite = this.imgToCopy.sprite;
			if (this.copyColor)
			{
				this.img.color = this.imgToCopy.color;
			}
		}
	}

	// Token: 0x04000638 RID: 1592
	private Image img;

	// Token: 0x04000639 RID: 1593
	public Image imgToCopy;

	// Token: 0x0400063A RID: 1594
	public CopyType copyType;

	// Token: 0x0400063B RID: 1595
	public bool copyColor;
}
