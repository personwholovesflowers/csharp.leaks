using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AF RID: 175
public class CheatMenuItem : MonoBehaviour
{
	// Token: 0x04000451 RID: 1105
	public TMP_Text longName;

	// Token: 0x04000452 RID: 1106
	public Image iconTarget;

	// Token: 0x04000453 RID: 1107
	[Space]
	public Button stateButton;

	// Token: 0x04000454 RID: 1108
	public Image stateBackground;

	// Token: 0x04000455 RID: 1109
	public TMP_Text stateText;

	// Token: 0x04000456 RID: 1110
	[Space]
	public TMP_Text bindButtonText;

	// Token: 0x04000457 RID: 1111
	public Button bindButton;

	// Token: 0x04000458 RID: 1112
	public Image bindButtonBack;

	// Token: 0x04000459 RID: 1113
	[Space]
	public Button resetBindButton;
}
