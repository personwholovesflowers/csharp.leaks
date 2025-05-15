using System;
using TMPro;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class SliderWith2WordsSliderParser : MonoBehaviour, IMessageBoxDialogueParser
{
	// Token: 0x06000943 RID: 2371 RVA: 0x0002B8C4 File Offset: 0x00029AC4
	public string[] ParseDialogue(MessageBoxDialogue dialogue)
	{
		string[] messages = dialogue.GetMessages();
		this.sliderTextRight.text = ((messages.Length > 2) ? messages[2] : "");
		this.sliderTextLeft.text = ((messages.Length > 1) ? messages[1] : "");
		return new string[] { (messages.Length != 0) ? messages[0] : "" };
	}

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private TextMeshProUGUI sliderTextLeft;

	// Token: 0x0400091C RID: 2332
	[SerializeField]
	private TextMeshProUGUI sliderTextRight;
}
