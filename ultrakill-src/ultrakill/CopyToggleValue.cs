using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EE RID: 238
public class CopyToggleValue : MonoBehaviour
{
	// Token: 0x060004A3 RID: 1187 RVA: 0x0001FD20 File Offset: 0x0001DF20
	private void Start()
	{
		this.currentToggle = base.GetComponent<Toggle>();
		this.currentToggle.isOn = this.target.isOn;
	}

	// Token: 0x04000649 RID: 1609
	public Toggle target;

	// Token: 0x0400064A RID: 1610
	private Toggle currentToggle;
}
