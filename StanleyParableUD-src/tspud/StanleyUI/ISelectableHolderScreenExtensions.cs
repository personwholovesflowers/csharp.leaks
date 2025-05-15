using System;
using UnityEngine;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000203 RID: 515
	public static class ISelectableHolderScreenExtensions
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x00035FF4 File Offset: 0x000341F4
		public static Selectable GetDefaultSelectableOrFirstActiveSibling(this ISelectableHolderScreen s)
		{
			if (!s.DefaultSelectable.gameObject.activeSelf)
			{
				for (int i = 0; i < s.DefaultSelectable.transform.parent.childCount; i++)
				{
					Selectable component = s.DefaultSelectable.transform.parent.GetChild(i).GetComponent<Selectable>();
					if (component != null && component.gameObject.activeSelf)
					{
						return component;
					}
				}
			}
			return s.DefaultSelectable;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0003606D File Offset: 0x0003426D
		public static GameObject DefaultGameObjectOrNull(this ISelectableHolderScreen s)
		{
			if (!(s.DefaultSelectable == null))
			{
				return s.GetDefaultSelectableOrFirstActiveSibling().gameObject;
			}
			return null;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0003608A File Offset: 0x0003428A
		public static GameObject LastGameObjectOrNull(this ISelectableHolderScreen s)
		{
			if (!(s.LastSelectable == null))
			{
				return s.LastSelectable.gameObject;
			}
			return null;
		}
	}
}
