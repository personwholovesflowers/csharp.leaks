using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CB RID: 203
[ExecuteInEditMode]
public class ProceduralSkyboxAndCloudBlender : MonoBehaviour
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x0001B404 File Offset: 0x00019604
	private void Update()
	{
		this.blend = Mathf.Clamp(this.blend, 0f, (float)(this.skyboxDefinitions.Count - 1));
		int num = Mathf.FloorToInt(this.blend);
		float num2 = this.blend % 1f;
		if (num == this.skyboxDefinitions.Count - 1)
		{
			num--;
			num2 = 1f;
		}
		ProceduralSkyboxAndCloudBlender.SkyboxDefinition skyboxDefinition = this.skyboxDefinitions[num];
		ProceduralSkyboxAndCloudBlender.SkyboxDefinition skyboxDefinition2 = this.skyboxDefinitions[num + 1];
		foreach (string text in this.ColourProperties)
		{
			this.skyboxOutput.SetColor(text, Color.Lerp(skyboxDefinition.skybox.GetColor(text), skyboxDefinition2.skybox.GetColor(text), num2));
		}
		foreach (string text2 in this.FloatProperties)
		{
			this.skyboxOutput.SetFloat(text2, Mathf.Lerp(skyboxDefinition.skybox.GetFloat(text2), skyboxDefinition2.skybox.GetFloat(text2), num2));
		}
		foreach (string text3 in this.VectorProperties)
		{
			this.skyboxOutput.SetVector(text3, Vector4.Lerp(skyboxDefinition.skybox.GetVector(text3), skyboxDefinition2.skybox.GetVector(text3), num2));
		}
		for (int j = 0; j < this.skyboxDefinitions.Count; j++)
		{
			bool flag = j == num || j == num + 1;
			if (this.skyboxDefinitions[j].skyDome.gameObject.activeSelf != flag)
			{
				this.skyboxDefinitions[j].skyDome.gameObject.SetActive(flag);
				this.skyboxDefinitions[j].skyDome.sharedMaterial.SetFloat("_Fade", 0f);
			}
			if (j == num)
			{
				this.skyboxDefinitions[j].skyDome.sharedMaterial.SetFloat("_Fade", 1f - num2);
			}
			if (j == num + 1)
			{
				this.skyboxDefinitions[j].skyDome.sharedMaterial.SetFloat("_Fade", num2);
			}
		}
	}

	// Token: 0x0400047F RID: 1151
	public List<ProceduralSkyboxAndCloudBlender.SkyboxDefinition> skyboxDefinitions;

	// Token: 0x04000480 RID: 1152
	public Material skyboxOutput;

	// Token: 0x04000481 RID: 1153
	public float blend;

	// Token: 0x04000482 RID: 1154
	private string[] ColourProperties = new string[] { "_topColor", "_horizonColor", "_bottomColor", "_sunColor", "_horizonHaloColor", "_sunHaloColor" };

	// Token: 0x04000483 RID: 1155
	private string[] FloatProperties = new string[] { "_topSkyColorBlending", "_bottomSkyColorBlending", "_sunIntensity", "_sunSharpness", "_horizonHaloIntensity", "_horizonHaloSize", "_sunHaloIntensity", "_sunHaloSize" };

	// Token: 0x04000484 RID: 1156
	private string[] VectorProperties = new string[] { "_sunDir" };

	// Token: 0x020003A1 RID: 929
	[Serializable]
	public class SkyboxDefinition
	{
		// Token: 0x04001351 RID: 4945
		public MeshRenderer skyDome;

		// Token: 0x04001352 RID: 4946
		public Material skybox;
	}
}
