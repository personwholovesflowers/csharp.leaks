using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000138 RID: 312
public class CustomContentButton : MonoBehaviour
{
	// Token: 0x0400081E RID: 2078
	public Button button;

	// Token: 0x0400081F RID: 2079
	public Image icon;

	// Token: 0x04000820 RID: 2080
	public Image iconInset;

	// Token: 0x04000821 RID: 2081
	public Image border;

	// Token: 0x04000822 RID: 2082
	public TMP_Text text;

	// Token: 0x04000823 RID: 2083
	public TMP_Text costText;

	// Token: 0x04000824 RID: 2084
	public List<GameObject> objectsToActivateIfAvailable;
}
