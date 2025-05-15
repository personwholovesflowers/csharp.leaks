using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000103 RID: 259
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class DebugUI : MonoSingleton<DebugUI>
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x00021D1C File Offset: 0x0001FF1C
	private void Update()
	{
		if (!DebugUI.previewRumble)
		{
			this.rumbleContainer.gameObject.SetActive(false);
			return;
		}
		float currentIntensity = MonoSingleton<RumbleManager>.Instance.currentIntensity;
		this.rumbleContainer.gameObject.SetActive(true);
		float num = Mathf.Sin(Time.time * 100f) * (currentIntensity * 6f);
		this.rumbleIconTransform.anchoredPosition = new Vector2(num, this.rumbleIconTransform.anchoredPosition.y);
		this.rumbleImage.color = new Color(1f, 1f, 1f, (currentIntensity == 0f) ? 0.3f : 1f);
		this.rumbleIntensityBar.gameObject.SetActive(currentIntensity > 0f);
		this.rumbleIntensityBar.value = currentIntensity;
	}

	// Token: 0x040006CD RID: 1741
	public static bool previewRumble;

	// Token: 0x040006CE RID: 1742
	[SerializeField]
	private GameObject rumbleContainer;

	// Token: 0x040006CF RID: 1743
	[SerializeField]
	private RectTransform rumbleIconTransform;

	// Token: 0x040006D0 RID: 1744
	[SerializeField]
	private Image rumbleImage;

	// Token: 0x040006D1 RID: 1745
	[SerializeField]
	private Slider rumbleIntensityBar;
}
