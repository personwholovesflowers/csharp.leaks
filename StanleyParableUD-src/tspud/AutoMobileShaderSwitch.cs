using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000097 RID: 151
public class AutoMobileShaderSwitch : MonoBehaviour
{
	// Token: 0x060003A8 RID: 936 RVA: 0x00017EF5 File Offset: 0x000160F5
	private void Awake()
	{
		this.switchMaterials = !this.UseDefaultConfigurationConfigurable.GetBooleanValue();
		if (this.switchMaterials)
		{
			this.UpdateMaterials();
			SceneManager.sceneLoaded += this.SceneLoaded;
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00017F2A File Offset: 0x0001612A
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= this.SceneLoaded;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00017F3D File Offset: 0x0001613D
	private void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		base.StartCoroutine(this.DelayUpdate());
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00017F4C File Offset: 0x0001614C
	private IEnumerator DelayUpdate()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.UpdateMaterials();
		yield break;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00017F5C File Offset: 0x0001615C
	private void UpdateMaterials()
	{
		this.renderers = Object.FindObjectsOfType<Renderer>();
		List<Material> list = new List<Material>();
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Material material in array[i].sharedMaterials)
			{
				if (material != null && material.shader != null && !list.Contains(material))
				{
					Shader shader = null;
					if (this.m_ReplacementList.GetReplacement(material.shader, out shader))
					{
						material.shader = shader;
						list.Add(material);
					}
				}
			}
		}
	}

	// Token: 0x0400039C RID: 924
	[SerializeField]
	private AutoMobileShaderSwitch.ReplacementList m_ReplacementList;

	// Token: 0x0400039D RID: 925
	[SerializeField]
	private Configurable UseDefaultConfigurationConfigurable;

	// Token: 0x0400039E RID: 926
	[SerializeField]
	private Renderer[] renderers = new Renderer[0];

	// Token: 0x0400039F RID: 927
	private bool switchMaterials;

	// Token: 0x02000393 RID: 915
	[Serializable]
	public class ReplacementDefinition
	{
		// Token: 0x0400131B RID: 4891
		public Shader original;

		// Token: 0x0400131C RID: 4892
		public Shader replacement;
	}

	// Token: 0x02000394 RID: 916
	[Serializable]
	public class ReplacementList
	{
		// Token: 0x0600167B RID: 5755 RVA: 0x00076F10 File Offset: 0x00075110
		public bool GetReplacement(Shader currentShader, out Shader replacement)
		{
			foreach (AutoMobileShaderSwitch.ReplacementDefinition replacementDefinition in this.items)
			{
				if (replacementDefinition.original == currentShader)
				{
					replacement = replacementDefinition.replacement;
					return true;
				}
			}
			replacement = null;
			return false;
		}

		// Token: 0x0400131D RID: 4893
		public AutoMobileShaderSwitch.ReplacementDefinition[] items = new AutoMobileShaderSwitch.ReplacementDefinition[0];
	}
}
