using System;

namespace Polybrush
{
	// Token: 0x02000222 RID: 546
	public struct z_ColorMask
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x00036A6D File Offset: 0x00034C6D
		public z_ColorMask(bool r, bool g, bool b, bool a)
		{
			this.r = r;
			this.b = b;
			this.g = g;
			this.a = a;
		}

		// Token: 0x04000BCD RID: 3021
		public bool r;

		// Token: 0x04000BCE RID: 3022
		public bool g;

		// Token: 0x04000BCF RID: 3023
		public bool b;

		// Token: 0x04000BD0 RID: 3024
		public bool a;
	}
}
