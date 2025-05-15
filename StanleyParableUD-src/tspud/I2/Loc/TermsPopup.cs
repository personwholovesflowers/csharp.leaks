using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002BE RID: 702
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x0600123B RID: 4667 RVA: 0x00062978 File Offset: 0x00060B78
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x00062987 File Offset: 0x00060B87
		// (set) Token: 0x0600123D RID: 4669 RVA: 0x0006298F File Offset: 0x00060B8F
		public string Filter { get; private set; }
	}
}
