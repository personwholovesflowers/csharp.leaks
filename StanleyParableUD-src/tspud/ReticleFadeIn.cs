using System;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class ReticleFadeIn : MonoBehaviour
{
	// Token: 0x060008B2 RID: 2226 RVA: 0x00028D63 File Offset: 0x00026F63
	private void Start()
	{
		GameMaster.OnPrepareLoadingLevel += this.GameMaster_OnPrepareLoadingLevel;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00028D76 File Offset: 0x00026F76
	private void GameMaster_OnPrepareLoadingLevel()
	{
		this.Opacity = -this.levelStartDelayTime;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00028D88 File Offset: 0x00026F88
	private void Update()
	{
		if (this.Opacity == 1f)
		{
			return;
		}
		if (this.Opacity > 1f)
		{
			this.Opacity = 1f;
		}
		else if (this.Opacity > 0f)
		{
			this.Opacity += Singleton<GameMaster>.Instance.GameSmoothedDeltaTime / this.fadeTime;
		}
		else
		{
			this.Opacity += Singleton<GameMaster>.Instance.GameSmoothedDeltaTime;
		}
		FloatEvent onOpacityChanged = this.OnOpacityChanged;
		if (onOpacityChanged == null)
		{
			return;
		}
		onOpacityChanged.Invoke(Mathf.Clamp01(this.Opacity));
	}

	// Token: 0x04000870 RID: 2160
	public float fadeTime = 2f;

	// Token: 0x04000871 RID: 2161
	public float levelStartDelayTime = 3f;

	// Token: 0x04000872 RID: 2162
	[Header("Debug")]
	public float Opacity = -2f;

	// Token: 0x04000873 RID: 2163
	public FloatEvent OnOpacityChanged;
}
