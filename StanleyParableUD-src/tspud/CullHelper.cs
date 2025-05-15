using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[ExecuteInEditMode]
public class CullHelper : MonoBehaviour
{
	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001A951 File Offset: 0x00018B51
	// (set) Token: 0x0600049D RID: 1181 RVA: 0x0001A958 File Offset: 0x00018B58
	public static CullHelper Instance { get; private set; }

	// Token: 0x0600049E RID: 1182 RVA: 0x0001A960 File Offset: 0x00018B60
	[ContextMenu("Referesh Instance")]
	private void Awake()
	{
		if (CullHelper.Instance != null)
		{
			Debug.LogWarning("Duplicate Cull helper!");
			return;
		}
		CullHelper.Instance = this;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001A980 File Offset: 0x00018B80
	private void Update()
	{
		if (Application.isPlaying)
		{
			base.enabled = false;
			return;
		}
		if (this.disablePotentiallyCulledItems != this.disabledPotentialCulledItemsPrev || this.disableDefinitelyCulledItems != this.disableDefinitelyCulledItemsPrev)
		{
			this.disabledPotentialCulledItemsPrev = this.disablePotentiallyCulledItems;
			this.disableDefinitelyCulledItemsPrev = this.disableDefinitelyCulledItems;
			foreach (PotentialCullItem potentialCullItem in Resources.FindObjectsOfTypeAll(typeof(PotentialCullItem)))
			{
				if (potentialCullItem.definitelyCulled)
				{
					potentialCullItem.TargetGameObject.SetActive(!this.disableDefinitelyCulledItems);
				}
				else
				{
					potentialCullItem.TargetGameObject.SetActive(!this.disablePotentiallyCulledItems);
				}
			}
		}
	}

	// Token: 0x04000466 RID: 1126
	public bool disablePotentiallyCulledItems;

	// Token: 0x04000467 RID: 1127
	public bool disableDefinitelyCulledItems;

	// Token: 0x04000468 RID: 1128
	private bool disabledPotentialCulledItemsPrev = true;

	// Token: 0x04000469 RID: 1129
	private bool disableDefinitelyCulledItemsPrev = true;
}
