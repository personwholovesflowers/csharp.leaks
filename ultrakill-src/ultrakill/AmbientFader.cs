using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class AmbientFader : MonoBehaviour
{
	// Token: 0x060001CA RID: 458 RVA: 0x00009652 File Offset: 0x00007852
	private void Start()
	{
		this.fadeAmount = Shader.GetGlobalFloat("_AmbientStrength");
		this.target = this.fadeAmount;
		if (this.onEnable)
		{
			this.activated = true;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00009680 File Offset: 0x00007880
	private void Update()
	{
		if (this.activated)
		{
			this.fadeAmount = Mathf.MoveTowards(this.fadeAmount, this.target, 1f / this.time * Time.deltaTime);
			Shader.SetGlobalFloat("_AmbientStrength", this.fadeAmount);
			if (this.fadeAmount == this.target)
			{
				this.activated = false;
			}
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x000096E3 File Offset: 0x000078E3
	public void FadeTo(float newTarget)
	{
		this.target = newTarget;
		if (this.time == 0f)
		{
			this.fadeAmount = this.target;
			Shader.SetGlobalFloat("_AmbientStrength", this.fadeAmount);
			return;
		}
		this.activated = true;
	}

	// Token: 0x040001E5 RID: 485
	private float fadeAmount = 1f;

	// Token: 0x040001E6 RID: 486
	public float target = 1f;

	// Token: 0x040001E7 RID: 487
	public float time;

	// Token: 0x040001E8 RID: 488
	private bool activated;

	// Token: 0x040001E9 RID: 489
	public bool onEnable;
}
