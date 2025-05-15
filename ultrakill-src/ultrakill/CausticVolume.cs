using System;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
[ExecuteInEditMode]
public class CausticVolume : MonoBehaviour
{
	// Token: 0x06001CA0 RID: 7328 RVA: 0x000F07BC File Offset: 0x000EE9BC
	private void OnValidate()
	{
		this.manager = MonoSingleton<CausticVolumeManager>.Instance;
		if (this.manager == null)
		{
			this.manager = Object.FindAnyObjectByType<CausticVolumeManager>(FindObjectsInactive.Include);
		}
		if (this.manager == null)
		{
			GameObject gameObject = new GameObject("CausticVolumeManager");
			this.manager = gameObject.AddComponent<CausticVolumeManager>();
		}
		this.manager.AddVolume(this);
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000F081F File Offset: 0x000EEA1F
	private void OnEnable()
	{
		this.manager.AddVolume(this);
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x000F082D File Offset: 0x000EEA2D
	private void OnDisable()
	{
		this.manager.RemoveVolume(this);
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000F083B File Offset: 0x000EEA3B
	private void OnDestroy()
	{
		if (this.manager == null)
		{
			return;
		}
		this.manager.RemoveVolume(this);
	}

	// Token: 0x040028A9 RID: 10409
	public Color color = Color.white;

	// Token: 0x040028AA RID: 10410
	public float intensity = 1f;

	// Token: 0x040028AB RID: 10411
	public float nearRadius = 1f;

	// Token: 0x040028AC RID: 10412
	public float farRadius = 2f;

	// Token: 0x040028AD RID: 10413
	[HideInInspector]
	public CausticVolumeManager manager;
}
