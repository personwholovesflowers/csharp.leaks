using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class VideoSettingsController : MonoBehaviour
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000A77 RID: 2679 RVA: 0x000312B7 File Offset: 0x0002F4B7
	// (set) Token: 0x06000A78 RID: 2680 RVA: 0x000312BE File Offset: 0x0002F4BE
	public static VideoSettingsController Instance { get; private set; }

	// Token: 0x06000A79 RID: 2681 RVA: 0x000312C8 File Offset: 0x0002F4C8
	private void Awake()
	{
		VideoSettingsController.Instance = this;
		IntConfigurable intConfigurable = this.antiAliasingIndexConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnAAIndexChanged));
		IntConfigurable intConfigurable2 = this.qualitySettingsIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable2.OnValueChanged, new Action<LiveData>(this.OnQualityLevelIndexChanged));
		BooleanConfigurable booleanConfigurable = this.vSyncConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.OnVSyncChanged));
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00031350 File Offset: 0x0002F550
	private void OnDestroy()
	{
		IntConfigurable intConfigurable = this.antiAliasingIndexConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnAAIndexChanged));
		IntConfigurable intConfigurable2 = this.qualitySettingsIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable2.OnValueChanged, new Action<LiveData>(this.OnQualityLevelIndexChanged));
		BooleanConfigurable booleanConfigurable = this.vSyncConfigurable;
		booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.OnVSyncChanged));
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x000313D2 File Offset: 0x0002F5D2
	private void OnAAIndexChanged(LiveData ld)
	{
		this.SetAAFromIndex(ld.IntValue);
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x000313E0 File Offset: 0x0002F5E0
	public void SetAAFromIndex(int val)
	{
		QualitySettings.antiAliasing = VideoSettingsController.IndexToAAValue(val);
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x000313ED File Offset: 0x0002F5ED
	public static int IndexToAAValue(int index)
	{
		switch (index)
		{
		default:
			return 0;
		case 1:
			return 2;
		case 2:
			return 4;
		case 3:
			return 8;
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0003140C File Offset: 0x0002F60C
	public static string IndexToAAOption(int index)
	{
		if (index == 0)
		{
			return "-";
		}
		return VideoSettingsController.IndexToAAValue(index) + "x AA";
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0003142C File Offset: 0x0002F62C
	private void OnQualityLevelIndexChanged(LiveData ld)
	{
		this.SetQualityLevelFromIndex(ld.IntValue);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0003143A File Offset: 0x0002F63A
	public void SetQualityLevelFromIndex(int val)
	{
		QualitySettings.SetQualityLevel(val);
		this.antiAliasingIndexConfigurable.ForceUpdate();
		this.vSyncConfigurable.ForceUpdate();
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00031458 File Offset: 0x0002F658
	private void OnVSyncChanged(LiveData ld)
	{
		this.SetVSync(ld.BooleanValue);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00031466 File Offset: 0x0002F666
	public void SetVSync(bool vsync)
	{
		QualitySettings.vSyncCount = (vsync ? 1 : 0);
	}

	// Token: 0x04000A6D RID: 2669
	[SerializeField]
	private IntConfigurable antiAliasingIndexConfigurable;

	// Token: 0x04000A6E RID: 2670
	[SerializeField]
	private IntConfigurable qualitySettingsIndexConfigurable;

	// Token: 0x04000A6F RID: 2671
	[SerializeField]
	private BooleanConfigurable vSyncConfigurable;
}
