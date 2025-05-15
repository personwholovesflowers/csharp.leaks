using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x0200057E RID: 1406
	public abstract class MapVarSetter : MonoBehaviour
	{
		// Token: 0x06001FE3 RID: 8163 RVA: 0x00102B5D File Offset: 0x00100D5D
		private void OnEnable()
		{
			if (this.setOnEnable)
			{
				this.SetVar();
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x00102B6D File Offset: 0x00100D6D
		private void Update()
		{
			if (this.setEveryFrame)
			{
				this.SetVar();
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void SetVar()
		{
		}

		// Token: 0x04002C25 RID: 11301
		public string variableName;

		// Token: 0x04002C26 RID: 11302
		public VariablePersistence persistence;

		// Token: 0x04002C27 RID: 11303
		public bool setOnEnable = true;

		// Token: 0x04002C28 RID: 11304
		public bool setEveryFrame;
	}
}
