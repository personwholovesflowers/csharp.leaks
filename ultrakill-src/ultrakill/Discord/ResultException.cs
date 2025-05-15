using System;

namespace Discord
{
	// Token: 0x0200068A RID: 1674
	public class ResultException : Exception
	{
		// Token: 0x06002547 RID: 9543 RVA: 0x0010F081 File Offset: 0x0010D281
		public ResultException(Result result)
			: base(result.ToString())
		{
		}

		// Token: 0x04002F95 RID: 12181
		public readonly Result Result;
	}
}
