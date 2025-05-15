using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000621 RID: 1569
	public class NoWeaponCooldown : ICheat
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x0010D8AD File Offset: 0x0010BAAD
		public static bool NoCooldown
		{
			get
			{
				return NoWeaponCooldown._lastInstance != null && NoWeaponCooldown._lastInstance.IsActive;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0010D8C2 File Offset: 0x0010BAC2
		public string LongName
		{
			get
			{
				return "No Weapon Cooldown";
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06002385 RID: 9093 RVA: 0x0010D8C9 File Offset: 0x0010BAC9
		public string Identifier
		{
			get
			{
				return "ultrakill.no-weapon-cooldown";
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x0010D8D0 File Offset: 0x0010BAD0
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x0010D8D8 File Offset: 0x0010BAD8
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x0010D8E0 File Offset: 0x0010BAE0
		public string Icon
		{
			get
			{
				return "no-weapon-cooldown";
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x0010D8E7 File Offset: 0x0010BAE7
		// (set) Token: 0x0600238A RID: 9098 RVA: 0x0010D8EF File Offset: 0x0010BAEF
		public bool IsActive { get; private set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600238B RID: 9099 RVA: 0x0010D8F8 File Offset: 0x0010BAF8
		public bool DefaultState { get; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0010D900 File Offset: 0x0010BB00
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			NoWeaponCooldown._lastInstance = this;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0010D90F File Offset: 0x0010BB0F
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E7D RID: 11901
		private static NoWeaponCooldown _lastInstance;
	}
}
