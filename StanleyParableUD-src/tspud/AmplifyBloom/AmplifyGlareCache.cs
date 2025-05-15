using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000333 RID: 819
	[Serializable]
	public class AmplifyGlareCache
	{
		// Token: 0x060014EF RID: 5359 RVA: 0x0006F9E8 File Offset: 0x0006DBE8
		public AmplifyGlareCache()
		{
			this.Starlines = new AmplifyStarlineCache[4];
			this.CromaticAberrationMat = new Vector4[4, 8];
			for (int i = 0; i < 4; i++)
			{
				this.Starlines[i] = new AmplifyStarlineCache();
			}
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0006FA30 File Offset: 0x0006DC30
		public void Destroy()
		{
			for (int i = 0; i < 4; i++)
			{
				this.Starlines[i].Destroy();
			}
			this.Starlines = null;
			this.CromaticAberrationMat = null;
		}

		// Token: 0x04001101 RID: 4353
		[SerializeField]
		internal AmplifyStarlineCache[] Starlines;

		// Token: 0x04001102 RID: 4354
		[SerializeField]
		internal Vector4 AverageWeight;

		// Token: 0x04001103 RID: 4355
		[SerializeField]
		internal Vector4[,] CromaticAberrationMat;

		// Token: 0x04001104 RID: 4356
		[SerializeField]
		internal int TotalRT;

		// Token: 0x04001105 RID: 4357
		[SerializeField]
		internal GlareDefData GlareDef;

		// Token: 0x04001106 RID: 4358
		[SerializeField]
		internal StarDefData StarDef;

		// Token: 0x04001107 RID: 4359
		[SerializeField]
		internal int CurrentPassCount;
	}
}
