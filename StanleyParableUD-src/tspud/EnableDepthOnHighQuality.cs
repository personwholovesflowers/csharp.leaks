using System;
using UnityEngine;

// Token: 0x020000B3 RID: 179
[RequireComponent(typeof(Camera))]
public class EnableDepthOnHighQuality : MonoBehaviour
{
	// Token: 0x06000427 RID: 1063 RVA: 0x000195DD File Offset: 0x000177DD
	private void Awake()
	{
		IntConfigurable intConfigurable = this.qualitySettingsConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnQualityLevelChange));
		QualitySettings.SetQualityLevel(this.qualitySettingsConfigurable.GetIntValue());
		this.OnQualityLevelChange(null);
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00019620 File Offset: 0x00017820
	private void OnQualityLevelChange(LiveData ld)
	{
		Camera component = base.GetComponent<Camera>();
		int qualityLevel = QualitySettings.GetQualityLevel();
		if (QualitySettings.names[qualityLevel] == "PC HQ")
		{
			component.depthTextureMode = DepthTextureMode.Depth;
			return;
		}
		component.depthTextureMode = DepthTextureMode.None;
	}

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private IntConfigurable qualitySettingsConfigurable;
}
