using System;

namespace I2.Loc
{
	// Token: 0x020002CC RID: 716
	internal class TashkeelLocation
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x000639AC File Offset: 0x00061BAC
		public TashkeelLocation(char tashkeel, int position)
		{
			this.tashkeel = tashkeel;
			this.position = position;
		}

		// Token: 0x04000EF1 RID: 3825
		public char tashkeel;

		// Token: 0x04000EF2 RID: 3826
		public int position;
	}
}
