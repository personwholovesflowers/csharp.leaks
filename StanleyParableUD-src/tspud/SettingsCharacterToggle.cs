using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class SettingsCharacterToggle : MonoBehaviour
{
	// Token: 0x06000701 RID: 1793 RVA: 0x00024E7F File Offset: 0x0002307F
	public void SetAnimationBooleanForToggle(bool isOn)
	{
		this.animator.SetBool(this.toggleString, isOn);
	}

	// Token: 0x0400073E RID: 1854
	[SerializeField]
	private Animator animator;

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private string toggleString = "Toggled";
}
