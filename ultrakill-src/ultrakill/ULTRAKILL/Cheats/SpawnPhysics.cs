using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062B RID: 1579
	public class SpawnPhysics : ICheat
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x0010DCA4 File Offset: 0x0010BEA4
		public static bool PhysicsDynamic
		{
			get
			{
				SpawnPhysics lastInstance = SpawnPhysics._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x0010DCC2 File Offset: 0x0010BEC2
		public string LongName
		{
			get
			{
				return "Spawn With Physics";
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x0010DCC9 File Offset: 0x0010BEC9
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.physics";
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x0010DCD0 File Offset: 0x0010BED0
		public string ButtonEnabledOverride
		{
			get
			{
				return "DYNAMIC";
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x0010DCD7 File Offset: 0x0010BED7
		public string ButtonDisabledOverride
		{
			get
			{
				return "STATIC";
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0010DCDE File Offset: 0x0010BEDE
		public string Icon
		{
			get
			{
				return "physics";
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0010DCE5 File Offset: 0x0010BEE5
		// (set) Token: 0x060023FF RID: 9215 RVA: 0x0010DCED File Offset: 0x0010BEED
		public bool IsActive { get; private set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0010DCF6 File Offset: 0x0010BEF6
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			SpawnPhysics._lastInstance = this;
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0010DD05 File Offset: 0x0010BF05
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E93 RID: 11923
		private static SpawnPhysics _lastInstance;
	}
}
