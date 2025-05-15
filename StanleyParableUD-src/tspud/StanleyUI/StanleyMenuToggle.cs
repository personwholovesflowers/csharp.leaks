using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000207 RID: 519
	public class StanleyMenuToggle : Toggle
	{
		// Token: 0x06000BFB RID: 3067 RVA: 0x00035BA2 File Offset: 0x00033DA2
		public override Selectable FindSelectableOnUp()
		{
			return StanleyMenuTools.GetPrevActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00035BC2 File Offset: 0x00033DC2
		public override Selectable FindSelectableOnDown()
		{
			return StanleyMenuTools.GetNextActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00035BE2 File Offset: 0x00033DE2
		public override void OnPointerEnter(PointerEventData eventData)
		{
			StanleyMenuTools.StanleyMenuOnPointerEnter(new Action<PointerEventData>(base.OnPointerEnter), eventData);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00035BF6 File Offset: 0x00033DF6
		public override void OnSelect(BaseEventData eventData)
		{
			StanleyMenuTools.StanleyMenuSelectableOnSelect(this, eventData);
			base.OnSelect(eventData);
		}
	}
}
