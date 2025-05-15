using System;

namespace Logic
{
	// Token: 0x02000588 RID: 1416
	[Serializable]
	public struct StringPart
	{
		// Token: 0x06001FF9 RID: 8185 RVA: 0x00102ED8 File Offset: 0x001010D8
		public string GetString()
		{
			switch (this.type)
			{
			case StringPartType.NormalText:
				return this.value;
			case StringPartType.NewLine:
				return "\n";
			case StringPartType.Variable:
				switch (this.variableType)
				{
				case VariableType.Bool:
					return MonoSingleton<MapVarManager>.Instance.GetBool(this.value).ToString();
				case VariableType.Int:
					return MonoSingleton<MapVarManager>.Instance.GetInt(this.value).ToString();
				case VariableType.String:
					return MonoSingleton<MapVarManager>.Instance.GetString(this.value);
				case VariableType.Float:
					return MonoSingleton<MapVarManager>.Instance.GetFloat(this.value).ToString();
				}
				break;
			}
			return string.Empty;
		}

		// Token: 0x04002C4C RID: 11340
		public StringPartType type;

		// Token: 0x04002C4D RID: 11341
		public VariableType variableType;

		// Token: 0x04002C4E RID: 11342
		public string value;
	}
}
