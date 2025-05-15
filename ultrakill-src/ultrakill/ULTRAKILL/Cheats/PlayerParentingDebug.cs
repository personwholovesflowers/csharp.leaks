using System;
using System.Collections;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200060B RID: 1547
	public class PlayerParentingDebug : ICheat, ICheatGUI
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x0010C800 File Offset: 0x0010AA00
		public static bool Active
		{
			get
			{
				return PlayerParentingDebug._lastInstance != null && PlayerParentingDebug._lastInstance.active;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x0010C815 File Offset: 0x0010AA15
		public string LongName
		{
			get
			{
				return "Player Parenting Debug";
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600228E RID: 8846 RVA: 0x0010C81C File Offset: 0x0010AA1C
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.player-parent-debug";
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x0010C823 File Offset: 0x0010AA23
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x0010C82B File Offset: 0x0010AA2B
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06002292 RID: 8850 RVA: 0x0010C833 File Offset: 0x0010AA33
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x0010C83B File Offset: 0x0010AA3B
		public bool DefaultState { get; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0010C843 File Offset: 0x0010AA43
		public void Enable(CheatsManager manager)
		{
			this.active = true;
			PlayerParentingDebug._lastInstance = this;
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x0010C852 File Offset: 0x0010AA52
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x0010C85B File Offset: 0x0010AA5B
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.active)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x0010C86A File Offset: 0x0010AA6A
		public void Update()
		{
			this.pmp = Object.FindObjectsOfType<PlayerMovementParenting>();
			if (this.pmp != null)
			{
				int num = this.pmp.Length;
			}
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x0010C888 File Offset: 0x0010AA88
		public void OnGUI()
		{
			GUILayout.Label("Player Parenting Debug", Array.Empty<GUILayoutOption>());
			if (this.pmp == null)
			{
				return;
			}
			foreach (PlayerMovementParenting playerMovementParenting in this.pmp)
			{
				if (!(playerMovementParenting == null))
				{
					GUILayout.Label(playerMovementParenting.gameObject.name, Array.Empty<GUILayoutOption>());
					GUILayout.Label("Attached to:", Array.Empty<GUILayoutOption>());
					foreach (Transform transform in playerMovementParenting.TrackedObjects)
					{
						if (transform == null)
						{
							GUILayout.Label("null", Array.Empty<GUILayoutOption>());
						}
						else
						{
							GUILayout.Label("- " + transform.name, Array.Empty<GUILayoutOption>());
						}
					}
					GUILayout.Label("------------------------------", Array.Empty<GUILayoutOption>());
				}
			}
		}

		// Token: 0x04002E23 RID: 11811
		private static PlayerParentingDebug _lastInstance;

		// Token: 0x04002E27 RID: 11815
		private bool active;

		// Token: 0x04002E28 RID: 11816
		private PlayerMovementParenting[] pmp;
	}
}
