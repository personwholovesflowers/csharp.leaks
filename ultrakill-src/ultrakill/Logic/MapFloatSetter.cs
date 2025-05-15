using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x0200058B RID: 1419
	public class MapFloatSetter : MapVarSetter
	{
		// Token: 0x06001FFA RID: 8186 RVA: 0x00102FA4 File Offset: 0x001011A4
		public override void SetVar()
		{
			base.SetVar();
			switch (this.inputType)
			{
			case FloatInputType.SetToNumber:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, this.number, false);
				return;
			case FloatInputType.AddNumber:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, MonoSingleton<MapVarManager>.Instance.GetFloat(this.variableName).GetValueOrDefault() + this.number, false);
				return;
			case FloatInputType.RandomRange:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, Random.Range(this.min, this.max), false);
				return;
			case FloatInputType.RandomFromList:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, this.list[Random.Range(0, this.list.Length)], false);
				return;
			case FloatInputType.CopyDifferentVariable:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, MonoSingleton<MapVarManager>.Instance.GetFloat(this.sourceVariableName).GetValueOrDefault(), false);
				return;
			case FloatInputType.MultiplyByNumber:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, (MonoSingleton<MapVarManager>.Instance.GetFloat(this.variableName) ?? 1f) * this.number, false);
				return;
			case FloatInputType.MultiplyByVariable:
				MonoSingleton<MapVarManager>.Instance.SetFloat(this.variableName, (MonoSingleton<MapVarManager>.Instance.GetFloat(this.variableName) ?? 1f) * (MonoSingleton<MapVarManager>.Instance.GetFloat(this.sourceVariableName) ?? 1f), false);
				return;
			default:
				return;
			}
		}

		// Token: 0x04002C57 RID: 11351
		[SerializeField]
		private FloatInputType inputType;

		// Token: 0x04002C58 RID: 11352
		[SerializeField]
		private string sourceVariableName;

		// Token: 0x04002C59 RID: 11353
		[SerializeField]
		private float min;

		// Token: 0x04002C5A RID: 11354
		[SerializeField]
		private float max = 1f;

		// Token: 0x04002C5B RID: 11355
		[SerializeField]
		private float[] list;

		// Token: 0x04002C5C RID: 11356
		[SerializeField]
		private float number;
	}
}
