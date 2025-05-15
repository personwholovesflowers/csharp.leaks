using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x0200029A RID: 666
	public class TranslationJob_GET : TranslationJob_WWW
	{
		// Token: 0x060010BD RID: 4285 RVA: 0x0005B61A File Offset: 0x0005981A
		public TranslationJob_GET(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
			this.GetState();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0005B644 File Offset: 0x00059844
		private void ExecuteNextQuery()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			int num = this.mQueries.Count - 1;
			string text = this.mQueries[num];
			this.mQueries.RemoveAt(num);
			string text2 = string.Format("{0}?action=Translate&list={1}", LocalizationManager.GetWebServiceURL(null), text);
			this.www = UnityWebRequest.Get(text2);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0005B6B8 File Offset: 0x000598B8
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			if (this.www == null)
			{
				this.ExecuteNextQuery();
			}
			return this.mJobState;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0005B724 File Offset: 0x00059924
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
				if (string.IsNullOrEmpty(errorMsg))
				{
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, null);
					}
					return;
				}
			}
			this.mJobState = TranslationJob.eJobState.Failed;
			this.mErrorMessage = errorMsg;
		}

		// Token: 0x04000DE3 RID: 3555
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04000DE4 RID: 3556
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x04000DE5 RID: 3557
		private List<string> mQueries;

		// Token: 0x04000DE6 RID: 3558
		public string mErrorMessage;
	}
}
