using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x0200059C RID: 1436
	public class MapStringSetter : MapVarSetter
	{
		// Token: 0x06002030 RID: 8240 RVA: 0x00104630 File Offset: 0x00102830
		public override void SetVar()
		{
			base.SetVar();
			StringInputType stringInputType = this.inputType;
			if (stringInputType == StringInputType.JustText)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.variableName, this.textValue, false);
				return;
			}
			if (stringInputType != StringInputType.CopyDifferentVariable)
			{
				return;
			}
			if (this.sourceVariableType == VariableType.String)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.variableName, MonoSingleton<MapVarManager>.Instance.GetString(this.sourceVariableName), false);
				return;
			}
			if (this.sourceVariableType == VariableType.Int)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.variableName, MonoSingleton<MapVarManager>.Instance.GetInt(this.sourceVariableName).ToString(), false);
				return;
			}
			if (this.sourceVariableType == VariableType.Float)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.variableName, MonoSingleton<MapVarManager>.Instance.GetFloat(this.sourceVariableName).ToString(), false);
				return;
			}
			if (this.sourceVariableType == VariableType.Bool)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.variableName, MonoSingleton<MapVarManager>.Instance.GetBool(this.sourceVariableName).ToString(), false);
			}
		}

		// Token: 0x04002C99 RID: 11417
		[SerializeField]
		private StringInputType inputType;

		// Token: 0x04002C9A RID: 11418
		[SerializeField]
		private string sourceVariableName;

		// Token: 0x04002C9B RID: 11419
		[SerializeField]
		private VariableType sourceVariableType;

		// Token: 0x04002C9C RID: 11420
		[SerializeField]
		private string textValue;
	}
}
