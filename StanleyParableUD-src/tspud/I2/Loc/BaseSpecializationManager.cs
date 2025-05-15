using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000291 RID: 657
	public class BaseSpecializationManager
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x00056620 File Offset: 0x00054820
		public virtual void InitializeSpecializations()
		{
			this.mSpecializations = new string[]
			{
				"Any", "PC", "Touch", "Controller", "VR", "XBox", "PS4", "OculusVR", "ViveVR", "GearVR",
				"Android", "IOS"
			};
			this.mSpecializationsFallbacks = new Dictionary<string, string>
			{
				{ "XBox", "Controller" },
				{ "PS4", "Controller" },
				{ "OculusVR", "VR" },
				{ "ViveVR", "VR" },
				{ "GearVR", "VR" },
				{ "Android", "Touch" },
				{ "IOS", "Touch" }
			};
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00056718 File Offset: 0x00054918
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00056730 File Offset: 0x00054930
		public virtual string GetFallbackSpecialization(string specialization)
		{
			if (this.mSpecializationsFallbacks == null)
			{
				this.InitializeSpecializations();
			}
			string text;
			if (this.mSpecializationsFallbacks.TryGetValue(specialization, out text))
			{
				return text;
			}
			return "Any";
		}

		// Token: 0x04000DCC RID: 3532
		public string[] mSpecializations;

		// Token: 0x04000DCD RID: 3533
		public Dictionary<string, string> mSpecializationsFallbacks;
	}
}
