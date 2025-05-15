using System;
using System.Text;

namespace SettingsMenu.Models
{
	// Token: 0x02000537 RID: 1335
	[Serializable]
	public class PreferenceEntry
	{
		// Token: 0x06001E33 RID: 7731 RVA: 0x000FA86E File Offset: 0x000F8A6E
		public bool IsBool()
		{
			return this.type == PreferenceType.Bool;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000FA879 File Offset: 0x000F8A79
		public bool IsInt()
		{
			return this.type == PreferenceType.Int;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000FA884 File Offset: 0x000F8A84
		public bool IsFloat()
		{
			return this.type == PreferenceType.Float;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000FA890 File Offset: 0x000F8A90
		public void Apply()
		{
			switch (this.type)
			{
			case PreferenceType.Bool:
				this.key.SetBoolValue(this.boolValue);
				return;
			case PreferenceType.Int:
				this.key.SetIntValue(this.intValue);
				return;
			case PreferenceType.Float:
				this.key.SetFloatValue(this.floatValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000FA8EC File Offset: 0x000F8AEC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("[{0}] ({1}) = ", this.key, this.type));
			switch (this.type)
			{
			case PreferenceType.Bool:
				stringBuilder.Append(this.boolValue);
				break;
			case PreferenceType.Int:
				stringBuilder.Append(this.intValue);
				break;
			case PreferenceType.Float:
				stringBuilder.Append(this.floatValue);
				break;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002ACC RID: 10956
		public PreferenceKey key;

		// Token: 0x04002ACD RID: 10957
		public PreferenceType type;

		// Token: 0x04002ACE RID: 10958
		public bool boolValue;

		// Token: 0x04002ACF RID: 10959
		public int intValue;

		// Token: 0x04002AD0 RID: 10960
		public float floatValue;
	}
}
