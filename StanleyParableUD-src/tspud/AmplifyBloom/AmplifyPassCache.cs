using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000331 RID: 817
	[Serializable]
	public class AmplifyPassCache
	{
		// Token: 0x060014EB RID: 5355 RVA: 0x0006F94E File Offset: 0x0006DB4E
		public AmplifyPassCache()
		{
			this.Offsets = new Vector4[16];
			this.Weights = new Vector4[16];
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0006F970 File Offset: 0x0006DB70
		public void Destroy()
		{
			this.Offsets = null;
			this.Weights = null;
		}

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		internal Vector4[] Offsets;

		// Token: 0x040010FF RID: 4351
		[SerializeField]
		internal Vector4[] Weights;
	}
}
