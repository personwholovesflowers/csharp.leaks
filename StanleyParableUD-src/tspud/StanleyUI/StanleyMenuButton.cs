using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x020001FE RID: 510
	public class StanleyMenuButton : Button
	{
		// Token: 0x06000BCA RID: 3018 RVA: 0x00035BA2 File Offset: 0x00033DA2
		public override Selectable FindSelectableOnUp()
		{
			return StanleyMenuTools.GetPrevActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00035BC2 File Offset: 0x00033DC2
		public override Selectable FindSelectableOnDown()
		{
			return StanleyMenuTools.GetNextActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00035BE2 File Offset: 0x00033DE2
		public override void OnPointerEnter(PointerEventData eventData)
		{
			StanleyMenuTools.StanleyMenuOnPointerEnter(new Action<PointerEventData>(base.OnPointerEnter), eventData);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00035BF6 File Offset: 0x00033DF6
		public override void OnSelect(BaseEventData eventData)
		{
			StanleyMenuTools.StanleyMenuSelectableOnSelect(this, eventData);
			base.OnSelect(eventData);
		}
	}
}
