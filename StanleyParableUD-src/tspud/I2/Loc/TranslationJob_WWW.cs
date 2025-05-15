using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x02000299 RID: 665
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x060010BB RID: 4283 RVA: 0x0005B5F6 File Offset: 0x000597F6
		public override void Dispose()
		{
			if (this.www != null)
			{
				this.www.Dispose();
			}
			this.www = null;
		}

		// Token: 0x04000DE2 RID: 3554
		public UnityWebRequest www;
	}
}
