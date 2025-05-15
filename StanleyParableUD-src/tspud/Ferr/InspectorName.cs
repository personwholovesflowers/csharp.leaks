using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002E8 RID: 744
	public class InspectorName : PropertyAttribute
	{
		// Token: 0x06001365 RID: 4965 RVA: 0x00067460 File Offset: 0x00065660
		public InspectorName(string aName)
		{
			this.mName = aName;
		}

		// Token: 0x04000F1C RID: 3868
		public string mName;
	}
}
