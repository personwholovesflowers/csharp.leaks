using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class UpdateBlur : MonoBehaviour
{
	// Token: 0x06000A68 RID: 2664 RVA: 0x00030F09 File Offset: 0x0002F109
	private void Update()
	{
		if (MainCamera.BlurValue == 0f)
		{
			this.blur.enabled = false;
			return;
		}
		this.blur.enabled = true;
		this.blur.BlurAmount = MainCamera.BlurValue * this.blurMultiplier;
	}

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private MobileBlur blur;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	private float blurMultiplier = 0.333f;
}
