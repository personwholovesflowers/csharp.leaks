using System;

namespace GameConsole
{
	// Token: 0x020005AF RID: 1455
	[Flags]
	public enum FilterLevel
	{
		// Token: 0x04002CF7 RID: 11511
		None = 0,
		// Token: 0x04002CF8 RID: 11512
		Info = 1,
		// Token: 0x04002CF9 RID: 11513
		Warning = 2,
		// Token: 0x04002CFA RID: 11514
		Error = 4,
		// Token: 0x04002CFB RID: 11515
		All = 7
	}
}
