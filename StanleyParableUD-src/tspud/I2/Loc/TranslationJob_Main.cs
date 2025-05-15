using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x0200029B RID: 667
	public class TranslationJob_Main : TranslationJob
	{
		// Token: 0x060010C1 RID: 4289 RVA: 0x0005B786 File Offset: 0x00059986
		public TranslationJob_Main(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mPost = new TranslationJob_POST(requests, OnTranslationReady);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0005B7AC File Offset: 0x000599AC
		public override TranslationJob.eJobState GetState()
		{
			if (this.mWeb != null)
			{
				switch (this.mWeb.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mWeb.Dispose();
					this.mWeb = null;
					this.mPost = new TranslationJob_POST(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mPost != null)
			{
				switch (this.mPost.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mPost.Dispose();
					this.mPost = null;
					this.mGet = new TranslationJob_GET(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mGet != null)
			{
				switch (this.mGet.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mErrorMessage = this.mGet.mErrorMessage;
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, this.mErrorMessage);
					}
					this.mGet.Dispose();
					this.mGet = null;
					break;
				}
			}
			return this.mJobState;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0005B8EC File Offset: 0x00059AEC
		public override void Dispose()
		{
			if (this.mPost != null)
			{
				this.mPost.Dispose();
			}
			if (this.mGet != null)
			{
				this.mGet.Dispose();
			}
			this.mPost = null;
			this.mGet = null;
		}

		// Token: 0x04000DE7 RID: 3559
		private TranslationJob_WEB mWeb;

		// Token: 0x04000DE8 RID: 3560
		private TranslationJob_POST mPost;

		// Token: 0x04000DE9 RID: 3561
		private TranslationJob_GET mGet;

		// Token: 0x04000DEA RID: 3562
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04000DEB RID: 3563
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x04000DEC RID: 3564
		public string mErrorMessage;
	}
}
