using System;
using System.Collections;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000631 RID: 1585
	public class HideWeapons : ICheat
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x0010E3B8 File Offset: 0x0010C5B8
		public static bool Active
		{
			get
			{
				HideWeapons instance = HideWeapons.Instance;
				return instance != null && instance.IsActive;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x0010E3D6 File Offset: 0x0010C5D6
		// (set) Token: 0x0600244B RID: 9291 RVA: 0x0010E3DD File Offset: 0x0010C5DD
		private static HideWeapons Instance { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x0010E3E5 File Offset: 0x0010C5E5
		public string LongName
		{
			get
			{
				return "Hide Weapons";
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x0010E3EC File Offset: 0x0010C5EC
		public string Identifier
		{
			get
			{
				return "ultrakill.hide-weapons";
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x0010E3F3 File Offset: 0x0010C5F3
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x0010E3FB File Offset: 0x0010C5FB
		public string ButtonDisabledOverride { get; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x0010E403 File Offset: 0x0010C603
		public string Icon { get; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x0010E40B File Offset: 0x0010C60B
		// (set) Token: 0x06002452 RID: 9298 RVA: 0x0010E413 File Offset: 0x0010C613
		public bool IsActive { get; private set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0010E41C File Offset: 0x0010C61C
		public void Enable(CheatsManager manager)
		{
			HideWeapons.Instance = this;
			this.IsActive = true;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0010E42B File Offset: 0x0010C62B
		public void Disable()
		{
			this.IsActive = false;
			if (!this.gunControlChanged)
			{
				return;
			}
			if (MonoSingleton<GunControl>.Instance != null && !MonoSingleton<GunControl>.Instance.activated)
			{
				MonoSingleton<GunControl>.Instance.YesWeapon();
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0010E460 File Offset: 0x0010C660
		private void Update()
		{
			if (!this.IsActive)
			{
				return;
			}
			if (MonoSingleton<GunControl>.Instance != null && MonoSingleton<GunControl>.Instance.activated)
			{
				this.gunControlChanged = true;
				MonoSingleton<GunControl>.Instance.NoWeapon();
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0010E495 File Offset: 0x0010C695
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.IsActive)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x04002EAB RID: 11947
		private bool gunControlChanged;
	}
}
