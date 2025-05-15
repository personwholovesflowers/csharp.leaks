using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000225 RID: 549
public class GearCheckImage : MonoBehaviour
{
	// Token: 0x06000BD0 RID: 3024 RVA: 0x00052D0C File Offset: 0x00050F0C
	private void Awake()
	{
		if (!this.image)
		{
			this.image = base.GetComponent<Image>();
		}
		if (!this.originalSprite)
		{
			this.originalSprite = this.image.sprite;
		}
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00052D45 File Offset: 0x00050F45
	private void OnEnable()
	{
		if (GameProgressSaver.CheckGear(this.gearName) == 0)
		{
			this.image.sprite = this.lockedSprite;
			return;
		}
		this.image.sprite = this.originalSprite;
	}

	// Token: 0x04000F69 RID: 3945
	public string gearName;

	// Token: 0x04000F6A RID: 3946
	private Image image;

	// Token: 0x04000F6B RID: 3947
	private Sprite originalSprite;

	// Token: 0x04000F6C RID: 3948
	[SerializeField]
	private Sprite lockedSprite;
}
