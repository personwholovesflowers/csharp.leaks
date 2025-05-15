using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000205 RID: 517
	public class StanleyMenuSelector : Button
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00035BA2 File Offset: 0x00033DA2
		public override Selectable FindSelectableOnUp()
		{
			return StanleyMenuTools.GetPrevActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00035BC2 File Offset: 0x00033DC2
		public override Selectable FindSelectableOnDown()
		{
			return StanleyMenuTools.GetNextActiveSiblingSelectable(base.transform, new Type[] { typeof(StanleyMenuSelectable) });
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00035BE2 File Offset: 0x00033DE2
		public override void OnPointerEnter(PointerEventData eventData)
		{
			StanleyMenuTools.StanleyMenuOnPointerEnter(new Action<PointerEventData>(base.OnPointerEnter), eventData);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00035BF6 File Offset: 0x00033DF6
		public override void OnSelect(BaseEventData eventData)
		{
			StanleyMenuTools.StanleyMenuSelectableOnSelect(this, eventData);
			base.OnSelect(eventData);
		}
	}
}
