using System;
using Steamworks;
using TMPro;
using UnityEngine;

namespace Sandbox
{
	// Token: 0x02000568 RID: 1384
	public class StatsDisplay : MonoBehaviour
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x000FF648 File Offset: 0x000FD848
		private void UpdateDisplay()
		{
			if (SteamController.Instance == null || !SteamClient.IsValid)
			{
				return;
			}
			SandboxStats sandboxStats = SteamController.Instance.GetSandboxStats();
			this.textContent.text = string.Format("<color=#FF4343>{0}</color> - Total boxes built\n", sandboxStats.brushesBuilt) + string.Format("<color=#FF4343>{0}</color> - Total props placed\n", sandboxStats.propsSpawned) + string.Format("<color=#FF4343>{0}</color> - Total enemies spawned\n", sandboxStats.enemiesSpawned) + string.Format("<color=#FF4343>{0:F1}h</color> - Total time in Sandbox\n", sandboxStats.hoursSpend);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000FF6D9 File Offset: 0x000FD8D9
		private void OnEnable()
		{
			this.UpdateDisplay();
			this.timeSinceUpdate = 0f;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000FF6F1 File Offset: 0x000FD8F1
		private void Update()
		{
			if (this.timeSinceUpdate > 2f)
			{
				this.timeSinceUpdate = 0f;
				this.UpdateDisplay();
			}
		}

		// Token: 0x04002BA1 RID: 11169
		[SerializeField]
		private TMP_Text textContent;

		// Token: 0x04002BA2 RID: 11170
		private TimeSince timeSinceUpdate;
	}
}
