using System;
using UnityEngine;

namespace ULTRAKILL.Cheats.UnityEditor
{
	// Token: 0x02000634 RID: 1588
	public class NapalmDebugVoxels : ICheat
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x0010E57C File Offset: 0x0010C77C
		public static bool Enabled
		{
			get
			{
				if (Application.isEditor && Debug.isDebugBuild)
				{
					NapalmDebugVoxels lastInstance = NapalmDebugVoxels._lastInstance;
					return lastInstance != null && lastInstance.active;
				}
				return false;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0010E5AA File Offset: 0x0010C7AA
		public string LongName
		{
			get
			{
				return "Napalm Debug Voxels";
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x0010E5B1 File Offset: 0x0010C7B1
		public string Identifier
		{
			get
			{
				return "ultrakill.editor.debug-napalm-voxels";
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x0010E5B8 File Offset: 0x0010C7B8
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0010E5C0 File Offset: 0x0010C7C0
		public void Enable(CheatsManager manager)
		{
			this.active = Application.isEditor;
			NapalmDebugVoxels._lastInstance = this;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0010E5D3 File Offset: 0x0010C7D3
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x04002EB1 RID: 11953
		private static NapalmDebugVoxels _lastInstance;

		// Token: 0x04002EB2 RID: 11954
		private bool active;
	}
}
