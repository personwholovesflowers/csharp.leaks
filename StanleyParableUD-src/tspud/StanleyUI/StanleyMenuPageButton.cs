using System;
using UnityEngine;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000201 RID: 513
	public class StanleyMenuPageButton : MonoBehaviour, ISelectableHolderScreen
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00035FDB File Offset: 0x000341DB
		public Selectable DefaultSelectable
		{
			get
			{
				return this.defaultSelectable;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00035FE3 File Offset: 0x000341E3
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00035FEB File Offset: 0x000341EB
		public Selectable LastSelectable
		{
			get
			{
				return this.lastSelectable;
			}
			set
			{
				this.lastSelectable = value;
			}
		}

		// Token: 0x04000B64 RID: 2916
		[SerializeField]
		private Selectable defaultSelectable;

		// Token: 0x04000B65 RID: 2917
		private Selectable lastSelectable;
	}
}
