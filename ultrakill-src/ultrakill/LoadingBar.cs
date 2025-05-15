using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004AC RID: 1196
public class LoadingBar : MonoBehaviour
{
	// Token: 0x06001B85 RID: 7045 RVA: 0x000E42AC File Offset: 0x000E24AC
	private void OnEnable()
	{
		this.loadTime = 0f;
		this.barImage = base.GetComponent<Image>();
		this.barMaterial = this.barImage.materialForRendering;
		this.barMaterial.SetFloat("_LoadTime", 0f);
		this.barImage.SetMaterialDirty();
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000E4301 File Offset: 0x000E2501
	private void Update()
	{
		this.loadTime += Time.deltaTime;
		this.barMaterial.SetFloat("_LoadTime", this.loadTime);
		this.barImage.SetMaterialDirty();
	}

	// Token: 0x040026CA RID: 9930
	private Material barMaterial;

	// Token: 0x040026CB RID: 9931
	private Image barImage;

	// Token: 0x040026CC RID: 9932
	private float loadTime;
}
