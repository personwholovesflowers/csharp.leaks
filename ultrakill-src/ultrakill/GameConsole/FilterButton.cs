using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameConsole
{
	// Token: 0x020005B1 RID: 1457
	[Serializable]
	public class FilterButton
	{
		// Token: 0x060020B4 RID: 8372 RVA: 0x001071FC File Offset: 0x001053FC
		public void SetOpacity(float opacity)
		{
			Color color = this.buttonBackground.color;
			color.a = opacity;
			this.buttonBackground.color = color;
			this.miniIndicator.color = color;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00107235 File Offset: 0x00105435
		public void SetCheckmark(bool isChecked)
		{
			this.checkmark.SetActive(isChecked);
		}

		// Token: 0x04002D01 RID: 11521
		public TMP_Text text;

		// Token: 0x04002D02 RID: 11522
		public Image buttonBackground;

		// Token: 0x04002D03 RID: 11523
		public Image miniIndicator;

		// Token: 0x04002D04 RID: 11524
		public GameObject checkmark;

		// Token: 0x04002D05 RID: 11525
		public bool active = true;
	}
}
