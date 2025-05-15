using System;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000314 RID: 788
	public static class RenderTextureEx
	{
		// Token: 0x06001409 RID: 5129 RVA: 0x0006ADE4 File Offset: 0x00068FE4
		public static RenderTexture GetTemporary(RenderTexture renderTexture)
		{
			return RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, renderTexture.depth, renderTexture.format);
		}
	}
}
