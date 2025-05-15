using System;
using System.Reflection;

namespace AmplifyColor
{
	// Token: 0x02000321 RID: 801
	[Serializable]
	public class VolumeEffectFieldFlags
	{
		// Token: 0x06001446 RID: 5190 RVA: 0x0006C951 File Offset: 0x0006AB51
		public VolumeEffectFieldFlags(FieldInfo pi)
		{
			this.fieldName = pi.Name;
			this.fieldType = pi.FieldType.FullName;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0006C976 File Offset: 0x0006AB76
		public VolumeEffectFieldFlags(VolumeEffectField field)
		{
			this.fieldName = field.fieldName;
			this.fieldType = field.fieldType;
			this.blendFlag = true;
		}

		// Token: 0x04001051 RID: 4177
		public string fieldName;

		// Token: 0x04001052 RID: 4178
		public string fieldType;

		// Token: 0x04001053 RID: 4179
		public bool blendFlag;
	}
}
