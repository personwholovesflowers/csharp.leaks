using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class StencilValuesByLayer : MonoBehaviour
{
	// Token: 0x06001B9D RID: 7069 RVA: 0x000E4FC0 File Offset: 0x000E31C0
	private void Start()
	{
		base.StartCoroutine(this.LateStart());
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000E4FCF File Offset: 0x000E31CF
	private IEnumerator LateStart()
	{
		yield return null;
		RenderSettings.skybox.SetFloat("_Stencil", 1f);
		StaticSceneOptimizer instance = MonoSingleton<StaticSceneOptimizer>.Instance;
		if (instance != null)
		{
			instance.UpdateRain(this.applyRainOutdoors);
		}
		this.rends = Object.FindObjectsOfType<Renderer>(true);
		foreach (Renderer renderer in this.rends)
		{
			if (!(renderer == null) && renderer.gameObject.layer == 24)
			{
				MeshRenderer meshRenderer = renderer as MeshRenderer;
				if (meshRenderer == null || !instance.staticMRends.Contains(meshRenderer))
				{
					renderer.GetMaterials(this.reusableMaterials);
					bool flag = false;
					foreach (Material material in this.reusableMaterials)
					{
						if (!(material == null))
						{
							Shader shader = material.shader;
							if (!(shader == null))
							{
								bool flag2 = shader == this.masterShader;
								flag = flag || flag2;
								if (flag2)
								{
									if (this.applyStencilValue)
									{
										material.SetFloat("_Stencil", 1f);
									}
									if (this.applyRainOutdoors)
									{
										material.EnableKeyword("RAIN");
									}
								}
							}
						}
					}
					if (flag)
					{
						renderer.SetMaterials(this.reusableMaterials);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x000E4FE0 File Offset: 0x000E31E0
	public void EnableRain(bool doEnable)
	{
		if (MonoSingleton<StaticSceneOptimizer>.Instance != null)
		{
			MonoSingleton<StaticSceneOptimizer>.Instance.UpdateRain(this.applyRainOutdoors);
		}
		foreach (Renderer renderer in this.rends)
		{
			if (!(renderer == null))
			{
				renderer.GetSharedMaterials(this.reusableMaterials);
				bool flag = false;
				foreach (Material material in this.reusableMaterials)
				{
					if (!(material == null))
					{
						Shader shader = material.shader;
						if (!(shader == null))
						{
							bool flag2 = shader == this.masterShader;
							flag = flag || flag2;
							if (flag2 && renderer.gameObject.layer == 24)
							{
								if (doEnable)
								{
									material.EnableKeyword("RAIN");
								}
								else
								{
									material.DisableKeyword("RAIN");
								}
							}
						}
					}
				}
				if (flag)
				{
					renderer.SetSharedMaterials(this.reusableMaterials);
				}
			}
		}
	}

	// Token: 0x040026DD RID: 9949
	public bool applyStencilValue = true;

	// Token: 0x040026DE RID: 9950
	public bool applyRainOutdoors;

	// Token: 0x040026DF RID: 9951
	public Shader masterShader;

	// Token: 0x040026E0 RID: 9952
	private Renderer[] rends;

	// Token: 0x040026E1 RID: 9953
	private List<Material> reusableMaterials = new List<Material>();
}
