using System;
using Logic;
using pcon.core.Attributes;
using pcon.core.Interfaces;
using plog;

namespace GameConsole.pcon
{
	// Token: 0x020005BD RID: 1469
	[RegisterIncomingMessage("ultrakill.mapvar.update")]
	public class MapVarChange : IReceive
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x00107EFC File Offset: 0x001060FC
		public string type
		{
			get
			{
				return "ultrakill.mapvar.update";
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00107F04 File Offset: 0x00106104
		public void Receive()
		{
			MapVarChange.Log.Fine(string.Format("Received change from console <b>{0}</b> - <b>{1}</b>", this.variable.value.type, this.variable.value.value), null, null, null);
			string type = this.variable.value.type;
			if (!(type == "System.Int32"))
			{
				if (!(type == "System.Boolean"))
				{
					if (!(type == "System.Single"))
					{
						if (!(type == "System.String"))
						{
							MapVarChange.Log.Error("Unknown type " + this.variable.value.type, null, null, null);
							return;
						}
						MonoSingleton<MapVarManager>.Instance.SetString(this.variable.name, this.variable.value.value.ToString(), false);
						return;
					}
					else
					{
						float num;
						if (float.TryParse(this.variable.value.value.ToString(), out num))
						{
							MonoSingleton<MapVarManager>.Instance.SetFloat(this.variable.name, num, false);
							return;
						}
						MapVarChange.Log.Warning(string.Format("Failed to parse {0} as float", this.variable.value.value), null, null, null);
						return;
					}
				}
				else
				{
					object value = this.variable.value.value;
					if (value is bool)
					{
						bool flag = (bool)value;
						MonoSingleton<MapVarManager>.Instance.SetBool(this.variable.name, flag, false);
						return;
					}
					MapVarChange.Log.Warning(string.Format("Failed to parse {0} as bool", this.variable.value.value), null, null, null);
					return;
				}
			}
			else
			{
				int num2;
				if (int.TryParse(this.variable.value.value.ToString(), out num2))
				{
					MonoSingleton<MapVarManager>.Instance.SetInt(this.variable.name, num2, false);
					return;
				}
				MapVarChange.Log.Warning(string.Format("Failed to parse {0} as int", this.variable.value.value), null, null, null);
				return;
			}
		}

		// Token: 0x04002D2B RID: 11563
		private static readonly Logger Log = new Logger("MapVarChange");

		// Token: 0x04002D2C RID: 11564
		private const string Type = "ultrakill.mapvar.update";

		// Token: 0x04002D2D RID: 11565
		public MapVarField variable;
	}
}
