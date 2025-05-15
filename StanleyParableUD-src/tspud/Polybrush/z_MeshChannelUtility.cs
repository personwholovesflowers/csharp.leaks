using System;

namespace Polybrush
{
	// Token: 0x02000219 RID: 537
	public static class z_MeshChannelUtility
	{
		// Token: 0x06000C2C RID: 3116 RVA: 0x0003656C File Offset: 0x0003476C
		public static z_MeshChannel StringToEnum(string str)
		{
			string text = str.ToUpper();
			foreach (object obj in Enum.GetValues(typeof(z_MeshChannel)))
			{
				if (text.Equals(((z_MeshChannel)obj).ToString().ToUpper()))
				{
					return (z_MeshChannel)obj;
				}
			}
			return z_MeshChannel.Null;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000365FC File Offset: 0x000347FC
		public static int UVChannelToIndex(z_MeshChannel channel)
		{
			if (channel == z_MeshChannel.UV0)
			{
				return 0;
			}
			if (channel == z_MeshChannel.UV2)
			{
				return 1;
			}
			if (channel == z_MeshChannel.UV3)
			{
				return 2;
			}
			if (channel == z_MeshChannel.UV4)
			{
				return 3;
			}
			return -1;
		}
	}
}
