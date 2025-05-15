using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000332 RID: 818
	[Serializable]
	public class AmplifyStarlineCache
	{
		// Token: 0x060014ED RID: 5357 RVA: 0x0006F980 File Offset: 0x0006DB80
		public AmplifyStarlineCache()
		{
			this.Passes = new AmplifyPassCache[4];
			for (int i = 0; i < 4; i++)
			{
				this.Passes[i] = new AmplifyPassCache();
			}
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0006F9B8 File Offset: 0x0006DBB8
		public void Destroy()
		{
			for (int i = 0; i < 4; i++)
			{
				this.Passes[i].Destroy();
			}
			this.Passes = null;
		}

		// Token: 0x04001100 RID: 4352
		[SerializeField]
		internal AmplifyPassCache[] Passes;
	}
}
