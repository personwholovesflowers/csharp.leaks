using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x0200032C RID: 812
	[Serializable]
	public class AmplifyBokehData
	{
		// Token: 0x060014A5 RID: 5285 RVA: 0x0006E42B File Offset: 0x0006C62B
		public AmplifyBokehData(Vector4[] offsets)
		{
			this.Offsets = offsets;
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0006E43A File Offset: 0x0006C63A
		public void Destroy()
		{
			if (this.BokehRenderTexture != null)
			{
				AmplifyUtils.ReleaseTempRenderTarget(this.BokehRenderTexture);
				this.BokehRenderTexture = null;
			}
			this.Offsets = null;
		}

		// Token: 0x040010C8 RID: 4296
		internal RenderTexture BokehRenderTexture;

		// Token: 0x040010C9 RID: 4297
		internal Vector4[] Offsets;
	}
}
