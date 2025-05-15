using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003EA RID: 1002
public class DropdownPipChanger : MonoBehaviour
{
	// Token: 0x0600168C RID: 5772 RVA: 0x000B536C File Offset: 0x000B356C
	private void Awake()
	{
		this.dropdown = base.GetComponent<TMP_Dropdown>();
		this.defaultSprite = this.pip.sprite;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x000B538B File Offset: 0x000B358B
	private void Update()
	{
		this.pip.sprite = (this.dropdown.IsExpanded ? this.openedSprite : this.defaultSprite);
	}

	// Token: 0x04001F24 RID: 7972
	private TMP_Dropdown dropdown;

	// Token: 0x04001F25 RID: 7973
	[SerializeField]
	private Image pip;

	// Token: 0x04001F26 RID: 7974
	private Sprite defaultSprite;

	// Token: 0x04001F27 RID: 7975
	[SerializeField]
	private Sprite openedSprite;
}
