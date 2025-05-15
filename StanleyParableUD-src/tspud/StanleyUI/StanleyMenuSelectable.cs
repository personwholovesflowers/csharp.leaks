using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000204 RID: 516
	public class StanleyMenuSelectable : Selectable
	{
		// Token: 0x06000BEC RID: 3052 RVA: 0x00035BA2 File Offset: 0x00033DA2
		public override Selectable FindSelectableOnUp()
		{
			return StanleyMenuTools.GetPrevActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00035BC2 File Offset: 0x00033DC2
		public override Selectable FindSelectableOnDown()
		{
			return StanleyMenuTools.GetNextActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00035BE2 File Offset: 0x00033DE2
		public override void OnPointerEnter(PointerEventData eventData)
		{
			StanleyMenuTools.StanleyMenuOnPointerEnter(new Action<PointerEventData>(base.OnPointerEnter), eventData);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00035BF6 File Offset: 0x00033DF6
		public override void OnSelect(BaseEventData eventData)
		{
			StanleyMenuTools.StanleyMenuSelectableOnSelect(this, eventData);
			base.OnSelect(eventData);
		}
	}
}
