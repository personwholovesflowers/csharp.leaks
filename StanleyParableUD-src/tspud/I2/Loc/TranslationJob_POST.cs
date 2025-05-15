using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x0200029C RID: 668
	public class TranslationJob_POST : TranslationJob_WWW
	{
		// Token: 0x060010C4 RID: 4292 RVA: 0x0005B924 File Offset: 0x00059B24
		public TranslationJob_POST(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			List<string> list = GoogleTranslation.ConvertTranslationRequest(requests, false);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("action", "Translate");
			wwwform.AddField("list", list[0]);
			this.www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(null), wwwform);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0005B994 File Offset: 0x00059B94
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			return this.mJobState;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x0005B9F0 File Offset: 0x00059BF0
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				this.mJobState = TranslationJob.eJobState.Failed;
				return;
			}
			errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
			if (this._OnTranslationReady != null)
			{
				this._OnTranslationReady(this._requests, errorMsg);
			}
			this.mJobState = TranslationJob.eJobState.Succeeded;
		}

		// Token: 0x04000DED RID: 3565
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x04000DEE RID: 3566
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
	}
}
