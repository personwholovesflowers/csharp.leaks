using System;
using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003BB RID: 955
public class WorldOptions : MonoBehaviour
{
	// Token: 0x060015C6 RID: 5574 RVA: 0x000B1114 File Offset: 0x000AF314
	private void Start()
	{
		if (MonoSingleton<MapVarManager>.Instance.GetBool("border_enabled").GetValueOrDefault())
		{
			this.SetBorderOn(true);
		}
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x000B1141 File Offset: 0x000AF341
	public void ToggleBorder()
	{
		this.SetBorderOn(!this.isBorderOn);
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000B1154 File Offset: 0x000AF354
	public void SetBorderOn(bool state)
	{
		this.isBorderOn = state;
		this.border.SetActive(state);
		this.borderIcon.color = (state ? Color.white : new Color(1f, 1f, 1f, 0.3f));
		this.borderStatus.text = (state ? "Enabled" : "Disabled");
		this.buttonText.text = (state ? "Disable" : "Enable");
		MonoSingleton<MapVarManager>.Instance.SetBool("border_enabled", state, true);
	}

	// Token: 0x04001DFD RID: 7677
	[SerializeField]
	private Image borderIcon;

	// Token: 0x04001DFE RID: 7678
	[SerializeField]
	private TMP_Text borderStatus;

	// Token: 0x04001DFF RID: 7679
	[SerializeField]
	private TMP_Text buttonText;

	// Token: 0x04001E00 RID: 7680
	[Space]
	[SerializeField]
	private GameObject border;

	// Token: 0x04001E01 RID: 7681
	private bool isBorderOn = true;

	// Token: 0x04001E02 RID: 7682
	public const string BorderEnabledKey = "border_enabled";
}
