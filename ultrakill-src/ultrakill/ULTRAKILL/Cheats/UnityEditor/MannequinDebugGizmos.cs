using System;
using UnityEngine;

namespace ULTRAKILL.Cheats.UnityEditor
{
	// Token: 0x02000633 RID: 1587
	public class MannequinDebugGizmos : ICheat
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x0010E510 File Offset: 0x0010C710
		public static bool Enabled
		{
			get
			{
				if (Application.isEditor && Debug.isDebugBuild)
				{
					MannequinDebugGizmos lastInstance = MannequinDebugGizmos._lastInstance;
					return lastInstance != null && lastInstance.IsActive;
				}
				return false;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x0010E53E File Offset: 0x0010C73E
		public string LongName
		{
			get
			{
				return "Mannequin Debug Gizmos";
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0010E545 File Offset: 0x0010C745
		public string Identifier
		{
			get
			{
				return "ultrakill.editor.debug-gizmos";
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x0010E54C File Offset: 0x0010C74C
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x0010E554 File Offset: 0x0010C754
		public bool IsActive { get; private set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0010E55D File Offset: 0x0010C75D
		public void Enable(CheatsManager manager)
		{
			this.IsActive = Application.isEditor;
			MannequinDebugGizmos._lastInstance = this;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0010E570 File Offset: 0x0010C770
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002EAF RID: 11951
		private static MannequinDebugGizmos _lastInstance;
	}
}
