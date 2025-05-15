using System;

namespace I2.Loc
{
	// Token: 0x02000298 RID: 664
	public class TranslationJob : IDisposable
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x0005B5EE File Offset: 0x000597EE
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Dispose()
		{
		}

		// Token: 0x04000DE1 RID: 3553
		public TranslationJob.eJobState mJobState;

		// Token: 0x02000484 RID: 1156
		public enum eJobState
		{
			// Token: 0x04001722 RID: 5922
			Running,
			// Token: 0x04001723 RID: 5923
			Succeeded,
			// Token: 0x04001724 RID: 5924
			Failed
		}
	}
}
