using System;
using UnityEngine;

namespace ULTRAKILL.Cheats.UnityEditor
{
	// Token: 0x02000635 RID: 1589
	public class OverwriteUnlocks : ICheat
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x0010E5DC File Offset: 0x0010C7DC
		public static bool Enabled
		{
			get
			{
				if (Application.isEditor && Debug.isDebugBuild)
				{
					OverwriteUnlocks lastInstance = OverwriteUnlocks._lastInstance;
					return lastInstance != null && lastInstance.IsActive;
				}
				return false;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x0010E60A File Offset: 0x0010C80A
		public string LongName
		{
			get
			{
				return "Overwrite Unlocks";
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x0010E611 File Offset: 0x0010C811
		public string Identifier
		{
			get
			{
				return "ultrakill.editor.overwrite-unlocks";
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x0010E618 File Offset: 0x0010C818
		// (set) Token: 0x06002480 RID: 9344 RVA: 0x0010E620 File Offset: 0x0010C820
		public bool IsActive { get; private set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06002482 RID: 9346 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0010E629 File Offset: 0x0010C829
		public void Enable(CheatsManager manager)
		{
			this.IsActive = Application.isEditor;
			OverwriteUnlocks._lastInstance = this;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0010E63C File Offset: 0x0010C83C
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002EB3 RID: 11955
		private static OverwriteUnlocks _lastInstance;
	}
}
