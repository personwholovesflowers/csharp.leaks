using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F3 RID: 755
	public class RenderUtility
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x00068A14 File Offset: 0x00066C14
		public static int GetFreeLayer()
		{
			for (int i = 16; i < 32; i++)
			{
				if (LayerMask.LayerToName(i) == "" && (RenderUtility.mReservedLayers == null || !RenderUtility.mReservedLayers.Contains(i)))
				{
					return i;
				}
			}
			Debug.LogError("Ferr is looking for an unnamed render layer after 15, but none are free!");
			return -1;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00068A62 File Offset: 0x00066C62
		public static void ReserveLayer(int aLayerID)
		{
			if (RenderUtility.mReservedLayers == null)
			{
				RenderUtility.mReservedLayers = new List<int>();
			}
			RenderUtility.mReservedLayers.Add(aLayerID);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00068A80 File Offset: 0x00066C80
		public static Camera CreateRenderCamera()
		{
			if (RenderUtility.mCamera == null)
			{
				RenderUtility.mCamera = new GameObject("Ferr Render Cam").AddComponent<Camera>();
				RenderUtility.mCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
				RenderUtility.mCamera.enabled = false;
			}
			return RenderUtility.mCamera;
		}

		// Token: 0x04000F51 RID: 3921
		private static List<int> mReservedLayers;

		// Token: 0x04000F52 RID: 3922
		private static Camera mCamera;
	}
}
