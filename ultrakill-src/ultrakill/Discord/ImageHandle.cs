using System;

namespace Discord
{
	// Token: 0x02000667 RID: 1639
	public struct ImageHandle
	{
		// Token: 0x06002506 RID: 9478 RVA: 0x0010EC81 File Offset: 0x0010CE81
		public static ImageHandle User(long id)
		{
			return ImageHandle.User(id, 128U);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0010EC90 File Offset: 0x0010CE90
		public static ImageHandle User(long id, uint size)
		{
			return new ImageHandle
			{
				Type = ImageType.User,
				Id = id,
				Size = size
			};
		}

		// Token: 0x04002F4A RID: 12106
		public ImageType Type;

		// Token: 0x04002F4B RID: 12107
		public long Id;

		// Token: 0x04002F4C RID: 12108
		public uint Size;
	}
}
