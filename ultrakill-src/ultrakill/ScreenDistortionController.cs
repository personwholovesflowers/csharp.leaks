using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class ScreenDistortionController : MonoSingleton<ScreenDistortionController>
{
	// Token: 0x0600160D RID: 5645 RVA: 0x000B2704 File Offset: 0x000B0904
	private void Start()
	{
		this.pp = MonoSingleton<PostProcessV2_Handler>.Instance;
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x000B2714 File Offset: 0x000B0914
	private void Update()
	{
		int num = this.lastCount;
		int count = this.fields.Count;
		if (this.lastCount != this.fields.Count)
		{
			this.pp.WickedEffect(this.fields.Count > 0);
			this.lastCount = this.fields.Count;
		}
		if (this.fields.Count == 0)
		{
			Shader.SetGlobalFloat("_RandomNoiseStrength", 0f);
			return;
		}
		float num2 = 0f;
		for (int i = this.fields.Count - 1; i >= 0; i--)
		{
			if (this.fields[i] == null)
			{
				this.fields.RemoveAt(i);
			}
			else if (this.fields[i].currentStrength > num2)
			{
				num2 = this.fields[i].currentStrength;
			}
		}
		Shader.SetGlobalFloat("_RandomNoiseStrength", num2);
	}

	// Token: 0x04001E5D RID: 7773
	public List<ScreenDistortionField> fields = new List<ScreenDistortionField>();

	// Token: 0x04001E5E RID: 7774
	private PostProcessV2_Handler pp;

	// Token: 0x04001E5F RID: 7775
	private int lastCount = int.MaxValue;
}
