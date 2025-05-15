using System;

namespace SettingsMenu.Models
{
	// Token: 0x02000539 RID: 1337
	[Serializable]
	public class SliderConfig
	{
		// Token: 0x04002AD5 RID: 10965
		public float minValue;

		// Token: 0x04002AD6 RID: 10966
		public float maxValue = 100f;

		// Token: 0x04002AD7 RID: 10967
		public bool wholeNumbers = true;

		// Token: 0x04002AD8 RID: 10968
		public SliderValueToTextConfig textConfig;
	}
}
