using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class SetChildShaderPropertiesOnPref : MonoBehaviour
{
	// Token: 0x06001682 RID: 5762 RVA: 0x000B507B File Offset: 0x000B327B
	private void Awake()
	{
		this.renderers = new List<Renderer>();
		this.renderers.AddRange(base.GetComponentsInChildren<Renderer>(true));
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x000B509C File Offset: 0x000B329C
	private void OnEnable()
	{
		if (this.localPref ? MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.prefName, false) : MonoSingleton<PrefsManager>.Instance.GetBool(this.prefName, false))
		{
			foreach (ShaderProperty shaderProperty in this.onTrue)
			{
				foreach (Renderer renderer in this.renderers)
				{
					shaderProperty.Set(renderer.material);
				}
			}
			return;
		}
		foreach (ShaderProperty shaderProperty2 in this.onFalse)
		{
			foreach (Renderer renderer2 in this.renderers)
			{
				shaderProperty2.Set(renderer2.material);
			}
		}
	}

	// Token: 0x04001F10 RID: 7952
	private List<Renderer> renderers;

	// Token: 0x04001F11 RID: 7953
	public bool localPref;

	// Token: 0x04001F12 RID: 7954
	public string prefName;

	// Token: 0x04001F13 RID: 7955
	[Space]
	public ShaderProperty[] onTrue;

	// Token: 0x04001F14 RID: 7956
	public ShaderProperty[] onFalse;
}
