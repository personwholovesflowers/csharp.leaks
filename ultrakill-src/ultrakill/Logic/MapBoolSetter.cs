using System;

namespace Logic
{
	// Token: 0x02000582 RID: 1410
	public class MapBoolSetter : MapVarSetter
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x00102BC4 File Offset: 0x00100DC4
		public override void SetVar()
		{
			base.SetVar();
			BoolInputType boolInputType = this.inputType;
			if (boolInputType == BoolInputType.Set)
			{
				MonoSingleton<MapVarManager>.Instance.SetBool(this.variableName, this.value, false);
				return;
			}
			if (boolInputType != BoolInputType.Toggle)
			{
				return;
			}
			bool valueOrDefault = MonoSingleton<MapVarManager>.Instance.GetBool(this.variableName).GetValueOrDefault();
			MonoSingleton<MapVarManager>.Instance.SetBool(this.variableName, !valueOrDefault, false);
		}

		// Token: 0x04002C39 RID: 11321
		public BoolInputType inputType;

		// Token: 0x04002C3A RID: 11322
		public bool value;
	}
}
