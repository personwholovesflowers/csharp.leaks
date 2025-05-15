using System;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x02000238 RID: 568
	internal static class Math
	{
		// Token: 0x06000D3C RID: 3388 RVA: 0x0003C22E File Offset: 0x0003A42E
		public static float Lerp(float a, float b, float mix)
		{
			return a * (1f - mix) + b * mix;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003C23D File Offset: 0x0003A43D
		public static Vector3 Lerp(Vector3 a, Vector3 b, float mix)
		{
			return a * (1f - mix) + b * mix;
		}
	}
}
