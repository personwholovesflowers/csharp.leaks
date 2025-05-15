using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200039A RID: 922
public class RumbleKeyOptionEntry : MonoBehaviour
{
	// Token: 0x0600152D RID: 5421 RVA: 0x000AD12B File Offset: 0x000AB32B
	public void ResetIntensity()
	{
		this.intensitySlider.SetValueWithoutNotify(MonoSingleton<RumbleManager>.Instance.ResolveDefaultIntensity(this.key));
		MonoSingleton<PrefsManager>.Instance.DeleteKey(string.Format("{0}{1}", this.key, ".intensity"));
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x000AD167 File Offset: 0x000AB367
	public void ResetDuration()
	{
		this.durationSlider.SetValueWithoutNotify(MonoSingleton<RumbleManager>.Instance.ResolveDefaultDuration(this.key));
		MonoSingleton<PrefsManager>.Instance.DeleteKey(string.Format("{0}{1}", this.key, ".duration"));
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x000AD1A3 File Offset: 0x000AB3A3
	public void SetIntensity(float value)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Format("{0}{1}", this.key, ".intensity"), value);
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x000AD1C5 File Offset: 0x000AB3C5
	public void SetDuration(float value)
	{
		MonoSingleton<PrefsManager>.Instance.SetFloat(string.Format("{0}{1}", this.key, ".duration"), value);
	}

	// Token: 0x04001D6E RID: 7534
	public RumbleKey key;

	// Token: 0x04001D6F RID: 7535
	public TMP_Text keyName;

	// Token: 0x04001D70 RID: 7536
	public Slider intensitySlider;

	// Token: 0x04001D71 RID: 7537
	public Slider durationSlider;

	// Token: 0x04001D72 RID: 7538
	public Button intensityWrapper;

	// Token: 0x04001D73 RID: 7539
	public Button durationWrapper;

	// Token: 0x04001D74 RID: 7540
	public GameObject durationContainer;
}
