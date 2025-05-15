using System;
using UnityEngine;

namespace DebugOverlays
{
	// Token: 0x02000603 RID: 1539
	public static class OnGUIHelper
	{
		// Token: 0x06002238 RID: 8760 RVA: 0x0010C1A4 File Offset: 0x0010A3A4
		public static Rect? GetOnScreenRect(Vector3 worldPosition, float width = 300f, float height = 100f)
		{
			Vector3 vector = MonoSingleton<CameraController>.Instance.cam.WorldToScreenPoint(worldPosition);
			if (vector.z < 0f)
			{
				return null;
			}
			return new Rect?(new Rect(vector.x - 50f, (float)Screen.height - vector.y - 50f, width, height));
		}
	}
}
