using System;
using TMPro;
using UnityEngine;

namespace StanleyUI.Setters
{
	// Token: 0x0200020C RID: 524
	public class GenericSlider : MonoBehaviour, ISettingsFloatListener
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0003625B File Offset: 0x0003445B
		public virtual void SetValue(float val)
		{
			this.textLabel.text = string.Concat(Mathf.RoundToInt(val));
		}

		// Token: 0x04000B75 RID: 2933
		public TextMeshProUGUI textLabel;
	}
}
