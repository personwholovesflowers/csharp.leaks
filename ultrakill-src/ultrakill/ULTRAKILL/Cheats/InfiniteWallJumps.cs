using System;
using System.Collections;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000618 RID: 1560
	public class InfiniteWallJumps : ICheat
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x0010D2B4 File Offset: 0x0010B4B4
		public string LongName
		{
			get
			{
				return "Infinite Wall Jumps";
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x0010D2BB File Offset: 0x0010B4BB
		public string Identifier
		{
			get
			{
				return "ultrakill.infinite-wall-jumps";
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0010D2C2 File Offset: 0x0010B4C2
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x0010D2CA File Offset: 0x0010B4CA
		public string ButtonDisabledOverride { get; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x0010D2D2 File Offset: 0x0010B4D2
		public string Icon
		{
			get
			{
				return "infinite-wall-jumps";
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0010D2D9 File Offset: 0x0010B4D9
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x0010D2E1 File Offset: 0x0010B4E1
		public bool IsActive { get; private set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x0010D2EA File Offset: 0x0010B4EA
		public bool DefaultState { get; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600232A RID: 9002 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0010D2F2 File Offset: 0x0010B4F2
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0010D2FB File Offset: 0x0010B4FB
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0010D304 File Offset: 0x0010B504
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.IsActive)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0010D313 File Offset: 0x0010B513
		public void Update()
		{
			MonoSingleton<NewMovement>.Instance.currentWallJumps = 0;
		}
	}
}
