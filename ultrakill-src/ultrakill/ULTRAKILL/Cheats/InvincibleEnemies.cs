using System;
using System.Collections;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200061B RID: 1563
	public class InvincibleEnemies : ICheat
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x0010D400 File Offset: 0x0010B600
		public static bool Enabled
		{
			get
			{
				InvincibleEnemies lastInstance = InvincibleEnemies._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x0010D41E File Offset: 0x0010B61E
		public string LongName
		{
			get
			{
				return "Invincible Enemies";
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x0010D425 File Offset: 0x0010B625
		public string Identifier
		{
			get
			{
				return "ultrakill.invincible-enemies";
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x0010D42C File Offset: 0x0010B62C
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x0010D434 File Offset: 0x0010B634
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0010D43C File Offset: 0x0010B63C
		public string Icon
		{
			get
			{
				return "invincible-enemies";
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x0010D443 File Offset: 0x0010B643
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x0010D44B File Offset: 0x0010B64B
		public bool IsActive { get; private set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x0010D454 File Offset: 0x0010B654
		public bool DefaultState { get; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0010D45C File Offset: 0x0010B65C
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			InvincibleEnemies._lastInstance = this;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0010D46B File Offset: 0x0010B66B
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0010D474 File Offset: 0x0010B674
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.IsActive)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0010D313 File Offset: 0x0010B513
		private void Update()
		{
			MonoSingleton<NewMovement>.Instance.currentWallJumps = 0;
		}

		// Token: 0x04002E6B RID: 11883
		private static InvincibleEnemies _lastInstance;
	}
}
