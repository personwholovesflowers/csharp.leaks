using System;

namespace I2.Loc
{
	// Token: 0x0200028A RID: 650
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x06001054 RID: 4180 RVA: 0x00055C80 File Offset: 0x00053E80
		public override string GetParameterValue(string ParamName)
		{
			if (ParamName == "WINNER")
			{
				return "Javier";
			}
			if (ParamName == "NUM PLAYERS")
			{
				return 5.ToString();
			}
			return null;
		}
	}
}
