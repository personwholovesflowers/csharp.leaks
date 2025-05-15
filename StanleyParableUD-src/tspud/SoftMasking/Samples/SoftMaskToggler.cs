using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x020001F1 RID: 497
	public class SoftMaskToggler : MonoBehaviour
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x000348A4 File Offset: 0x00032AA4
		public void Toggle(bool enabled)
		{
			if (this.mask)
			{
				this.mask.GetComponent<SoftMask>().enabled = enabled;
				this.mask.GetComponent<Mask>().enabled = !enabled;
				if (!this.doNotTouchImage)
				{
					this.mask.GetComponent<Image>().enabled = !enabled;
				}
			}
		}

		// Token: 0x04000B24 RID: 2852
		public GameObject mask;

		// Token: 0x04000B25 RID: 2853
		public bool doNotTouchImage;
	}
}
