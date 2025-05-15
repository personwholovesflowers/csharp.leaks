using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000624 RID: 1572
	public class ExperimentalArmRotation : ICheat
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060023A9 RID: 9129 RVA: 0x0010D9B9 File Offset: 0x0010BBB9
		public string LongName
		{
			get
			{
				return "Experimental Arm Rotation";
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x0010D9C0 File Offset: 0x0010BBC0
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.enable-experimental-rotation";
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060023AB RID: 9131 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060023AD RID: 9133 RVA: 0x0010D9C7 File Offset: 0x0010BBC7
		public string Icon
		{
			get
			{
				return "rotate";
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x0010D9CE File Offset: 0x0010BBCE
		public bool IsActive
		{
			get
			{
				return ExperimentalArmRotation.Enabled;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0010D9D5 File Offset: 0x0010BBD5
		public void Enable(CheatsManager manager)
		{
			ExperimentalArmRotation.Enabled = true;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0010D9DD File Offset: 0x0010BBDD
		public void Disable()
		{
			ExperimentalArmRotation.Enabled = false;
		}

		// Token: 0x04002E88 RID: 11912
		public static bool Enabled;
	}
}
