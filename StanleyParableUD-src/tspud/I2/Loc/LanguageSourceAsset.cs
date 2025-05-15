using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002A1 RID: 673
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0005C293 File Offset: 0x0005A493
		// (set) Token: 0x060010DF RID: 4319 RVA: 0x0005C29B File Offset: 0x0005A49B
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x04000E13 RID: 3603
		public LanguageSourceData mSource = new LanguageSourceData();
	}
}
