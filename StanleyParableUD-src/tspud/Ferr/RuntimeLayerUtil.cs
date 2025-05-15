using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002EE RID: 750
	public static class RuntimeLayerUtil
	{
		// Token: 0x06001386 RID: 4998 RVA: 0x00067F78 File Offset: 0x00066178
		public static int GetFreeLayer()
		{
			for (int i = 16; i < 32; i++)
			{
				if (LayerMask.LayerToName(i) == "" && (RuntimeLayerUtil.mReservedLayers == null || !RuntimeLayerUtil.mReservedLayers.Contains(i)))
				{
					return i;
				}
			}
			Debug.LogError("Ferr is looking for an unnamed render layer after 15, but none are free!");
			return -1;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00067FC6 File Offset: 0x000661C6
		public static void ReserveLayer(int aLayerID)
		{
			if (RuntimeLayerUtil.mReservedLayers == null)
			{
				RuntimeLayerUtil.mReservedLayers = new List<int>();
			}
			RuntimeLayerUtil.mReservedLayers.Add(aLayerID);
		}

		// Token: 0x04000F41 RID: 3905
		private static List<int> mReservedLayers;
	}
}
