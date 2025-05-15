using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
[RequireComponent(typeof(Terrain))]
[ExecuteInEditMode]
public class TerrainDetailQualitySettings : MonoBehaviour
{
	// Token: 0x060004C6 RID: 1222 RVA: 0x0001BE60 File Offset: 0x0001A060
	private void Start()
	{
		if (CullForSwitchController.IsSwitchEnvironment || (Application.platform == RuntimePlatform.WindowsEditor && this.forceLowInEditor))
		{
			this.SetLow();
			return;
		}
		this.SetHigh();
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0001BE86 File Offset: 0x0001A086
	[ContextMenu("SetHigh")]
	public void SetHigh()
	{
		Debug.Log("Setting high quality terrain detail settings");
		this.SetQuality(this.highQuality);
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001BE9E File Offset: 0x0001A09E
	[ContextMenu("SetLow")]
	public void SetLow()
	{
		Debug.Log("Setting low quality terrain detail settings");
		this.SetQuality(this.lowQuality);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001BEB8 File Offset: 0x0001A0B8
	public void SetQuality(TerrainDetailQualitySettings.TerrainQualitySettings q)
	{
		Terrain component = base.GetComponent<Terrain>();
		component.detailObjectDistance = q.drawDistanceMax;
		component.detailObjectDensity = q.detailDensity;
		component.basemapDistance = q.baseMapDistance;
		Shader.SetGlobalFloat("_GrassDetailFadeOutDistance", q.drawDistanceMax);
		Shader.SetGlobalFloat("_GrassDetailFadeInDistance", q.drawDistanceFadeStart);
	}

	// Token: 0x0400049C RID: 1180
	public TerrainDetailQualitySettings.TerrainQualitySettings highQuality;

	// Token: 0x0400049D RID: 1181
	public TerrainDetailQualitySettings.TerrainQualitySettings lowQuality;

	// Token: 0x0400049E RID: 1182
	public bool forceLowInEditor;

	// Token: 0x020003A5 RID: 933
	[Serializable]
	public class TerrainQualitySettings
	{
		// Token: 0x04001362 RID: 4962
		public float drawDistanceMax = 60f;

		// Token: 0x04001363 RID: 4963
		public float drawDistanceFadeStart = 50f;

		// Token: 0x04001364 RID: 4964
		[Range(0f, 1f)]
		public float detailDensity = 1f;

		// Token: 0x04001365 RID: 4965
		public float baseMapDistance = 500f;
	}
}
