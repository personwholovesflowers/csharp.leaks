using System;
using TMPro;
using UnityEngine;

namespace StanleyUI.Setters
{
	// Token: 0x0200020A RID: 522
	public class BumpscosityIndexSlider : MonoBehaviour, ISettingsIntListener
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x000361D8 File Offset: 0x000343D8
		public void SetValueFloatHACK(float f)
		{
			this.SetValue(Mathf.RoundToInt(f));
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x000361E6 File Offset: 0x000343E6
		public void SetValue(int val)
		{
			this.tmpro.text = this.replacementNumbers[Mathf.Clamp(val, 0, this.replacementNumbers.Length - 1)];
		}

		// Token: 0x04000B73 RID: 2931
		public TextMeshProUGUI tmpro;

		// Token: 0x04000B74 RID: 2932
		public string[] replacementNumbers = new string[] { "0", "1", "12", "50", "100", "1000" };
	}
}
