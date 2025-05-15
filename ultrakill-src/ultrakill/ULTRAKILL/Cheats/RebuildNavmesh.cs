using System;
using System.Collections;
using UnityEngine.Events;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000628 RID: 1576
	public class RebuildNavmesh : ICheat
	{
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x0010DB13 File Offset: 0x0010BD13
		public string LongName
		{
			get
			{
				return "Enemy Navigation";
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0010DB1A File Offset: 0x0010BD1A
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.rebuild-nav";
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x0010DB21 File Offset: 0x0010BD21
		public string ButtonEnabledOverride
		{
			get
			{
				return "REBUILDING...";
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x0010DB28 File Offset: 0x0010BD28
		public string ButtonDisabledOverride
		{
			get
			{
				return "REBUILD";
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x0010DB2F File Offset: 0x0010BD2F
		public string Icon
		{
			get
			{
				return "navmesh";
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x0010DB36 File Offset: 0x0010BD36
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x0010DB3E File Offset: 0x0010BD3E
		public bool IsActive { get; private set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0010DB47 File Offset: 0x0010BD47
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			MonoSingleton<CheatsManager>.Instance.StartCoroutine(this.RebuildDelayed());
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x0010DB61 File Offset: 0x0010BD61
		private IEnumerator RebuildDelayed()
		{
			yield return null;
			SandboxNavmesh instance = MonoSingleton<SandboxNavmesh>.Instance;
			instance.navmeshBuilt = (UnityAction)Delegate.Combine(instance.navmeshBuilt, new UnityAction(this.NavmeshBuilt));
			MonoSingleton<SandboxNavmesh>.Instance.Rebake();
			yield break;
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0010DB70 File Offset: 0x0010BD70
		private void NavmeshBuilt()
		{
			SandboxNavmesh instance = MonoSingleton<SandboxNavmesh>.Instance;
			instance.navmeshBuilt = (UnityAction)Delegate.Remove(instance.navmeshBuilt, new UnityAction(this.NavmeshBuilt));
			this.IsActive = false;
			MonoSingleton<CheatsManager>.Instance.UpdateCheatState(this);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Disable()
		{
		}
	}
}
