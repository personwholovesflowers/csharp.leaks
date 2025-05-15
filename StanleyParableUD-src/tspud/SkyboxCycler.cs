using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class SkyboxCycler : MonoBehaviour
{
	// Token: 0x06000940 RID: 2368 RVA: 0x0002B7E0 File Offset: 0x000299E0
	private void Awake()
	{
		if (this.SetOnAwake && this.defaultConfigurationConfigurable != null)
		{
			RenderSettings.skybox = (this.defaultConfigurationConfigurable.GetBooleanValue() ? this.skyboxMaterials : this.skyboxMaterialsLowEnd)[0];
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0002B81C File Offset: 0x00029A1C
	public void CycleSkybox()
	{
		Material[] array;
		if (this.defaultConfigurationConfigurable != null)
		{
			array = (this.defaultConfigurationConfigurable.GetBooleanValue() ? this.skyboxMaterials : this.skyboxMaterialsLowEnd);
		}
		else
		{
			array = this.skyboxMaterials;
		}
		int num = Array.FindIndex<Material>(array, (Material s) => s == RenderSettings.skybox);
		if (num != -1)
		{
			this.currentIndex = num;
		}
		this.currentIndex = (this.currentIndex + 1) % this.skyboxMaterials.Length;
		RenderSettings.skybox = array[this.currentIndex];
	}

	// Token: 0x04000916 RID: 2326
	public Material[] skyboxMaterials;

	// Token: 0x04000917 RID: 2327
	public Material[] skyboxMaterialsLowEnd;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private Configurable defaultConfigurationConfigurable;

	// Token: 0x04000919 RID: 2329
	private int currentIndex = -1;

	// Token: 0x0400091A RID: 2330
	public bool SetOnAwake;
}
