using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class FadeOutBars : MonoBehaviour
{
	// Token: 0x0600090F RID: 2319 RVA: 0x0003C300 File Offset: 0x0003A500
	private void Start()
	{
		this.CheckState();
		this.slids = base.GetComponentsInChildren<SliderToFillAmount>();
		SliderToFillAmount[] array = this.slids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].mama = this;
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0003C33D File Offset: 0x0003A53D
	private void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0003C35F File Offset: 0x0003A55F
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0003C381 File Offset: 0x0003A581
	private void OnPrefChanged(string key, object value)
	{
		if (key == "crossHairHudFade" && value is bool)
		{
			bool flag = (bool)value;
			this.CheckState();
		}
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0003C3A5 File Offset: 0x0003A5A5
	private void Update()
	{
		if (this.fadeOut)
		{
			this.fadeOutTime = Mathf.MoveTowards(this.fadeOutTime, 0f, Time.unscaledDeltaTime);
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0003C3CA File Offset: 0x0003A5CA
	public void CheckState()
	{
		this.fadeOut = MonoSingleton<PrefsManager>.Instance.GetBool("crossHairHudFade", false);
		this.ResetTimer();
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0003C3E8 File Offset: 0x0003A5E8
	public void ResetTimer()
	{
		bool flag = MonoSingleton<PrefsManager>.Instance.GetInt("crossHairHud", 0) == 0;
		if (!flag && HideUI.Active)
		{
			flag = true;
		}
		this.fadeOutTime = (float)(flag ? 0 : 2);
	}

	// Token: 0x04000B87 RID: 2951
	private bool fadeOut;

	// Token: 0x04000B88 RID: 2952
	public float fadeOutTime;

	// Token: 0x04000B89 RID: 2953
	private SliderToFillAmount[] slids;
}
