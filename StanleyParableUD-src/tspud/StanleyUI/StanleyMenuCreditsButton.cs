using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x020001FF RID: 511
	public class StanleyMenuCreditsButton : Button
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x00035BE2 File Offset: 0x00033DE2
		public override void OnPointerEnter(PointerEventData eventData)
		{
			StanleyMenuTools.StanleyMenuOnPointerEnter(new Action<PointerEventData>(base.OnPointerEnter), eventData);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00035BF6 File Offset: 0x00033DF6
		public override void OnSelect(BaseEventData eventData)
		{
			StanleyMenuTools.StanleyMenuSelectableOnSelect(this, eventData);
			base.OnSelect(eventData);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00035C0E File Offset: 0x00033E0E
		public void OpenWebPageOnPC(string url)
		{
			if (PlatformSettings.Instance.isStandalone.GetBooleanValue())
			{
				Application.OpenURL(url);
			}
		}
	}
}
