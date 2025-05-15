using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x020002C1 RID: 705
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x06001244 RID: 4676 RVA: 0x00062A0B File Offset: 0x00060C0B
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00062A2F File Offset: 0x00060C2F
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00062A42 File Offset: 0x00060C42
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		// Token: 0x04000E8E RID: 3726
		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
