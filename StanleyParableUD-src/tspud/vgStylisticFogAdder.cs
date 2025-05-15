using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000D2 RID: 210
public class vgStylisticFogAdder : MonoBehaviour
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x0001C910 File Offset: 0x0001AB10
	private void Start()
	{
		this.fog = StanleyController.Instance.cam.gameObject.AddComponent<vgStylisticFog>();
		this.fog.fogShader = this.shaderToSet;
		this.fog._customFogData = this.fogDataToSet;
		SceneManager.sceneUnloaded += this.SceneManager_sceneUnloaded;
		RenderSettings.fogStartDistance = 5000f;
		RenderSettings.fogEndDistance = 20000f;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001C97E File Offset: 0x0001AB7E
	private void SceneManager_sceneUnloaded(Scene arg0)
	{
		if (arg0.name.StartsWith("Loading"))
		{
			return;
		}
		Object.Destroy(this.fog);
		SceneManager.sceneUnloaded -= this.SceneManager_sceneUnloaded;
	}

	// Token: 0x040004B6 RID: 1206
	public Shader shaderToSet;

	// Token: 0x040004B7 RID: 1207
	public vgStylisticFogData fogDataToSet;

	// Token: 0x040004B8 RID: 1208
	private vgStylisticFog fog;
}
