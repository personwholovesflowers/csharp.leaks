using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x02000590 RID: 1424
	public class MapIntSetter : MapVarSetter
	{
		// Token: 0x06002004 RID: 8196 RVA: 0x00103360 File Offset: 0x00101560
		public override void SetVar()
		{
			base.SetVar();
			switch (this.inputType)
			{
			case IntInputType.SetToNumber:
				MonoSingleton<MapVarManager>.Instance.SetInt(this.variableName, this.number, false);
				return;
			case IntInputType.AddNumber:
				MonoSingleton<MapVarManager>.Instance.SetInt(this.variableName, this.number + MonoSingleton<MapVarManager>.Instance.GetInt(this.variableName).GetValueOrDefault(), false);
				return;
			case IntInputType.RandomRange:
				MonoSingleton<MapVarManager>.Instance.SetInt(this.variableName, Random.Range(this.min, this.max), false);
				return;
			case IntInputType.RandomFromList:
				MonoSingleton<MapVarManager>.Instance.SetInt(this.variableName, this.list[Random.Range(0, this.list.Length)], false);
				return;
			case IntInputType.CopyDifferentVariable:
				MonoSingleton<MapVarManager>.Instance.SetInt(this.variableName, MonoSingleton<MapVarManager>.Instance.GetInt(this.sourceVariableName) ?? (-1), false);
				return;
			default:
				return;
			}
		}

		// Token: 0x04002C6F RID: 11375
		[SerializeField]
		private IntInputType inputType;

		// Token: 0x04002C70 RID: 11376
		[SerializeField]
		private string sourceVariableName;

		// Token: 0x04002C71 RID: 11377
		[SerializeField]
		private int min;

		// Token: 0x04002C72 RID: 11378
		[SerializeField]
		private int max = 1;

		// Token: 0x04002C73 RID: 11379
		[SerializeField]
		private int[] list;

		// Token: 0x04002C74 RID: 11380
		[SerializeField]
		private int number;
	}
}
